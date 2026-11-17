namespace LojaVirtual.Model.Entidades;

// Endereço salvo do cliente.
// Composição: o endereço só existe junto com o cliente.
public class Endereco
{
    public int Id { get; set; }                    // chave primária
    public string Apelido { get; set; } = "";   // ex.: "Casa", "Trabalho"
    public string Logradouro { get; set; } = "";   // rua ou avenida
    public string Numero { get; set; } = "";       // string porque pode ter letras (ex.: "100A")
    public string Bairro { get; set; } = "";
    public string Cidade { get; set; } = "";
    public string Uf { get; set; } = "";           // sigla do estado (ex.: "ES")
    public string Cep { get; set; } = "";

    public int ClienteId { get; set; }       // chave estrangeira: de qual cliente é o endereço
    public Pessoa? Cliente { get; set; }     // navegação: do endereço chego ao cliente ("?" = pode ser null)

    // Texto mostrado na lista de endereços do checkout.
    public override string ToString()
    {
        // Junta os campos num texto só, ex.: "Casa: Rua das Flores, 100 - Centro, Vitória/ES"
        return Apelido + ": " + Logradouro + ", " + Numero + " - " + Bairro + ", " + Cidade + "/" + Uf;
    }
}
