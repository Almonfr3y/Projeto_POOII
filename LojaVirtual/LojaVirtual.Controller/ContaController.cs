using LojaVirtual.Data;               // para usar LojaDbContext
using LojaVirtual.Model.Entidades;    // para usar Pessoa

namespace LojaVirtual.Controller;

// Controlador da tela de login.
public class ContaController
{
    // Devolve o cliente se o e-mail e a senha estiverem certos, ou null se não estiverem.
    public Pessoa? Entrar(string email, string senha)
    {
        // using: cria a conexão com o banco e fecha sozinho no fim do bloco.
        using (LojaDbContext db = new LojaDbContext())
        {
            // O EF já devolve um PessoaFisica ou um PessoaJuridica, conforme a coluna TipoPessoa.
            // FirstOrDefault = o primeiro que atender a condição, ou null se não houver. Trim tira espaços.
            Pessoa? cliente = db.Pessoas.FirstOrDefault(p => p.Email == email.Trim());

            if (cliente == null || !cliente.SenhaConfere(senha))   // e-mail não existe OU senha errada
                return null;                                       // login inválido

            return cliente;   // login certo: devolve o cliente
        }
    }
}
