namespace LojaVirtual.Model.Entidades.Pagamentos;

public class PagamentoBoleto : Pagamento
{
    private const int DiasParaVencimento = 3;
    private const int TamanhoCodigoDeBarras = 44;

    public string CodigoDeBarras { get; set; } = "";
    public DateTime Vencimento { get; set; }

    public override string Descricao
    {
        get { return "Boleto (vence em " + Vencimento.ToString("dd/MM/yyyy") + ")"; }
    }

    protected override void GerarDadosDoPagamento()
    {
        Vencimento = DateTime.Today.AddDays(DiasParaVencimento);

        // Simula o código de barras com 44 números aleatórios.
        Random random = new Random();
        CodigoDeBarras = "";
        for (int i = 0; i < TamanhoCodigoDeBarras; i++)
            CodigoDeBarras += random.Next(10).ToString();
    }
}
