namespace LojaVirtual.Model.Entidades;

/// <summary>
/// Cliente que faz login e compra. HERANÇA: classe abstrata, só existem PessoaFisica e PessoaJuridica.
/// No banco, as duas ficam na tabela tb_pessoa (TPH).
/// </summary>
public abstract class Pessoa
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;  // texto simples: projeto acadêmico, sem dados reais

    public ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>(); // COMPOSIÇÃO 1×N
    public ICollection<Produto> Favoritos { get; set; } = new List<Produto>();   // N×N (tb_favorito)
    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();       // 1×N

    /// <summary>"CPF ..." ou "CNPJ ...". Cada subclasse responde do seu jeito (polimorfismo / LSP).</summary>
    public abstract string DescreverDocumento();

    public bool SenhaConfere(string senha) => Senha == senha;

    public void Favoritar(Produto produto)
    {
        if (!Favoritos.Any(p => p.Id == produto.Id)) // não duplica o favorito
            Favoritos.Add(produto);
    }

    public void Desfavoritar(int produtoId)
    {
        Produto? produto = Favoritos.FirstOrDefault(p => p.Id == produtoId);
        if (produto is not null)
            Favoritos.Remove(produto);
    }
}
