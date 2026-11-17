using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Excecoes;

namespace LojaVirtual.Model.Carrinho;

// Carrinho de compras. Fica só na memória (não é gravado no banco).
public class CarrinhoDeCompras
{
    public List<PedidoItem> Itens { get; private set; } = new List<PedidoItem>();

    public void Adicionar(Produto produto, int quantidade)
    {
        PedidoItem? item = BuscarItem(produto.Id);

        if (item == null)
        {
            if (quantidade > produto.Estoque)
                throw new EstoqueInsuficienteException(produto, quantidade);

            item = new PedidoItem();
            item.Produto = produto;
            item.ProdutoId = produto.Id;
            item.Quantidade = quantidade;
            item.PrecoUnitario = produto.Preco;
            Itens.Add(item);
        }
        else
        {
            // O produto já está no carrinho: só soma a quantidade.
            int novaQuantidade = item.Quantidade + quantidade;
            if (novaQuantidade > produto.Estoque)
                throw new EstoqueInsuficienteException(produto, novaQuantidade);

            item.Quantidade = novaQuantidade;
        }
    }

    public void AlterarQuantidade(int produtoId, int novaQuantidade)
    {
        PedidoItem? item = BuscarItem(produtoId);
        if (item == null || item.Produto == null)
            return;

        if (novaQuantidade <= 0)
        {
            Itens.Remove(item);   // quantidade zero remove o item
            return;
        }

        if (novaQuantidade > item.Produto.Estoque)
            throw new EstoqueInsuficienteException(item.Produto, novaQuantidade);

        item.Quantidade = novaQuantidade;
    }

    public void Remover(int produtoId)
    {
        PedidoItem? item = BuscarItem(produtoId);
        if (item != null)
            Itens.Remove(item);
    }

    public void Limpar()
    {
        Itens.Clear();
    }

    public decimal CalcularSubtotal()
    {
        decimal subtotal = 0;
        foreach (PedidoItem item in Itens)
            subtotal += item.CalcularSubtotal();
        return subtotal;
    }

    // Peso de todas as peças, usado para calcular o frete.
    public decimal CalcularPesoTotal()
    {
        decimal peso = 0;
        foreach (PedidoItem item in Itens)
        {
            if (item.Produto != null)
                peso += item.Produto.PesoKg * item.Quantidade;
        }
        return peso;
    }

    private PedidoItem? BuscarItem(int produtoId)
    {
        foreach (PedidoItem item in Itens)
        {
            if (item.ProdutoId == produtoId)
                return item;
        }
        return null;
    }
}
