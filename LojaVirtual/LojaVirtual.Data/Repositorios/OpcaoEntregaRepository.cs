using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Repositorios;

namespace LojaVirtual.Data.Repositorios;

/// <summary>Opções da etapa Entrega do checkout (Econômica, Padrão, Expressa).</summary>
public class OpcaoEntregaRepository : IOpcaoEntregaRepository
{
    private readonly LojaDbContext _contexto;

    public OpcaoEntregaRepository(LojaDbContext contexto) => _contexto = contexto;

    // Da mais lenta para a mais rápida.
    public IReadOnlyList<OpcaoEntrega> Listar() => _contexto.OpcoesEntrega.OrderBy(o => o.PrazoDias).ToList();

    // Find busca pela chave primária (e reaproveita o objeto se já estiver carregado).
    public OpcaoEntrega? ObterPorId(int id) => _contexto.OpcoesEntrega.Find(id);
}
