using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Repositorios;

public interface IOpcaoEntregaRepository
{
    IReadOnlyList<OpcaoEntrega> Listar();
    OpcaoEntrega? ObterPorId(int id);
}
