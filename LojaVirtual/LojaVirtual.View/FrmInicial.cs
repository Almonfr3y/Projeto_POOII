using LojaVirtual.Controller;         // para usar os controladores
using LojaVirtual.Model.Entidades;    // para usar Categoria e Produto
using LojaVirtual.Model.Excecoes;     // para usar EstoqueInsuficienteException

namespace LojaVirtual.View;

// Tela Inicial: busca, categorias, lista de peças e botões Carrinho, Favoritos e Meus Pedidos.
public partial class FrmInicial : Form
{
    private SessaoDoUsuario _sessao;                                    // cliente logado e carrinho
    private CatalogoController _catalogo = new CatalogoController();    // busca categorias e produtos
    private FavoritosController _favoritos = new FavoritosController(); // para o botão Favoritar

    // Construtor: recebe a sessão criada depois do login.
    public FrmInicial(SessaoDoUsuario sessao)
    {
        _sessao = sessao;
        InitializeComponent();   // cria os controles desenhados no Designer
    }

    // Executa quando a tela abre.
    private void FrmInicial_Load(object sender, EventArgs e)
    {
        lblCliente.Text = "Olá, " + _sessao.Cliente.Nome;   // saudação com o nome do cliente

        // Primeira opção do filtro é "(todas)", depois as categorias do banco.
        cmbCategoria.Items.Add("(todas)");
        foreach (Categoria categoria in _catalogo.ListarCategorias())
            cmbCategoria.Items.Add(categoria);   // o ComboBox mostra o ToString() (o nome)
        cmbCategoria.SelectedIndex = 0;   // isso já dispara o evento que carrega a lista
    }

    // Clique em "Buscar".
    private void BtnBuscar_Click(object sender, EventArgs e)
    {
        AtualizarLista();
    }

    // Trocou a categoria no ComboBox.
    private void CmbCategoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        AtualizarLista();
    }

    // Clique em "Adicionar ao carrinho".
    private void BtnAdicionar_Click(object sender, EventArgs e)
    {
        Produto? produto = ProdutoSelecionado();   // produto da linha selecionada
        if (produto == null)                       // nenhuma linha selecionada
            return;

        try   // tenta adicionar; se faltar estoque, cai no catch
        {
            _sessao.Carrinho.Adicionar(produto, 1);   // 1 unidade por clique
            MessageBox.Show("\"" + produto.Nome + "\" adicionado ao carrinho.");
        }
        catch (EstoqueInsuficienteException ex)   // só trata a falta de estoque
        {
            MessageBox.Show(ex.Message);          // mostra a mensagem da exceção
        }
    }

    // Clique em "Favoritar".
    private void BtnFavoritar_Click(object sender, EventArgs e)
    {
        Produto? produto = ProdutoSelecionado();
        if (produto == null)
            return;

        _favoritos.Favoritar(_sessao.Cliente.Id, produto.Id);   // grava em tb_favorito
        MessageBox.Show("\"" + produto.Nome + "\" adicionado aos favoritos.");
    }

    // Clique em "Carrinho": abre a tela do carrinho.
    private void BtnCarrinho_Click(object sender, EventArgs e)
    {
        FrmCarrinho tela = new FrmCarrinho(_sessao);   // passa a mesma sessão
        tela.ShowDialog();                             // ShowDialog: espera a tela fechar
        AtualizarLista();   // o estoque pode ter mudado depois de uma compra
    }

    // Clique em "Favoritos": abre a tela de favoritos.
    private void BtnFavoritos_Click(object sender, EventArgs e)
    {
        FrmFavoritos tela = new FrmFavoritos(_sessao);
        tela.ShowDialog();
    }

    // Clique em "Meus Pedidos": abre a tela de pedidos.
    private void BtnMeusPedidos_Click(object sender, EventArgs e)
    {
        FrmMeusPedidos tela = new FrmMeusPedidos(_sessao);
        tela.ShowDialog();
    }

    // Busca os produtos no banco (com os filtros) e mostra na grade.
    private void AtualizarLista()
    {
        // Se a categoria escolhida for "(todas)", passa 0.
        int categoriaId = 0;
        Categoria? categoria = cmbCategoria.SelectedItem as Categoria;   // "as": converte, ou null se for o "(todas)"
        if (categoria != null)
            categoriaId = categoria.Id;

        List<Produto> produtos = _catalogo.ListarProdutos(categoriaId, txtBusca.Text);   // pede ao controlador

        // A grade mostra só algumas colunas. O Id fica escondido e é usado nos botões.
        // Select cria um objeto anônimo por produto; o nome de cada campo vira o título da coluna.
        dgvProdutos.DataSource = produtos.Select(p => new
        {
            p.Id,
            Peça = p.Nome,
            Categoria = p.Categoria == null ? "" : p.Categoria.Nome,   // "?:" = if/else numa linha
            Especificações = p.Especificacoes,
            Preço = p.Preco.ToString("C"),   // "C" = formato de moeda (R$)
            p.Estoque
        }).ToList();
        dgvProdutos.Columns["Id"].Visible = false;   // esconde a coluna Id
    }

    // Busca no banco o produto da linha selecionada na grade.
    private Produto? ProdutoSelecionado()
    {
        if (dgvProdutos.CurrentRow == null)   // nenhuma linha selecionada
            return null;

        int produtoId = (int)dgvProdutos.CurrentRow.Cells["Id"].Value;   // lê o Id escondido da linha
        return _catalogo.BuscarProduto(produtoId);                       // busca o produto atualizado
    }
}
