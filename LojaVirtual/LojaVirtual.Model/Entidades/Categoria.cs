namespace LojaVirtual.Model.Entidades;

/// <summary>Agrupa produtos. AGREGAÇÃO: o produto continua existindo sem a categoria.</summary>
public class Categoria
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;

    public ICollection<Produto> Produtos { get; set; } = new List<Produto>(); // lado 1×N

    // Usado pelo ComboBox de categorias para mostrar o nome.
    public override string ToString() => Nome;
}
