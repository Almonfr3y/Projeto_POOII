using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Descontos;

public class DescontoQuantidade : IDescontoStrategy
{
    // TODO (grupo): confirmar os valores da regra de negócio.
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
