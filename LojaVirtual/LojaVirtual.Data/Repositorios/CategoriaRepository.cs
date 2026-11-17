using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Repositorios;

namespace LojaVirtual.Data.Repositorios;

/// <summary>Categorias do filtro da Tela Inicial (Hardware, Periféricos, PCs).</summary>
public class CategoriaRepository : ICategoriaRepository
{
    private readonly LojaDbContext _contexto;

    public CategoriaRepository(LojaDbContext contexto) => _contexto = contexto;

    // Ordem do Id = mesma ordem do desenho de telas.
    public IReadOnlyList<Categoria> Listar() => _contexto.Categorias.OrderBy(c => c.Id).ToList();
}
