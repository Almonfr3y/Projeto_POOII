namespace LojaVirtual.Model.Entidades;

/// <summary>Endereço salvo do cliente. COMPOSIÇÃO: só existe junto com o cliente.</summary>
public class Endereco
{
    public int Id { get; set; }
    public string Apelido { get; set; } = string.Empty;   // ex.: "Casa", "Trabalho"
    public string Logradouro { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public string Cep { get; set; } = string.Empty;

    public int ClienteId { get; set; }       // chave estrangeira
    public Pessoa? Cliente { get; set; }     // navegação para o dono do endereço

    // Texto mostrado na lista da etapa Endereço e na Revisão.
    public override string ToString() => $"{Apelido}: {Logradouro}, {Numero} — {Bairro}, {Cidade}/{Uf}";
}
