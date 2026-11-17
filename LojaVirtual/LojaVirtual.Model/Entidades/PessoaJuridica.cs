namespace LojaVirtual.Model.Entidades;

/// <summary>Cliente pessoa jurídica: herda tudo de Pessoa e acrescenta CNPJ e razão social.</summary>
public class PessoaJuridica : Pessoa
{
    public string Cnpj { get; set; } = string.Empty;
    public string RazaoSocial { get; set; } = string.Empty;

    public override string DescreverDocumento() => $"CNPJ {Cnpj} — {RazaoSocial}";
}
