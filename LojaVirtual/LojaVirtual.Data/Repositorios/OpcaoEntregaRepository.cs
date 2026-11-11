using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Repositorios;

namespace LojaVirtual.Data.Repositorios;

public class OpcaoEntregaRepository : IOpcaoEntregaRepository
{
    private readonly LojaDbContext _contexto;

    public OpcaoEntregaRepository(LojaDbContext contexto) => _contexto = contexto;

    public IReadOnlyList<OpcaoEntrega> Listar() => _contexto.OpcoesEntrega.OrderBy(o => o.PrazoDias).ToList();

    public OpcaoEntrega? ObterPorId(int id) => _contexto.OpcoesEntrega.Find(id);
}
