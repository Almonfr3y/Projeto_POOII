using LojaVirtual.Model.Entidades;   // para usar Produto e PedidoItem
using LojaVirtual.Model.Excecoes;    // para usar EstoqueInsuficienteException

namespace LojaVirtual.Model.Carrinho;

// Carrinho de compras. Fica só na memória (não é gravado no banco).
public class CarrinhoDeCompras
{
    // A lista de itens. "private set": de fora ninguém troca a lista inteira (encapsulamento).
    public List<PedidoItem> Itens { get; private set; } = new List<PedidoItem>();

    // Coloca um produto no carrinho, conferindo o estoque.
    public void Adicionar(Produto produto, int quantidade)
    {
        PedidoItem? item = BuscarItem(produto.Id);   // o produto já está no carrinho?

        if (item == null)   // ainda não está: cria um item novo
        {
            if (quantidade > produto.Estoque)   // pediu mais do que tem
                throw new EstoqueInsuficienteException(produto, quantidade);

            item = new PedidoItem();              // cria o item
            item.Produto = produto;               // guarda o produto
            item.ProdutoId = produto.Id;          // e o id dele
            item.Quantidade = quantidade;         // quantas unidades
            item.PrecoUnitario = produto.Preco;   // preço atual
            Itens.Add(item);                      // coloca na lista
        }
        else
        {
            // O produto já está no carrinho: só soma a quantidade.
            int novaQuantidade = item.Quantidade + quantidade;
            if (novaQuantidade > produto.Estoque)   // a soma passaria do estoque
                throw new EstoqueInsuficienteException(produto, novaQuantidade);

            item.Quantidade = novaQuantidade;   // atualiza a quantidade
        }
    }

    // Muda a quantidade de um item que já está no carrinho.
    public void AlterarQuantidade(int produtoId, int novaQuantidade)
    {
        PedidoItem? item = BuscarItem(produtoId);   // procura o item
        if (item == null || item.Produto == null)   // não achou: não faz nada
            return;

        if (novaQuantidade <= 0)
        {
            Itens.Remove(item);   // quantidade zero remove o item
            return;
        }

        if (novaQuantidade > item.Produto.Estoque)   // passaria do estoque
            throw new EstoqueInsuficienteException(item.Produto, novaQuantidade);

        item.Quantidade = novaQuantidade;   // atualiza a quantidade
    }

    // Tira um produto do carrinho.
    public void Remover(int produtoId)
    {
        PedidoItem? item = BuscarItem(produtoId);   // procura o item
        if (item != null)                           // se achou...
            Itens.Remove(item);                     // ...remove da lista
    }

    // Esvazia o carrinho (usado depois que a compra dá certo).
    public void Limpar()
    {
        Itens.Clear();   // apaga todos os itens da lista
    }

    // Soma o valor de todos os itens.
    public decimal CalcularSubtotal()
    {
        decimal subtotal = 0;
        foreach (PedidoItem item in Itens)
            subtotal += item.CalcularSubtotal();   // quantidade x preço de cada item
        return subtotal;
    }

    // Peso de todas as peças, usado para calcular o frete.
    public decimal CalcularPesoTotal()
    {
        decimal peso = 0;
        foreach (PedidoItem item in Itens)
        {
            if (item.Produto != null)                          // segurança: o item tem produto
                peso += item.Produto.PesoKg * item.Quantidade; // peso da peça x quantidade
        }
        return peso;
    }

    // Procura o item de um produto. Devolve null se não estiver no carrinho.
    // private: só esta classe usa este método.
    private PedidoItem? BuscarItem(int produtoId)
    {
        foreach (PedidoItem item in Itens)
        {
            if (item.ProdutoId == produtoId)
                return item;   // achou
        }
        return null;   // não achou
    }
}
