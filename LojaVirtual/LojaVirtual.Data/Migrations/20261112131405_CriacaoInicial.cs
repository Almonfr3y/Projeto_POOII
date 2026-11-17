using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LojaVirtual.Data.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_categoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_categoria", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_opcao_entrega",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PrazoDias = table.Column<int>(type: "int", nullable: false),
                    ValorBase = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    ValorPorKg = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_opcao_entrega", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_pessoa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Senha = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoPessoa = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cpf = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cnpj = table.Column<string>(type: "varchar(18)", maxLength: 18, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RazaoSocial = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_pessoa", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_produto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Especificacoes = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Preco = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    PesoKg = table.Column<decimal>(type: "decimal(8,3)", precision: 8, scale: 3, nullable: false),
                    Estoque = table.Column<int>(type: "int", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_produto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_produto_tb_categoria_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "tb_categoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_endereco",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Apelido = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Logradouro = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Numero = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Bairro = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cidade = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Uf = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cep = table.Column<string>(type: "varchar(9)", maxLength: 9, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClienteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_endereco", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_endereco_tb_pessoa_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "tb_pessoa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_favorito",
                columns: table => new
                {
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    ProdutoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_favorito", x => new { x.ClienteId, x.ProdutoId });
                    table.ForeignKey(
                        name: "FK_tb_favorito_tb_pessoa_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "tb_pessoa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_favorito_tb_produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "tb_produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_pedido",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Data = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ValorFrete = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    ValorDesconto = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    EnderecoEntregaId = table.Column<int>(type: "int", nullable: false),
                    OpcaoEntregaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_pedido", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_pedido_tb_endereco_EnderecoEntregaId",
                        column: x => x.EnderecoEntregaId,
                        principalTable: "tb_endereco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_pedido_tb_opcao_entrega_OpcaoEntregaId",
                        column: x => x.OpcaoEntregaId,
                        principalTable: "tb_opcao_entrega",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_pedido_tb_pessoa_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "tb_pessoa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_pagamento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Valor = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    DataProcessamento = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PedidoId = table.Column<int>(type: "int", nullable: false),
                    Forma = table.Column<string>(type: "varchar(13)", maxLength: 13, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CodigoDeBarras = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Vencimento = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    NomeTitular = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UltimosDigitos = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Parcelas = table.Column<int>(type: "int", nullable: true),
                    CodigoPix = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_pagamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_pagamento_tb_pedido_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "tb_pedido",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_pedido_item",
                columns: table => new
                {
                    PedidoId = table.Column<int>(type: "int", nullable: false),
                    ProdutoId = table.Column<int>(type: "int", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_pedido_item", x => new { x.PedidoId, x.ProdutoId });
                    table.ForeignKey(
                        name: "FK_tb_pedido_item_tb_pedido_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "tb_pedido",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_pedido_item_tb_produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "tb_produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "tb_categoria",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Hardware" },
                    { 2, "Periféricos" },
                    { 3, "PCs" }
                });

            migrationBuilder.InsertData(
                table: "tb_opcao_entrega",
                columns: new[] { "Id", "Nome", "PrazoDias", "ValorBase", "ValorPorKg" },
                values: new object[,]
                {
                    { 1, "Econômica", 10, 12m, 3m },
                    { 2, "Padrão", 5, 18m, 4.5m },
                    { 3, "Expressa", 2, 30m, 7m }
                });

            migrationBuilder.InsertData(
                table: "tb_pessoa",
                columns: new[] { "Id", "Cpf", "Email", "Nome", "Senha", "TipoPessoa" },
                values: new object[] { 1, "529.982.247-25", "ana@teste.com", "Ana Teste", "123456", "PF" });

            migrationBuilder.InsertData(
                table: "tb_pessoa",
                columns: new[] { "Id", "Cnpj", "Email", "Nome", "RazaoSocial", "Senha", "TipoPessoa" },
                values: new object[] { 2, "11.222.333/0001-81", "empresa@teste.com", "Compras Tech Teste", "Tech Teste Ltda", "123456", "PJ" });

            migrationBuilder.InsertData(
                table: "tb_endereco",
                columns: new[] { "Id", "Apelido", "Bairro", "Cep", "Cidade", "ClienteId", "Logradouro", "Numero", "Uf" },
                values: new object[,]
                {
                    { 1, "Casa", "Centro", "29010-000", "Vitória", 1, "Rua das Flores", "100", "ES" },
                    { 2, "Trabalho", "Goiabeiras", "29075-910", "Vitória", 1, "Av. Fernando Ferrari", "1000", "ES" },
                    { 3, "Pais", "Praia da Costa", "29101-000", "Vila Velha", 1, "Rua Sete", "25", "ES" },
                    { 4, "Casa", "Jardim Camburi", "29090-000", "Vitória", 2, "Rua do Comércio", "45", "ES" }
                });

            migrationBuilder.InsertData(
                table: "tb_produto",
                columns: new[] { "Id", "CategoriaId", "Especificacoes", "Estoque", "Nome", "PesoKg", "Preco" },
                values: new object[,]
                {
                    { 1, 1, "6 núcleos, 12 threads, AM4", 15, "Processador Ryzen 5 5600", 0.30m, 899.90m },
                    { 2, 1, "8 GB GDDR6, PCIe 4.0", 8, "Placa de vídeo RTX 4060 8 GB", 1.20m, 2199.90m },
                    { 3, 1, "2×8 GB, CL16", 30, "Memória RAM DDR4 16 GB 3200 MHz", 0.10m, 349.90m },
                    { 4, 1, "Leitura 3500 MB/s, M.2 2280", 25, "SSD NVMe 1 TB", 0.05m, 459.90m },
                    { 5, 1, "AM4, micro-ATX, 4 slots DDR4", 10, "Placa-mãe B550M", 0.90m, 799.90m },
                    { 6, 1, "Semimodular, PFC ativo", 12, "Fonte 650 W 80 Plus Bronze", 1.80m, 429.90m },
                    { 7, 2, "Switch azul, RGB", 20, "Teclado mecânico ABNT2", 0.90m, 249.90m },
                    { 8, 2, "6 botões, RGB", 40, "Mouse gamer 12000 DPI", 0.12m, 149.90m },
                    { 9, 2, "Full HD, painel IPS", 6, "Monitor 24\" 144 Hz", 4.50m, 1099.90m },
                    { 10, 2, "USB, microfone removível", 18, "Headset 7.1", 0.35m, 279.90m },
                    { 11, 3, "16 GB RAM, SSD 1 TB, fonte 650 W", 4, "PC Gamer Ryzen 5 / RTX 4060", 9.50m, 5499.90m },
                    { 12, 3, "8 GB RAM, SSD 256 GB", 7, "PC Escritório Core i3", 6.00m, 2299.90m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_endereco_ClienteId",
                table: "tb_endereco",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_favorito_ProdutoId",
                table: "tb_favorito",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_pagamento_PedidoId",
                table: "tb_pagamento",
                column: "PedidoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_pedido_ClienteId",
                table: "tb_pedido",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_pedido_EnderecoEntregaId",
                table: "tb_pedido",
                column: "EnderecoEntregaId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_pedido_OpcaoEntregaId",
                table: "tb_pedido",
                column: "OpcaoEntregaId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_pedido_item_ProdutoId",
                table: "tb_pedido_item",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_pessoa_Cnpj",
                table: "tb_pessoa",
                column: "Cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_pessoa_Cpf",
                table: "tb_pessoa",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_pessoa_Email",
                table: "tb_pessoa",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_produto_CategoriaId",
                table: "tb_produto",
                column: "CategoriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_favorito");

            migrationBuilder.DropTable(
                name: "tb_pagamento");

            migrationBuilder.DropTable(
                name: "tb_pedido_item");

            migrationBuilder.DropTable(
                name: "tb_pedido");

            migrationBuilder.DropTable(
                name: "tb_produto");

            migrationBuilder.DropTable(
                name: "tb_endereco");

            migrationBuilder.DropTable(
                name: "tb_opcao_entrega");

            migrationBuilder.DropTable(
                name: "tb_categoria");

            migrationBuilder.DropTable(
                name: "tb_pessoa");
        }
    }
}
