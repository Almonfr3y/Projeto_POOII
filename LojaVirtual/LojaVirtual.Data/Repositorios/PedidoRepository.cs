using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Repositorios;

namespace LojaVirtual.Data.Repositorios;

public class PedidoRepository : IPedidoRepository
{
    private readonly LojaDbContext _contexto;

    public PedidoRepository(LojaDbContext contexto) => _contexto = contexto;

    // Só marca o pedido (com itens e pagamento) para inserir; quem grava é a UnidadeDeTrabalho.
    public void Adicionar(Pedido pedido) => _contexto.Pedidos.Add(pedido);
}
