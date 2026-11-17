using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Excecoes;
using LojaVirtual.Model.Repositorios;

namespace LojaVirtual.Model.Servicos;

/// <summary>Regra do login. Não sabe nada de banco: recebe o repositório pela interface (DIP).</summary>
public class AutenticacaoService : IAutenticacaoService
{
    private readonly IClienteRepository _clientes;

    public AutenticacaoService(IClienteRepository clientes) => _clientes = clientes;

    public Pessoa Entrar(string email, string senha)
    {
        Pessoa? cliente = _clientes.ObterPorEmail(email.Trim()); // Trim: ignora espaços digitados sem querer

        // Mesma mensagem para e-mail e senha errados: não revela qual dos dois falhou.
        if (cliente is null || !cliente.SenhaConfere(senha))
            throw new LoginInvalidoException();

        return cliente;
    }
}
