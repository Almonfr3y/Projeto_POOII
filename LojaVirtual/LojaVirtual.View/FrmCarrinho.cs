using LojaVirtual.Model.Excecoes;

namespace LojaVirtual.View;

// Tela Carrinho: alterar quantidade, remover item, ver subtotal e finalizar a compra.
public partial class FrmCarrinho : Form
{
    private SessaoDoUsuario _sessao;

    public FrmCarrinho(SessaoDoUsuario sessao)
    {
        _sessao = sessao;
        InitializeComponent();
    }

    private void FrmCarrinho_Load(object sender, EventArgs e)
    {
        AtualizarItens();
    }

    // Ao clicar numa linha, o campo de quantidade mostra a quantidade daquele item.
    private void DgvItens_SelectionChanged(object sender, EventArgs e)
    {
        if (dgvItens.CurrentRow != null)
            nudQuantidade.Value = (int)dgvItens.CurrentRow.Cells["Quantidade"].Value;
    }

    private void BtnAlterar_Click(object sender, EventArgs e)
    {
        if (dgvItens.CurrentRow == null)
            return;

        int produtoId = (int)dgvItens.CurrentRow.Cells["Id"].Value;
        try
        {
            _sessao.Carrinho.AlterarQuantidade(produtoId, (int)nudQuantidade.Value);
            AtualizarItens();
        }
        catch (EstoqueInsuficienteException ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void BtnRemover_Click(object sender, EventArgs e)
    {
        if (dgvItens.CurrentRow == null)
            return;

        int produtoId = (int)dgvItens.CurrentRow.Cells["Id"].Value;
        _sessao.Carrinho.Remover(produtoId);
        AtualizarItens();
    }

    private void BtnFinalizar_Click(object sender, EventArgs e)
    {
        FrmCheckout checkout = new FrmCheckout(_sessao);
        checkout.ShowDialog();

        // Se a compra deu certo o carrinho ficou vazio e esta tela pode fechar.
        if (_sessao.Carrinho.Itens.Count == 0)
            Close();
        else
            AtualizarItens();
    }

    private void AtualizarItens()
    {
        dgvItens.DataSource = _sessao.Carrinho.Itens.Select(i => new
        {
            Id = i.ProdutoId,
            Peça = i.Produto == null ? "" : i.Produto.Nome,
            i.Quantidade,
            PreçoUnitário = i.PrecoUnitario.ToString("C"),
            Subtotal = i.CalcularSubtotal().ToString("C")
        }).ToList();
        dgvItens.Columns["Id"].Visible = false;

        lblSubtotal.Text = "Subtotal: " + _sessao.Carrinho.CalcularSubtotal().ToString("C");
        btnFinalizar.Enabled = _sessao.Carrinho.Itens.Count > 0;   // sem itens não dá para finalizar
    }
}
