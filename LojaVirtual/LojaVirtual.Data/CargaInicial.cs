using LojaVirtual.Model.Entidades;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Data;

/// <summary>
/// Dados inseridos pela Migration (seed). Cadastro de produtos e de clientes ficou fora do escopo:
/// tudo o que a compra precisa já vem pronto no banco.
/// </summary>
internal static class CargaInicial
{
    private const int Hardware = 1;
    private const int Perifericos = 2;
    private const int Pcs = 3;

    public static void Aplicar(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>().HasData(
            new Categoria { Id = Hardware, Nome = "Hardware" },
            new Categoria { Id = Perifericos, Nome = "Periféricos" },
            new Categoria { Id = Pcs, Nome = "PCs" });

        modelBuilder.Entity<Produto>().HasData(
            Peca(1, Hardware, "Processador Ryzen 5 5600", "6 núcleos, 12 threads, AM4", 899.90m, 0.30m, 15),
            Peca(2, Hardware, "Placa de vídeo RTX 4060 8 GB", "8 GB GDDR6, PCIe 4.0", 2199.90m, 1.20m, 8),
            Peca(3, Hardware, "Memória RAM DDR4 16 GB 3200 MHz", "2×8 GB, CL16", 349.90m, 0.10m, 30),
            Peca(4, Hardware, "SSD NVMe 1 TB", "Leitura 3500 MB/s, M.2 2280", 459.90m, 0.05m, 25),
            Peca(5, Hardware, "Placa-mãe B550M", "AM4, micro-ATX, 4 slots DDR4", 799.90m, 0.90m, 10),
            Peca(6, Hardware, "Fonte 650 W 80 Plus Bronze", "Semimodular, PFC ativo", 429.90m, 1.80m, 12),
            Peca(7, Perifericos, "Teclado mecânico ABNT2", "Switch azul, RGB", 249.90m, 0.90m, 20),
            Peca(8, Perifericos, "Mouse gamer 12000 DPI", "6 botões, RGB", 149.90m, 0.12m, 40),
            Peca(9, Perifericos, "Monitor 24\" 144 Hz", "Full HD, painel IPS", 1099.90m, 4.50m, 6),
            Peca(10, Perifericos, "Headset 7.1", "USB, microfone removível", 279.90m, 0.35m, 18),
            Peca(11, Pcs, "PC Gamer Ryzen 5 / RTX 4060", "16 GB RAM, SSD 1 TB, fonte 650 W", 5499.90m, 9.50m, 4),
            Peca(12, Pcs, "PC Escritório Core i3", "8 GB RAM, SSD 256 GB", 2299.90m, 6.00m, 7));

        modelBuilder.Entity<OpcaoEntrega>().HasData(
            new OpcaoEntrega { Id = 1, Nome = "Econômica", PrazoDias = 10, ValorBase = 12m, ValorPorKg = 3m },
            new OpcaoEntrega { Id = 2, Nome = "Padrão", PrazoDias = 5, ValorBase = 18m, ValorPorKg = 4.5m },
            new OpcaoEntrega { Id = 3, Nome = "Expressa", PrazoDias = 2, ValorBase = 30m, ValorPorKg = 7m });

        // Clientes de demonstração (dados fictícios), um de cada tipo da herança. Senha de ambos: 123456.
        modelBuilder.Entity<PessoaFisica>().HasData(
            new PessoaFisica { Id = 1, Nome = "Ana Teste", Cpf = "529.982.247-25", Email = "ana@teste.com", Senha = "123456" });
        modelBuilder.Entity<PessoaJuridica>().HasData(
            new PessoaJuridica { Id = 2, Nome = "Compras Tech Teste", RazaoSocial = "Tech Teste Ltda", Cnpj = "11.222.333/0001-81", Email = "empresa@teste.com", Senha = "123456" });

        modelBuilder.Entity<Endereco>().HasData(
            new Endereco { Id = 1, ClienteId = 1, Apelido = "Casa", Logradouro = "Rua das Flores", Numero = "100", Bairro = "Centro", Cidade = "Vitória", Uf = "ES", Cep = "29010-000" },
            new Endereco { Id = 2, ClienteId = 1, Apelido = "Trabalho", Logradouro = "Av. Fernando Ferrari", Numero = "1000", Bairro = "Goiabeiras", Cidade = "Vitória", Uf = "ES", Cep = "29075-910" },
            new Endereco { Id = 3, ClienteId = 1, Apelido = "Pais", Logradouro = "Rua Sete", Numero = "25", Bairro = "Praia da Costa", Cidade = "Vila Velha", Uf = "ES", Cep = "29101-000" },
            new Endereco { Id = 4, ClienteId = 2, Apelido = "Casa", Logradouro = "Rua do Comércio", Numero = "45", Bairro = "Jardim Camburi", Cidade = "Vitória", Uf = "ES", Cep = "29090-000" });
    }

    private static Produto Peca(int id, int categoriaId, string nome, string especificacoes, decimal preco, decimal pesoKg, int estoque) =>
        new() { Id = id, CategoriaId = categoriaId, Nome = nome, Especificacoes = especificacoes, Preco = preco, PesoKg = pesoKg, Estoque = estoque };
}
