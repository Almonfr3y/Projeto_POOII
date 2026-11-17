using LojaVirtual.Model.Checkout;
using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Servicos;

public interface IPedidoService
{
    /// <summary>Fecha a compra e devolve o pedido gravado (já com número).</summary>
    Pedido FinalizarPedido(DadosCheckout dados, IReadOnlyList<PedidoItem> itensDoCarrinho);

    /// <summary>Desconto que o carrinho receberia, mostrado na etapa Revisão antes de confirmar.</summary>
    decimal SimularDesconto(IReadOnlyList<PedidoItem> itensDoCarrinho);
}
