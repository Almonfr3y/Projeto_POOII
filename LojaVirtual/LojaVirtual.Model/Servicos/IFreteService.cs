using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Servicos;

public interface IFreteService
{
    /// <summary>Frete dos itens com a opção de entrega escolhida.</summary>
    decimal CalcularFrete(IEnumerable<PedidoItem> itens, OpcaoEntrega opcao);
}
