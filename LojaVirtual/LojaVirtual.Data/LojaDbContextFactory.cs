using Microsoft.EntityFrameworkCore.Design;

namespace LojaVirtual.Data;

/// <summary>
/// Usada só pelas ferramentas do EF (Add-Migration / Update-Database) para criar o contexto
/// fora do programa. O aplicativo em si não usa esta classe.
/// </summary>
public class LojaDbContextFactory : IDesignTimeDbContextFactory<LojaDbContext>
{
    public LojaDbContext CreateDbContext(string[] args) => new(ConfiguracaoBanco.CriarOpcoes());
}
