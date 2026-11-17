namespace LojaVirtual.Model.Entidades;

// Endereço salvo do cliente.
// Composição: o endereço só existe junto com o cliente.
public class Endereco
{
    public int Id { get; set; }
    public string Apelido { get; set; } = "";   // ex.: "Casa", "Trabalho"
    public string Logradouro { get; set; } = "";
    public string Numero { get; set; } = "";
    public string Bairro { get; set; } = "";
    public string Cidade { get; set; } = "";
    public string Uf { get; set; } = "";
    public string Cep { get; set; } = "";

    public int ClienteId { get; set; }
    public Pessoa? Cliente { get; set; }

    public override string ToString()
    {
        return Apelido + ": " + Logradouro + ", " + Numero + " - " + Bairro + ", " + Cidade + "/" + Uf;
    }
}
