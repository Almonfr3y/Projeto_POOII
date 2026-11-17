using LojaVirtual.Model.Consultas;

namespace LojaVirtual.Controller;

/// <summary>Tela "Meus Pedidos". A consulta por trás é SQL puro (MeusPedidosDAO).</summary>
public class MeusPedidosController
{
    private readonly IMeusPedidosConsulta _consulta;

    public MeusPedidosController(IMeusPedidosConsulta consulta) => _consulta = consulta;

    public IReadOnlyList<ResumoPedido> Listar(int clienteId) => _consulta.ListarPorCliente(clienteId);
}
