using LojaVirtual.Model.Repositorios;

namespace LojaVirtual.Data;

/// <summary>
/// Grava de uma vez tudo o que os repositórios prepararam no mesmo contexto.
/// O SaveChanges do EF roda numa transação: ou grava tudo, ou nada.
/// </summary>
public class UnidadeDeTrabalho : IUnidadeDeTrabalho
{
    private readonly LojaDbContext _contexto;

    public UnidadeDeTrabalho(LojaDbContext contexto) => _contexto = contexto;

    public void SalvarAlteracoes() => _contexto.SaveChanges();
}
