namespace LojaVirtual.Model.Repositorios;

/// <summary>
/// Os repositórios só preparam as alterações; esta interface grava tudo de uma vez.
/// Assim pedido, itens, pagamento e baixa de estoque são salvos numa única transação.
/// </summary>
public interface IUnidadeDeTrabalho
{
    void SalvarAlteracoes();
}
