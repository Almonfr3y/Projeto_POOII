using LojaVirtual.Controller;
using LojaVirtual.Controller.Composicao;

namespace LojaVirtual.View;

public partial class FrmCarrinho : Form
{
    private readonly SessaoDoUsuario _sessao;
    private readonly FabricaDeControladores _fabrica;

    public FrmCarrinho(SessaoDoUsuario sessao, FabricaDeControladores fabrica)
    {
        _sessao = sessao;
        _fabrica = fabrica;
        InitializeComponent();
    }

    // TODO (grupo): Itens de _sessao.Carrinho.Carrinho (produto, quantidade, R$), alterar/remover e subtotal.
    // "Finalizar Compra" abre new FrmCheckout(_fabrica.CriarPedidoController(), _sessao).
}
