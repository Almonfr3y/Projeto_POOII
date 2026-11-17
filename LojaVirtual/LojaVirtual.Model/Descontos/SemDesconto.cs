using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Descontos;

// Regra alternativa: nenhum desconto.
public class SemDesconto : IDescontoStrategy
{
    public decimal CalcularDesconto(Pedido pedido)
    {
        return 0;
    }
}
