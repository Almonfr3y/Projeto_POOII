namespace LojaVirtual.Model.Entidades;

/// <summary>
/// Uma linha do pedido (ou do carrinho). É a classe de associação do N×N Pedido—Produto:
/// chave composta (PedidoId, ProdutoId) mais os dados da venda (quantidade e preço).
/// </summary>
public class PedidoItem
{
    public int PedidoId { get; set; }
    public Pedido? Pedido { get; set; }

    public int ProdutoId { get; set; }
    public Produto? Produto { get; set; }

    public int Quantidade { get; set; }

    // Preço copiado no momento da compra: se o produto mudar de preço depois, o pedido não muda.
    public decimal PrecoUnitario { get; set; }

    public decimal CalcularSubtotal() => Quantidade * PrecoUnitario;
}
