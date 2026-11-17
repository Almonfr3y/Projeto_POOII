using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Repositorios;

public interface IPedidoRepository
{
    /// <summary>Prepara o pedido para gravar; a gravação acontece em IUnidadeDeTrabalho.SalvarAlteracoes.</summary>
    void Adicionar(Pedido pedido);
}
