namespace LojaVirtual.Model.Entidades.Pagamentos;

public class PagamentoCartao : Pagamento
{
    public const int MaximoDeParcelas = 10;   // public: a tela usa como limite do campo de parcelas

    public string NomeTitular { get; set; } = string.Empty;
    public string UltimosDigitos { get; set; } = string.Empty;   // só os 4 últimos: o número inteiro nunca é guardado
    public int Parcelas { get; set; } = 1;

    public override string Descricao => $"Cartão final {UltimosDigitos} em {Parcelas}x";

    // No cartão, "gerar os dados" é só validar o parcelamento.
    protected override void GerarDadosDoPagamento()
    {
        if (Parcelas < 1 || Parcelas > MaximoDeParcelas)
            throw new InvalidOperationException($"Parcelamento permitido: de 1 a {MaximoDeParcelas} vezes.");
    }
}
