using LojaVirtual.Controller;         // para usar os controladores
using LojaVirtual.Model.Entidades;    // para usar Produto
using LojaVirtual.Model.Excecoes;     // para usar EstoqueInsuficienteException

namespace LojaVirtual.View;

// Tela Favoritos: pesquisar, desfavoritar e mandar para o carrinho.
public partial class FrmFavoritos : Form
{
    private SessaoDoUsuario _sessao;                                     // cliente logado e carrinho
    private FavoritosController _favoritos = new FavoritosController();  // listar/desfavoritar
    private CatalogoController _catalogo = new CatalogoController();     // buscar o produto para o carrinho

    public FrmFavoritos(SessaoDoUsuario sessao)
    {
        _sessao = sessao;
        InitializeComponent();
    }

    // Executa quando a tela abre.
    private void FrmFavoritos_Load(object sender, EventArgs e)
    {
        AtualizarLista();
    }

    // Clique em "Buscar".
    private void BtnBuscar_Click(object sender, EventArgs e)
    {
        AtualizarLista();
    }

    // Clique em "Desfavoritar".
    private void BtnDesfavoritar_Click(object sender, EventArgs e)
    {
        if (dgvFavoritos.CurrentRow == null)   // nenhuma linha selecionada
            return;

        int produtoId = (int)dgvFavoritos.CurrentRow.Cells["Id"].Value;   // Id escondido da linha
        _favoritos.Desfavoritar(_sessao.Cliente.Id, produtoId);           // apaga de tb_favorito
        AtualizarLista();                                                 // o produto some da lista
    }

    // Clique em "Adicionar ao carrinho".
    private void BtnAdicionar_Click(object sender, EventArgs e)
    {
        if (dgvFavoritos.CurrentRow == null)
            return;

        int produtoId = (int)dgvFavoritos.CurrentRow.Cells["Id"].Value;
        Produto? produto = _catalogo.BuscarProduto(produtoId);   // estoque atual do banco
        if (produto == null)
            return;

        try
        {
            _sessao.Carrinho.Adicionar(produto, 1);   // 1 unidade
            MessageBox.Show("\"" + produto.Nome + "\" adicionado ao carrinho.");
        }
        catch (EstoqueInsuficienteException ex)   // faltou estoque
        {
            MessageBox.Show(ex.Message);
        }
    }

    // Carrega os favoritos (com o filtro da busca) na grade.
    private void AtualizarLista()
    {
        List<Produto> favoritos = _favoritos.Listar(_sessao.Cliente.Id, txtBusca.Text);

        // Objeto anônimo: só as colunas que interessam. O nome do campo vira o título da coluna.
        dgvFavoritos.DataSource = favoritos.Select(p => new
        {
            p.Id,
            Peça = p.Nome,
            Especificações = p.Especificacoes,
            Preço = p.Preco.ToString("C"),
            p.Estoque
        }).ToList();
        dgvFavoritos.Columns["Id"].Visible = false;   // esconde o Id
    }
}
