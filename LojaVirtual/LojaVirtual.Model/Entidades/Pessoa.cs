namespace LojaVirtual.Model.Entidades;

/// <summary>
/// Cliente que faz login e compra. Classe abstrata (herança): só existem PessoaFisica e PessoaJuridica.
/// Persistida em TPH: uma tabela tb_pessoa com a coluna discriminadora TipoPessoa.
/// Os clientes já vêm cadastrados no banco (seed).
/// </summary>
public abstract class Pessoa
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    /// <summary>Texto simples: o projeto é acadêmico e não trata segurança de senha.</summary>
    public string Senha { get; set; } = string.Empty;

    /// <summary>Composição 1×N: os endereços não existem sem o cliente.</summary>
    public ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();

    /// <summary>N×N Pessoa—Produto (tabela de junção tb_favorito).</summary>
    public ICollection<Produto> Favoritos { get; set; } = new List<Produto>();

    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    /// <summary>
    /// Documento do comprador ("CPF ..." ou "CNPJ ..."), exibido na Revisão e em Concluído.
    /// As telas usam este método sem saber se é PF ou PJ (LSP).
    /// </summary>
    public abstract string DescreverDocumento();

    public bool SenhaConfere(string senha) => Senha == senha;

    public void Favoritar(Produto produto)
    {
        if (!Favoritos.Any(p => p.Id == produto.Id))
            Favoritos.Add(produto);
    }

    public void Desfavoritar(int produtoId)
    {
        Produto? produto = Favoritos.FirstOrDefault(p => p.Id == produtoId);
        if (produto is not null)
            Favoritos.Remove(produto);
    }
}
