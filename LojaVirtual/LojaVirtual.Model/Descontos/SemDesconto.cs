using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Descontos;

/// <summary>Estratégia alternativa: nenhum desconto. Troque na FabricaDeControladores para testar o OCP.</summary>
public class SemDesconto : IDescontoStrategy
{
    public decimal CalcularDesconto(Pedido pedido) => 0m;
}
