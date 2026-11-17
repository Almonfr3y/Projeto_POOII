using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Descontos;

// Regra da loja: 5% de desconto a partir de 5 peças no pedido.
public class DescontoQuantidade : IDescontoStrategy
{
    private const int QuantidadeMinima = 5;         // constante: mínimo de peças para ter desconto
    private const decimal Percentual = 0.05m;       // constante: 5% ("m" = número decimal)

    public decimal CalcularDesconto(Pedido pedido)
    {
        int totalDePecas = 0;                       // conta as peças, não os itens
        foreach (PedidoItem item in pedido.Itens)   // passa por cada item
            totalDePecas += item.Quantidade;        // ex.: 5 mouses = 5 peças

        if (totalDePecas >= QuantidadeMinima)       // tem 5 peças ou mais
            return Math.Round(pedido.CalcularSubtotal() * Percentual, 2);   // centavos: 2 casas decimais

        return 0;                                   // menos de 5 peças: sem desconto
    }
}
