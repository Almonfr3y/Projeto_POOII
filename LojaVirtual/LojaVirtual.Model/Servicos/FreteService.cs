using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Servicos;

/// <summary>
/// Frete simulado pelo peso total. Um só lugar para a regra (DRY): a etapa Entrega usa para mostrar
/// o frete de cada opção, e o PedidoService usa para gravar o frete do pedido.
/// </summary>
public class FreteService : IFreteService
{
    public decimal CalcularFrete(IEnumerable<PedidoItem> itens, OpcaoEntrega opcao)
    {
        decimal pesoTotalKg = itens.Sum(item => (item.Produto?.PesoKg ?? 0m) * item.Quantidade); // peso × quantidade
        return opcao.CalcularFrete(pesoTotalKg);
    }
}
