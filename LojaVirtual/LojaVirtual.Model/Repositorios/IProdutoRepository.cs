using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Repositorios;

public interface IProdutoRepository
{
    /// <summary>Lista para exibição: sempre lê o estoque atual do banco.</summary>
    IReadOnlyList<Produto> Listar(int? categoriaId = null, string? textoBusca = null);
    /// <summary>Produto rastreado: alterações (estoque, favoritos) são gravadas em SalvarAlteracoes.</summary>
    Produto? ObterPorId(int id);

    /// <summary>Leitura sempre atualizada do banco, sem rastreamento (ex.: estoque visto pelo carrinho).</summary>
    Produto? ObterParaConsulta(int id);
}
