namespace LojaVirtual.Model.Entidades;

// Cliente da loja. É abstrata: só existem PessoaFisica e PessoaJuridica (herança).
// No banco as duas ficam na mesma tabela, tb_pessoa.
public abstract class Pessoa
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public string Email { get; set; } = "";
    public string Senha { get; set; } = "";   // texto simples (projeto acadêmico)

    public ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();   // 1xN (composição)
    public ICollection<Produto> Favoritos { get; set; } = new List<Produto>();     // NxN (tb_favorito)
    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();         // 1xN

    // Cada subclasse devolve o seu documento (CPF ou CNPJ).
    public abstract string DescreverDocumento();

    public bool SenhaConfere(string senha)
    {
        return Senha == senha;
    }

    public void Favoritar(Produto produto)
    {
        foreach (Produto favorito in Favoritos)
        {
            if (favorito.Id == produto.Id)
                return;   // já está nos favoritos
        }
        Favoritos.Add(produto);
    }

    public void Desfavoritar(int produtoId)
    {
        foreach (Produto favorito in Favoritos)
        {
            if (favorito.Id == produtoId)
            {
                Favoritos.Remove(favorito);
                return;
            }
        }
    }
}
