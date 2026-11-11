namespace LojaVirtual.Model.Entidades;

/// <summary>Opções da tela de Entrega. O frete é calculado pelo peso total do pedido.</summary>
public class OpcaoEntrega
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int PrazoDias { get; set; }
    public decimal ValorBase { get; set; }
    public decimal ValorPorKg { get; set; }

    public decimal CalcularFrete(decimal pesoTotalKg) => ValorBase + pesoTotalKg * ValorPorKg;

    public override string ToString() => $"{Nome} ({PrazoDias} dias úteis)";
}
