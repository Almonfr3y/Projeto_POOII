namespace LojaVirtual.Model.Entidades;

// Cliente da loja. É abstrata: só existem PessoaFisica e PessoaJuridica (herança).
// "abstract" = não dá para fazer new Pessoa(); só new PessoaFisica() ou new PessoaJuridica().
// No banco as duas ficam na mesma tabela, tb_pessoa.
public abstract class Pessoa
{
    public int Id { get; set; }               // chave primária
    public string Nome { get; set; } = "";    // nome do cliente
    public string Email { get; set; } = "";   // usado no login (único no banco)
    public string Senha { get; set; } = "";   // texto simples (projeto acadêmico)

    public ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();   // 1xN (composição)
    public ICollection<Produto> Favoritos { get; set; } = new List<Produto>();     // NxN (tb_favorito)
    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();         // 1xN

    // Cada subclasse devolve o seu documento (CPF ou CNPJ).
    // Método abstrato: não tem corpo aqui; cada filha é obrigada a escrever o seu (override).
    public abstract string DescreverDocumento();

    // Confere a senha digitada no login.
    public bool SenhaConfere(string senha)
    {
        return Senha == senha;   // true se for igual, false se não for
    }

    // Adiciona o produto aos favoritos, sem repetir.
    public void Favoritar(Produto produto)
    {
        foreach (Produto favorito in Favoritos)   // procura o produto na lista
        {
            if (favorito.Id == produto.Id)
                return;   // já está nos favoritos
        }
        Favoritos.Add(produto);   // não estava: adiciona (vira uma linha em tb_favorito)
    }

    // Tira o produto dos favoritos.
    public void Desfavoritar(int produtoId)
    {
        foreach (Produto favorito in Favoritos)   // procura o produto na lista
        {
            if (favorito.Id == produtoId)
            {
                Favoritos.Remove(favorito);   // achou: remove (apaga a linha de tb_favorito)
                return;                       // sai do método (não pode continuar o foreach depois de remover)
            }
        }
    }
}
