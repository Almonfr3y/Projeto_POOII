namespace LojaVirtual.Model.Entidades;

public class PessoaFisica : Pessoa
{
    public string Cpf { get; set; } = string.Empty;

    public override string DescreverDocumento() => $"CPF {Cpf}";
}
