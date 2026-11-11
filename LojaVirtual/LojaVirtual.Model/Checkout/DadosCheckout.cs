using LojaVirtual.Model.Entidades.Pagamentos;

namespace LojaVirtual.Model.Checkout;

/// <summary>
/// Escolhas feitas nas telas Endereço → Entrega → Pagamento.
/// Agrupar em um objeto evita métodos com parâmetros demais (Clean Code).
/// O Pagamento chega preenchido com os dados da forma escolhida, ainda sem valor.
/// </summary>
public record DadosCheckout(int ClienteId, int EnderecoEntregaId, int OpcaoEntregaId, Pagamento Pagamento);
