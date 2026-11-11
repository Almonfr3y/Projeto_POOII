using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Descontos;

public class SemDesconto : IDescontoStrategy
{
    public decimal CalcularDesconto(Pedido pedido) => 0m;
}
