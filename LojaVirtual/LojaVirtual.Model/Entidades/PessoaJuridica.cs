namespace LojaVirtual.Model.Entidades;

public class PessoaJuridica : Pessoa
{
    public string Cnpj { get; set; } = string.Empty;
    public string RazaoSocial { get; set; } = string.Empty;

    public override string DescreverDocumento() => $"CNPJ {Cnpj} — {RazaoSocial}";
}
