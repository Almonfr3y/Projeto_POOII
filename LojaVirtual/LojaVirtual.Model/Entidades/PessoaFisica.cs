namespace LojaVirtual.Model.Entidades;

// Cliente pessoa física: herda de Pessoa e tem CPF.
// ": Pessoa" = herança: PessoaFisica ganha tudo o que Pessoa tem.
public class PessoaFisica : Pessoa
{
    public string Cpf { get; set; } = "";   // só pessoa física tem CPF

    // override: a implementação deste tipo para o método abstrato da classe mãe.
    public override string DescreverDocumento()
    {
        return "CPF " + Cpf;   // ex.: "CPF 529.982.247-25"
    }
}
