namespace LojaVirtual.Model.Entidades;

/// <summary>
/// Classe de associação do relacionamento N×N Pedido—Produto.
/// Chave composta (PedidoId, ProdutoId).
/// </summary>
public class PedidoItem
{
    public int PedidoId { get; set; }
    public Pedido? Pedido { get; set; }

    public int ProdutoId { get; set; }
    public Produto? Produto { get; set; }

    public int Quantidade { get; set; }

    /// <summary>Preço congelado no momento da compra.</summary>
    public decimal PrecoUnitario { get; set; }

    public decimal CalcularSubtotal() => Quantidade * PrecoUnitario;
}
