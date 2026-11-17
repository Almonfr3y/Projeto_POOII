using LojaVirtual.Model.Descontos;               // para usar IDescontoStrategy
using LojaVirtual.Model.Entidades.Pagamentos;    // para usar Pagamento
using LojaVirtual.Model.Enums;                   // para usar StatusPedido

namespace LojaVirtual.Model.Entidades;

// Uma compra do cliente.
public class Pedido
{
    public int Id { get; set; }   // número do pedido (gerado pelo banco)
    public DateTime Data { get; set; }   // data e hora da compra
    public StatusPedido Status { get; set; } = StatusPedido.Aberto;   // todo pedido começa Aberto
    public decimal ValorFrete { get; set; }      // frete calculado no checkout
    public decimal ValorDesconto { get; set; }   // desconto calculado pela estratégia

    // 1xN: um cliente faz vários pedidos
    public int ClienteId { get; set; }       // chave estrangeira do cliente
    public Pessoa? Cliente { get; set; }     // navegação para o cliente

    public int EnderecoEntregaId { get; set; }        // chave estrangeira do endereço escolhido
    public Endereco? EnderecoEntrega { get; set; }    // navegação para o endereço

    public int OpcaoEntregaId { get; set; }           // chave estrangeira da entrega escolhida
    public OpcaoEntrega? OpcaoEntrega { get; set; }   // navegação para a opção de entrega

    // Composição: os itens só existem dentro do pedido
    public List<PedidoItem> Itens { get; set; } = new List<PedidoItem>();

    // 1x1: cada pedido tem um pagamento (Pix, Boleto ou Cartão)
    public Pagamento? Pagamento { get; set; }

    // Soma o subtotal de todos os itens (quantidade x preço de cada um).
    public decimal CalcularSubtotal()
    {
        decimal subtotal = 0;                    // começa em zero
        foreach (PedidoItem item in Itens)       // passa por cada item do pedido
            subtotal += item.CalcularSubtotal(); // soma o subtotal do item
        return subtotal;                         // devolve a soma
    }

    // Total que o cliente paga.
    public decimal CalcularTotal()
    {
        return CalcularSubtotal() + ValorFrete - ValorDesconto;   // itens + frete - desconto
    }

    // O pedido não sabe qual regra de desconto está sendo usada (padrão Strategy).
    public void AplicarDesconto(IDescontoStrategy desconto)
    {
        ValorDesconto = desconto.CalcularDesconto(this);   // "this" = este próprio pedido
    }

    // Conclui o pedido. Só é possível se tiver itens e ainda estiver Aberto.
    public void Finalizar()
    {
        if (Itens.Count == 0)   // pedido sem nenhum item
            throw new InvalidOperationException("Não é possível finalizar um pedido sem itens.");
        if (Status != StatusPedido.Aberto)   // já foi finalizado antes
            throw new InvalidOperationException("Este pedido já foi finalizado.");

        Status = StatusPedido.Finalizado;   // muda a situação do pedido
    }
}
