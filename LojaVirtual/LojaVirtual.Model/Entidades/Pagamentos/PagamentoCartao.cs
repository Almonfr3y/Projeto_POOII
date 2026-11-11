namespace LojaVirtual.Model.Entidades.Pagamentos;

public class PagamentoCartao : Pagamento
{
    public const int MaximoDeParcelas = 10;

    public string NomeTitular { get; set; } = string.Empty;

    /// <summary>Só os 4 últimos dígitos: o número completo do cartão nunca é guardado.</summary>
    public string UltimosDigitos { get; set; } = string.Empty;

    public int Parcelas { get; set; } = 1;

    public override string Descricao => $"Cartão final {UltimosDigitos} em {Parcelas}x";

    protected override void GerarDadosDoPagamento()
    {
        if (Parcelas < 1 || Parcelas > MaximoDeParcelas)
            throw new InvalidOperationException($"Parcelamento permitido: de 1 a {MaximoDeParcelas} vezes.");
    }
}
