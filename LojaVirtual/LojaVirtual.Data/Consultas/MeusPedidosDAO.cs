using LojaVirtual.Model;
using MySqlConnector;

namespace LojaVirtual.Data.Consultas;

// Funcionalidade feita com SQL puro (pedido no enunciado): a tela Meus Pedidos
// usa ADO.NET (MySqlConnection, MySqlCommand e MySqlDataReader), sem o Entity Framework.
public class MeusPedidosDAO
{
    public List<ResumoPedido> ListarPorCliente(int clienteId)
    {
        // Junta pedido, itens e pagamento, agrupa por pedido e soma os itens.
        string sql =
            "SELECT p.Id AS Numero, p.Data, p.Status, " +
            "       COALESCE(pg.Forma, '-') AS FormaPagamento, " +
            "       SUM(i.Quantidade) AS QuantidadeItens, " +
            "       SUM(i.Quantidade * i.PrecoUnitario) + p.ValorFrete - p.ValorDesconto AS Total " +
            "FROM tb_pedido p " +
            "JOIN tb_pedido_item i ON i.PedidoId = p.Id " +
            "LEFT JOIN tb_pagamento pg ON pg.PedidoId = p.Id " +
            "WHERE p.ClienteId = @clienteId " +
            "GROUP BY p.Id, p.Data, p.Status, pg.Forma, p.ValorFrete, p.ValorDesconto " +
            "ORDER BY p.Data DESC";

        List<ResumoPedido> pedidos = new List<ResumoPedido>();

        using (MySqlConnection conexao = new MySqlConnection(LojaDbContext.StringDeConexao))
        {
            conexao.Open();

            MySqlCommand comando = new MySqlCommand(sql, conexao);
            comando.Parameters.AddWithValue("@clienteId", clienteId);   // parâmetro: evita SQL injection

            using (MySqlDataReader leitor = comando.ExecuteReader())
            {
                while (leitor.Read())
                {
                    ResumoPedido resumo = new ResumoPedido();
                    resumo.Numero = leitor.GetInt32("Numero");
                    resumo.Data = leitor.GetDateTime("Data");
                    resumo.Status = leitor.GetString("Status");
                    resumo.FormaPagamento = leitor.GetString("FormaPagamento");
                    resumo.QuantidadeItens = Convert.ToInt32(leitor["QuantidadeItens"]);
                    resumo.Total = leitor.GetDecimal("Total");
                    pedidos.Add(resumo);
                }
            }
        }

        return pedidos;
    }
}
