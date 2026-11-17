using LojaVirtual.Controller;
using LojaVirtual.Model;

namespace LojaVirtual.View;

// Tela Meus Pedidos. Os dados vêm da consulta em SQL puro (MeusPedidosDAO).
public partial class FrmMeusPedidos : Form
{
    private SessaoDoUsuario _sessao;
    private MeusPedidosController _meusPedidos = new MeusPedidosController();

    public FrmMeusPedidos(SessaoDoUsuario sessao)
    {
        _sessao = sessao;
        InitializeComponent();
    }

    private void FrmMeusPedidos_Load(object sender, EventArgs e)
    {
        List<ResumoPedido> pedidos = _meusPedidos.Listar(_sessao.Cliente.Id);

        dgvPedidos.DataSource = pedidos.Select(p => new
        {
            Nº = p.Numero,
            Data = p.Data.ToString("dd/MM/yyyy HH:mm"),
            Situação = p.Status,
            Itens = p.QuantidadeItens,
            FormaDePagamento = p.FormaPagamento,
            Total = p.Total.ToString("C")
        }).ToList();

        if (pedidos.Count == 0)
        {
            lblResumo.Text = "Você ainda não fez nenhum pedido.";
        }
        else
        {
            decimal totalGasto = 0;
            foreach (ResumoPedido pedido in pedidos)
                totalGasto += pedido.Total;

            lblResumo.Text = pedidos.Count + " pedido(s) - total gasto: " + totalGasto.ToString("C");
        }
    }
}
