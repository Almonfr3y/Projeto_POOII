using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Servicos;

public interface IFreteService
{
    decimal CalcularFrete(IEnumerable<PedidoItem> itens, OpcaoEntrega opcao);
}
