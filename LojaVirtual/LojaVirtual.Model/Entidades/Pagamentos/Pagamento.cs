namespace LojaVirtual.Model.Entidades.Pagamentos;

// Classe base dos pagamentos. Pix, Boleto e Cartão herdam dela (tabela tb_pagamento).
public abstract class Pagamento
{
    public int Id { get; set; }
    public decimal Valor { get; set; }
    public DateTime? DataProcessamento { get; set; }

    public int PedidoId { get; set; }   // 1x1 com o pedido
    public Pedido? Pedido { get; set; }

    // Texto mostrado na tela de compra concluída (não vai para o banco).
    public abstract string Descricao { get; }

    // O pagamento é simulado. A parte comum fica aqui e a parte
    // diferente de cada forma de pagamento fica em GerarDadosDoPagamento.
    public void Processar()
    {
        if (Valor <= 0)
            throw new InvalidOperationException("O valor do pagamento deve ser maior que zero.");

        GerarDadosDoPagamento();
        DataProcessamento = DateTime.Now;
    }

    protected abstract void GerarDadosDoPagamento();
}
