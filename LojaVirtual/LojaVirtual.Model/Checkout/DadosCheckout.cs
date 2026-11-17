using LojaVirtual.Model.Entidades.Pagamentos;

namespace LojaVirtual.Model.Checkout;

/// <summary>
/// As escolhas do cliente nas etapas Endereço → Entrega → Pagamento, juntas num objeto só
/// (evita método com parâmetros demais — Clean Code). O pagamento chega sem valor; o
/// PedidoService preenche o valor ao finalizar.
/// </summary>
public record DadosCheckout(int ClienteId, int EnderecoEntregaId, int OpcaoEntregaId, Pagamento Pagamento);
