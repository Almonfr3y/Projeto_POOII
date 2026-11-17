namespace LojaVirtual.Model.Entidades;

// Um item do pedido (ou do carrinho).
// É a classe de associação do NxN entre Pedido e Produto.
public class PedidoItem
{
    public int PedidoId { get; set; }      // chave estrangeira do pedido (parte da chave composta)
    public Pedido? Pedido { get; set; }    // navegação para o pedido

    public int ProdutoId { get; set; }     // chave estrangeira do produto (parte da chave composta)
    public Produto? Produto { get; set; }  // navegação para o produto

    public int Quantidade { get; set; }    // quantas unidades deste produto

    // Preço guardado no momento da compra: se o produto mudar de preço, o pedido não muda.
    public decimal PrecoUnitario { get; set; }

    // Valor deste item = quantidade x preço.
    public decimal CalcularSubtotal()
    {
        return Quantidade * PrecoUnitario;
    }
}
