using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Descontos;

// Regra alternativa: nenhum desconto.
// ": IDescontoStrategy" = esta classe implementa a interface (é obrigada a ter CalcularDesconto).
public class SemDesconto : IDescontoStrategy
{
    public decimal CalcularDesconto(Pedido pedido)
    {
        return 0;   // nunca dá desconto
    }
}
