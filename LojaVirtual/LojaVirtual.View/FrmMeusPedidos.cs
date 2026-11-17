using LojaVirtual.Controller;   // para usar MeusPedidosController
using LojaVirtual.Model;        // para usar ResumoPedido

namespace LojaVirtual.View;

// Tela Meus Pedidos. Os dados vêm da consulta em SQL puro (MeusPedidosDAO).
public partial class FrmMeusPedidos : Form
{
    private SessaoDoUsuario _sessao;                                          // cliente logado
    private MeusPedidosController _meusPedidos = new MeusPedidosController(); // chama o DAO

    public FrmMeusPedidos(SessaoDoUsuario sessao)
    {
        _sessao = sessao;
        InitializeComponent();
    }

    // Executa quando a tela abre: busca e mostra os pedidos.
    private void FrmMeusPedidos_Load(object sender, EventArgs e)
    {
        List<ResumoPedido> pedidos = _meusPedidos.Listar(_sessao.Cliente.Id);   // SQL puro

        // Uma linha por pedido, com data e valores formatados.
        dgvPedidos.DataSource = pedidos.Select(p => new
        {
            Nº = p.Numero,
            Data = p.Data.ToString("dd/MM/yyyy HH:mm"),   // dia/mês/ano hora:minuto
            Situação = p.Status,
            Itens = p.QuantidadeItens,
            FormaDePagamento = p.FormaPagamento,
            Total = p.Total.ToString("C")
        }).ToList();

        if (pedidos.Count == 0)   // nenhum pedido ainda
        {
            lblResumo.Text = "Você ainda não fez nenhum pedido.";
        }
        else
        {
            decimal totalGasto = 0;                  // soma de todos os pedidos
            foreach (ResumoPedido pedido in pedidos)
                totalGasto += pedido.Total;

            lblResumo.Text = pedidos.Count + " pedido(s) - total gasto: " + totalGasto.ToString("C");
        }
    }
}
