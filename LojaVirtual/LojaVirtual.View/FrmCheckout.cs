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
/// Cada etapa é um painel; só um fica visível por vez. A tela só coleta as escolhas:
/// a regra de finalização fica no PedidoService (SRP).
/// </summary>
public partial class FrmCheckout : Form
{
    private const int DigitosMinimosDoCartao = 13;   // menor número de cartão válido
    private const int DigitosMaximosDoCartao = 19;   // maior número de cartão válido
    private const int DigitosVisiveisDoCartao = 4;   // só os 4 últimos são mostrados e guardados

    // As etapas em ordem. O número de cada uma (0 a 4) é usado para avançar e voltar.
    private enum Etapa { Endereco, Entrega, Pagamento, Revisao, Concluido }

    private static readonly string[] NomesDasEtapas = { "Endereço", "Entrega", "Pagamento", "Revisão", "Concluído" };

    private readonly PedidoController _pedidos;
    private readonly SessaoDoUsuario _sessao;
    private Etapa _etapa = Etapa.Endereco;
    private bool _compraConcluida;   // vira true quando o pedido é gravado

    public FrmCheckout(PedidoController pedidos, SessaoDoUsuario sessao)
    {
        _pedidos = pedidos;
        _sessao = sessao;
        InitializeComponent();
    }

    // Atalhos para não repetir caminhos longos.
    private CarrinhoDeCompras Carrinho => _sessao.Carrinho.Carrinho;
    private Endereco? EnderecoEscolhido => lstEnderecos.SelectedItem as Endereco;
    private OpcaoComFrete? EntregaEscolhida => lstEntregas.SelectedItem as OpcaoComFrete;
    private string FinalDoCartao => SomenteDigitos(txtNumeroCartao.Text)[^DigitosVisiveisDoCartao..]; // ^4.. = 4 últimos

    private void FrmCheckout_Load(object sender, EventArgs e)
    {
        lstEnderecos.DataSource = _pedidos.ListarEnderecos(_sessao.Cliente.Id);

        // Cada opção de entrega já aparece com o frete calculado para este carrinho.
        lstEntregas.DataSource = _pedidos.ListarOpcoesEntrega()
            .Select(opcao => new OpcaoComFrete(opcao, _pedidos.SimularFrete(Carrinho, opcao)))
            .ToList();

        nudParcelas.Maximum = PagamentoCartao.MaximoDeParcelas;
        rdoPix.Checked = true;            // Pix vem marcado
        MostrarEtapa(Etapa.Endereco);
    }

    // Ao fechar a janela depois da compra, avisa o carrinho que deu certo (ele também fecha).
    private void FrmCheckout_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (_compraConcluida)
            DialogResult = DialogResult.OK;
    }

    // Os campos do cartão só ficam habilitados com "Cartão" marcado.
    private void FormaDePagamento_CheckedChanged(object sender, EventArgs e) => grpCartao.Enabled = rdoCartao.Checked;

    private void BtnVoltar_Click(object sender, EventArgs e) => MostrarEtapa(_etapa - 1); // etapa anterior

    /// <summary>Botão principal: avança se a etapa atual estiver preenchida; na Revisão, confirma a compra.</summary>
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

    /// <summary>Mostra só o painel da etapa e ajusta título e botões.</summary>
    private void MostrarEtapa(Etapa etapa)
    {
        _etapa = etapa;
        pnlEndereco.Visible = etapa == Etapa.Endereco;
        pnlEntrega.Visible = etapa == Etapa.Entrega;
        pnlPagamento.Visible = etapa == Etapa.Pagamento;
        pnlRevisao.Visible = etapa == Etapa.Revisao;
        pnlConcluido.Visible = etapa == Etapa.Concluido;

        lblEtapa.Text = $"Etapa {(int)etapa + 1} de {NomesDasEtapas.Length} — {NomesDasEtapas[(int)etapa]}";
        btnVoltar.Enabled = etapa is Etapa.Entrega or Etapa.Pagamento or Etapa.Revisao; // não volta da 1ª nem depois de comprar
        btnAvancar.Text = etapa switch
        {
            Etapa.Revisao => "Confirmar compra",
            Etapa.Concluido => "Fechar",
            _ => "Avançar",
        };
    }

    /// <summary>Pix e Boleto não pedem dados; no Cartão, confere titular e número.</summary>
    private bool PagamentoEhValido()
    {
        if (!rdoCartao.Checked)
            return true;

        int digitos = SomenteDigitos(txtNumeroCartao.Text).Length;
        if (string.IsNullOrWhiteSpace(txtTitular.Text))
            Mensagem.Aviso(this, "Informe o nome do titular do cartão.");
        else if (digitos is < DigitosMinimosDoCartao or > DigitosMaximosDoCartao)
            Mensagem.Aviso(this, "Número do cartão inválido.");
        else
            return true;

        return false;
    }

    /// <summary>Cria o objeto da forma escolhida (herança). O valor é preenchido pelo PedidoService.</summary>
    private Pagamento CriarPagamento()
    {
        if (rdoBoleto.Checked)
            return new PagamentoBoleto();
        if (rdoCartao.Checked)
            return new PagamentoCartao
            {
                NomeTitular = txtTitular.Text.Trim(),
                UltimosDigitos = FinalDoCartao,
                Parcelas = (int)nudParcelas.Value,
            };

        return new PagamentoPix();
    }

    // Texto da forma de pagamento na Revisão (antes de processar, o boleto ainda não tem vencimento).
    private string DescreverPagamentoEscolhido() =>
        rdoCartao.Checked ? $"Cartão final {FinalDoCartao} em {nudParcelas.Value}x"
        : rdoBoleto.Checked ? "Boleto bancário"
        : "Pix";

    /// <summary>Resumo da etapa Revisão: cliente, endereço, entrega, pagamento, itens e valores.</summary>
    private string MontarRevisao()
    {
        OpcaoComFrete entrega = EntregaEscolhida!;            // "!": já validado no BtnAvancar
        decimal subtotal = Carrinho.CalcularSubtotal();
        decimal desconto = _pedidos.SimularDesconto(Carrinho);
        var texto = new StringBuilder();                      // monta o texto linha a linha

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

    /// <summary>Envia as escolhas para o controlador. Se der erro, mostra o motivo e fica na Revisão.</summary>
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
        catch (EstoqueInsuficienteException ex)   // alguém comprou a peça antes
        {
            Mensagem.Aviso(this, ex.Message);
        }
        catch (InvalidOperationException ex)      // carrinho vazio, opção inexistente, parcelas inválidas
        {
            Mensagem.Aviso(this, ex.Message);
        }
    }

    // Texto da etapa Concluído, com os dados já gravados (número do pedido, código do pagamento).
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

    private static string SomenteDigitos(string texto) => string.Concat(texto.Where(char.IsDigit)); // tira espaços e traços

    /// <summary>Item da lista de Entrega: a opção junto com o frete já simulado para este carrinho.</summary>
    private sealed record OpcaoComFrete(OpcaoEntrega Opcao, decimal Frete)
    {
        public override string ToString() => $"{Opcao} — frete {Frete:C}";
    }
}
