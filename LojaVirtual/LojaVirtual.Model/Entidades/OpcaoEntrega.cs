namespace LojaVirtual.Model.Entidades;

/// <summary>Opção da etapa Entrega (Econômica, Padrão, Expressa).</summary>
public class OpcaoEntrega
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int PrazoDias { get; set; }
    public decimal ValorBase { get; set; }   // valor fixo da entrega
    public decimal ValorPorKg { get; set; }  // valor somado por quilo

    /// <summary>Frete = valor fixo + peso × valor por quilo.</summary>
    public decimal CalcularFrete(decimal pesoTotalKg) => ValorBase + pesoTotalKg * ValorPorKg;

    public override string ToString() => $"{Nome} ({PrazoDias} dias úteis)";
}
