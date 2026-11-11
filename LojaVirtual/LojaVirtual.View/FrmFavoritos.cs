using LojaVirtual.Controller;

namespace LojaVirtual.View;

public partial class FrmFavoritos : Form
{
    private readonly FavoritosController _favoritos;
    private readonly SessaoDoUsuario _sessao;

    public FrmFavoritos(FavoritosController favoritos, SessaoDoUsuario sessao)
    {
        _favoritos = favoritos;
        _sessao = sessao;
        InitializeComponent();
    }

    // TODO (grupo): Caixa "pesquise favorito" + lista de _favoritos.Listar(_sessao.Cliente.Id, texto);
    // botões para desfavoritar e para adicionar ao carrinho (_sessao.Carrinho.Adicionar).
}
