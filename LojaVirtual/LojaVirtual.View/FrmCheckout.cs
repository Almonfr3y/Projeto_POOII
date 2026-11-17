using System.Text;
using LojaVirtual.Controller;
using LojaVirtual.Model.Carrinho;
using LojaVirtual.Model.Checkout;
using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Entidades.Pagamentos;
using LojaVirtual.Model.Excecoes;

namespace LojaVirtual.View;

/// <summary>
/// Assistente do checkout, como no desenho: Endereço → Entrega → Pagamento → Revisão → Concluído.
/// A tela só coleta as escolhas e monta o DadosCheckout; a regra de finalização fica no PedidoService (SRP).
/// </summary>
public partial class FrmCheckout : Form
{
    private const int DigitosMinimosDoCartao = 13;
    private const int DigitosMaximosDoCartao = 19;
    private const int DigitosVisiveisDoCartao = 4;

    private enum Etapa { Endereco, Entrega, Pagamento, Revisao, Concluido }

    private static readonly string[] NomesDasEtapas = { "Endereço", "Entrega", "Pagamento", "Revisão", "Concluído" };

    private readonly PedidoController _pedidos;
    private readonly SessaoDoUsuario _sessao;
    private Etapa _etapa = Etapa.Endereco;
    private bool _compraConcluida;

    public FrmCheckout(PedidoController pedidos, SessaoDoUsuario sessao)
    {
        _pedidos = pedidos;
        _sessao = sessao;
        InitializeComponent();
    }

    private CarrinhoDeCompras Carrinho => _sessao.Carrinho.Carrinho;

    private Endereco? EnderecoEscolhido => lstEnderecos.SelectedItem as Endereco;

    private OpcaoComFrete? EntregaEscolhida => lstEntregas.SelectedItem as OpcaoComFrete;

    private void FrmCheckout_Load(object sender, EventArgs e)
    {
        lstEnderecos.DataSource = _pedidos.ListarEnderecos(_sessao.Cliente.Id);
        lstEntregas.DataSource = _pedidos.ListarOpcoesEntrega()
            .Select(opcao => new OpcaoComFrete(opcao, _pedidos.SimularFrete(Carrinho, opcao)))
            .ToList();
        nudParcelas.Maximum = PagamentoCartao.MaximoDeParcelas;
        rdoPix.Checked = true;
        MostrarEtapa(Etapa.Endereco);
    }

