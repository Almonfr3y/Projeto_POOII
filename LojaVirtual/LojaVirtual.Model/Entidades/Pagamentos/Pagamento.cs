namespace LojaVirtual.Model.Entidades.Pagamentos;

/// <summary>
/// HERANÇA: Pix, Boleto e Cartão são tipos de Pagamento (tabela tb_pagamento, TPH).
/// Padrão Template Method: Processar() é igual para todos e chama GerarDadosDoPagamento(),
/// que cada subclasse implementa do seu jeito. Quem processa não precisa saber o tipo (LSP).
/// </summary>
public abstract class Pagamento
{
    public int Id { get; set; }
    public decimal Valor { get; set; }
    public DateTime? DataProcessamento { get; set; }   // null = ainda não processado

    public int PedidoId { get; set; }                  // 1×1 com o pedido
    public Pedido? Pedido { get; set; }

    /// <summary>Texto mostrado na Revisão e em Concluído (não é gravado no banco).</summary>
    public abstract string Descricao { get; }

    /// <summary>Pagamento simulado: valida o valor, gera os dados da forma escolhida e registra a data.</summary>
    public void Processar()
    {
        if (Valor <= 0)
            throw new InvalidOperationException("O valor do pagamento deve ser maior que zero.");

        GerarDadosDoPagamento();       // parte que muda em cada subclasse
        DataProcessamento = DateTime.Now;
    }

    protected abstract void GerarDadosDoPagamento();
}
