using LojaVirtual.Model.Descontos;
using LojaVirtual.Model.Entidades.Pagamentos;
using LojaVirtual.Model.Enums;

namespace LojaVirtual.Model.Entidades;

// Uma compra do cliente.
public class Pedido
{
    public int Id { get; set; }   // número do pedido (gerado pelo banco)
    public DateTime Data { get; set; }
    public StatusPedido Status { get; set; } = StatusPedido.Aberto;
    public decimal ValorFrete { get; set; }
    public decimal ValorDesconto { get; set; }

    // 1xN: um cliente faz vários pedidos
    public int ClienteId { get; set; }
    public Pessoa? Cliente { get; set; }

    public int EnderecoEntregaId { get; set; }
    public Endereco? EnderecoEntrega { get; set; }

    public int OpcaoEntregaId { get; set; }
    public OpcaoEntrega? OpcaoEntrega { get; set; }

    // Composição: os itens só existem dentro do pedido
    public List<PedidoItem> Itens { get; set; } = new List<PedidoItem>();

    // 1x1: cada pedido tem um pagamento (Pix, Boleto ou Cartão)
    public Pagamento? Pagamento { get; set; }

    public decimal CalcularSubtotal()
    {
        decimal subtotal = 0;
        foreach (PedidoItem item in Itens)
            subtotal += item.CalcularSubtotal();
        return subtotal;
    }

    public decimal CalcularTotal()
    {
        return CalcularSubtotal() + ValorFrete - ValorDesconto;
    }

    // O pedido não sabe qual regra de desconto está sendo usada (padrão Strategy).
    public void AplicarDesconto(IDescontoStrategy desconto)
    {
        ValorDesconto = desconto.CalcularDesconto(this);
    }

    public void Finalizar()
    {
        if (Itens.Count == 0)
            throw new InvalidOperationException("Não é possível finalizar um pedido sem itens.");
        if (Status != StatusPedido.Aberto)
            throw new InvalidOperationException("Este pedido já foi finalizado.");

        Status = StatusPedido.Finalizado;
    }
}
