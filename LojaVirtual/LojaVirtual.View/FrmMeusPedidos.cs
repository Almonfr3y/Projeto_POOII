using System.ComponentModel;
using System.Data.Common;
using LojaVirtual.Controller;
using LojaVirtual.Model.Consultas;

namespace LojaVirtual.View;

/// <summary>Tela "Meus Pedidos". Os dados vêm da consulta em SQL puro (MeusPedidosDAO, ADO.NET).</summary>
public partial class FrmMeusPedidos : Form
{
    private readonly MeusPedidosController _meusPedidos;
    private readonly SessaoDoUsuario _sessao;

    public FrmMeusPedidos(MeusPedidosController meusPedidos, SessaoDoUsuario sessao)
    {
        _meusPedidos = meusPedidos;
        _sessao = sessao;
        InitializeComponent();
    }

    private void FrmMeusPedidos_Load(object sender, EventArgs e)
    {
        try
        {
            IReadOnlyList<ResumoPedido> pedidos = _meusPedidos.Listar(_sessao.Cliente.Id);
            dgvPedidos.DataSource = pedidos.Select(LinhaPedido.De).ToList();
            lblResumo.Text = pedidos.Count == 0
                ? "Você ainda não fez nenhum pedido."
                : $"{pedidos.Count} pedido(s) — total gasto: {pedidos.Sum(p => p.Total):C}";
        }
        catch (DbException ex)
        {
            MessageBox.Show($"Não foi possível consultar os pedidos: {ex.Message}", Text,
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private sealed record LinhaPedido(
        [property: DisplayName("Nº")] int Numero,
        [property: DisplayName("Data")] string Data,
        [property: DisplayName("Situação")] string Status,
        [property: DisplayName("Itens")] int Itens,
        [property: DisplayName("Forma de pagamento")] string FormaPagamento,
        [property: DisplayName("Total")] string Total)
    {
        public static LinhaPedido De(ResumoPedido resumo) =>
            new(resumo.Numero, resumo.Data.ToString("dd/MM/yyyy HH:mm"), resumo.Status, resumo.QuantidadeItens,
                resumo.FormaPagamento, resumo.Total.ToString("C"));
    }
}
