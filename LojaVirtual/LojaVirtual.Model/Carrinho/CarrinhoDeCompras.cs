using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Excecoes;

namespace LojaVirtual.Model.Carrinho;

/// <summary>Carrinho em memória (coleção de PedidoItem). Não é persistido.</summary>
public class CarrinhoDeCompras
{
    private readonly List<PedidoItem> _itens = new();

    public IReadOnlyList<PedidoItem> Itens => _itens;

    public bool EstaVazio => _itens.Count == 0;

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

    public void AlterarQuantidade(int produtoId, int novaQuantidade)
    {
        PedidoItem item = BuscarItem(produtoId)
            ?? throw new InvalidOperationException("Produto não está no carrinho.");

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

    private static void GarantirEstoqueDisponivel(Produto produto, int quantidade)
    {
        if (quantidade > produto.Estoque)
            throw new EstoqueInsuficienteException(produto, quantidade);
    }
}
