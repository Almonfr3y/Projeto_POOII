using LojaVirtual.Data.Consultas;
using LojaVirtual.Model;

namespace LojaVirtual.Controller;

// Controlador da tela Meus Pedidos. A consulta é feita em SQL puro no MeusPedidosDAO.
public class MeusPedidosController
{
    public List<ResumoPedido> Listar(int clienteId)
    {
        MeusPedidosDAO dao = new MeusPedidosDAO();
        return dao.ListarPorCliente(clienteId);
    }
}
