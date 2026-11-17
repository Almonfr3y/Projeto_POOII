using LojaVirtual.Controller;
using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Excecoes;

namespace LojaVirtual.View;

// Tela Inicial: busca, categorias, lista de peças e botões Carrinho, Favoritos e Meus Pedidos.
public partial class FrmInicial : Form
{
    private SessaoDoUsuario _sessao;
    private CatalogoController _catalogo = new CatalogoController();
    private FavoritosController _favoritos = new FavoritosController();

    public FrmInicial(SessaoDoUsuario sessao)
    {
        _sessao = sessao;
        InitializeComponent();
    }

    private void FrmInicial_Load(object sender, EventArgs e)
    {
        lblCliente.Text = "Olá, " + _sessao.Cliente.Nome;

        // Primeira opção do filtro é "(todas)", depois as categorias do banco.
        cmbCategoria.Items.Add("(todas)");
        foreach (Categoria categoria in _catalogo.ListarCategorias())
            cmbCategoria.Items.Add(categoria);
        cmbCategoria.SelectedIndex = 0;   // isso já dispara o evento que carrega a lista
    }

    private void BtnBuscar_Click(object sender, EventArgs e)
    {
        AtualizarLista();
    }

    private void CmbCategoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        AtualizarLista();
    }

    private void BtnAdicionar_Click(object sender, EventArgs e)
    {
        Produto? produto = ProdutoSelecionado();
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

    private void BtnFavoritar_Click(object sender, EventArgs e)
    {
        Produto? produto = ProdutoSelecionado();
        if (produto == null)
            return;

        _favoritos.Favoritar(_sessao.Cliente.Id, produto.Id);
        MessageBox.Show("\"" + produto.Nome + "\" adicionado aos favoritos.");
    }

    private void BtnCarrinho_Click(object sender, EventArgs e)
    {
        FrmCarrinho tela = new FrmCarrinho(_sessao);
        tela.ShowDialog();
        AtualizarLista();   // o estoque pode ter mudado depois de uma compra
    }

    private void BtnFavoritos_Click(object sender, EventArgs e)
    {
        FrmFavoritos tela = new FrmFavoritos(_sessao);
        tela.ShowDialog();
    }

    private void BtnMeusPedidos_Click(object sender, EventArgs e)
    {
        FrmMeusPedidos tela = new FrmMeusPedidos(_sessao);
        tela.ShowDialog();
    }

    private void AtualizarLista()
    {
        // Se a categoria escolhida for "(todas)", passa 0.
        int categoriaId = 0;
        Categoria? categoria = cmbCategoria.SelectedItem as Categoria;
        if (categoria != null)
            categoriaId = categoria.Id;

        List<Produto> produtos = _catalogo.ListarProdutos(categoriaId, txtBusca.Text);

        // A grade mostra só algumas colunas. O Id fica escondido e é usado nos botões.
        dgvProdutos.DataSource = produtos.Select(p => new
        {
            p.Id,
            Peça = p.Nome,
            Categoria = p.Categoria == null ? "" : p.Categoria.Nome,
            Especificações = p.Especificacoes,
            Preço = p.Preco.ToString("C"),
            p.Estoque
        }).ToList();
        dgvProdutos.Columns["Id"].Visible = false;
    }

    // Busca no banco o produto da linha selecionada na grade.
    private Produto? ProdutoSelecionado()
    {
        if (dgvProdutos.CurrentRow == null)
            return null;

        int produtoId = (int)dgvProdutos.CurrentRow.Cells["Id"].Value;
        return _catalogo.BuscarProduto(produtoId);
    }
}
