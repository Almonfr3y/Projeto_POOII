namespace LojaVirtual.Model.Entidades;   // pasta/agrupamento lógico das classes de entidade

// Categoria das peças (Hardware, Periféricos, PCs).
// Agregação: o produto continua existindo mesmo sem a categoria.
public class Categoria
{
    public int Id { get; set; }        // chave primária (o banco gera o número)
    public string Nome { get; set; } = "";   // nome da categoria; começa vazio para nunca ser null

    // Lado "1" do relacionamento 1xN: uma categoria tem vários produtos.
    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();

    // O ComboBox de categorias mostra o que o ToString devolve.
    public override string ToString()   // override: substitui o ToString padrão de object
    {
        return Nome;   // mostra só o nome na tela
    }
}
