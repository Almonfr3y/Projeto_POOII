using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Excecoes;

namespace LojaVirtual.Model.Carrinho;

/// <summary>Carrinho em memória: uma coleção de PedidoItem. Não é gravado no banco.</summary>
public class CarrinhoDeCompras
{
    private readonly List<PedidoItem> _itens = new();

    // Fora da classe a lista é só leitura: os itens só mudam pelos métodos abaixo (encapsulamento).
    public IReadOnlyList<PedidoItem> Itens => _itens;

    public bool EstaVazio => _itens.Count == 0;

    /// <summary>Adiciona o produto ou, se ele já está no carrinho, soma a quantidade.</summary>
    public void Adicionar(Produto produto, int quantidade)
    {
        PedidoItem? existente = BuscarItem(produto.Id);
        int quantidadeFinal = (existente?.Quantidade ?? 0) + quantidade;
        GarantirEstoqueDisponivel(produto, quantidadeFinal);

        if (existente is null)
            _itens.Add(new PedidoItem { Produto = produto, ProdutoId = produto.Id, Quantidade = quantidade, PrecoUnitario = produto.Preco });
        else
            existente.Quantidade = quantidadeFinal;
    }

    /// <summary>Muda a quantidade; zero ou menos remove o item.</summary>
    public void AlterarQuantidade(int produtoId, int novaQuantidade)
    {
        PedidoItem item = BuscarItem(produtoId) ?? throw new InvalidOperationException("Produto não está no carrinho.");

        if (novaQuantidade <= 0)
        {
            _itens.Remove(item);
            return;
        }

        GarantirEstoqueDisponivel(item.Produto!, novaQuantidade);
        item.Quantidade = novaQuantidade;
    }

    public void Remover(int produtoId) => _itens.RemoveAll(item => item.ProdutoId == produtoId);

    public void Limpar() => _itens.Clear();

    public decimal CalcularSubtotal() => _itens.Sum(item => item.CalcularSubtotal());

    private PedidoItem? BuscarItem(int produtoId) => _itens.Find(item => item.ProdutoId == produtoId);

    // Não deixa o carrinho pedir mais peças do que o estoque tem.
    private static void GarantirEstoqueDisponivel(Produto produto, int quantidade)
    {
        if (quantidade > produto.Estoque)
            throw new EstoqueInsuficienteException(produto, quantidade);
    }
}
