using System.ComponentModel;
using LojaVirtual.Controller.Composicao;
using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Excecoes;

namespace LojaVirtual.View;

/// <summary>Tela "Carrinho": itens, alterar quantidade, remover, subtotal e "Finalizar Compra".</summary>
public partial class FrmCarrinho : Form
{
    private readonly SessaoDoUsuario _sessao;
    private readonly FabricaDeControladores _fabrica;

    public FrmCarrinho(SessaoDoUsuario sessao, FabricaDeControladores fabrica)
    {
        _sessao = sessao;
        _fabrica = fabrica;
        InitializeComponent();
    }

    private void FrmCarrinho_Load(object sender, EventArgs e) => AtualizarItens();

    private void DgvItens_SelectionChanged(object sender, EventArgs e)
    {
        if (ItemSelecionado() is LinhaCarrinho item)
            nudQuantidade.Value = item.Quantidade;
    }

    private void BtnAlterar_Click(object sender, EventArgs e)
    {
        if (ItemSelecionado() is not LinhaCarrinho item)
            return;

        try
        {
            _sessao.Carrinho.AlterarQuantidade(item.ProdutoId, (int)nudQuantidade.Value);
            AtualizarItens();
        }
        catch (EstoqueInsuficienteException ex)
        {
            MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnRemover_Click(object sender, EventArgs e)
    {
        if (ItemSelecionado() is not LinhaCarrinho item)
            return;

        _sessao.Carrinho.Remover(item.ProdutoId);
        AtualizarItens();
    }

    private void BtnFinalizar_Click(object sender, EventArgs e)
    {
        if (_sessao.Carrinho.Carrinho.EstaVazio)
        {
            MessageBox.Show("O carrinho está vazio.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var checkout = new FrmCheckout(_fabrica.CriarPedidoController(), _sessao);
        if (checkout.ShowDialog(this) == DialogResult.OK)
            Close();
        else
            AtualizarItens();
    }

    private LinhaCarrinho? ItemSelecionado() => dgvItens.CurrentRow?.DataBoundItem as LinhaCarrinho;

    private void AtualizarItens()
    {
        dgvItens.DataSource = _sessao.Carrinho.Carrinho.Itens.Select(LinhaCarrinho.De).ToList();
        lblSubtotal.Text = $"Subtotal: {_sessao.Carrinho.CalcularSubtotal():C}";
        btnFinalizar.Enabled = !_sessao.Carrinho.Carrinho.EstaVazio;
    }

    private sealed record LinhaCarrinho(
        [property: Browsable(false)] int ProdutoId,
        [property: DisplayName("Peça")] string Produto,
        [property: DisplayName("Quantidade")] int Quantidade,
        [property: DisplayName("Preço unitário")] string PrecoUnitario,
        [property: DisplayName("Subtotal")] string Subtotal)
    {
        public static LinhaCarrinho De(PedidoItem item) =>
            new(item.ProdutoId, item.Produto?.Nome ?? "-", item.Quantidade,
                item.PrecoUnitario.ToString("C"), item.CalcularSubtotal().ToString("C"));
    }
}
