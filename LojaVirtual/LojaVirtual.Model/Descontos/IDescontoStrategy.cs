using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Descontos;

// Padrão Strategy: cada regra de desconto é uma classe que implementa esta interface.
// Para criar um desconto novo basta criar outra classe, sem mexer no Pedido (OCP).
public interface IDescontoStrategy
{
    decimal CalcularDesconto(Pedido pedido);
}
