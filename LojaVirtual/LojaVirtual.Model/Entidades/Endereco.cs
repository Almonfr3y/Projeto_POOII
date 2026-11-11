namespace LojaVirtual.Model.Entidades;

public class Endereco
{
    public int Id { get; set; }

    /// <summary>Ex.: "Casa", "Trabalho".</summary>
    public string Apelido { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public string Cep { get; set; } = string.Empty;

    public int ClienteId { get; set; }
    public Pessoa? Cliente { get; set; }

    public override string ToString() => $"{Apelido}: {Logradouro}, {Numero} — {Bairro}, {Cidade}/{Uf}";
}
