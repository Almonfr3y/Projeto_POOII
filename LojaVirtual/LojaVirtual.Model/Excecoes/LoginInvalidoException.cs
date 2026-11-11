namespace LojaVirtual.Model.Excecoes;

public class LoginInvalidoException : Exception
{
    public LoginInvalidoException() : base("E-mail ou senha inválidos.")
    {
    }
}
