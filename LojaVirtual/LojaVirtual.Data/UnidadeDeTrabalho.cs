using LojaVirtual.Model.Repositorios;

namespace LojaVirtual.Data;

public class UnidadeDeTrabalho : IUnidadeDeTrabalho
{
    private readonly LojaDbContext _contexto;

    public UnidadeDeTrabalho(LojaDbContext contexto) => _contexto = contexto;

    /// <summary>SaveChanges do EF já roda dentro de uma transação.</summary>
    public void SalvarAlteracoes() => _contexto.SaveChanges();
}
