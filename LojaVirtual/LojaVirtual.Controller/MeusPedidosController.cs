using LojaVirtual.Model.Consultas;

namespace LojaVirtual.Controller;

/// <summary>Tela "Meus Pedidos" (consulta em SQL puro).</summary>
public class MeusPedidosController
{
    private readonly IMeusPedidosConsulta _consulta;

    public MeusPedidosController(IMeusPedidosConsulta consulta) => _consulta = consulta;

    public IReadOnlyList<ResumoPedido> Listar(int clienteId) => _consulta.ListarPorCliente(clienteId);
}
