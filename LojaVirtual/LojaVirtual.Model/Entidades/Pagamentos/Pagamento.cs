namespace LojaVirtual.Model.Entidades.Pagamentos;

// Classe base dos pagamentos. Pix, Boleto e Cartão herdam dela (tabela tb_pagamento).
// "abstract": não existe "um pagamento" sem forma; só as classes filhas podem ser criadas.
public abstract class Pagamento
{
    public int Id { get; set; }                         // chave primária
    public decimal Valor { get; set; }                  // quanto foi pago (preenchido ao finalizar)
    public DateTime? DataProcessamento { get; set; }    // "?" = pode ser null (ainda não processado)

    public int PedidoId { get; set; }   // 1x1 com o pedido
    public Pedido? Pedido { get; set; } // navegação para o pedido

    // Texto mostrado na tela de compra concluída (não vai para o banco).
    // Propriedade abstrata: cada forma de pagamento escreve o seu texto.
    public abstract string Descricao { get; }

    // O pagamento é simulado. A parte comum fica aqui e a parte
    // diferente de cada forma de pagamento fica em GerarDadosDoPagamento.
    // (padrão Template Method)
    public void Processar()
    {
        if (Valor <= 0)   // não existe pagamento de valor zero ou negativo
            throw new InvalidOperationException("O valor do pagamento deve ser maior que zero.");

        GerarDadosDoPagamento();            // chama a versão da classe filha (polimorfismo)
        DataProcessamento = DateTime.Now;   // registra quando foi pago
    }

    // protected: só esta classe e as filhas enxergam. abstract: cada filha implementa.
    protected abstract void GerarDadosDoPagamento();
}
