using LojaVirtual.Model.Excecoes;   // para usar EstoqueInsuficienteException

namespace LojaVirtual.View;

// Tela Carrinho: alterar quantidade, remover item, ver subtotal e finalizar a compra.
public partial class FrmCarrinho : Form
{
    private SessaoDoUsuario _sessao;   // o carrinho está dentro da sessão

    public FrmCarrinho(SessaoDoUsuario sessao)
    {
        _sessao = sessao;
        InitializeComponent();
    }

    // Executa quando a tela abre.
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

    // Clique em "Alterar": muda a quantidade do item selecionado.
    private void BtnAlterar_Click(object sender, EventArgs e)
    {
        if (dgvItens.CurrentRow == null)
            return;

        int produtoId = (int)dgvItens.CurrentRow.Cells["Id"].Value;   // Id escondido da linha
        try
        {
            _sessao.Carrinho.AlterarQuantidade(produtoId, (int)nudQuantidade.Value);   // o carrinho confere o estoque
            AtualizarItens();
        }
        catch (EstoqueInsuficienteException ex)   // pediu mais do que tem
        {
            MessageBox.Show(ex.Message);
        }
    }

    // Clique em "Remover do carrinho".
    private void BtnRemover_Click(object sender, EventArgs e)
    {
        if (dgvItens.CurrentRow == null)
            return;

        int produtoId = (int)dgvItens.CurrentRow.Cells["Id"].Value;
        _sessao.Carrinho.Remover(produtoId);
        AtualizarItens();
    }

    // Clique em "Finalizar Compra": abre o checkout.
    private void BtnFinalizar_Click(object sender, EventArgs e)
    {
        FrmCheckout checkout = new FrmCheckout(_sessao);
        checkout.ShowDialog();   // espera o checkout fechar

        // Se a compra deu certo o carrinho ficou vazio e esta tela pode fechar.
        if (_sessao.Carrinho.Itens.Count == 0)
            Close();
        else
            AtualizarItens();
    }

    // Mostra os itens do carrinho na grade e atualiza o subtotal.
    private void AtualizarItens()
    {
        dgvItens.DataSource = _sessao.Carrinho.Itens.Select(i => new
        {
            Id = i.ProdutoId,                                     // escondido, usado nos botões
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
