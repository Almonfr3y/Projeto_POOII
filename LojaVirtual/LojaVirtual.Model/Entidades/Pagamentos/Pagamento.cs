namespace LojaVirtual.Model.Entidades.Pagamentos;

/// <summary>
/// Herança (exigência do enunciado): Pix, Boleto e Cartão especializam Pagamento.
/// Persistido em TPH: uma tabela tb_pagamento com a coluna discriminadora Forma.
/// O checkout trata qualquer pagamento pela classe base (LSP).
/// </summary>
public abstract class Pagamento
{
    public int Id { get; set; }
    public decimal Valor { get; set; }
    public DateTime? DataProcessamento { get; set; }

    public int PedidoId { get; set; }
    public Pedido? Pedido { get; set; }

    /// <summary>Texto mostrado nas telas de Revisão e Concluído.</summary>
    public abstract string Descricao { get; }

    /// <summary>Pagamento simulado: cada forma gera seus próprios dados.</summary>
    public void Processar()
    {
        if (Valor <= 0)
            throw new InvalidOperationException("O valor do pagamento deve ser maior que zero.");

        GerarDadosDoPagamento();
        DataProcessamento = DateTime.Now;
    }

    protected abstract void GerarDadosDoPagamento();
}
