namespace LojaVirtual.Model.Entidades.Pagamentos;

// Pagamento por Pix: herda de Pagamento.
public class PagamentoPix : Pagamento
{
    public string CodigoPix { get; set; } = "";   // código "copia e cola" simulado

    // Texto desta forma de pagamento.
    public override string Descricao
    {
        get { return "Pix"; }
    }

    // A parte do Processar que é diferente no Pix.
    protected override void GerarDadosDoPagamento()
    {
        // Simula o código "copia e cola" com um código aleatório.
        // Guid = identificador único; "N" = sem traços; ToUpper = letras maiúsculas.
        CodigoPix = Guid.NewGuid().ToString("N").ToUpper();
    }
}
