using LojaVirtual.Data;
using LojaVirtual.Model.Carrinho;
using LojaVirtual.Model.Descontos;
using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Entidades.Pagamentos;

namespace LojaVirtual.Controller;

// Controlador do checkout: Endereço -> Entrega -> Pagamento -> Revisão -> Concluído.
public class PedidoController
{
    // A regra de desconto vem de fora (Strategy). O controlador não sabe qual é.
    private readonly IDescontoStrategy _desconto;

    public PedidoController(IDescontoStrategy desconto)
    {
        _desconto = desconto;
    }

    public List<Endereco> ListarEnderecos(int clienteId)
    {
        using (LojaDbContext db = new LojaDbContext())
        {
            return db.Enderecos.Where(e => e.ClienteId == clienteId).ToList();
        }
    }

    public List<OpcaoEntrega> ListarOpcoesEntrega()
    {
        using (LojaDbContext db = new LojaDbContext())
        {
            return db.OpcoesEntrega.OrderBy(o => o.PrazoDias).ToList();
        }
    }

    // Usado na etapa Entrega (para mostrar o frete) e na hora de finalizar.
    public decimal CalcularFrete(CarrinhoDeCompras carrinho, OpcaoEntrega opcao)
    {
        return opcao.CalcularFrete(carrinho.CalcularPesoTotal());
    }

    // Desconto mostrado na Revisão, antes de confirmar a compra.
    public decimal CalcularDesconto(CarrinhoDeCompras carrinho)
    {
        Pedido pedidoTemporario = new Pedido();
        pedidoTemporario.Itens = carrinho.Itens;
        return _desconto.CalcularDesconto(pedidoTemporario);
    }

    // Finaliza a compra e devolve o pedido gravado (com o número gerado pelo banco).
    public Pedido Finalizar(int clienteId, int enderecoId, OpcaoEntrega opcaoEntrega, Pagamento pagamento, CarrinhoDeCompras carrinho)
    {
        if (carrinho.Itens.Count == 0)
            throw new InvalidOperationException("O carrinho está vazio.");

        using (LojaDbContext db = new LojaDbContext())
        {
            Pedido pedido = new Pedido();
            pedido.ClienteId = clienteId;
            pedido.EnderecoEntregaId = enderecoId;
            pedido.OpcaoEntregaId = opcaoEntrega.Id;
            pedido.Data = DateTime.Now;

            // 1. Cria os itens lendo cada produto de novo do banco (estoque e preço atuais)
            //    e já baixa o estoque. Se faltar alguma peça, BaixarEstoque lança
            //    EstoqueInsuficienteException e nada é gravado, porque o SaveChanges só acontece no fim.
            foreach (PedidoItem itemDoCarrinho in carrinho.Itens)
            {
                Produto? produto = db.Produtos.Find(itemDoCarrinho.ProdutoId);
                if (produto == null)
                    throw new InvalidOperationException("Um produto do carrinho não existe mais.");

                produto.BaixarEstoque(itemDoCarrinho.Quantidade);

                PedidoItem item = new PedidoItem();
                item.Produto = produto;
                item.ProdutoId = produto.Id;
                item.Quantidade = itemDoCarrinho.Quantidade;
                item.PrecoUnitario = produto.Preco;
                pedido.Itens.Add(item);
            }

            // 2. Frete e desconto
            pedido.ValorFrete = CalcularFrete(carrinho, opcaoEntrega);
            pedido.AplicarDesconto(_desconto);

            // 3. Pagamento: funciona igual para Pix, Boleto ou Cartão (polimorfismo)
            pagamento.Valor = pedido.CalcularTotal();
            pagamento.Processar();
            pedido.Pagamento = pagamento;

            // 4. Grava pedido, itens, pagamento e estoque de uma vez só
            pedido.Finalizar();
            db.Pedidos.Add(pedido);
            db.SaveChanges();

            carrinho.Limpar();
            return pedido;
        }
    }
}
