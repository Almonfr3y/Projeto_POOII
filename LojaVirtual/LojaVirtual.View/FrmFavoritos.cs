using System.ComponentModel;
using LojaVirtual.Controller;
using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Excecoes;

namespace LojaVirtual.View;

/// <summary>Tela "Favoritos": "pesquise favorito", desfavoritar e mandar para o carrinho.</summary>
public partial class FrmFavoritos : Form
{
    private const int QuantidadePadrao = 1;

    private readonly FavoritosController _favoritos;
    private readonly SessaoDoUsuario _sessao;

    public FrmFavoritos(FavoritosController favoritos, SessaoDoUsuario sessao)
    {
        _favoritos = favoritos;
        _sessao = sessao;
        InitializeComponent();
    }

    private void FrmFavoritos_Load(object sender, EventArgs e) => AtualizarLista();

    private void BtnBuscar_Click(object sender, EventArgs e) => AtualizarLista();

    private void BtnDesfavoritar_Click(object sender, EventArgs e)
    {
        if (FavoritoSelecionado() is not LinhaFavorito favorito)
            return;

        _favoritos.Desfavoritar(_sessao.Cliente.Id, favorito.ProdutoId);
        AtualizarLista(); // some da lista na hora
    }

    private void BtnAdicionar_Click(object sender, EventArgs e)
    {
        if (FavoritoSelecionado() is not LinhaFavorito favorito)
            return;

        try
        {
            _sessao.Carrinho.Adicionar(favorito.ProdutoId, QuantidadePadrao);
            Mensagem.Info(this, $"\"{favorito.Nome}\" adicionado ao carrinho.");
        }
        catch (EstoqueInsuficienteException ex)
        {
            Mensagem.Aviso(this, ex.Message);
        }
    }

    private LinhaFavorito? FavoritoSelecionado() => dgvFavoritos.CurrentRow?.DataBoundItem as LinhaFavorito;

    private void AtualizarLista() =>
        dgvFavoritos.DataSource = _favoritos.Listar(_sessao.Cliente.Id, txtBusca.Text)
            .Select(LinhaFavorito.De)
            .ToList();

    /// <summary>O que a grade mostra: só as colunas que interessam ao cliente.</summary>
    private sealed record LinhaFavorito(
        [property: Browsable(false)] int ProdutoId,
        [property: DisplayName("Peça")] string Nome,
        [property: DisplayName("Especificações")] string Especificacoes,
        [property: DisplayName("Preço")] string Preco,
        [property: DisplayName("Estoque")] int Estoque)
    {
        public static LinhaFavorito De(Produto produto) =>
            new(produto.Id, produto.Nome, produto.Especificacoes, produto.Preco.ToString("C"), produto.Estoque);
    }
}
