using LojaVirtual.Data;
using LojaVirtual.Data.Consultas;
using LojaVirtual.Data.Repositorios;
using LojaVirtual.Model.Descontos;
using LojaVirtual.Model.Servicos;

namespace LojaVirtual.Controller.Composicao;

/// <summary>
/// "Composition root": o ÚNICO lugar que cria as classes concretas da camada Data e as entrega
/// aos controladores como interfaces (DIP). Por isso a View nunca referencia a Data.
/// </summary>
public class FabricaDeControladores
{
    public ContaController CriarContaController() =>
        new(new AutenticacaoService(new ClienteRepository(NovoContexto())));

    public CatalogoController CriarCatalogoController()
    {
        var contexto = NovoContexto(); // os dois repositórios usam o mesmo contexto
        return new CatalogoController(new ProdutoRepository(contexto), new CategoriaRepository(contexto));
    }

    public CarrinhoController CriarCarrinhoController() => new(new ProdutoRepository(NovoContexto()));

    public FavoritosController CriarFavoritosController()
    {
        var contexto = NovoContexto();
        return new FavoritosController(new ClienteRepository(contexto), new ProdutoRepository(contexto),
            new UnidadeDeTrabalho(contexto));
    }

    /// <summary>Um controlador novo a cada checkout, com um contexto novo (dados sempre atuais).</summary>
    public PedidoController CriarPedidoController()
    {
        // Tudo no MESMO contexto: assim o SaveChanges grava pedido e estoque juntos, numa transação.
        var contexto = NovoContexto();
        var opcoesEntrega = new OpcaoEntregaRepository(contexto);
        var frete = new FreteService();

        // OCP: para trocar a regra de desconto, troque DescontoQuantidade por outra IDescontoStrategy (ex.: SemDesconto).
        var pedidoService = new PedidoService(new ProdutoRepository(contexto), new PedidoRepository(contexto),
            opcoesEntrega, frete, new DescontoQuantidade(), new UnidadeDeTrabalho(contexto));

        return new PedidoController(pedidoService, new ClienteRepository(contexto), opcoesEntrega, frete);
    }

    // Meus Pedidos não usa o EF: o DAO abre a própria conexão com a string de conexão.
    public MeusPedidosController CriarMeusPedidosController() =>
        new(new MeusPedidosDAO(ConfiguracaoBanco.StringDeConexao));

    private static LojaDbContext NovoContexto() => new(ConfiguracaoBanco.CriarOpcoes());
}
