using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Descontos;

/// <summary>Regra da loja: 5% de desconto sobre o subtotal a partir de 5 peças no pedido.</summary>
public class DescontoQuantidade : IDescontoStrategy
{
    private const int QuantidadeMinimaDeItens = 5;
    private const decimal PercentualDesconto = 0.05m;

    public decimal CalcularDesconto(Pedido pedido)
    {
        int totalDeItens = pedido.Itens.Sum(item => item.Quantidade);
        return totalDeItens >= QuantidadeMinimaDeItens
            ? pedido.CalcularSubtotal() * PercentualDesconto
            : 0m;
    }
}
