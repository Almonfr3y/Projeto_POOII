using LojaVirtual.Model.Entidades;               // classes do Model (Pessoa, Produto...)
using LojaVirtual.Model.Entidades.Pagamentos;    // Pagamento e suas filhas
using Microsoft.EntityFrameworkCore;             // Entity Framework Core (o ORM)

namespace LojaVirtual.Data;

// Code First: o banco loja_pecas é criado a partir das classes do Model e da configuração abaixo.
// Se mudar alguma classe, gere uma nova migration (Add-Migration) e rode Update-Database.
// ": DbContext" = herda do EF; é a classe que representa o banco dentro do C#.
public class LojaDbContext : DbContext
{
    // Troque aqui se o MySQL de vocês tiver outra senha ou outra porta.
    public const string StringDeConexao = "server=localhost;port=3306;database=loja_pecas;user=root;password=;";

    // Cada DbSet é uma tabela que o código consulta com LINQ (ex.: db.Produtos.Where(...)).
    public DbSet<Pessoa> Pessoas { get; set; }           // tb_pessoa (PF e PJ juntas)
    public DbSet<Endereco> Enderecos { get; set; }       // tb_endereco
    public DbSet<Categoria> Categorias { get; set; }     // tb_categoria
    public DbSet<Produto> Produtos { get; set; }         // tb_produto
    public DbSet<OpcaoEntrega> OpcoesEntrega { get; set; } // tb_opcao_entrega
    public DbSet<Pedido> Pedidos { get; set; }           // tb_pedido
    public DbSet<PedidoItem> PedidoItens { get; set; }   // tb_pedido_item
    public DbSet<Pagamento> Pagamentos { get; set; }     // tb_pagamento (Pix, Boleto e Cartão juntos)

