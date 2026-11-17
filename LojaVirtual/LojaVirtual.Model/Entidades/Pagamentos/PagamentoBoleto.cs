namespace LojaVirtual.Model.Entidades.Pagamentos;

// Pagamento por boleto: herda de Pagamento.
public class PagamentoBoleto : Pagamento
{
    private const int DiasParaVencimento = 3;       // constante: o boleto vence em 3 dias
    private const int TamanhoCodigoDeBarras = 44;   // constante: um boleto tem 44 números

    public string CodigoDeBarras { get; set; } = "";   // código de barras simulado
    public DateTime Vencimento { get; set; }           // data limite para pagar

    // Texto com a data de vencimento, ex.: "Boleto (vence em 20/11/2026)"
    public override string Descricao
    {
        get { return "Boleto (vence em " + Vencimento.ToString("dd/MM/yyyy") + ")"; }
    }

    // A parte do Processar que é diferente no boleto.
    protected override void GerarDadosDoPagamento()
    {
        Vencimento = DateTime.Today.AddDays(DiasParaVencimento);   // hoje + 3 dias

        // Simula o código de barras com 44 números aleatórios.
        Random random = new Random();                  // gerador de números aleatórios
        CodigoDeBarras = "";                           // começa vazio
        for (int i = 0; i < TamanhoCodigoDeBarras; i++)
            CodigoDeBarras += random.Next(10).ToString();   // sorteia de 0 a 9 e junta no texto
    }
}
