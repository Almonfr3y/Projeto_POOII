namespace LojaVirtual.Model.Entidades.Pagamentos;

public class PagamentoBoleto : Pagamento
{
    private const int DiasParaVencimento = 3;
    private const int DigitosCodigoDeBarras = 44;   // tamanho do código de barras de boleto

    public string CodigoDeBarras { get; set; } = string.Empty;
    public DateTime Vencimento { get; set; }

    public override string Descricao => $"Boleto (vence em {Vencimento:dd/MM/yyyy})";

    protected override void GerarDadosDoPagamento()
    {
        Vencimento = DateTime.Today.AddDays(DiasParaVencimento);
        // 44 dígitos aleatórios de 0 a 9 (simulação).
        CodigoDeBarras = string.Concat(Enumerable.Range(0, DigitosCodigoDeBarras).Select(_ => Random.Shared.Next(10)));
    }
}
