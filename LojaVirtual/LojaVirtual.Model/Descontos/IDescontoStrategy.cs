using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Descontos;

/// <summary>
/// Padrão Strategy + OCP (SOLID): cada regra de desconto é uma classe que implementa esta interface.
/// Para criar um desconto novo, basta uma classe nova — Pedido e PedidoService não mudam.
/// A regra em uso é escolhida na FabricaDeControladores.
/// </summary>
public interface IDescontoStrategy
{
    decimal CalcularDesconto(Pedido pedido);
}
