namespace LojaVirtual.Model.Entidades;

// Categoria das peças (Hardware, Periféricos, PCs).
// Agregação: o produto continua existindo mesmo sem a categoria.
public class Categoria
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";

    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();

    // O ComboBox de categorias mostra o que o ToString devolve.
    public override string ToString()
    {
        return Nome;
    }
}
