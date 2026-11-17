namespace LojaVirtual.Model.Excecoes;

/// <summary>Lançada pelo AutenticacaoService quando e-mail ou senha não conferem.</summary>
public class LoginInvalidoException : Exception
{
    public LoginInvalidoException() : base("E-mail ou senha inválidos.")
    {
    }
}
