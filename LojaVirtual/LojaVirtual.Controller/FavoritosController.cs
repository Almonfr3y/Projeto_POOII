using LojaVirtual.Data;               // para usar LojaDbContext
using LojaVirtual.Model.Entidades;    // para usar Pessoa e Produto
using Microsoft.EntityFrameworkCore;  // para usar Include

namespace LojaVirtual.Controller;

// Controlador da tela Favoritos (relacionamento NxN entre cliente e produto).
public class FavoritosController
{
    // Lista os favoritos do cliente que contêm o texto da busca (vazio = todos).
    public List<Produto> Listar(int clienteId, string textoBusca)
    {
        using (LojaDbContext db = new LojaDbContext())
        {
            Pessoa cliente = BuscarCliente(db, clienteId);   // cliente com a lista de favoritos

            List<Produto> lista = new List<Produto>();
            foreach (Produto produto in cliente.Favoritos)
            {
                // ToLower nos dois lados: a busca não diferencia maiúsculas de minúsculas.
                if (produto.Nome.ToLower().Contains(textoBusca.ToLower()))
                    lista.Add(produto);
            }

            return lista.OrderBy(p => p.Nome).ToList();   // em ordem alfabética
        }
    }

    // Marca um produto como favorito do cliente.
    public void Favoritar(int clienteId, int produtoId)
    {
        using (LojaDbContext db = new LojaDbContext())
        {
            Pessoa cliente = BuscarCliente(db, clienteId);   // carrega o cliente
            Produto? produto = db.Produtos.Find(produtoId);  // carrega o produto
            if (produto == null)                             // produto não existe: não faz nada
                return;

            cliente.Favoritar(produto);   // a regra (não repetir) fica na classe Pessoa
            db.SaveChanges();   // grava uma linha em tb_favorito
        }
    }

    // Tira um produto dos favoritos do cliente.
    public void Desfavoritar(int clienteId, int produtoId)
    {
        using (LojaDbContext db = new LojaDbContext())
        {
            Pessoa cliente = BuscarCliente(db, clienteId);   // carrega o cliente com os favoritos
            cliente.Desfavoritar(produtoId);                 // remove da lista
            db.SaveChanges();   // apaga a linha de tb_favorito
        }
    }

    // Carrega o cliente junto com a lista de favoritos (Include).
    // First: o primeiro que atende a condição (o cliente logado sempre existe).
    private Pessoa BuscarCliente(LojaDbContext db, int clienteId)
    {
        return db.Pessoas.Include(p => p.Favoritos).First(p => p.Id == clienteId);
    }
}
