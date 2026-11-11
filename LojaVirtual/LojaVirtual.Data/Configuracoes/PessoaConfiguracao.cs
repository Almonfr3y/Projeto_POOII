using LojaVirtual.Model.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LojaVirtual.Data.Configuracoes;

/// <summary>Herança em TPH: uma única tabela tb_pessoa com a coluna discriminadora TipoPessoa.</summary>
public class PessoaConfiguracao : IEntityTypeConfiguration<Pessoa>
{
    private const string TabelaFavoritos = "tb_favorito";

    public void Configure(EntityTypeBuilder<Pessoa> builder)
    {
        builder.ToTable("tb_pessoa");
        builder.HasDiscriminator<string>("TipoPessoa")
            .HasValue<PessoaFisica>("PF")
            .HasValue<PessoaJuridica>("PJ");

        builder.Property(p => p.Nome).IsRequired().HasMaxLength(150);
        builder.Property(p => p.Email).IsRequired().HasMaxLength(150);
        builder.Property(p => p.Senha).IsRequired().HasMaxLength(50);
        builder.HasIndex(p => p.Email).IsUnique();

        // Composição 1×N: excluir o cliente exclui seus endereços.
        builder.HasMany(p => p.Enderecos).WithOne(e => e.Cliente)
            .HasForeignKey(e => e.ClienteId).OnDelete(DeleteBehavior.Cascade);

        // N×N Pessoa—Produto sem classe de associação: o EF cria a tabela de junção tb_favorito.
        builder.HasMany(p => p.Favoritos).WithMany(p => p.FavoritadoPor)
            .UsingEntity<Dictionary<string, object>>(
                TabelaFavoritos,
                produto => produto.HasOne<Produto>().WithMany().HasForeignKey("ProdutoId"),
                cliente => cliente.HasOne<Pessoa>().WithMany().HasForeignKey("ClienteId"));
    }
}

public class PessoaFisicaConfiguracao : IEntityTypeConfiguration<PessoaFisica>
{
    public void Configure(EntityTypeBuilder<PessoaFisica> builder)
    {
        builder.Property(p => p.Cpf).HasMaxLength(14);
        builder.HasIndex(p => p.Cpf).IsUnique();
    }
}

public class PessoaJuridicaConfiguracao : IEntityTypeConfiguration<PessoaJuridica>
{
    public void Configure(EntityTypeBuilder<PessoaJuridica> builder)
    {
        builder.Property(p => p.Cnpj).HasMaxLength(18);
        builder.Property(p => p.RazaoSocial).HasMaxLength(150);
        builder.HasIndex(p => p.Cnpj).IsUnique();
    }
}

public class EnderecoConfiguracao : IEntityTypeConfiguration<Endereco>
{
    public void Configure(EntityTypeBuilder<Endereco> builder)
    {
        builder.ToTable("tb_endereco");
        builder.Property(e => e.Apelido).HasMaxLength(40);
        builder.Property(e => e.Logradouro).IsRequired().HasMaxLength(150);
        builder.Property(e => e.Numero).HasMaxLength(10);
        builder.Property(e => e.Bairro).HasMaxLength(80);
        builder.Property(e => e.Cidade).IsRequired().HasMaxLength(80);
        builder.Property(e => e.Uf).IsRequired().HasMaxLength(2);
        builder.Property(e => e.Cep).IsRequired().HasMaxLength(9);
    }
}
