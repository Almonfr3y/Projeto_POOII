using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Repositorios;

public interface IClienteRepository
{
    /// <summary>Usado no login.</summary>
    Pessoa? ObterPorEmail(string email);

    /// <summary>Carrega também endereços e favoritos.</summary>
    Pessoa? ObterComDetalhes(int id);
}
