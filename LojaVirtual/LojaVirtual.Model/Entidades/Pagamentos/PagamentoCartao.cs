namespace LojaVirtual.Model.Entidades.Pagamentos;

// Pagamento com cartão de crédito: herda de Pagamento.
public class PagamentoCartao : Pagamento
{
    public const int MaximoDeParcelas = 10;   // public: a tela usa como limite do campo de parcelas

    public string NomeTitular { get; set; } = "";
    public string UltimosDigitos { get; set; } = "";   // só os 4 últimos números são guardados
    public int Parcelas { get; set; } = 1;             // começa em 1x

    // Texto do cartão, ex.: "Cartão final 1111 em 3x"
    public override string Descricao
    {
        get { return "Cartão final " + UltimosDigitos + " em " + Parcelas + "x"; }
    }

    // A parte do Processar que é diferente no cartão.
    protected override void GerarDadosDoPagamento()
    {
        // No cartão só conferimos o número de parcelas.
        if (Parcelas < 1 || Parcelas > MaximoDeParcelas)   // "||" = ou
            throw new InvalidOperationException("O parcelamento deve ser de 1 a " + MaximoDeParcelas + " vezes.");
    }
}
