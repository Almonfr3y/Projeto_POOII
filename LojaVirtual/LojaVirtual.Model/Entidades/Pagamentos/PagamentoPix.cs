namespace LojaVirtual.Model.Entidades.Pagamentos;

public class PagamentoPix : Pagamento
{
    public string CodigoPix { get; set; } = "";

    public override string Descricao
    {
        get { return "Pix"; }
    }

    protected override void GerarDadosDoPagamento()
    {
        // Simula o código "copia e cola" com um código aleatório.
        CodigoPix = Guid.NewGuid().ToString("N").ToUpper();
    }
}
