using LojaVirtual.Model.Servicos;

namespace LojaVirtual.Controller;

/// <summary>Tela de Log In.</summary>
public class ContaController
{
    private readonly IAutenticacaoService _autenticacao;

    public ContaController(IAutenticacaoService autenticacao) => _autenticacao = autenticacao;

    public ClienteLogado Entrar(string email, string senha) => _autenticacao.Entrar(email, senha);
}
