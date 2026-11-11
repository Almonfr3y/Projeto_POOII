using LojaVirtual.Controller;

namespace LojaVirtual.View;

public partial class FrmCheckout : Form
{
    private readonly PedidoController _pedidos;
    private readonly SessaoDoUsuario _sessao;

    public FrmCheckout(PedidoController pedidos, SessaoDoUsuario sessao)
    {
        _pedidos = pedidos;
        _sessao = sessao;
        InitializeComponent();
    }

    // TODO (grupo): Assistente em etapas, como no desenho: Endereço (ListarEnderecos) → Entrega (ListarOpcoesEntrega
    // + SimularFrete de cada opção) → Pagamento (criar PagamentoPix, PagamentoBoleto ou PagamentoCartao)
    // → Revisão (produtos, endereço, frete, total) → Concluído (pedido.Pagamento.Descricao).
    // Montar DadosCheckout e chamar _pedidos.Finalizar(...). Tratar EstoqueInsuficienteException.
}
