using LojaVirtual.Model.Carrinho;
using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Repositorios;

namespace LojaVirtual.Controller;

/// <summary>Carrinho. Existe uma instância só, guardada na sessão e usada por todas as telas.</summary>
public class CarrinhoController
{
    private readonly IProdutoRepository _produtos;

    public CarrinhoController(IProdutoRepository produtos) => _produtos = produtos;

    public CarrinhoDeCompras Carrinho { get; } = new();

    public void Adicionar(int produtoId, int quantidade)
    {
        // Lê sem rastreamento: o carrinho vive a sessão inteira e precisa ver o estoque atual.
        Produto produto = _produtos.ObterParaConsulta(produtoId)
            ?? throw new InvalidOperationException("Produto não encontrado.");
        Carrinho.Adicionar(produto, quantidade); // pode lançar EstoqueInsuficienteException
    }

    public void AlterarQuantidade(int produtoId, int quantidade) => Carrinho.AlterarQuantidade(produtoId, quantidade);

    public void Remover(int produtoId) => Carrinho.Remover(produtoId);

    public decimal CalcularSubtotal() => Carrinho.CalcularSubtotal();
}
