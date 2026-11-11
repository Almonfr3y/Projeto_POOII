using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Repositorios;

namespace LojaVirtual.Controller;

/// <summary>Tela Inicial: categorias, busca e lista de peças.</summary>
public class CatalogoController
{
    private readonly IProdutoRepository _produtos;
    private readonly ICategoriaRepository _categorias;

    public CatalogoController(IProdutoRepository produtos, ICategoriaRepository categorias)
    {
        _produtos = produtos;
        _categorias = categorias;
    }

    public IReadOnlyList<Categoria> ListarCategorias() => _categorias.Listar();

    public IReadOnlyList<Produto> ListarProdutos(int? categoriaId, string? textoBusca) =>
        _produtos.Listar(categoriaId, textoBusca);
}
