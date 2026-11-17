using LojaVirtual.Controller;                    // para usar PedidoController
using LojaVirtual.Model.Descontos;               // para usar DescontoQuantidade
using LojaVirtual.Model.Entidades;               // para usar Endereco, OpcaoEntrega, Pedido...
using LojaVirtual.Model.Entidades.Pagamentos;    // para usar os pagamentos
using LojaVirtual.Model.Excecoes;                // para usar EstoqueInsuficienteException

namespace LojaVirtual.View;

// Checkout em etapas: Endereço -> Entrega -> Pagamento -> Revisão -> Concluído.
// Cada etapa é um painel, e só o painel da etapa atual fica visível.
public partial class FrmCheckout : Form
{
    // Número de cada etapa (constantes em vez de números soltos no código).
    private const int EtapaEndereco = 0;
    private const int EtapaEntrega = 1;
    private const int EtapaPagamento = 2;
    private const int EtapaRevisao = 3;
    private const int EtapaConcluido = 4;

    private string[] _nomesDasEtapas = { "Endereço", "Entrega", "Pagamento", "Revisão", "Concluído" };   // títulos
    private Panel[] _paineis;   // vetor com os 5 painéis, na ordem das etapas
    private int _etapa;         // etapa atual (0 a 4)

    private SessaoDoUsuario _sessao;                                     // cliente e carrinho
    private List<OpcaoEntrega> _opcoesEntrega = new List<OpcaoEntrega>(); // opções na mesma ordem da lista da tela

    // Para trocar a regra de desconto, basta trocar DescontoQuantidade por outra classe (ex.: SemDesconto).
    private PedidoController _pedidos = new PedidoController(new DescontoQuantidade());

    public FrmCheckout(SessaoDoUsuario sessao)
    {
        _sessao = sessao;
        InitializeComponent();   // cria os painéis e controles do Designer
        _paineis = new Panel[] { pnlEndereco, pnlEntrega, pnlPagamento, pnlRevisao, pnlConcluido };   // depois de criados
    }

    // Executa quando a tela abre: carrega endereços e opções de entrega.
    private void FrmCheckout_Load(object sender, EventArgs e)
    {
        lstEnderecos.DataSource = _pedidos.ListarEnderecos(_sessao.Cliente.Id);   // a lista mostra o ToString() de cada Endereco

        // Cada opção de entrega aparece com o frete já calculado para este carrinho.
        _opcoesEntrega = _pedidos.ListarOpcoesEntrega();
        foreach (OpcaoEntrega opcao in _opcoesEntrega)
        {
            decimal frete = _pedidos.CalcularFrete(_sessao.Carrinho, opcao);
            lstEntregas.Items.Add(opcao + " - frete " + frete.ToString("C"));   // ex.: "Padrão (5 dias úteis) - frete R$ 20,70"
        }

        nudParcelas.Maximum = PagamentoCartao.MaximoDeParcelas;   // limite de parcelas vem da classe do cartão
        rdoPix.Checked = true;            // Pix vem marcado
        MostrarEtapa(EtapaEndereco);      // começa na primeira etapa
    }

    // Os campos do cartão só ficam habilitados quando "Cartão" está marcado.
    private void FormaDePagamento_CheckedChanged(object sender, EventArgs e)
    {
        grpCartao.Enabled = rdoCartao.Checked;
    }

    // Clique em "Voltar": vai para a etapa anterior.
    private void BtnVoltar_Click(object sender, EventArgs e)
    {
        MostrarEtapa(_etapa - 1);
    }

    // Clique em "Avançar" (ou "Confirmar compra" / "Fechar", conforme a etapa).
    private void BtnAvancar_Click(object sender, EventArgs e)
    {
        if (_etapa == EtapaEndereco)
        {
            if (lstEnderecos.SelectedItem == null)   // não escolheu endereço
            {
                MessageBox.Show("Escolha um endereço.");
                return;
            }
            MostrarEtapa(EtapaEntrega);
        }
        else if (_etapa == EtapaEntrega)
        {
            if (lstEntregas.SelectedIndex < 0)   // -1 = nada selecionado
            {
                MessageBox.Show("Escolha uma opção de entrega.");
                return;
            }
            MostrarEtapa(EtapaPagamento);
        }
        else if (_etapa == EtapaPagamento)
        {
            if (!CartaoEhValido())   // dados do cartão incompletos
                return;
            txtRevisao.Text = MontarRevisao();   // monta o resumo antes de mostrar a Revisão
            MostrarEtapa(EtapaRevisao);
        }
        else if (_etapa == EtapaRevisao)
        {
            ConfirmarCompra();   // grava o pedido
        }
        else
        {
            Close();   // etapa Concluído: o botão vira "Fechar"
        }
    }

