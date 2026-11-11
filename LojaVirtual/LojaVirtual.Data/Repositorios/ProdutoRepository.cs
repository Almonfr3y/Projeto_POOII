using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Data.Repositorios;

public class ProdutoRepository : IProdutoRepository
{
    private readonly LojaDbContext _contexto;

    public ProdutoRepository(LojaDbContext contexto) => _contexto = contexto;

    public IReadOnlyList<Produto> Listar(int? categoriaId = null, string? textoBusca = null)
    {
        IQueryable<Produto> consulta = _contexto.Produtos.Include(p => p.Categoria);

        if (categoriaId.HasValue)
            consulta = consulta.Where(p => p.CategoriaId == categoriaId.Value);

        if (!string.IsNullOrWhiteSpace(textoBusca))
            consulta = consulta.Where(p => p.Nome.Contains(textoBusca));

        return consulta.OrderBy(p => p.Nome).ToList();
    }

    public Produto? ObterPorId(int id) => _contexto.Produtos.Include(p => p.Categoria).FirstOrDefault(p => p.Id == id);
}
