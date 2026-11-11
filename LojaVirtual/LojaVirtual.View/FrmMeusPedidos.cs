using LojaVirtual.Controller;

namespace LojaVirtual.View;

public partial class FrmMeusPedidos : Form
{
    private readonly MeusPedidosController _meusPedidos;
    private readonly SessaoDoUsuario _sessao;

    public FrmMeusPedidos(MeusPedidosController meusPedidos, SessaoDoUsuario sessao)
    {
        _meusPedidos = meusPedidos;
        _sessao = sessao;
        InitializeComponent();
    }

    // TODO (grupo): DataGridView com Nº, Data, Itens, Forma de pagamento e R$ a partir de
    // _meusPedidos.Listar(_sessao.Cliente.Id) — esta é a consulta em SQL puro.
}
