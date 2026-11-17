using LojaVirtual.Controller;
using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Excecoes;

namespace LojaVirtual.View;

// Tela Favoritos: pesquisar, desfavoritar e mandar para o carrinho.
public partial class FrmFavoritos : Form
{
    private SessaoDoUsuario _sessao;
    private FavoritosController _favoritos = new FavoritosController();
    private CatalogoController _catalogo = new CatalogoController();

    public FrmFavoritos(SessaoDoUsuario sessao)
    {
        _sessao = sessao;
        InitializeComponent();
    }

    private void FrmFavoritos_Load(object sender, EventArgs e)
    {
        AtualizarLista();
    }

    private void BtnBuscar_Click(object sender, EventArgs e)
    {
        AtualizarLista();
    }

    private void BtnDesfavoritar_Click(object sender, EventArgs e)
    {
        if (dgvFavoritos.CurrentRow == null)
            return;

        int produtoId = (int)dgvFavoritos.CurrentRow.Cells["Id"].Value;
        _favoritos.Desfavoritar(_sessao.Cliente.Id, produtoId);
        AtualizarLista();
    }

    private void BtnAdicionar_Click(object sender, EventArgs e)
    {
        if (dgvFavoritos.CurrentRow == null)
            return;

        int produtoId = (int)dgvFavoritos.CurrentRow.Cells["Id"].Value;
        Produto? produto = _catalogo.BuscarProduto(produtoId);
        if (produto == null)
            return;

        try
        {
            _sessao.Carrinho.Adicionar(produto, 1);
            MessageBox.Show("\"" + produto.Nome + "\" adicionado ao carrinho.");
        }
        catch (EstoqueInsuficienteException ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void AtualizarLista()
    {
        List<Produto> favoritos = _favoritos.Listar(_sessao.Cliente.Id, txtBusca.Text);

        dgvFavoritos.DataSource = favoritos.Select(p => new
        {
            p.Id,
            Peça = p.Nome,
            Especificações = p.Especificacoes,
            Preço = p.Preco.ToString("C"),
            p.Estoque
        }).ToList();
        dgvFavoritos.Columns["Id"].Visible = false;
    }
}
