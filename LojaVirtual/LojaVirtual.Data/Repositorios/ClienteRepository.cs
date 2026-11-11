using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Data.Repositorios;

public class ClienteRepository : IClienteRepository
{
    private readonly LojaDbContext _contexto;

    public ClienteRepository(LojaDbContext contexto) => _contexto = contexto;

    public Pessoa? ObterPorEmail(string email) => _contexto.Pessoas.FirstOrDefault(c => c.Email == email);

    public Pessoa? ObterComDetalhes(int id) =>
        _contexto.Pessoas
            .Include(c => c.Enderecos)
            .Include(c => c.Favoritos).ThenInclude(p => p.Categoria)
            .FirstOrDefault(c => c.Id == id);
}
