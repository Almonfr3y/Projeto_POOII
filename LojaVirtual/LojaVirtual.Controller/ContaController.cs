using LojaVirtual.Data;
using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Controller;

// Controlador da tela de login.
public class ContaController
{
    // Devolve o cliente se o e-mail e a senha estiverem certos, ou null se não estiverem.
    public Pessoa? Entrar(string email, string senha)
    {
        using (LojaDbContext db = new LojaDbContext())
        {
            // O EF já devolve um PessoaFisica ou um PessoaJuridica, conforme a coluna TipoPessoa.
            Pessoa? cliente = db.Pessoas.FirstOrDefault(p => p.Email == email.Trim());

            if (cliente == null || !cliente.SenhaConfere(senha))
                return null;

            return cliente;
        }
    }
}
