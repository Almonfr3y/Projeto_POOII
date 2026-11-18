using System.Windows;                 // classes do WPF (Window, MessageBox...)
using System.Windows.Controls;        // controles do WPF (SelectionChangedEventArgs)
using LojaVirtual.Model.Excecoes;     // para usar EstoqueInsuficienteException

namespace LojaVirtual.View;

// Tela Carrinho: alterar quantidade, remover item, ver subtotal e finalizar a compra.
public partial class FrmCarrinho : Window
{
    private SessaoDoUsuario _sessao;   // o carrinho está dentro da sessão

    public FrmCarrinho(SessaoDoUsuario sessao)
    {
        _sessao = sessao;
        InitializeComponent();
    }

    // Executa quando a tela abre.
    private void FrmCarrinho_Loaded(object sender, RoutedEventArgs e)
    {
        AtualizarItens();
    }

    // Ao clicar numa linha, o campo de quantidade mostra a quantidade daquele item.
    private void DgItens_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (dgItens.SelectedIndex >= 0)   // a linha está na mesma posição da lista de itens do carrinho
            txtQuantidade.Text = _sessao.Carrinho.Itens[dgItens.SelectedIndex].Quantidade.ToString();
    }

    // Clique em "Alterar": muda a quantidade do item selecionado.
    private void BtnAlterar_Click(object sender, RoutedEventArgs e)
    {
        if (dgItens.SelectedIndex < 0)   // nenhuma linha selecionada
            return;

        // int.TryParse: tenta converter o texto em número; devolve false se não for um número.
        int quantidade;
        if (!int.TryParse(txtQuantidade.Text, out quantidade) || quantidade < 1)
        {
            MessageBox.Show("Digite uma quantidade válida (1 ou mais).");
            return;
        }

        int produtoId = _sessao.Carrinho.Itens[dgItens.SelectedIndex].ProdutoId;   // produto da linha
        try
        {
            _sessao.Carrinho.AlterarQuantidade(produtoId, quantidade);   // o carrinho confere o estoque
            AtualizarItens();
        }
        catch (EstoqueInsuficienteException ex)   // pediu mais do que tem
        {
            MessageBox.Show(ex.Message);
        }
    }

    // Clique em "Remover do carrinho".
    private void BtnRemover_Click(object sender, RoutedEventArgs e)
    {
        if (dgItens.SelectedIndex < 0)
            return;

        int produtoId = _sessao.Carrinho.Itens[dgItens.SelectedIndex].ProdutoId;
        _sessao.Carrinho.Remover(produtoId);
        AtualizarItens();
    }

    // Clique em "Finalizar Compra": abre o checkout.
    private void BtnFinalizar_Click(object sender, RoutedEventArgs e)
    {
        FrmCheckout checkout = new FrmCheckout(_sessao);
        checkout.Owner = this;     // abre centralizado sobre esta tela
        checkout.ShowDialog();     // espera o checkout fechar

        // Se a compra deu certo o carrinho ficou vazio e esta tela pode fechar.
        if (_sessao.Carrinho.Itens.Count == 0)
            Close();
        else
            AtualizarItens();
    }

    // Mostra os itens do carrinho na grade e atualiza o subtotal.
    private void AtualizarItens()
    {
        dgItens.ItemsSource = _sessao.Carrinho.Itens.Select(i => new
        {
            Peça = i.Produto == null ? "" : i.Produto.Nome,
            i.Quantidade,
            PreçoUnitário = i.PrecoUnitario.ToString("C"),
            Subtotal = i.CalcularSubtotal().ToString("C")
        }).ToList();

        lblSubtotal.Text = "Subtotal: " + _sessao.Carrinho.CalcularSubtotal().ToString("C");
        btnFinalizar.IsEnabled = _sessao.Carrinho.Itens.Count > 0;   // sem itens não dá para finalizar
    }
}
