namespace LojaVirtual.Model.Entidades.Pagamentos;

public class PagamentoCartao : Pagamento
{
    public const int MaximoDeParcelas = 10;

    public string NomeTitular { get; set; } = "";
    public string UltimosDigitos { get; set; } = "";   // só os 4 últimos números são guardados
    public int Parcelas { get; set; } = 1;

    public override string Descricao
    {
        get { return "Cartão final " + UltimosDigitos + " em " + Parcelas + "x"; }
    }

    protected override void GerarDadosDoPagamento()
    {
        // No cartão só conferimos o número de parcelas.
        if (Parcelas < 1 || Parcelas > MaximoDeParcelas)
            throw new InvalidOperationException("O parcelamento deve ser de 1 a " + MaximoDeParcelas + " vezes.");
    }
}
