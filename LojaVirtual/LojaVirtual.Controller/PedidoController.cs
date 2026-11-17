using LojaVirtual.Data;                          // para usar LojaDbContext
using LojaVirtual.Model.Carrinho;                // para usar CarrinhoDeCompras
using LojaVirtual.Model.Descontos;               // para usar IDescontoStrategy
using LojaVirtual.Model.Entidades;               // para usar Pedido, Produto, Endereco...
using LojaVirtual.Model.Entidades.Pagamentos;    // para usar Pagamento

namespace LojaVirtual.Controller;

// Controlador do checkout: Endereço -> Entrega -> Pagamento -> Revisão -> Concluído.
public class PedidoController
{
    // A regra de desconto vem de fora (Strategy). O controlador não sabe qual é.
    // readonly: só pode receber valor no construtor.
    private readonly IDescontoStrategy _desconto;

    // Construtor: recebe a regra de desconto (ex.: new DescontoQuantidade()).
    public PedidoController(IDescontoStrategy desconto)
    {
        _desconto = desconto;   // guarda a regra para usar depois
    }

    // Endereços salvos do cliente (etapa Endereço).
    public List<Endereco> ListarEnderecos(int clienteId)
    {
        using (LojaDbContext db = new LojaDbContext())
        {
            return db.Enderecos.Where(e => e.ClienteId == clienteId).ToList();   // só os deste cliente
        }
    }

    // Opções de entrega, da mais rápida para a mais lenta (etapa Entrega).
    public List<OpcaoEntrega> ListarOpcoesEntrega()
    {
        using (LojaDbContext db = new LojaDbContext())
        {
            return db.OpcoesEntrega.OrderBy(o => o.PrazoDias).ToList();   // menor prazo primeiro
        }
    }

    // Usado na etapa Entrega (para mostrar o frete) e na hora de finalizar.
    public decimal CalcularFrete(CarrinhoDeCompras carrinho, OpcaoEntrega opcao)
    {
        return opcao.CalcularFrete(carrinho.CalcularPesoTotal());   // a regra fica na OpcaoEntrega
    }

    // Desconto mostrado na Revisão, antes de confirmar a compra.
    public decimal CalcularDesconto(CarrinhoDeCompras carrinho)
    {
        Pedido pedidoTemporario = new Pedido();          // pedido só para o cálculo (não é gravado)
        pedidoTemporario.Itens = carrinho.Itens;         // com os itens do carrinho
        return _desconto.CalcularDesconto(pedidoTemporario);   // a estratégia calcula
    }

    // Finaliza a compra e devolve o pedido gravado (com o número gerado pelo banco).
    public Pedido Finalizar(int clienteId, int enderecoId, OpcaoEntrega opcaoEntrega, Pagamento pagamento, CarrinhoDeCompras carrinho)
    {
        if (carrinho.Itens.Count == 0)   // não deixa gravar um pedido sem itens
            throw new InvalidOperationException("O carrinho está vazio.");

        using (LojaDbContext db = new LojaDbContext())   // uma conexão para a compra inteira
        {
            Pedido pedido = new Pedido();              // o pedido novo
            pedido.ClienteId = clienteId;              // quem comprou
            pedido.EnderecoEntregaId = enderecoId;     // para onde vai
            pedido.OpcaoEntregaId = opcaoEntrega.Id;   // como vai
            pedido.Data = DateTime.Now;                // quando

            // 1. Cria os itens lendo cada produto de novo do banco (estoque e preço atuais)
            //    e já baixa o estoque. Se faltar alguma peça, BaixarEstoque lança
            //    EstoqueInsuficienteException e nada é gravado, porque o SaveChanges só acontece no fim.
            foreach (PedidoItem itemDoCarrinho in carrinho.Itens)
            {
                Produto? produto = db.Produtos.Find(itemDoCarrinho.ProdutoId);   // lê o produto atualizado
                if (produto == null)                                             // foi apagado do banco
                    throw new InvalidOperationException("Um produto do carrinho não existe mais.");

                produto.BaixarEstoque(itemDoCarrinho.Quantidade);   // desconta (só na memória, por enquanto)

                PedidoItem item = new PedidoItem();          // item do pedido
                item.Produto = produto;                      // qual produto
                item.ProdutoId = produto.Id;
                item.Quantidade = itemDoCarrinho.Quantidade; // quantas unidades
                item.PrecoUnitario = produto.Preco;          // preço congelado no momento da compra
                pedido.Itens.Add(item);                      // coloca no pedido
            }

            // 2. Frete e desconto
            pedido.ValorFrete = CalcularFrete(carrinho, opcaoEntrega);   // mesmo cálculo da etapa Entrega
            pedido.AplicarDesconto(_desconto);                           // a estratégia escolhida calcula

            // 3. Pagamento: funciona igual para Pix, Boleto ou Cartão (polimorfismo)
            pagamento.Valor = pedido.CalcularTotal();   // itens + frete - desconto
            pagamento.Processar();                      // cada forma gera seus dados
            pedido.Pagamento = pagamento;               // liga o pagamento ao pedido (1x1)

            // 4. Grava pedido, itens, pagamento e estoque de uma vez só
            pedido.Finalizar();          // muda o status para Finalizado
            db.Pedidos.Add(pedido);      // marca o pedido (com itens e pagamento) para inserir
            db.SaveChanges();            // grava tudo numa transação; aqui o banco gera o Id

            carrinho.Limpar();   // compra feita: esvazia o carrinho
            return pedido;       // devolve o pedido para a tela mostrar o número
        }
    }
}
