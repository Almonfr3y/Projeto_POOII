namespace LojaVirtual.Model.Entidades;

/// <summary>Cliente pessoa física: herda tudo de Pessoa e acrescenta o CPF.</summary>
public class PessoaFisica : Pessoa
{
    public string Cpf { get; set; } = string.Empty;

    public override string DescreverDocumento() => $"CPF {Cpf}";
}
