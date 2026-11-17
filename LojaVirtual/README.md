# LojaVirtual — solução do Visual Studio

> **Uso de IA:** o código foi gerado com auxílio de IA (Claude Code) e revisado pelo grupo.
> O professor liberou o uso de IA sem limite de nível; o registro está na Seção 10 do plano.

## Escopo

O sistema segue **o ponto de vista do cliente fazendo uma compra**, conforme o desenho de telas:

Login → Tela Inicial (catálogo, busca, categorias) → Favoritos / Carrinho →
Checkout: Endereço → Entrega → Pagamento → Revisão → Concluído → Meus Pedidos

**Fora do escopo:** cadastro de produtos, categorias, clientes e endereços (tudo vem pronto na carga
inicial), área do operador e relatórios. Senhas em texto simples. O cliente pode ser pessoa física ou jurídica.

## Projetos (MVC em camadas)

| Projeto | Camada | Conteúdo |
|---|---|---|
| `LojaVirtual.View` | View | Windows Forms: `FrmLogin`, `FrmInicial`, `FrmFavoritos`, `FrmCarrinho`, `FrmCheckout`, `FrmMeusPedidos` + `SessaoDoUsuario` (cliente logado e carrinho) |
| `LojaVirtual.Controller` | Controller | Um controlador por tela: `ContaController`, `CatalogoController`, `FavoritosController`, `PedidoController`, `MeusPedidosController` |
| `LojaVirtual.Model` | Model | Entidades, heranças (`Pessoa` → PF/PJ, `Pagamento` → Pix/Boleto/Cartão), `CarrinhoDeCompras`, descontos (`IDescontoStrategy`), `EstoqueInsuficienteException` |
| `LojaVirtual.Data` | Dados | `LojaDbContext` (Code First + Fluent API), `CargaInicial` (seed), Migrations e `MeusPedidosDAO` (SQL puro / ADO.NET) |

Referências: View → Controller → Model e Data; Data → Model. A View nunca acessa o banco direto.
Cada método de controlador abre um `LojaDbContext` com `using`, faz o que precisa e fecha a conexão.

## Requisitos do enunciado → onde estão

| Requisito | Onde |
|---|---|
| Herança | `Pessoa` → `PessoaFisica` (CPF) / `PessoaJuridica` (CNPJ), TPH em `tb_pessoa`; `Pagamento` → `PagamentoPix` / `PagamentoBoleto` / `PagamentoCartao`, TPH em `tb_pagamento` |
| 1×1 | Pedido—Pagamento |
| 1×N | Pessoa—Pedido, Pessoa—Endereco (composição), Categoria—Produto (agregação), Pedido—PedidoItem (composição) |
| N×N | Pedido—Produto via `PedidoItem`; Pessoa—Produto (favoritos, `tb_favorito`) |
| SQL puro | `LojaVirtual.Data/Consultas/MeusPedidosDAO.cs` (tela Meus Pedidos) |
| Code First | `LojaDbContext` + Migrations |
| SOLID | Seção 8 do plano (ex.: OCP com `IDescontoStrategy`, LSP com as heranças) |
| Clean Code | Seção 9 do plano |

## Como rodar no Visual Studio

1. Abra `LojaVirtual.sln` e defina `LojaVirtual.View` como projeto de inicialização.
2. A conexão fica em `LojaDbContext.StringDeConexao`:
   `server=localhost;port=3306;database=loja_pecas;user=root;password=;`. Se o MySQL de vocês tiver senha, troque ali.
3. Crie o banco pelo Code First. A migration `CriacaoInicial` já está em `LojaVirtual.Data/Migrations`
   (cria as 9 tabelas e a carga inicial). No Package Manager Console:
   ```
   Update-Database -Project LojaVirtual.Data -StartupProject LojaVirtual.Data
   ```
   Se mudarem alguma entidade, gerem uma nova migration antes:
   `Add-Migration <Nome> -Project LojaVirtual.Data -StartupProject LojaVirtual.Data`.
4. Entre com **ana@teste.com** (pessoa física) ou **empresa@teste.com** (pessoa jurídica), senha **123456**.

## Como a compra é finalizada (`PedidoController.Finalizar`)

1. Para cada item do carrinho, relê o produto do banco e chama `BaixarEstoque` (lança
   `EstoqueInsuficienteException` se faltar peça) e guarda o preço atual no item;
2. calcula o frete (`OpcaoEntrega.CalcularFrete`) e o desconto (`IDescontoStrategy`, 5% a partir de 5 peças);
3. processa o pagamento escolhido (Pix, Boleto ou Cartão, sem saber qual — polimorfismo);
4. grava pedido, itens, pagamento e estoque com um único `SaveChanges`. Se algo der errado antes disso,
   nada é gravado.

Para demonstrar o OCP: em `FrmCheckout`, troque `new DescontoQuantidade()` por `new SemDesconto()`.

O comparativo com a versão escrita sem IA está em [`../Comparativo_IA/`](../Comparativo_IA/).

As telas WinForms só rodam no Windows: **abram cada uma no Visual Studio para revisar o layout** antes de gravar o vídeo.
