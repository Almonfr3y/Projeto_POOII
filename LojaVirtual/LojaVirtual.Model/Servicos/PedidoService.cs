using LojaVirtual.Model.Checkout;
using LojaVirtual.Model.Descontos;
using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Entidades.Pagamentos;
using LojaVirtual.Model.Excecoes;
using LojaVirtual.Model.Repositorios;

namespace LojaVirtual.Model.Servicos;

/// <summary>
/// Regra de finalização da compra (SRP: só isso). Recebe tudo por interfaces no construtor (DIP),
/// então não sabe se os dados vêm do MySQL, de outro banco ou de um teste.
/// </summary>
public class PedidoService : IPedidoService
{
    // Dependências recebidas no construtor (montadas na FabricaDeControladores).
    private readonly IProdutoRepository _produtos;
    private readonly IPedidoRepository _pedidos;
    private readonly IOpcaoEntregaRepository _opcoesEntrega;
    private readonly IFreteService _frete;
    private readonly IDescontoStrategy _desconto;
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

    public PedidoService(IProdutoRepository produtos, IPedidoRepository pedidos, IOpcaoEntregaRepository opcoesEntrega,
        IFreteService frete, IDescontoStrategy desconto, IUnidadeDeTrabalho unidadeDeTrabalho)
    {
        _produtos = produtos;
        _pedidos = pedidos;
        _opcoesEntrega = opcoesEntrega;
        _frete = frete;
        _desconto = desconto;
        _unidadeDeTrabalho = unidadeDeTrabalho;
    }

    /// <summary>
    /// Fecha a compra: valida tudo antes de alterar qualquer dado e grava pedido, itens,
    /// pagamento e baixa de estoque de uma só vez (uma transação).
    /// </summary>
    /// <exception cref="EstoqueInsuficienteException">Alguma peça não tem estoque suficiente.</exception>
    /// <exception cref="InvalidOperationException">Carrinho vazio, produto ou opção de entrega inexistente.</exception>
    public Pedido FinalizarPedido(DadosCheckout dados, IReadOnlyList<PedidoItem> itensDoCarrinho)
    {
        // 1ª parte: só monta e confere. Se algo falhar aqui, nenhum dado foi alterado.
        Pedido pedido = MontarPedido(dados, itensDoCarrinho);
        GarantirEstoqueDeTodosOsItens(pedido);
        CalcularValores(pedido, dados.OpcaoEntregaId);
        RegistrarPagamento(pedido, dados.Pagamento);

        // 2ª parte: com tudo válido, altera o estoque e grava tudo numa única transação.
        BaixarEstoque(pedido);
        pedido.Finalizar();                      // Status = Finalizado
        _pedidos.Adicionar(pedido);
        _unidadeDeTrabalho.SalvarAlteracoes();   // aqui o banco gera o número (Id) do pedido
        return pedido;
    }

    // Cria um pedido temporário (não gravado) só para a estratégia calcular o desconto.
    public decimal SimularDesconto(IReadOnlyList<PedidoItem> itensDoCarrinho) =>
        _desconto.CalcularDesconto(new Pedido { Itens = itensDoCarrinho.ToList() });

    private Pedido MontarPedido(DadosCheckout dados, IReadOnlyList<PedidoItem> itensDoCarrinho)
    {
        if (itensDoCarrinho.Count == 0)
            throw new InvalidOperationException("O carrinho está vazio.");

        return new Pedido
        {
            ClienteId = dados.ClienteId,
            EnderecoEntregaId = dados.EnderecoEntregaId,
            OpcaoEntregaId = dados.OpcaoEntregaId,
            Data = DateTime.Now,
            Itens = itensDoCarrinho.Select(CriarItemComPrecoAtual).ToList(), // um item novo por item do carrinho
        };
    }

    /// <summary>
    /// O carrinho guarda produtos lidos em outro momento; aqui o produto é relido do banco
    /// para usar o estoque atual e congelar o preço do momento da compra.
    /// </summary>
    private PedidoItem CriarItemComPrecoAtual(PedidoItem itemDoCarrinho)
    {
        Produto produto = _produtos.ObterPorId(itemDoCarrinho.ProdutoId)
            ?? throw new InvalidOperationException("Um dos produtos do carrinho não existe mais.");

        return new PedidoItem
        {
            Produto = produto,
            ProdutoId = produto.Id,
            Quantidade = itemDoCarrinho.Quantidade,
            PrecoUnitario = produto.Preco,
        };
    }

    /// <summary>Verifica todos os itens antes de baixar qualquer um, para não deixar baixa pela metade.</summary>
    private static void GarantirEstoqueDeTodosOsItens(Pedido pedido)
    {
        PedidoItem? semEstoque = pedido.Itens.FirstOrDefault(item => item.Quantidade > item.Produto!.Estoque);
        if (semEstoque is not null)
            throw new EstoqueInsuficienteException(semEstoque.Produto!, semEstoque.Quantidade);
    }

    private void CalcularValores(Pedido pedido, int opcaoEntregaId)
    {
        OpcaoEntrega opcao = _opcoesEntrega.ObterPorId(opcaoEntregaId)
            ?? throw new InvalidOperationException("Opção de entrega não encontrada.");

        pedido.ValorFrete = _frete.CalcularFrete(pedido.Itens, opcao);  // mesma regra da etapa Entrega
        pedido.AplicarDesconto(_desconto);                              // regra escolhida na fábrica (Strategy)
    }

    /// <summary>LSP: funciona para Pix, Boleto ou Cartão sem saber qual é.</summary>
    private static void RegistrarPagamento(Pedido pedido, Pagamento pagamento)
    {
        pagamento.Valor = pedido.CalcularTotal();   // subtotal + frete - desconto
        pagamento.Processar();                      // gera código Pix, boleto ou valida o cartão
        pedido.Pagamento = pagamento;               // liga o pagamento ao pedido (1×1)
    }

    private static void BaixarEstoque(Pedido pedido)
    {
        foreach (PedidoItem item in pedido.Itens)
            item.Produto!.BaixarEstoque(item.Quantidade); // "!" = o produto sempre existe (preenchido em MontarPedido)
    }
}
