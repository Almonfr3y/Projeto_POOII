namespace LojaVirtual.Model.Entidades.Pagamentos;

public class PagamentoPix : Pagamento
{
    public string CodigoPix { get; set; } = string.Empty;

    public override string Descricao => "Pix";

    // Simula o código "copia e cola" com um identificador aleatório de 32 caracteres.
    protected override void GerarDadosDoPagamento() => CodigoPix = Guid.NewGuid().ToString("N").ToUpperInvariant();
}
