using LojaVirtual.Controller;
using LojaVirtual.Model.Descontos;
using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Entidades.Pagamentos;
using LojaVirtual.Model.Excecoes;

namespace LojaVirtual.View;

// Checkout em etapas: Endereço -> Entrega -> Pagamento -> Revisão -> Concluído.
// Cada etapa é um painel, e só o painel da etapa atual fica visível.
public partial class FrmCheckout : Form
{
    private const int EtapaEndereco = 0;
    private const int EtapaEntrega = 1;
    private const int EtapaPagamento = 2;
    private const int EtapaRevisao = 3;
    private const int EtapaConcluido = 4;

    private string[] _nomesDasEtapas = { "Endereço", "Entrega", "Pagamento", "Revisão", "Concluído" };
    private Panel[] _paineis;
    private int _etapa;

    private SessaoDoUsuario _sessao;
    private List<OpcaoEntrega> _opcoesEntrega = new List<OpcaoEntrega>();

    // Para trocar a regra de desconto, basta trocar DescontoQuantidade por outra classe (ex.: SemDesconto).
    private PedidoController _pedidos = new PedidoController(new DescontoQuantidade());

    public FrmCheckout(SessaoDoUsuario sessao)
    {
        _sessao = sessao;
        InitializeComponent();
        _paineis = new Panel[] { pnlEndereco, pnlEntrega, pnlPagamento, pnlRevisao, pnlConcluido };
    }

    private void FrmCheckout_Load(object sender, EventArgs e)
    {
        lstEnderecos.DataSource = _pedidos.ListarEnderecos(_sessao.Cliente.Id);

        // Cada opção de entrega aparece com o frete já calculado para este carrinho.
        _opcoesEntrega = _pedidos.ListarOpcoesEntrega();
        foreach (OpcaoEntrega opcao in _opcoesEntrega)
        {
            decimal frete = _pedidos.CalcularFrete(_sessao.Carrinho, opcao);
            lstEntregas.Items.Add(opcao + " - frete " + frete.ToString("C"));
        }

        nudParcelas.Maximum = PagamentoCartao.MaximoDeParcelas;
        rdoPix.Checked = true;
        MostrarEtapa(EtapaEndereco);
    }

    // Os campos do cartão só ficam habilitados quando "Cartão" está marcado.
    private void FormaDePagamento_CheckedChanged(object sender, EventArgs e)
    {
        grpCartao.Enabled = rdoCartao.Checked;
    }

    private void BtnVoltar_Click(object sender, EventArgs e)
    {
        MostrarEtapa(_etapa - 1);
    }

    private void BtnAvancar_Click(object sender, EventArgs e)
    {
        if (_etapa == EtapaEndereco)
        {
            if (lstEnderecos.SelectedItem == null)
            {
                MessageBox.Show("Escolha um endereço.");
                return;
            }
            MostrarEtapa(EtapaEntrega);
        }
        else if (_etapa == EtapaEntrega)
        {
            if (lstEntregas.SelectedIndex < 0)
            {
                MessageBox.Show("Escolha uma opção de entrega.");
                return;
            }
            MostrarEtapa(EtapaPagamento);
        }
        else if (_etapa == EtapaPagamento)
        {
            if (!CartaoEhValido())
                return;
            txtRevisao.Text = MontarRevisao();
            MostrarEtapa(EtapaRevisao);
        }
        else if (_etapa == EtapaRevisao)
        {
            ConfirmarCompra();
        }
        else
        {
            Close();
        }
    }

    private void MostrarEtapa(int etapa)
    {
        _etapa = etapa;

        for (int i = 0; i < _paineis.Length; i++)
            _paineis[i].Visible = (i == etapa);

        lblEtapa.Text = "Etapa " + (etapa + 1) + " de 5 - " + _nomesDasEtapas[etapa];

        // Não dá para voltar da primeira etapa nem depois da compra concluída.
        btnVoltar.Enabled = etapa != EtapaEndereco && etapa != EtapaConcluido;

        if (etapa == EtapaRevisao)
            btnAvancar.Text = "Confirmar compra";
        else if (etapa == EtapaConcluido)
            btnAvancar.Text = "Fechar";
        else
            btnAvancar.Text = "Avançar";
    }

