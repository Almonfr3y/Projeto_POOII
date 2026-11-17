using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Repositorios;

namespace LojaVirtual.Controller;

/// <summary>Tela "Favoritos" (N×N Cliente—Produto).</summary>
public class FavoritosController
{
    private readonly IClienteRepository _clientes;
    private readonly IProdutoRepository _produtos;
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

    public FavoritosController(IClienteRepository clientes, IProdutoRepository produtos, IUnidadeDeTrabalho unidadeDeTrabalho)
    {
        _clientes = clientes;
        _produtos = produtos;
        _unidadeDeTrabalho = unidadeDeTrabalho;
    }

    /// <summary>Favoritos do cliente em ordem alfabética, com filtro opcional ("pesquise favorito").</summary>
    public IReadOnlyList<Produto> Listar(int clienteId, string? textoBusca = null)
    {
        IEnumerable<Produto> favoritos = ObterCliente(clienteId).Favoritos;
        if (!string.IsNullOrWhiteSpace(textoBusca))
            favoritos = favoritos.Where(p => p.Nome.Contains(textoBusca, StringComparison.OrdinalIgnoreCase)); // ignora maiúsculas

        return favoritos.OrderBy(p => p.Nome).ToList();
    }

    public void Favoritar(int clienteId, int produtoId)
    {
        Produto produto = _produtos.ObterPorId(produtoId) ?? throw new InvalidOperationException("Produto não encontrado.");
        ObterCliente(clienteId).Favoritar(produto);
        _unidadeDeTrabalho.SalvarAlteracoes(); // insere a linha em tb_favorito
    }

    public void Desfavoritar(int clienteId, int produtoId)
    {
        ObterCliente(clienteId).Desfavoritar(produtoId);
        _unidadeDeTrabalho.SalvarAlteracoes(); // apaga a linha de tb_favorito
    }

    private Pessoa ObterCliente(int clienteId) =>
        _clientes.ObterComDetalhes(clienteId) ?? throw new InvalidOperationException("Cliente não encontrado.");
}
