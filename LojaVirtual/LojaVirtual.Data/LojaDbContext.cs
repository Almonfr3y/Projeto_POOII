using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Entidades.Pagamentos;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Data;

/// <summary>Code First: o schema loja_pecas é gerado a partir destas classes via Migrations.</summary>
public class LojaDbContext : DbContext
{
    public LojaDbContext(DbContextOptions<LojaDbContext> options) : base(options)
    {
    }

    public DbSet<Pessoa> Pessoas => Set<Pessoa>();
    public DbSet<PessoaFisica> PessoasFisicas => Set<PessoaFisica>();
    public DbSet<PessoaJuridica> PessoasJuridicas => Set<PessoaJuridica>();
    public DbSet<Endereco> Enderecos => Set<Endereco>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<OpcaoEntrega> OpcoesEntrega => Set<OpcaoEntrega>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<PedidoItem> PedidoItens => Set<PedidoItem>();
    public DbSet<Pagamento> Pagamentos => Set<Pagamento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LojaDbContext).Assembly);
        CargaInicial.Aplicar(modelBuilder);
    }
}
