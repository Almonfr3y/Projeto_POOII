namespace LojaVirtual.Model.Entidades;

// Um item do pedido (ou do carrinho).
// É a classe de associação do NxN entre Pedido e Produto.
public class PedidoItem
{
    public int PedidoId { get; set; }
    public Pedido? Pedido { get; set; }

    public int ProdutoId { get; set; }
    public Produto? Produto { get; set; }

    public int Quantidade { get; set; }

    // Preço guardado no momento da compra: se o produto mudar de preço, o pedido não muda.
    public decimal PrecoUnitario { get; set; }

    public decimal CalcularSubtotal()
    {
        return Quantidade * PrecoUnitario;
    }
}