    // Mostra só o painel da etapa e ajusta o título e os botões.
    private void MostrarEtapa(int etapa)
    {
        _etapa = etapa;   // guarda a etapa atual

        for (int i = 0; i < _paineis.Length; i++)
            _paineis[i].Visible = (i == etapa);   // só o painel da etapa fica visível

        lblEtapa.Text = "Etapa " + (etapa + 1) + " de 5 - " + _nomesDasEtapas[etapa];   // ex.: "Etapa 2 de 5 - Entrega"

        // Não dá para voltar da primeira etapa nem depois da compra concluída.
        btnVoltar.Enabled = etapa != EtapaEndereco && etapa != EtapaConcluido;

        // O texto do botão principal muda conforme a etapa.
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
        if (!rdoCartao.Checked)   // não é cartão: nada para conferir
            return true;

        if (txtTitular.Text.Trim() == "")   // titular em branco
        {
            MessageBox.Show("Informe o nome do titular do cartão.");
            return false;
        }

        string numero = NumeroDoCartao();
        // Cartões têm de 13 a 19 dígitos; All(char.IsDigit) confere se todos os caracteres são números.
        if (numero.Length < 13 || numero.Length > 19 || !numero.All(char.IsDigit))
        {
            MessageBox.Show("Número do cartão inválido.");
            return false;
        }

        return true;   // tudo certo
    }

    // Número do cartão sem espaços e traços.
    private string NumeroDoCartao()
    {
        return txtNumeroCartao.Text.Replace(" ", "").Replace("-", "");
    }

    // Os 4 últimos dígitos do cartão (o único pedaço que é guardado).
    private string UltimosDigitosDoCartao()
    {
        string numero = NumeroDoCartao();
        return numero.Substring(numero.Length - 4);   // Substring(início): do início até o fim do texto
    }

    // A opção de entrega escolhida: a posição na lista da tela é a mesma da lista _opcoesEntrega.
    private OpcaoEntrega EntregaEscolhida()
    {
        return _opcoesEntrega[lstEntregas.SelectedIndex];
    }

    // Cria o pagamento da forma escolhida (herança). O valor é preenchido ao finalizar.
    // O retorno é do tipo Pagamento, mas o objeto é Pix, Boleto ou Cartão.
    private Pagamento CriarPagamento()
    {
        if (rdoBoleto.Checked)
            return new PagamentoBoleto();

        if (rdoCartao.Checked)
        {
            PagamentoCartao cartao = new PagamentoCartao();
            cartao.NomeTitular = txtTitular.Text.Trim();
            cartao.UltimosDigitos = UltimosDigitosDoCartao();
            cartao.Parcelas = (int)nudParcelas.Value;   // NumericUpDown devolve decimal: converte para int
            return cartao;
        }

        return new PagamentoPix();   // padrão: Pix
    }

    // Texto da etapa Revisão: cliente, endereço, entrega, pagamento, produtos e valores.
    private string MontarRevisao()
    {
        decimal subtotal = _sessao.Carrinho.CalcularSubtotal();
        decimal frete = _pedidos.CalcularFrete(_sessao.Carrinho, EntregaEscolhida());
        decimal desconto = _pedidos.CalcularDesconto(_sessao.Carrinho);   // mesma regra usada ao finalizar

        // Descrição do pagamento escolhido.
        string pagamento = "Pix";
        if (rdoBoleto.Checked)
            pagamento = "Boleto bancário";
        else if (rdoCartao.Checked)
            pagamento = "Cartão final " + UltimosDigitosDoCartao() + " em " + nudParcelas.Value + "x";

        // Monta o texto linha por linha ("\r\n" = quebra de linha no Windows).
        string texto = "";
        texto += "Cliente: " + _sessao.Cliente.Nome + " - " + _sessao.Cliente.DescreverDocumento() + "\r\n";   // CPF ou CNPJ (polimorfismo)
        texto += "Endereço: " + lstEnderecos.SelectedItem + "\r\n";
        texto += "Entrega: " + EntregaEscolhida() + "\r\n";
        texto += "Pagamento: " + pagamento + "\r\n\r\n";

        texto += "Produtos:\r\n";
        foreach (PedidoItem item in _sessao.Carrinho.Itens)
            texto += "  " + item.Quantidade + "x " + item.Produto?.Nome + " - " + item.CalcularSubtotal().ToString("C") + "\r\n";   // "?." = só lê Nome se Produto não for null

        texto += "\r\nSubtotal: " + subtotal.ToString("C") + "\r\n";
        texto += "Frete: " + frete.ToString("C") + "\r\n";
        texto += "Desconto: " + desconto.ToString("C") + "\r\n";
        texto += "Total: " + (subtotal + frete - desconto).ToString("C") + "\r\n";
        return texto;
    }

    // Envia tudo para o controlador gravar. Se der erro, mostra o motivo e continua na Revisão.
    private void ConfirmarCompra()
    {
        Endereco? endereco = lstEnderecos.SelectedItem as Endereco;   // endereço escolhido na 1ª etapa
        if (endereco == null)
            return;

        try
        {
            Pedido pedido = _pedidos.Finalizar(_sessao.Cliente.Id, endereco.Id, EntregaEscolhida(), CriarPagamento(), _sessao.Carrinho);

            // Comprovante da etapa Concluído, com o número gerado pelo banco.
            string texto = "Pedido nº " + pedido.Id + " realizado com sucesso!\r\n\r\n";
            texto += "Pagamento: " + pedido.Pagamento?.Descricao + "\r\n";   // Descricao de Pix/Boleto/Cartão (polimorfismo)
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
