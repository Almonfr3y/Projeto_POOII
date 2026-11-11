using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Repositorios;

namespace LojaVirtual.Data.Repositorios;

public class PedidoRepository : IPedidoRepository
{
    private readonly LojaDbContext _contexto;

    public PedidoRepository(LojaDbContext contexto) => _contexto = contexto;

    public void Adicionar(Pedido pedido) => _contexto.Pedidos.Add(pedido);
}
