using LojaVirtual.Model.Entidades;
using LojaVirtual.Model.Entidades.Pagamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LojaVirtual.Data.Configuracoes;

public class PedidoConfiguracao : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.ToTable("tb_pedido");

        // Gravado como texto ("Finalizado") para que a consulta em SQL puro mostre o nome.
        builder.Property(p => p.Status).HasConversion<string>().HasMaxLength(15);
        builder.Property(p => p.ValorFrete).HasPrecision(10, 2);
        builder.Property(p => p.ValorDesconto).HasPrecision(10, 2);

        // 1×N Cliente—Pedido
        builder.HasOne(p => p.Cliente).WithMany(c => c.Pedidos)
            .HasForeignKey(p => p.ClienteId).OnDelete(DeleteBehavior.Restrict);

        // N×1: endereço e opção de entrega escolhidos no checkout
        builder.HasOne(p => p.EnderecoEntrega).WithMany()
            .HasForeignKey(p => p.EnderecoEntregaId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(p => p.OpcaoEntrega).WithMany()
            .HasForeignKey(p => p.OpcaoEntregaId).OnDelete(DeleteBehavior.Restrict);

        // Composição: excluir o pedido exclui os itens
        builder.HasMany(p => p.Itens).WithOne(i => i.Pedido)
            .HasForeignKey(i => i.PedidoId).OnDelete(DeleteBehavior.Cascade);

        // 1×1 Pedido—Pagamento (FK única no lado do pagamento)
        builder.HasOne(p => p.Pagamento).WithOne(g => g.Pedido)
            .HasForeignKey<Pagamento>(g => g.PedidoId);
    }
}

/// <summary>N×N Pedido—Produto pela classe de associação PedidoItem (chave composta).</summary>
public class PedidoItemConfiguracao : IEntityTypeConfiguration<PedidoItem>
{
    public void Configure(EntityTypeBuilder<PedidoItem> builder)
    {
        builder.ToTable("tb_pedido_item");
        builder.HasKey(i => new { i.PedidoId, i.ProdutoId });
        builder.Property(i => i.PrecoUnitario).HasPrecision(10, 2);

        builder.HasOne(i => i.Produto).WithMany()
            .HasForeignKey(i => i.ProdutoId).OnDelete(DeleteBehavior.Restrict);
    }
}

/// <summary>Herança em TPH: uma tabela tb_pagamento com a coluna discriminadora Forma.</summary>
public class PagamentoConfiguracao : IEntityTypeConfiguration<Pagamento>
{
    public void Configure(EntityTypeBuilder<Pagamento> builder)
    {
        builder.ToTable("tb_pagamento");
        builder.Ignore(g => g.Descricao);
        builder.Property(g => g.Valor).HasPrecision(10, 2);

        builder.HasDiscriminator<string>("Forma")
            .HasValue<PagamentoPix>("Pix")
            .HasValue<PagamentoBoleto>("Boleto")
            .HasValue<PagamentoCartao>("Cartao");
    }
}

public class PagamentoPixConfiguracao : IEntityTypeConfiguration<PagamentoPix>
{
    public void Configure(EntityTypeBuilder<PagamentoPix> builder) =>
        builder.Property(g => g.CodigoPix).HasMaxLength(40);
}

public class PagamentoBoletoConfiguracao : IEntityTypeConfiguration<PagamentoBoleto>
{
    public void Configure(EntityTypeBuilder<PagamentoBoleto> builder) =>
        builder.Property(g => g.CodigoDeBarras).HasMaxLength(60);
}

public class PagamentoCartaoConfiguracao : IEntityTypeConfiguration<PagamentoCartao>
{
    public void Configure(EntityTypeBuilder<PagamentoCartao> builder)
    {
        builder.Property(g => g.NomeTitular).HasMaxLength(100);
        builder.Property(g => g.UltimosDigitos).HasMaxLength(4);
    }
}
