using LojaVirtual.Model.Checkout;
using LojaVirtual.Model.Descontos;
using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Repositorios;

namespace LojaVirtual.Model.Servicos;

/// <summary>SRP: concentra a regra de finalização. DIP: depende só de interfaces.</summary>
public class PedidoService : IPedidoService
{
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

    public Pedido FinalizarPedido(DadosCheckout dados, IReadOnlyList<PedidoItem> itensDoCarrinho)
    {
        // TODO (grupo): implementar em métodos pequenos (exemplo de Clean Code).
        // Sugestão de passos, cada um em um método privado:
        //  1. Montar o Pedido (ClienteId, EnderecoEntregaId, OpcaoEntregaId, Data = DateTime.Now) com itens
        //     novos a partir de _produtos.ObterPorId (preço congelado em PrecoUnitario).
        //  2. ValorFrete = _frete.CalcularFrete(itens, _opcoesEntrega.ObterPorId(...)); pedido.AplicarDesconto(_desconto).
        //  3. Para cada item: produto.BaixarEstoque(quantidade)  -> pode lançar EstoqueInsuficienteException.
        //  4. dados.Pagamento.Valor = pedido.CalcularTotal(); dados.Pagamento.Processar(); pedido.Pagamento = dados.Pagamento.
        //  5. pedido.Finalizar(); _pedidos.Adicionar(pedido); _unidadeDeTrabalho.SalvarAlteracoes()  -> uma transação.
        throw new NotImplementedException();
    }
}
