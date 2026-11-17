using LojaVirtual.Model.Consultas;
using MySqlConnector;

namespace LojaVirtual.Data.Consultas;

/// <summary>
/// Funcionalidade específica em SQL puro (exigência do enunciado): a tela "Meus Pedidos" usa
/// ADO.NET (MySqlConnection, MySqlCommand e MySqlDataReader), sem passar pelo Entity Framework.
/// </summary>
public class MeusPedidosDAO : IMeusPedidosConsulta
{
    // Junta pedido + itens + pagamento, agrupa por pedido e soma os itens para calcular o total.
    // @clienteId é um parâmetro: o valor nunca é colado no texto do SQL (evita SQL injection).
    private const string SqlPedidosDoCliente = """
        SELECT p.Id                                AS Numero,
               p.Data                              AS Data,
               p.Status                            AS Status,
               COALESCE(pg.Forma, '-')             AS FormaPagamento,
               CAST(SUM(i.Quantidade) AS SIGNED) AS QuantidadeItens,
               SUM(i.Quantidade * i.PrecoUnitario) + p.ValorFrete - p.ValorDesconto AS Total
        FROM tb_pedido p
        JOIN tb_pedido_item i ON i.PedidoId = p.Id
        LEFT JOIN tb_pagamento pg ON pg.PedidoId = p.Id
        WHERE p.ClienteId = @clienteId
        GROUP BY p.Id, p.Data, p.Status, pg.Forma, p.ValorFrete, p.ValorDesconto
        ORDER BY p.Data DESC;
        """;

    private readonly string _stringDeConexao;

    public MeusPedidosDAO(string stringDeConexao) => _stringDeConexao = stringDeConexao;

    public IReadOnlyList<ResumoPedido> ListarPorCliente(int clienteId)
    {
        // "using" fecha a conexão, o comando e o leitor automaticamente no fim do método.
        using var conexao = new MySqlConnection(_stringDeConexao);
        conexao.Open();

        using var comando = new MySqlCommand(SqlPedidosDoCliente, conexao);
        comando.Parameters.AddWithValue("@clienteId", clienteId); // preenche o parâmetro do SQL

        using MySqlDataReader leitor = comando.ExecuteReader();   // executa o SELECT
        var pedidos = new List<ResumoPedido>();
        while (leitor.Read())                                     // uma volta por linha do resultado
        {
            // Cada coluna do SELECT vira um campo do ResumoPedido.
            pedidos.Add(new ResumoPedido(
                leitor.GetInt32("Numero"),
                leitor.GetDateTime("Data"),
                leitor.GetString("Status"),
                leitor.GetString("FormaPagamento"),
                Convert.ToInt32(leitor["QuantidadeItens"]), // SUM vem como número grande; converte para int
                leitor.GetDecimal("Total")));
        }

        return pedidos;
    }
}
