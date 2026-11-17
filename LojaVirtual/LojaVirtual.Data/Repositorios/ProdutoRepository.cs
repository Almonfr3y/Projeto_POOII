using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Data.Repositorios;

public class ProdutoRepository : IProdutoRepository
{
    private readonly LojaDbContext _contexto;

    public ProdutoRepository(LojaDbContext contexto) => _contexto = contexto;

    /// <summary>Catálogo com filtro opcional por categoria e por parte do nome.</summary>
    public IReadOnlyList<Produto> Listar(int? categoriaId = null, string? textoBusca = null)
    {
        // AsNoTracking: só leitura, então o EF sempre busca o estoque atual no banco.
        IQueryable<Produto> consulta = _contexto.Produtos.AsNoTracking().Include(p => p.Categoria);

        if (categoriaId.HasValue)                          // filtro da lista de categorias
            consulta = consulta.Where(p => p.CategoriaId == categoriaId.Value);

        if (!string.IsNullOrWhiteSpace(textoBusca))        // filtro da caixa "pesquise aqui"
            consulta = consulta.Where(p => p.Nome.Contains(textoBusca));

        return consulta.OrderBy(p => p.Nome).ToList();     // a consulta só vai ao banco aqui
    }

    // Com rastreamento: mudanças no produto (estoque, favoritos) são gravadas no SaveChanges.
    public Produto? ObterPorId(int id) => _contexto.Produtos.Include(p => p.Categoria).FirstOrDefault(p => p.Id == id);

    // Sem rastreamento: sempre relê do banco (o carrinho usa para ver o estoque atual).
    public Produto? ObterParaConsulta(int id) =>
        _contexto.Produtos.AsNoTracking().Include(p => p.Categoria).FirstOrDefault(p => p.Id == id);
}
