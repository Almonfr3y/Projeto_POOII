using LojaVirtual.Model.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LojaVirtual.Data.Configuracoes;

public class CategoriaConfiguracao : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("tb_categoria");
        builder.Property(c => c.Nome).IsRequired().HasMaxLength(80);
    }
}

public class ProdutoConfiguracao : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.ToTable("tb_produto");
        builder.Property(p => p.Nome).IsRequired().HasMaxLength(150);
        builder.Property(p => p.Especificacoes).HasMaxLength(500);
        builder.Property(p => p.Preco).HasPrecision(10, 2);
        builder.Property(p => p.PesoKg).HasPrecision(8, 3);

        // Agregação 1×N: apagar a categoria não apaga produtos (Restrict).
        builder.HasOne(p => p.Categoria).WithMany(c => c.Produtos)
            .HasForeignKey(p => p.CategoriaId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class OpcaoEntregaConfiguracao : IEntityTypeConfiguration<OpcaoEntrega>
{
    public void Configure(EntityTypeBuilder<OpcaoEntrega> builder)
    {
        builder.ToTable("tb_opcao_entrega");
        builder.Property(o => o.Nome).IsRequired().HasMaxLength(60);
        builder.Property(o => o.ValorBase).HasPrecision(10, 2);
        builder.Property(o => o.ValorPorKg).HasPrecision(10, 2);
    }
}
