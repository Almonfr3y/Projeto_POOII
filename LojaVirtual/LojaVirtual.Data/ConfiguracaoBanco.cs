using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Data;

/// <summary>Onde fica o banco e como o EF se conecta a ele.</summary>
public static class ConfiguracaoBanco
{
    // Para usar outro banco/senha, defina a variável de ambiente LOJA_VIRTUAL_DB com a string de conexão.
    private const string VariavelDeAmbiente = "LOJA_VIRTUAL_DB";
    private const string ConexaoPadrao = "server=localhost;port=3306;database=loja_pecas;user=root;password=;";

    // Versão do MySQL informada direto: assim o Add-Migration funciona mesmo com o banco desligado.
    private static readonly MySqlServerVersion VersaoServidor = new(new Version(8, 0, 36));

    /// <summary>A string de conexão da variável de ambiente ou, se ela não existir, a padrão.</summary>
    public static string StringDeConexao =>
        Environment.GetEnvironmentVariable(VariavelDeAmbiente) ?? ConexaoPadrao;

    /// <summary>Opções usadas para criar cada LojaDbContext (provedor Pomelo para MySQL).</summary>
    public static DbContextOptions<LojaDbContext> CriarOpcoes() =>
        new DbContextOptionsBuilder<LojaDbContext>()
            .UseMySql(StringDeConexao, VersaoServidor)
            .Options;
}
