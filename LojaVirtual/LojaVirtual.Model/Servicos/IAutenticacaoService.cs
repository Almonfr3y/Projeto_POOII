using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Servicos;

public interface IAutenticacaoService
{
    /// <summary>Devolve o cliente (PF ou PJ) se e-mail e senha conferem; senão lança LoginInvalidoException.</summary>
    Pessoa Entrar(string email, string senha);
}
