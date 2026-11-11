using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Excecoes;
using LojaVirtual.Model.Repositorios;

namespace LojaVirtual.Model.Servicos;

public class AutenticacaoService : IAutenticacaoService
{
    private readonly IClienteRepository _clientes;

    public AutenticacaoService(IClienteRepository clientes) => _clientes = clientes;

    public ClienteLogado Entrar(string email, string senha)
    {
        Pessoa? cliente = _clientes.ObterPorEmail(email.Trim());
        if (cliente is null || !cliente.SenhaConfere(senha))
            throw new LoginInvalidoException();

        return new ClienteLogado(cliente.Id, cliente.Nome);
    }
}
