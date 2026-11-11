using LojaVirtual.Model.Carrinho;
using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Repositorios;

namespace LojaVirtual.Controller;

/// <summary>Tela de Carrinho. Uma instância por sessão, compartilhada entre as telas.</summary>
public class CarrinhoController
{
    private readonly IProdutoRepository _produtos;

    public CarrinhoController(IProdutoRepository produtos) => _produtos = produtos;

    public CarrinhoDeCompras Carrinho { get; } = new();

    public void Adicionar(int produtoId, int quantidade)
    {
        Produto produto = _produtos.ObterPorId(produtoId)
            ?? throw new InvalidOperationException("Produto não encontrado.");
        Carrinho.Adicionar(produto, quantidade);
    }

    public void AlterarQuantidade(int produtoId, int quantidade) => Carrinho.AlterarQuantidade(produtoId, quantidade);

    public void Remover(int produtoId) => Carrinho.Remover(produtoId);

    public decimal CalcularSubtotal() => Carrinho.CalcularSubtotal();
}
