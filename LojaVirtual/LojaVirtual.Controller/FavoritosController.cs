using LojaVirtual.Data;
using LojaVirtual.Model.Entidades;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Controller;

// Controlador da tela Favoritos (relacionamento NxN entre cliente e produto).
public class FavoritosController
{
    public List<Produto> Listar(int clienteId, string textoBusca)
    {
        using (LojaDbContext db = new LojaDbContext())
        {
            Pessoa cliente = BuscarCliente(db, clienteId);

            List<Produto> lista = new List<Produto>();
            foreach (Produto produto in cliente.Favoritos)
            {
                if (produto.Nome.ToLower().Contains(textoBusca.ToLower()))
                    lista.Add(produto);
            }

            return lista.OrderBy(p => p.Nome).ToList();
        }
    }

    public void Favoritar(int clienteId, int produtoId)
    {
        using (LojaDbContext db = new LojaDbContext())
        {
            Pessoa cliente = BuscarCliente(db, clienteId);
            Produto? produto = db.Produtos.Find(produtoId);
            if (produto == null)
                return;

            cliente.Favoritar(produto);
            db.SaveChanges();   // grava uma linha em tb_favorito
        }
    }

    public void Desfavoritar(int clienteId, int produtoId)
    {
        using (LojaDbContext db = new LojaDbContext())
        {
            Pessoa cliente = BuscarCliente(db, clienteId);
            cliente.Desfavoritar(produtoId);
            db.SaveChanges();   // apaga a linha de tb_favorito
        }
    }

    // Carrega o cliente junto com a lista de favoritos (Include).
    private Pessoa BuscarCliente(LojaDbContext db, int clienteId)
    {
        return db.Pessoas.Include(p => p.Favoritos).First(p => p.Id == clienteId);
    }
}
