using LojaVirtual.Data.Consultas;   // para usar MeusPedidosDAO
using LojaVirtual.Model;            // para usar ResumoPedido

namespace LojaVirtual.Controller;

// Controlador da tela Meus Pedidos. A consulta é feita em SQL puro no MeusPedidosDAO.
public class MeusPedidosController
{
    // Devolve os pedidos do cliente para a tela.
    public List<ResumoPedido> Listar(int clienteId)
    {
        MeusPedidosDAO dao = new MeusPedidosDAO();   // cria o objeto que faz o SQL
        return dao.ListarPorCliente(clienteId);      // executa a consulta e devolve a lista
    }
}
