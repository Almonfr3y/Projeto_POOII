using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Descontos;

// Regra da loja: 5% de desconto a partir de 5 peças no pedido.
public class DescontoQuantidade : IDescontoStrategy
{
    private const int QuantidadeMinima = 5;
    private const decimal Percentual = 0.05m;

    public decimal CalcularDesconto(Pedido pedido)
    {
        int totalDePecas = 0;
        foreach (PedidoItem item in pedido.Itens)
            totalDePecas += item.Quantidade;

        if (totalDePecas >= QuantidadeMinima)
            return Math.Round(pedido.CalcularSubtotal() * Percentual, 2);   // centavos: 2 casas decimais

        return 0;
    }
}
