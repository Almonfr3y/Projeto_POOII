using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Servicos;

namespace LojaVirtual.Controller;

/// <summary>
/// Tela de Log In. Os controladores só ligam a tela às regras do Model: não têm regra de negócio
/// nem acesso direto ao banco (MVC).
/// </summary>
public class ContaController
{
    private readonly IAutenticacaoService _autenticacao;

    public ContaController(IAutenticacaoService autenticacao) => _autenticacao = autenticacao;

    // Devolve o cliente logado ou lança LoginInvalidoException (a tela mostra a mensagem).
    public Pessoa Entrar(string email, string senha) => _autenticacao.Entrar(email, senha);
}
