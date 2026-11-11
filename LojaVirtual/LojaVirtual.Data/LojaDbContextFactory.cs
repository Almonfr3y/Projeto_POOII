using Microsoft.EntityFrameworkCore.Design;

namespace LojaVirtual.Data;

/// <summary>Usada pelas ferramentas do EF (Add-Migration / Update-Database).</summary>
public class LojaDbContextFactory : IDesignTimeDbContextFactory<LojaDbContext>
{
    public LojaDbContext CreateDbContext(string[] args) => new(ConfiguracaoBanco.CriarOpcoes());
}