    // Pix e Boleto não pedem dados. No cartão, confere o titular e o número.
    private bool CartaoEhValido()
    {
        if (!rdoCartao.Checked)
            return true;

        if (txtTitular.Text.Trim() == "")
        {
            MessageBox.Show("Informe o nome do titular do cartão.");
            return false;
        }

        string numero = NumeroDoCartao();
        if (numero.Length < 13 || numero.Length > 19 || !numero.All(char.IsDigit))
        {
            MessageBox.Show("Número do cartão inválido.");
            return false;
        }

        return true;
    }

    // Número do cartão sem espaços e traços.
    private string NumeroDoCartao()
    {
        return txtNumeroCartao.Text.Replace(" ", "").Replace("-", "");
    }

    private string UltimosDigitosDoCartao()
    {
        string numero = NumeroDoCartao();
        return numero.Substring(numero.Length - 4);
    }

    private OpcaoEntrega EntregaEscolhida()
    {
        return _opcoesEntrega[lstEntregas.SelectedIndex];
    }

    // Cria o pagamento da forma escolhida (herança). O valor é preenchido ao finalizar.
    private Pagamento CriarPagamento()
    {
        if (rdoBoleto.Checked)
            return new PagamentoBoleto();

        if (rdoCartao.Checked)
        {
            PagamentoCartao cartao = new PagamentoCartao();
            cartao.NomeTitular = txtTitular.Text.Trim();
            cartao.UltimosDigitos = UltimosDigitosDoCartao();
            cartao.Parcelas = (int)nudParcelas.Value;
            return cartao;
        }

        return new PagamentoPix();
    }

    private string MontarRevisao()
    {
        decimal subtotal = _sessao.Carrinho.CalcularSubtotal();
        decimal frete = _pedidos.CalcularFrete(_sessao.Carrinho, EntregaEscolhida());
        decimal desconto = _pedidos.CalcularDesconto(_sessao.Carrinho);

        string pagamento = "Pix";
        if (rdoBoleto.Checked)
            pagamento = "Boleto bancário";
        else if (rdoCartao.Checked)
            pagamento = "Cartão final " + UltimosDigitosDoCartao() + " em " + nudParcelas.Value + "x";

        string texto = "";
        texto += "Cliente: " + _sessao.Cliente.Nome + " - " + _sessao.Cliente.DescreverDocumento() + "\r\n";
        texto += "Endereço: " + lstEnderecos.SelectedItem + "\r\n";
        texto += "Entrega: " + EntregaEscolhida() + "\r\n";
        texto += "Pagamento: " + pagamento + "\r\n\r\n";

        texto += "Produtos:\r\n";
        foreach (PedidoItem item in _sessao.Carrinho.Itens)
            texto += "  " + item.Quantidade + "x " + item.Produto?.Nome + " - " + item.CalcularSubtotal().ToString("C") + "\r\n";

        texto += "\r\nSubtotal: " + subtotal.ToString("C") + "\r\n";
        texto += "Frete: " + frete.ToString("C") + "\r\n";
        texto += "Desconto: " + desconto.ToString("C") + "\r\n";
        texto += "Total: " + (subtotal + frete - desconto).ToString("C") + "\r\n";
        return texto;
    }

    private void ConfirmarCompra()
    {
        Endereco? endereco = lstEnderecos.SelectedItem as Endereco;
        if (endereco == null)
            return;

        try
        {
            Pedido pedido = _pedidos.Finalizar(_sessao.Cliente.Id, endereco.Id, EntregaEscolhida(), CriarPagamento(), _sessao.Carrinho);

            string texto = "Pedido nº " + pedido.Id + " realizado com sucesso!\r\n\r\n";
            texto += "Pagamento: " + pedido.Pagamento?.Descricao + "\r\n";
            texto += "Entrega: " + EntregaEscolhida() + "\r\n";
            texto += "Total pago: " + pedido.CalcularTotal().ToString("C") + "\r\n";
            txtConcluido.Text = texto;

            MostrarEtapa(EtapaConcluido);
        }
        catch (EstoqueInsuficienteException ex)   // alguém comprou a peça antes
        {
            MessageBox.Show(ex.Message);
        }
        catch (InvalidOperationException ex)      // carrinho vazio, parcelas inválidas...
        {
            MessageBox.Show(ex.Message);
        }
    }
}
