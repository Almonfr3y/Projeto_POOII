using LojaVirtual.Data;
using LojaVirtual.Model.Entidades;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Controller;

// Controlador da Tela Inicial: categorias e lista de peças.
public class CatalogoController
{
    public List<Categoria> ListarCategorias()
    {
        using (LojaDbContext db = new LojaDbContext())
        {
            return db.Categorias.OrderBy(c => c.Id).ToList();
        }
    }

    // categoriaId = 0 mostra todas as categorias.
    public List<Produto> ListarProdutos(int categoriaId, string textoBusca)
    {
        using (LojaDbContext db = new LojaDbContext())
        {
            IQueryable<Produto> consulta = db.Produtos.Include(p => p.Categoria);

            if (categoriaId != 0)
                consulta = consulta.Where(p => p.CategoriaId == categoriaId);

            if (textoBusca != "")
                consulta = consulta.Where(p => p.Nome.Contains(textoBusca));

            return consulta.OrderBy(p => p.Nome).ToList();
        }
    }

    public Produto? BuscarProduto(int produtoId)
    {
        using (LojaDbContext db = new LojaDbContext())
        {
            return db.Produtos.Find(produtoId);
        }
    }
}
