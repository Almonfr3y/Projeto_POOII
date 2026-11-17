namespace LojaVirtual.Model.Entidades;

// Cliente pessoa física: herda de Pessoa e tem CPF.
public class PessoaFisica : Pessoa
{
    public string Cpf { get; set; } = "";

    public override string DescreverDocumento()
    {
        return "CPF " + Cpf;
    }
}
