namespace LojaVirtual.Model.Consultas;

/// <summary>Tela "Meus Pedidos". Implementada em LojaVirtual.Data com SQL puro (MeusPedidosDAO).</summary>
public interface IMeusPedidosConsulta
{
    IReadOnlyList<ResumoPedido> ListarPorCliente(int clienteId);
}

/// <summary>Uma linha da tela Meus Pedidos (resultado do SELECT, não é tabela do banco).</summary>
public record ResumoPedido(int Numero, DateTime Data, string Status, string FormaPagamento, int QuantidadeItens, decimal Total);
