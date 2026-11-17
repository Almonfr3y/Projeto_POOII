using LojaVirtual.Model.Entidades;   // para usar Pedido

namespace LojaVirtual.Model.Descontos;

// Padrão Strategy: cada regra de desconto é uma classe que implementa esta interface.
// Para criar um desconto novo basta criar outra classe, sem mexer no Pedido (OCP).
// interface = só diz O QUE fazer (a assinatura do método), não COMO.
public interface IDescontoStrategy
{
    decimal CalcularDesconto(Pedido pedido);   // devolve o valor do desconto em reais
}
