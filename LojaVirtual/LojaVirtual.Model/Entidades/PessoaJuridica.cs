namespace LojaVirtual.Model.Entidades;

// Cliente pessoa jurídica: herda de Pessoa e tem CNPJ e razão social.
public class PessoaJuridica : Pessoa
{
    public string Cnpj { get; set; } = "";          // só pessoa jurídica tem CNPJ
    public string RazaoSocial { get; set; } = "";   // nome oficial da empresa

    // override: cada tipo de pessoa descreve o seu documento.
    public override string DescreverDocumento()
    {
        return "CNPJ " + Cnpj + " - " + RazaoSocial;   // ex.: "CNPJ 11.222.333/0001-81 - Tech Teste Ltda"
    }
}
