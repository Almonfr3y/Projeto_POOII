namespace LojaVirtual.Model;

// Uma linha da tela Meus Pedidos. Não é uma tabela: é o resultado da consulta SQL.
// Cada propriedade corresponde a uma coluna do SELECT do MeusPedidosDAO.
public class ResumoPedido
{
    public int Numero { get; set; }                     // número do pedido (p.Id)
    public DateTime Data { get; set; }                  // data da compra
    public string Status { get; set; } = "";            // "Finalizado"
    public string FormaPagamento { get; set; } = "";    // "Pix", "Boleto" ou "Cartao"
    public int QuantidadeItens { get; set; }            // soma das quantidades
    public decimal Total { get; set; }                  // itens + frete - desconto
}
