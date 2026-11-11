namespace LojaVirtual.Model.Servicos;

public interface IAutenticacaoService
{
    ClienteLogado Entrar(string email, string senha);
}
