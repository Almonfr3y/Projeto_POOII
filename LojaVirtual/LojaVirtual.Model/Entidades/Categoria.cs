namespace LojaVirtual.Model.Entidades;

/// <summary>Agrega produtos (agregação: o produto existe sem a categoria).</summary>
public class Categoria
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;

    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();

    public override string ToString() => Nome;
}
