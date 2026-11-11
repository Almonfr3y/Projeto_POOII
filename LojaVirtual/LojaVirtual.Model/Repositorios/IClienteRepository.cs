using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Repositorios;

public interface IClienteRepository
{
    Pessoa? ObterPorEmail(string email);

    /// <summary>Carrega também endereços e favoritos.</summary>
    Pessoa? ObterComDetalhes(int id);
}
