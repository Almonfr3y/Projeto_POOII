using LojaVirtual.Data;
using LojaVirtual.Data.Consultas;
using LojaVirtual.Data.Repositorios;
using LojaVirtual.Model.Descontos;
using LojaVirtual.Model.Servicos;

namespace LojaVirtual.Controller.Composicao;

/// <summary>
/// Composition root: único lugar que conhece as classes concretas da camada Data.
/// Os controladores recebem apenas interfaces (DIP), e a View não referencia a Data.
/// Cada controlador ganha seu próprio LojaDbContext, compartilhado pelos seus repositórios.
/// </summary>
public class FabricaDeControladores
{
    public ContaController CriarContaController() =>
        new(new AutenticacaoService(new ClienteRepository(NovoContexto())));

    public CatalogoController CriarCatalogoController()
    {
        var contexto = NovoContexto();
        return new CatalogoController(new ProdutoRepository(contexto), new CategoriaRepository(contexto));
    }

    public CarrinhoController CriarCarrinhoController() => new(new ProdutoRepository(NovoContexto()));

    public FavoritosController CriarFavoritosController()
    {
        var contexto = NovoContexto();
        return new FavoritosController(new ClienteRepository(contexto), new ProdutoRepository(contexto),
            new UnidadeDeTrabalho(contexto));
    }

    public PedidoController CriarPedidoController()
    {
        var contexto = NovoContexto();
        var opcoesEntrega = new OpcaoEntregaRepository(contexto);
        var frete = new FreteService();
        // OCP: para trocar a regra de desconto, basta passar outra IDescontoStrategy aqui.
        var pedidoService = new PedidoService(new ProdutoRepository(contexto), new PedidoRepository(contexto),
            opcoesEntrega, frete, new DescontoQuantidade(), new UnidadeDeTrabalho(contexto));
        return new PedidoController(pedidoService, new ClienteRepository(contexto), opcoesEntrega, frete);
    }

    public MeusPedidosController CriarMeusPedidosController() =>
        new(new MeusPedidosDAO(ConfiguracaoBanco.StringDeConexao));

    private static LojaDbContext NovoContexto() => new(ConfiguracaoBanco.CriarOpcoes());
}
