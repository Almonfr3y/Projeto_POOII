using LojaVirtual.Model.Checkout;
using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Servicos;

public interface IPedidoService
{
    Pedido FinalizarPedido(DadosCheckout dados, IReadOnlyList<PedidoItem> itensDoCarrinho);
}
