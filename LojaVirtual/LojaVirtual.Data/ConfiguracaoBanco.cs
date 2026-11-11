using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Data;

public static class ConfiguracaoBanco
{
    private const string VariavelDeAmbiente = "LOJA_VIRTUAL_DB";
    private const string ConexaoPadrao = "server=localhost;port=3306;database=loja_pecas;user=root;password=;";

    /// <summary>Versão fixa: evita que as Migrations precisem conectar no banco só para detectar a versão.</summary>
    public static readonly MySqlServerVersion VersaoServidor = new(new Version(8, 0, 36));

    /// <summary>Lida da variável de ambiente LOJA_VIRTUAL_DB, ou usa o padrão local.</summary>
    public static string StringDeConexao =>
        Environment.GetEnvironmentVariable(VariavelDeAmbiente) ?? ConexaoPadrao;

    public static DbContextOptions<LojaDbContext> CriarOpcoes() =>
        new DbContextOptionsBuilder<LojaDbContext>()
            .UseMySql(StringDeConexao, VersaoServidor)
            .Options;
}
