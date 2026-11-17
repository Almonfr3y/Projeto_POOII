using LojaVirtual.Model.Descontos;
using LojaVirtual.Model.Entidades.Pagamentos;
using LojaVirtual.Model.Enums;

namespace LojaVirtual.Model.Entidades;

/// <summary>Uma compra do cliente.</summary>
public class Pedido
{
    public int Id { get; set; }                                   // número do pedido (gerado pelo banco)
    public DateTime Data { get; set; }
    public StatusPedido Status { get; set; } = StatusPedido.Aberto;
    public decimal ValorFrete { get; set; }
    public decimal ValorDesconto { get; set; }

    public int ClienteId { get; set; }                            // 1×N: quem comprou
    public Pessoa? Cliente { get; set; }

    public int EnderecoEntregaId { get; set; }                    // N×1: para onde vai
    public Endereco? EnderecoEntrega { get; set; }

    public int OpcaoEntregaId { get; set; }                       // N×1: como vai
    public OpcaoEntrega? OpcaoEntrega { get; set; }

    public List<PedidoItem> Itens { get; set; } = new();          // COMPOSIÇÃO: itens só existem no pedido

    public Pagamento? Pagamento { get; set; }                     // 1×1: Pix, Boleto ou Cartão

    public decimal CalcularSubtotal() => Itens.Sum(item => item.CalcularSubtotal());

    public decimal CalcularTotal() => CalcularSubtotal() + ValorFrete - ValorDesconto;

    /// <summary>Calcula o desconto com a regra recebida (Strategy): o pedido não sabe qual regra é.</summary>
    public void AplicarDesconto(IDescontoStrategy estrategia) => ValorDesconto = estrategia.CalcularDesconto(this);

    /// <summary>Marca o pedido como concluído, só se ele tiver itens e ainda estiver aberto.</summary>
    public void Finalizar()
    {
        if (Itens.Count == 0)
            throw new InvalidOperationException("Não é possível finalizar um pedido sem itens.");
        if (Status != StatusPedido.Aberto)
            throw new InvalidOperationException($"Pedido em situação {Status} não pode ser finalizado.");

        Status = StatusPedido.Finalizado;
    }
}
