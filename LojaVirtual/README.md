# LojaVirtual — estrutura inicial da solução

> **Uso de IA (nível IA-2):** esta estrutura inicial foi gerada com auxílio de IA (Claude Code)
> e precisa passar por revisão humana completa antes da entrega. Registre isso na Tabela 7 do plano
> e no Trello.

## Escopo

O sistema segue **o ponto de vista do cliente fazendo uma compra**, conforme o desenho de telas:

Login → Tela Inicial (catálogo, busca, categorias) → Favoritos / Carrinho →
Checkout: Endereço → Entrega → Pagamento → Revisão → Concluído → Meus Pedidos

**Fora do escopo:** cadastro de produtos, categorias, clientes e endereços (tudo vem pronto na carga
inicial), área do operador e relatórios. Senhas em texto simples. O cliente pode ser pessoa física ou jurídica.

## Projetos (MVC em camadas)

| Projeto | Camada | Conteúdo |
|---|---|---|
| `LojaVirtual.View` | View | Windows Forms: `FrmLogin`, `FrmInicial`, `FrmFavoritos`, `FrmCarrinho`, `FrmCheckout`, `FrmMeusPedidos` |
| `LojaVirtual.Controller` | Controller | Controladores + `Composicao/FabricaDeControladores` (composition root) |
| `LojaVirtual.Model` | Model | Entidades, herança de `Pagamento` (Pix/Boleto/Cartão), carrinho, descontos (Strategy), serviços, interfaces |
| `LojaVirtual.Data` | Dados | `LojaDbContext` (Code First + Fluent API + seed), repositórios EF, `MeusPedidosDAO` (SQL puro / ADO.NET) |

Referências: View → Controller → Model; Controller → Data (somente na fábrica); Data → Model.

## Requisitos do enunciado → onde estão

| Requisito | Onde |
|---|---|
| Herança | `Pessoa` → `PessoaFisica` (CPF) / `PessoaJuridica` (CNPJ), TPH em `tb_pessoa`; `Pagamento` → `PagamentoPix` / `PagamentoBoleto` / `PagamentoCartao`, TPH em `tb_pagamento` |
| 1×1 | Pedido—Pagamento |
| 1×N | Pessoa—Pedido, Pessoa—Endereco (composição), Categoria—Produto (agregação), Pedido—PedidoItem (composição) |
| N×N | Pedido—Produto via `PedidoItem`; Pessoa—Produto (favoritos, `tb_favorito`) |
| SQL puro | `LojaVirtual.Data/Consultas/MeusPedidosDAO.cs` (tela Meus Pedidos) |
| Code First | `LojaDbContext` + Migrations |

## Como rodar no Visual Studio

1. Abra `LojaVirtual.sln` e defina `LojaVirtual.View` como projeto de inicialização.
2. A conexão vem da variável de ambiente `LOJA_VIRTUAL_DB`. Sem ela, é usado
   `server=localhost;port=3306;database=loja_pecas;user=root;password=;` (`LojaVirtual.Data/ConfiguracaoBanco.cs`).
3. Crie o banco pelo Code First (Package Manager Console, projeto padrão `LojaVirtual.Data`):
   ```
   Add-Migration CriacaoInicial
   Update-Database
   ```
4. Entre com **ana@teste.com** (pessoa física) ou **empresa@teste.com** (pessoa jurídica), senha **123456** (dados fictícios da carga inicial).

## O que está pronto × o que é do grupo

Pronto (para revisar): entidades e relacionamentos, Fluent API, carga inicial (3 categorias, 12 peças,
3 opções de entrega, 1 cliente PF e 1 cliente PJ com endereços), repositórios, login, carrinho, frete por peso, desconto por
quantidade, herança de pagamento com processamento simulado, consulta **Meus Pedidos** em SQL puro, e as
telas `FrmLogin` e `FrmInicial` como exemplo do fluxo View → Controller.

Marcado com `TODO (grupo)` (busque por `TODO` no Visual Studio):

- `PedidoService.FinalizarPedido` — passos descritos no comentário. É o trecho escolhido para o
  **comparativo com e sem IA** (Seção 10.4 do plano): a versão A deve ser escrita pelo grupo sem IA.
- Telas `FrmFavoritos`, `FrmCarrinho`, `FrmCheckout` (assistente em etapas) e `FrmMeusPedidos`.
- Confirmar os valores de `DescontoQuantidade`.
