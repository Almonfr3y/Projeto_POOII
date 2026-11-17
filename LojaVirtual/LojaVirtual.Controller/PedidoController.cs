using LojaVirtual.Model.Carrinho;
using LojaVirtual.Model.Checkout;
using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Repositorios;
using LojaVirtual.Model.Servicos;

namespace LojaVirtual.Controller;

/// <summary>Checkout: Endereço → Entrega → Pagamento → Revisão → Concluído.</summary>
public class PedidoController
{
    private readonly IPedidoService _pedidoService;
    private readonly IClienteRepository _clientes;
    private readonly IOpcaoEntregaRepository _opcoesEntrega;
    private readonly IFreteService _frete;

    public PedidoController(IPedidoService pedidoService, IClienteRepository clientes,
        IOpcaoEntregaRepository opcoesEntrega, IFreteService frete)
    {
        _pedidoService = pedidoService;
        _clientes = clientes;
        _opcoesEntrega = opcoesEntrega;
        _frete = frete;
    }

    public IReadOnlyList<Endereco> ListarEnderecos(int clienteId) => ObterCliente(clienteId).Enderecos.ToList();

    /// <summary>"CPF ..." ou "CNPJ ...", mostrado na Revisão sem a tela saber se é PF ou PJ (LSP).</summary>
    public string DescreverDocumento(int clienteId) => ObterCliente(clienteId).DescreverDocumento();

    public IReadOnlyList<OpcaoEntrega> ListarOpcoesEntrega() => _opcoesEntrega.Listar();

    /// <summary>Mostra o frete de cada opção na tela de Entrega.</summary>
    public decimal SimularFrete(CarrinhoDeCompras carrinho, OpcaoEntrega opcao) => _frete.CalcularFrete(carrinho.Itens, opcao);

    public Pedido Finalizar(DadosCheckout dados, CarrinhoDeCompras carrinho)
    {
        Pedido pedido = _pedidoService.FinalizarPedido(dados, carrinho.Itens);
        carrinho.Limpar();
        return pedido;
    }

    private Pessoa ObterCliente(int clienteId) =>
        _clientes.ObterComDetalhes(clienteId) ?? throw new InvalidOperationException("Cliente não encontrado.");
}
