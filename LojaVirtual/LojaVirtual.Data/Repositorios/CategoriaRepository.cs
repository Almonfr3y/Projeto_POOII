using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Repositorios;

namespace LojaVirtual.Data.Repositorios;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly LojaDbContext _contexto;

    public CategoriaRepository(LojaDbContext contexto) => _contexto = contexto;

    public IReadOnlyList<Categoria> Listar() => _contexto.Categorias.OrderBy(c => c.Id).ToList();
}
