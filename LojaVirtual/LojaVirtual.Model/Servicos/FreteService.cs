using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Servicos;

/// <summary>Frete simulado: peso total do pedido aplicado à opção de entrega escolhida.</summary>
public class FreteService : IFreteService
{
    public decimal CalcularFrete(IEnumerable<PedidoItem> itens, OpcaoEntrega opcao) =>
        opcao.CalcularFrete(itens.Sum(item => (item.Produto?.PesoKg ?? 0m) * item.Quantidade));
}