    private void FrmCheckout_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (_compraConcluida)
            DialogResult = DialogResult.OK;
    }

    private void FormaDePagamento_CheckedChanged(object sender, EventArgs e) => grpCartao.Enabled = rdoCartao.Checked;

    private void BtnVoltar_Click(object sender, EventArgs e) => MostrarEtapa(_etapa - 1);

    private void BtnAvancar_Click(object sender, EventArgs e)
    {
        switch (_etapa)
        {
            case Etapa.Endereco when EnderecoEscolhido is not null:
                MostrarEtapa(Etapa.Entrega);
                break;
            case Etapa.Entrega when EntregaEscolhida is not null:
                MostrarEtapa(Etapa.Pagamento);
                break;
            case Etapa.Pagamento when PagamentoEhValido():
                txtRevisao.Text = MontarRevisao();
                MostrarEtapa(Etapa.Revisao);
                break;
            case Etapa.Revisao:
                ConfirmarCompra();
                break;
            case Etapa.Concluido:
                Close();
                break;
        }
    }

    private void MostrarEtapa(Etapa etapa)
    {
        _etapa = etapa;
        pnlEndereco.Visible = etapa == Etapa.Endereco;
        pnlEntrega.Visible = etapa == Etapa.Entrega;
        pnlPagamento.Visible = etapa == Etapa.Pagamento;
        pnlRevisao.Visible = etapa == Etapa.Revisao;
        pnlConcluido.Visible = etapa == Etapa.Concluido;

        lblEtapa.Text = $"Etapa {(int)etapa + 1} de {NomesDasEtapas.Length} — {NomesDasEtapas[(int)etapa]}";
        btnVoltar.Enabled = etapa is Etapa.Entrega or Etapa.Pagamento or Etapa.Revisao;
        btnAvancar.Text = etapa switch
        {
            Etapa.Revisao => "Confirmar compra",
            Etapa.Concluido => "Fechar",
            _ => "Avançar",
        };
    }

    private bool PagamentoEhValido()
    {
        if (!rdoCartao.Checked)
            return true;

        string digitos = SomenteDigitos(txtNumeroCartao.Text);
        if (string.IsNullOrWhiteSpace(txtTitular.Text))
            return Avisar("Informe o nome do titular do cartão.");
        if (digitos.Length is < DigitosMinimosDoCartao or > DigitosMaximosDoCartao)
            return Avisar("Número do cartão inválido.");

        return true;
    }

    /// <summary>Cria a forma de pagamento escolhida. O valor é definido na finalização do pedido.</summary>
    private Pagamento CriarPagamento()
    {
        if (rdoBoleto.Checked)
            return new PagamentoBoleto();
        if (rdoCartao.Checked)
            return new PagamentoCartao
            {
                NomeTitular = txtTitular.Text.Trim(),
                UltimosDigitos = SomenteDigitos(txtNumeroCartao.Text)[^DigitosVisiveisDoCartao..],
                Parcelas = (int)nudParcelas.Value,
            };

        return new PagamentoPix();
    }

    private string DescreverPagamentoEscolhido() =>
        rdoCartao.Checked
            ? $"Cartão final {SomenteDigitos(txtNumeroCartao.Text)[^DigitosVisiveisDoCartao..]} em {nudParcelas.Value}x"
            : rdoBoleto.Checked ? "Boleto bancário" : "Pix";

    private string MontarRevisao()
    {
        OpcaoComFrete entrega = EntregaEscolhida!;
        decimal subtotal = Carrinho.CalcularSubtotal();
        decimal desconto = _pedidos.SimularDesconto(Carrinho);
        var texto = new StringBuilder();

        texto.AppendLine($"Cliente: {_sessao.Cliente.Nome} — {_pedidos.DescreverDocumento(_sessao.Cliente.Id)}");
        texto.AppendLine($"Endereço: {EnderecoEscolhido}");
        texto.AppendLine($"Entrega: {entrega}");
        texto.AppendLine($"Pagamento: {DescreverPagamentoEscolhido()}");
        texto.AppendLine();
        texto.AppendLine("Produtos:");
        foreach (PedidoItem item in Carrinho.Itens)
            texto.AppendLine($"  {item.Quantidade}x {item.Produto?.Nome} — {item.CalcularSubtotal():C}");
        texto.AppendLine();
        texto.AppendLine($"Subtotal: {subtotal:C}");
        texto.AppendLine($"Frete:    {entrega.Frete:C}");
        texto.AppendLine($"Desconto: {desconto:C}");
        texto.AppendLine($"Total:    {subtotal + entrega.Frete - desconto:C}");
        return texto.ToString();
    }

    private void ConfirmarCompra()
    {
        var dados = new DadosCheckout(_sessao.Cliente.Id, EnderecoEscolhido!.Id, EntregaEscolhida!.Opcao.Id, CriarPagamento());
        try
        {
            Pedido pedido = _pedidos.Finalizar(dados, Carrinho);
            _compraConcluida = true;
            txtConcluido.Text = MontarComprovante(pedido);
            MostrarEtapa(Etapa.Concluido);
        }
        catch (EstoqueInsuficienteException ex)
        {
            Avisar(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            Avisar(ex.Message);
        }
    }

    private string MontarComprovante(Pedido pedido)
    {
        var texto = new StringBuilder();
        texto.AppendLine($"Pedido nº {pedido.Id} realizado com sucesso!");
        texto.AppendLine();
        texto.AppendLine($"Pagamento: {pedido.Pagamento?.Descricao}");
        texto.AppendLine($"Entrega: {EntregaEscolhida}");
        if (pedido.ValorDesconto > 0)
            texto.AppendLine($"Desconto: {pedido.ValorDesconto:C}");
        texto.AppendLine($"Total pago: {pedido.CalcularTotal():C}");
        return texto.ToString();
    }

    private bool Avisar(string mensagem)
    {
        MessageBox.Show(mensagem, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return false;
    }

    private static string SomenteDigitos(string texto) => string.Concat(texto.Where(char.IsDigit));

    /// <summary>Item da lista de Entrega: a opção com o frete já simulado para este carrinho.</summary>
    private sealed record OpcaoComFrete(OpcaoEntrega Opcao, decimal Frete)
    {
        public override string ToString() => $"{Opcao} — frete {Frete:C}";
    }
}
