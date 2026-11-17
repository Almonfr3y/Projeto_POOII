namespace LojaVirtual.Model.Entidades;

// Opção de entrega do checkout (Econômica, Padrão, Expressa).
public class OpcaoEntrega
{
    public int Id { get; set; }                // chave primária
    public string Nome { get; set; } = "";     // ex.: "Padrão"
    public int PrazoDias { get; set; }        // prazo em dias úteis
    public decimal ValorBase { get; set; }    // valor fixo
    public decimal ValorPorKg { get; set; }   // valor por quilo

    // Frete = valor fixo + peso total x valor por quilo
    public decimal CalcularFrete(decimal pesoTotalKg)
    {
        return ValorBase + pesoTotalKg * ValorPorKg;   // ex.: 18 + 0,6 kg x 4,50 = 20,70
    }

    // Texto mostrado na lista de entregas, ex.: "Padrão (5 dias úteis)"
    public override string ToString()
    {
        return Nome + " (" + PrazoDias + " dias úteis)";
    }
}
