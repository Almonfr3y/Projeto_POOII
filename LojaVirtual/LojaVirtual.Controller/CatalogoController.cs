using LojaVirtual.Data;               // para usar LojaDbContext
using LojaVirtual.Model.Entidades;    // para usar Categoria e Produto
using Microsoft.EntityFrameworkCore;  // para usar Include

namespace LojaVirtual.Controller;

// Controlador da Tela Inicial: categorias e lista de peças.
public class CatalogoController
{
    // Lista as categorias para o filtro da tela.
    public List<Categoria> ListarCategorias()
    {
        using (LojaDbContext db = new LojaDbContext())
        {
            return db.Categorias.OrderBy(c => c.Id).ToList();   // ordena pelo Id e transforma em lista
        }
    }

    // categoriaId = 0 mostra todas as categorias.
    public List<Produto> ListarProdutos(int categoriaId, string textoBusca)
    {
        using (LojaDbContext db = new LojaDbContext())
        {
            // Include: traz a categoria junto com cada produto (para mostrar o nome dela na grade).
            IQueryable<Produto> consulta = db.Produtos.Include(p => p.Categoria);

            if (categoriaId != 0)   // escolheu uma categoria: filtra por ela
                consulta = consulta.Where(p => p.CategoriaId == categoriaId);

            if (textoBusca != "")   // digitou algo na busca: filtra pelo nome
                consulta = consulta.Where(p => p.Nome.Contains(textoBusca));

            return consulta.OrderBy(p => p.Nome).ToList();   // a consulta só vai ao banco no ToList
        }
    }

    // Busca um produto pelo Id (usado ao adicionar no carrinho).
    public Produto? BuscarProduto(int produtoId)
    {
        using (LojaDbContext db = new LojaDbContext())
        {
            return db.Produtos.Find(produtoId);   // Find procura pela chave primária; null se não achar
        }
    }
}
