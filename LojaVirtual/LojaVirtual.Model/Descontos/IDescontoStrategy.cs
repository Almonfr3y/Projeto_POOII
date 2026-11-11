using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Descontos;

/// <summary>
/// OCP (SOLID): novos descontos são novas classes que implementam esta interface,
/// sem alterar Pedido nem PedidoService. A estratégia usada é escolhida na FabricaDeControladores.
/// </summary>
public interface IDescontoStrategy
{
    decimal CalcularDesconto(Pedido pedido);
}
