namespace LojaVirtual.Model.Consultas;

/// <summary>Tela "Meus Pedidos". Implementada em LojaVirtual.Data com SQL puro (ADO.NET).</summary>
public interface IMeusPedidosConsulta
{
    IReadOnlyList<ResumoPedido> ListarPorCliente(int clienteId);
}

public record ResumoPedido(int Numero, DateTime Data, string Status, string FormaPagamento, int QuantidadeItens, decimal Total);
