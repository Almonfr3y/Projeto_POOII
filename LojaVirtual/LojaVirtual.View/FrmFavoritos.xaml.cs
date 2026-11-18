using System.Windows;                 // classes do WPF (Window, MessageBox...)
using LojaVirtual.Controller;         // para usar os controladores
using LojaVirtual.Model.Entidades;    // para usar Produto
using LojaVirtual.Model.Excecoes;     // para usar EstoqueInsuficienteException

namespace LojaVirtual.View;

// Tela Favoritos: pesquisar, desfavoritar e mandar para o carrinho.
public partial class FrmFavoritos : Window
{
    private SessaoDoUsuario _sessao;                                     // cliente logado e carrinho
    private FavoritosController _favoritos = new FavoritosController();  // listar/desfavoritar
    private CatalogoController _catalogo = new CatalogoController();     // buscar o produto para o carrinho
    private List<Produto> _favoritosDaTela = new List<Produto>();        // produtos na mesma ordem da grade

    public FrmFavoritos(SessaoDoUsuario sessao)
    {
        _sessao = sessao;
        InitializeComponent();
    }

    // Executa quando a tela abre.
    private void FrmFavoritos_Loaded(object sender, RoutedEventArgs e)
    {
        AtualizarLista();
    }

    // Clique em "Buscar".
    private void BtnBuscar_Click(object sender, RoutedEventArgs e)
    {
        AtualizarLista();
    }

    // Clique em "Desfavoritar".
    private void BtnDesfavoritar_Click(object sender, RoutedEventArgs e)
    {
        if (dgFavoritos.SelectedIndex < 0)   // -1 = nenhuma linha selecionada
            return;

        int produtoId = _favoritosDaTela[dgFavoritos.SelectedIndex].Id;   // a linha está na mesma posição da lista
        _favoritos.Desfavoritar(_sessao.Cliente.Id, produtoId);           // apaga de tb_favorito
        AtualizarLista();                                                 // o produto some da lista
    }

    // Clique em "Adicionar ao carrinho".
    private void BtnAdicionar_Click(object sender, RoutedEventArgs e)
    {
        if (dgFavoritos.SelectedIndex < 0)
            return;

        int produtoId = _favoritosDaTela[dgFavoritos.SelectedIndex].Id;
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
        _favoritosDaTela = _favoritos.Listar(_sessao.Cliente.Id, txtBusca.Text);

        // Objeto anônimo: só as colunas que interessam. O nome do campo vira o título da coluna.
        dgFavoritos.ItemsSource = _favoritosDaTela.Select(p => new
        {
            Peça = p.Nome,
            Especificações = p.Especificacoes,
            Preço = p.Preco.ToString("C"),
            p.Estoque
        }).ToList();
    }
}
