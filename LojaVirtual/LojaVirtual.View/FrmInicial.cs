using LojaVirtual.Controller;
using LojaVirtual.Controller.Composicao;
using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Excecoes;

namespace LojaVirtual.View;

/// <summary>
/// "Tela Inicial" do desenho: busca, categorias, peças e os atalhos 🛒 ⭐ Meus Pedidos.
/// Exemplo do fluxo View → Controller: o formulário só captura e exibe dados (SRP).
/// </summary>
public partial class FrmInicial : Form
{
    private const int QuantidadePadrao = 1;
    private const string TodasAsCategorias = "(todas)";

    private readonly FabricaDeControladores _fabrica;
    private readonly SessaoDoUsuario _sessao;
    private readonly CatalogoController _catalogo;

    public FrmInicial(FabricaDeControladores fabrica, SessaoDoUsuario sessao)
    {
        _fabrica = fabrica;
        _sessao = sessao;
        _catalogo = fabrica.CriarCatalogoController();
        InitializeComponent();
    }

    private void FrmInicial_Load(object sender, EventArgs e)
    {
        lblCliente.Text = $"Olá, {_sessao.Cliente.Nome}";

        var categorias = new List<object> { TodasAsCategorias };
        categorias.AddRange(_catalogo.ListarCategorias());
        cmbCategoria.DataSource = categorias;
        AtualizarLista();
    }

    private void BtnBuscar_Click(object sender, EventArgs e) => AtualizarLista();

    private void CmbCategoria_SelectedIndexChanged(object sender, EventArgs e) => AtualizarLista();

    private void BtnAdicionar_Click(object sender, EventArgs e)
    {
        if (ProdutoSelecionado() is not Produto produto)
            return;

        try
        {
            _sessao.Carrinho.Adicionar(produto.Id, QuantidadePadrao);
            MessageBox.Show($"\"{produto.Nome}\" adicionado ao carrinho.", Text);
        }
        catch (EstoqueInsuficienteException ex)
        {
            MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnFavoritar_Click(object sender, EventArgs e)
    {
        if (ProdutoSelecionado() is not Produto produto)
            return;

        _fabrica.CriarFavoritosController().Favoritar(_sessao.Cliente.Id, produto.Id);
        MessageBox.Show($"\"{produto.Nome}\" adicionado aos favoritos.", Text);
    }

    private void BtnCarrinho_Click(object sender, EventArgs e) => Abrir(new FrmCarrinho(_sessao, _fabrica));

    private void BtnFavoritos_Click(object sender, EventArgs e) =>
        Abrir(new FrmFavoritos(_fabrica.CriarFavoritosController(), _sessao));

    private void BtnMeusPedidos_Click(object sender, EventArgs e) =>
        Abrir(new FrmMeusPedidos(_fabrica.CriarMeusPedidosController(), _sessao));

    private Produto? ProdutoSelecionado() => dgvProdutos.CurrentRow?.DataBoundItem as Produto;

    private void AtualizarLista()
    {
        if (!IsHandleCreated)
            return;

        int? categoriaId = (cmbCategoria.SelectedItem as Categoria)?.Id;
        dgvProdutos.DataSource = _catalogo.ListarProdutos(categoriaId, txtBusca.Text).ToList();
    }

    private void Abrir(Form formulario)
    {
        using (formulario)
        {
            formulario.ShowDialog(this);
        }
    }
}
