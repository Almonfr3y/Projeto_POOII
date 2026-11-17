using System.ComponentModel;
using LojaVirtual.Controller;
using LojaVirtual.Controller.Composicao;
using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Excecoes;

namespace LojaVirtual.View;

/// <summary>
/// "Tela Inicial": busca, categorias, peças e os botões Carrinho, Favoritos e Meus Pedidos.
/// A tela só captura cliques e mostra dados; as regras ficam no Controller/Model (MVC).
/// </summary>
public partial class FrmInicial : Form
{
    private const int QuantidadePadrao = 1;              // cada clique em "Adicionar" põe 1 unidade
    private const string TodasAsCategorias = "(todas)";  // primeira opção do filtro

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

        // Filtro: "(todas)" seguido das categorias do banco.
        var categorias = new List<object> { TodasAsCategorias };
        categorias.AddRange(_catalogo.ListarCategorias());
        cmbCategoria.DataSource = categorias;
        AtualizarLista();
    }

    private void BtnBuscar_Click(object sender, EventArgs e) => AtualizarLista();

    private void CmbCategoria_SelectedIndexChanged(object sender, EventArgs e) => AtualizarLista();

    private void BtnAdicionar_Click(object sender, EventArgs e)
    {
        if (ProdutoSelecionado() is not LinhaProduto produto) // nenhuma linha selecionada
            return;

        try
        {
            _sessao.Carrinho.Adicionar(produto.ProdutoId, QuantidadePadrao);
            Mensagem.Info(this, $"\"{produto.Nome}\" adicionado ao carrinho.");
        }
        catch (EstoqueInsuficienteException ex)
        {
            Mensagem.Aviso(this, ex.Message);
        }
    }

    private void BtnFavoritar_Click(object sender, EventArgs e)
    {
        if (ProdutoSelecionado() is not LinhaProduto produto)
            return;

        _fabrica.CriarFavoritosController().Favoritar(_sessao.Cliente.Id, produto.ProdutoId);
        Mensagem.Info(this, $"\"{produto.Nome}\" adicionado aos favoritos.");
    }

    // Botões do topo: cada um abre a sua tela.
    private void BtnCarrinho_Click(object sender, EventArgs e) => Abrir(new FrmCarrinho(_sessao, _fabrica));

    private void BtnFavoritos_Click(object sender, EventArgs e) =>
        Abrir(new FrmFavoritos(_fabrica.CriarFavoritosController(), _sessao));

    private void BtnMeusPedidos_Click(object sender, EventArgs e) =>
        Abrir(new FrmMeusPedidos(_fabrica.CriarMeusPedidosController(), _sessao));

    // A linha selecionada na grade (ou null).
    private LinhaProduto? ProdutoSelecionado() => dgvProdutos.CurrentRow?.DataBoundItem as LinhaProduto;

    private void AtualizarLista()
    {
        if (!IsHandleCreated) // o ComboBox dispara o evento antes da tela terminar de carregar
            return;

        int? categoriaId = (cmbCategoria.SelectedItem as Categoria)?.Id; // "(todas)" não é Categoria → null
        dgvProdutos.DataSource = _catalogo.ListarProdutos(categoriaId, txtBusca.Text)
            .Select(LinhaProduto.De)
            .ToList();
    }

    private void Abrir(Form formulario)
    {
        using (formulario)               // libera a tela da memória ao fechar
            formulario.ShowDialog(this); // ShowDialog: a Tela Inicial espera a outra fechar

        AtualizarLista(); // o estoque pode ter mudado depois de uma compra
    }

    /// <summary>O que a grade mostra: só as colunas que interessam ao cliente.</summary>
    private sealed record LinhaProduto(
        [property: Browsable(false)] int ProdutoId,              // escondido, usado nos botões
        [property: DisplayName("Peça")] string Nome,
        [property: DisplayName("Categoria")] string Categoria,
        [property: DisplayName("Especificações")] string Especificacoes,
        [property: DisplayName("Preço")] string Preco,
        [property: DisplayName("Estoque")] int Estoque)
    {
        public static LinhaProduto De(Produto produto) =>
            new(produto.Id, produto.Nome, produto.Categoria?.Nome ?? "-", produto.Especificacoes,
                produto.Preco.ToString("C"), produto.Estoque); // "C" = formato de moeda (R$)
    }
}
