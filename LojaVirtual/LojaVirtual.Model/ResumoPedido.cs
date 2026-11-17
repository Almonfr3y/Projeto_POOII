namespace LojaVirtual.Model;

// Uma linha da tela Meus Pedidos. Não é uma tabela: é o resultado da consulta SQL.
public class ResumoPedido
{
    public int Numero { get; set; }
    public DateTime Data { get; set; }
    public string Status { get; set; } = "";
    public string FormaPagamento { get; set; } = "";
    public int QuantidadeItens { get; set; }
    public decimal Total { get; set; }
}
