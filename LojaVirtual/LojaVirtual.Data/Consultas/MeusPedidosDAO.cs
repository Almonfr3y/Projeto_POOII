using LojaVirtual.Model;   // para usar ResumoPedido
using MySql.Data.MySqlClient;   // biblioteca ADO.NET do MySQL, da Oracle (MySqlConnection, MySqlCommand...)

namespace LojaVirtual.Data.Consultas;

// Funcionalidade feita com SQL puro (pedido no enunciado): a tela Meus Pedidos
// usa ADO.NET (MySqlConnection, MySqlCommand e MySqlDataReader), sem o Entity Framework.
// DAO = Data Access Object: uma classe só para acessar os dados.
public class MeusPedidosDAO
{
    // Devolve um resumo de cada pedido do cliente.
    public List<ResumoPedido> ListarPorCliente(int clienteId)
    {
        // Junta pedido, itens e pagamento, agrupa por pedido e soma os itens.
        string sql =
            "SELECT p.Id AS Numero, p.Data, p.Status, " +                          // colunas do pedido
            "       COALESCE(pg.Forma, '-') AS FormaPagamento, " +                   // forma de pagamento ("-" se não tiver)
            "       SUM(i.Quantidade) AS QuantidadeItens, " +                        // total de peças
            "       SUM(i.Quantidade * i.PrecoUnitario) + p.ValorFrete - p.ValorDesconto AS Total " +   // valor final
            "FROM tb_pedido p " +                                                    // "p" é o apelido de tb_pedido
            "JOIN tb_pedido_item i ON i.PedidoId = p.Id " +                          // cada pedido com seus itens
            "LEFT JOIN tb_pagamento pg ON pg.PedidoId = p.Id " +                     // LEFT: mantém o pedido mesmo sem pagamento
            "WHERE p.ClienteId = @clienteId " +                                      // só os pedidos deste cliente
            "GROUP BY p.Id, p.Data, p.Status, pg.Forma, p.ValorFrete, p.ValorDesconto " +   // uma linha por pedido
            "ORDER BY p.Data DESC";                                                  // mais recente primeiro

        List<ResumoPedido> pedidos = new List<ResumoPedido>();   // lista que será devolvida

        // using: fecha a conexão automaticamente no fim do bloco, mesmo se der erro.
        using (MySqlConnection conexao = new MySqlConnection(LojaDbContext.StringDeConexao))
        {
            conexao.Open();   // abre a conexão com o MySQL

            MySqlCommand comando = new MySqlCommand(sql, conexao);   // o comando SQL que será executado
            comando.Parameters.AddWithValue("@clienteId", clienteId);   // parâmetro: evita SQL injection

            using (MySqlDataReader leitor = comando.ExecuteReader())   // executa o SELECT
            {
                while (leitor.Read())   // Read() avança para a próxima linha; false quando acabar
                {
                    ResumoPedido resumo = new ResumoPedido();                          // um objeto por linha
                    resumo.Numero = leitor.GetInt32("Numero");                         // lê a coluna Numero
                    resumo.Data = leitor.GetDateTime("Data");                          // lê a coluna Data
                    resumo.Status = leitor.GetString("Status");                        // lê a coluna Status
                    resumo.FormaPagamento = leitor.GetString("FormaPagamento");        // lê a forma de pagamento
                    resumo.QuantidadeItens = Convert.ToInt32(leitor["QuantidadeItens"]);   // SUM vem como número grande: converte para int
                    resumo.Total = leitor.GetDecimal("Total");                         // lê o total
                    pedidos.Add(resumo);                                               // coloca na lista
                }
            }
        }

        return pedidos;   // devolve a lista para o controlador
    }
}
