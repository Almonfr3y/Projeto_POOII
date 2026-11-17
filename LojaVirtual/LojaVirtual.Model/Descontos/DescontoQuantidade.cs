using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Descontos;

/// <summary>Regra da loja: 5% de desconto sobre o subtotal a partir de 5 peças no pedido.</summary>
public class DescontoQuantidade : IDescontoStrategy
{
    // Constantes com nome em vez de números soltos no código (Clean Code).
    private const int QuantidadeMinimaDeItens = 5;
    private const decimal PercentualDesconto = 0.05m;

    public decimal CalcularDesconto(Pedido pedido)
    {
        int totalDePecas = pedido.Itens.Sum(item => item.Quantidade); // soma as quantidades, não os itens
        return totalDePecas >= QuantidadeMinimaDeItens
            ? pedido.CalcularSubtotal() * PercentualDesconto
            : 0m;
    }
}
