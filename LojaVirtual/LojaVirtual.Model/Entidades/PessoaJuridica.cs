namespace LojaVirtual.Model.Entidades;

// Cliente pessoa jurídica: herda de Pessoa e tem CNPJ e razão social.
public class PessoaJuridica : Pessoa
{
    public string Cnpj { get; set; } = "";
    public string RazaoSocial { get; set; } = "";

    public override string DescreverDocumento()
    {
        return "CNPJ " + Cnpj + " - " + RazaoSocial;
    }
}
