using LojaVirtual.Model.Descontos;
using LojaVirtual.Model.Entidades.Pagamentos;
using LojaVirtual.Model.Enums;

namespace LojaVirtual.Model.Entidades;

public class Pedido
{
    public int Id { get; set; }
    public DateTime Data { get; set; }
    public StatusPedido Status { get; set; } = StatusPedido.Aberto;
    public decimal ValorFrete { get; set; }
    public decimal ValorDesconto { get; set; }

    public int ClienteId { get; set; }
    public Pessoa? Cliente { get; set; }

    public int EnderecoEntregaId { get; set; }
    public Endereco? EnderecoEntrega { get; set; }

    public int OpcaoEntregaId { get; set; }
    public OpcaoEntrega? OpcaoEntrega { get; set; }

    /// <summary>Composição: os itens só existem dentro do pedido.</summary>
    public List<PedidoItem> Itens { get; set; } = new();

    /// <summary>1×1: cada pedido tem um pagamento (Pix, Boleto ou Cartão).</summary>
    public Pagamento? Pagamento { get; set; }

    public decimal CalcularSubtotal() => Itens.Sum(item => item.CalcularSubtotal());

    public decimal CalcularTotal() => CalcularSubtotal() + ValorFrete - ValorDesconto;

    public void AplicarDesconto(IDescontoStrategy estrategia)
    {
        ValorDesconto = estrategia.CalcularDesconto(this);
    }

    public void Finalizar()
    {
        if (Itens.Count == 0)
            throw new InvalidOperationException("Não é possível finalizar um pedido sem itens.");
        if (Status != StatusPedido.Aberto)
            throw new InvalidOperationException($"Pedido em situação {Status} não pode ser finalizado.");

        Status = StatusPedido.Finalizado;
    }
}
