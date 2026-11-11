namespace LojaVirtual.Model.Entidades.Pagamentos;

public class PagamentoPix : Pagamento
{
    public string CodigoPix { get; set; } = string.Empty;

    public override string Descricao => "Pix";

    protected override void GerarDadosDoPagamento()
    {
        CodigoPix = Guid.NewGuid().ToString("N").ToUpperInvariant();
    }
}