    // Diz ao EF qual banco usar: MySQL (provedor oficial da Oracle, MySql.EntityFrameworkCore).
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL(StringDeConexao);
    }

    // Fluent API: nomes das tabelas, tamanhos dos campos, relacionamentos e heranças.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigurarClientes(modelBuilder);     // tb_pessoa, tb_endereco e tb_favorito
        ConfigurarCatalogo(modelBuilder);     // tb_categoria, tb_produto e tb_opcao_entrega
        ConfigurarPedidos(modelBuilder);      // tb_pedido e tb_pedido_item
        ConfigurarPagamentos(modelBuilder);   // tb_pagamento
        CargaInicial.Aplicar(modelBuilder);   // dados iniciais (produtos, clientes...)
    }

    // Clientes: herança PF/PJ, endereços (composição) e favoritos (NxN).
    private static void ConfigurarClientes(ModelBuilder modelBuilder)
    {
        // Entity<T>(...) = configura a tabela da classe T. "pessoa =>" é uma lambda: um apelido para a configuração.
        modelBuilder.Entity<Pessoa>(pessoa =>
        {
            pessoa.ToTable("tb_pessoa");   // nome da tabela no MySQL

            // Herança (TPH): PF e PJ ficam na mesma tabela; a coluna TipoPessoa diz qual é qual.
            pessoa.HasDiscriminator<string>("TipoPessoa")
                .HasValue<PessoaFisica>("PF")
                .HasValue<PessoaJuridica>("PJ");

            // IsRequired = NOT NULL; HasMaxLength = tamanho máximo do texto (varchar(150)).
            pessoa.Property(p => p.Nome).IsRequired().HasMaxLength(150);
            pessoa.Property(p => p.Email).IsRequired().HasMaxLength(150);
            pessoa.Property(p => p.Senha).IsRequired().HasMaxLength(50);
            pessoa.HasIndex(p => p.Email).IsUnique(); // não pode haver dois clientes com o mesmo e-mail

            // Composição 1xN: apagar o cliente apaga os endereços dele (Cascade).
            // HasMany/WithOne = 1xN; HasForeignKey = coluna que liga as tabelas; OnDelete = o que fazer ao apagar.
            pessoa.HasMany(p => p.Enderecos).WithOne(e => e.Cliente)
                .HasForeignKey(e => e.ClienteId).OnDelete(DeleteBehavior.Cascade);

            // NxN sem classe de associação: o EF cria sozinho a tabela de junção tb_favorito.
            pessoa.HasMany(p => p.Favoritos).WithMany(p => p.FavoritadoPor)
                .UsingEntity<Dictionary<string, object>>(
                    "tb_favorito",
                    favorito => favorito.HasOne<Produto>().WithMany().HasForeignKey("ProdutoId"),
                    favorito => favorito.HasOne<Pessoa>().WithMany().HasForeignKey("ClienteId"));
        });

        // Colunas que só existem nas subclasses (ficam nulas para o outro tipo).
        modelBuilder.Entity<PessoaFisica>(pf =>
        {
            pf.Property(p => p.Cpf).HasMaxLength(14);
            pf.HasIndex(p => p.Cpf).IsUnique();
        });
        modelBuilder.Entity<PessoaJuridica>(pj =>
        {
            pj.Property(p => p.Cnpj).HasMaxLength(18);
            pj.Property(p => p.RazaoSocial).HasMaxLength(150);
            pj.HasIndex(p => p.Cnpj).IsUnique();
        });

        modelBuilder.Entity<Endereco>(endereco =>
        {
            endereco.ToTable("tb_endereco");
            endereco.Property(e => e.Apelido).HasMaxLength(40);
            endereco.Property(e => e.Logradouro).IsRequired().HasMaxLength(150);
            endereco.Property(e => e.Numero).HasMaxLength(10);
            endereco.Property(e => e.Bairro).HasMaxLength(80);
            endereco.Property(e => e.Cidade).IsRequired().HasMaxLength(80);
            endereco.Property(e => e.Uf).IsRequired().HasMaxLength(2);
            endereco.Property(e => e.Cep).IsRequired().HasMaxLength(9);
        });
    }

    // Catálogo: categorias, produtos (agregação) e opções de entrega.
    private static void ConfigurarCatalogo(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(categoria =>
        {
            categoria.ToTable("tb_categoria");
            categoria.Property(c => c.Nome).IsRequired().HasMaxLength(80);
        });

        modelBuilder.Entity<Produto>(produto =>
        {
            produto.ToTable("tb_produto");
            produto.Property(p => p.Nome).IsRequired().HasMaxLength(150);
            produto.Property(p => p.Especificacoes).HasMaxLength(500);
            produto.Property(p => p.Preco).HasPrecision(10, 2);  // decimal(10,2): até 99.999.999,99
            produto.Property(p => p.PesoKg).HasPrecision(8, 3);  // gramas de precisão

            // Agregação 1xN: o produto existe sem a categoria, então apagar a categoria é bloqueado (Restrict).
            produto.HasOne(p => p.Categoria).WithMany(c => c.Produtos)
                .HasForeignKey(p => p.CategoriaId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<OpcaoEntrega>(opcao =>
        {
            opcao.ToTable("tb_opcao_entrega");
            opcao.Property(o => o.Nome).IsRequired().HasMaxLength(60);
            opcao.Property(o => o.ValorBase).HasPrecision(10, 2);
            opcao.Property(o => o.ValorPorKg).HasPrecision(10, 2);
        });
    }

    // Pedidos: ligações com cliente, endereço, entrega, itens (composição) e pagamento (1x1).
    private static void ConfigurarPedidos(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pedido>(pedido =>
        {
            pedido.ToTable("tb_pedido");

            // O enum é gravado como texto ("Finalizado") para a consulta em SQL puro mostrar o nome.
            pedido.Property(p => p.Status).HasConversion<string>().HasMaxLength(15);
            pedido.Property(p => p.ValorFrete).HasPrecision(10, 2);
            pedido.Property(p => p.ValorDesconto).HasPrecision(10, 2);

            // 1xN: um cliente faz vários pedidos.
            pedido.HasOne(p => p.Cliente).WithMany(c => c.Pedidos)
                .HasForeignKey(p => p.ClienteId).OnDelete(DeleteBehavior.Restrict);

            // Nx1: endereço e opção de entrega escolhidos no checkout.
            pedido.HasOne(p => p.EnderecoEntrega).WithMany()
                .HasForeignKey(p => p.EnderecoEntregaId).OnDelete(DeleteBehavior.Restrict);
            pedido.HasOne(p => p.OpcaoEntrega).WithMany()
                .HasForeignKey(p => p.OpcaoEntregaId).OnDelete(DeleteBehavior.Restrict);

            // Composição 1xN: o item não existe sem o pedido (Cascade).
            pedido.HasMany(p => p.Itens).WithOne(i => i.Pedido)
                .HasForeignKey(i => i.PedidoId).OnDelete(DeleteBehavior.Cascade);

            // 1x1: cada pedido tem um pagamento; a chave estrangeira fica em tb_pagamento.
            pedido.HasOne(p => p.Pagamento).WithOne(g => g.Pedido)
                .HasForeignKey<Pagamento>(g => g.PedidoId);
        });

        // NxN Pedido—Produto pela classe de associação PedidoItem (chave composta).
        modelBuilder.Entity<PedidoItem>(item =>
        {
            item.ToTable("tb_pedido_item");
            item.HasKey(i => new { i.PedidoId, i.ProdutoId }); // chave composta: o mesmo produto não se repete no pedido
            item.Property(i => i.PrecoUnitario).HasPrecision(10, 2);
            item.HasOne(i => i.Produto).WithMany()
                .HasForeignKey(i => i.ProdutoId).OnDelete(DeleteBehavior.Restrict);
        });
    }

    // Pagamentos: herança Pix/Boleto/Cartão numa tabela só.
    private static void ConfigurarPagamentos(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pagamento>(pagamento =>
        {
            pagamento.ToTable("tb_pagamento");
            pagamento.Ignore(g => g.Descricao); // texto calculado, não vira coluna
            pagamento.Property(g => g.Valor).HasPrecision(10, 2);

            // Herança (TPH): a coluna Forma diz se a linha é Pix, Boleto ou Cartão.
            pagamento.HasDiscriminator<string>("Forma")
                .HasValue<PagamentoPix>("Pix")
                .HasValue<PagamentoBoleto>("Boleto")
                .HasValue<PagamentoCartao>("Cartao");
        });

        // Colunas próprias de cada forma de pagamento.
        modelBuilder.Entity<PagamentoPix>().Property(g => g.CodigoPix).HasMaxLength(40);
        modelBuilder.Entity<PagamentoBoleto>().Property(g => g.CodigoDeBarras).HasMaxLength(60);
        modelBuilder.Entity<PagamentoCartao>(cartao =>
        {
            cartao.Property(g => g.NomeTitular).HasMaxLength(100);
            cartao.Property(g => g.UltimosDigitos).HasMaxLength(4);
        });
    }
}
