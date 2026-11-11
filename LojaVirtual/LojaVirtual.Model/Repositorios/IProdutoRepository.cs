using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Repositorios;

public interface IProdutoRepository
{
    IReadOnlyList<Produto> Listar(int? categoriaId = null, string? textoBusca = null);
    Produto? ObterPorId(int id);
}
