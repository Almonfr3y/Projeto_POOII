namespace LojaVirtual.Model.Entidades;

// Opção de entrega do checkout (Econômica, Padrão, Expressa).
public class OpcaoEntrega
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public int PrazoDias { get; set; }
    public decimal ValorBase { get; set; }    // valor fixo
    public decimal ValorPorKg { get; set; }   // valor por quilo

    // Frete = valor fixo + peso total x valor por quilo
    public decimal CalcularFrete(decimal pesoTotalKg)
    {
        return ValorBase + pesoTotalKg * ValorPorKg;
    }

    public override string ToString()
    {
        return Nome + " (" + PrazoDias + " dias úteis)";
    }
}
