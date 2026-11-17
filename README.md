# Loja Virtual de Peças de Computador — Projeto POO II

Atividade avaliativa da disciplina de Programação Orientada a Objetos II (Stack EW) — Engenharia da Computação, UVV.
Entrega e apresentação: **18/11/2026**.

Sistema em **C# (.NET 8) com Windows Forms**, organizado em camadas **MVC**, com persistência em **MySQL**
via **Entity Framework Core (Code First)** e uma funcionalidade em **SQL puro (ADO.NET)**.

O escopo segue **o ponto de vista do cliente (pessoa física ou jurídica) fazendo uma compra**:

> Login → Tela Inicial (catálogo, busca, categorias) → Favoritos / Carrinho →
> Endereço → Entrega → Pagamento (Pix, Boleto ou Cartão) → Revisão → Concluído → Meus Pedidos

Produtos, categorias, clientes e endereços já vêm cadastrados no banco (carga inicial).

## Conteúdo do repositório

| Caminho | O que é |
|---|---|
| [`LojaVirtual/`](LojaVirtual/) | Solution do Visual Studio (`LojaVirtual.sln`) com os projetos View, Controller, Model e Data — veja o [README da solution](LojaVirtual/README.md) |
| `Plano_Implementacao_Loja_Virtual_Pecas_PC.docx` / `.pdf` | Plano de implementação (escopo, arquitetura, modelo de dados, SOLID, Clean Code, uso de IA, cronograma e roteiro do vídeo) |
| `diagrama_casos_de_uso.png` | Diagrama de casos de uso |
| `diagrama_de_classes.png` | Diagrama de classes |
| `diagrama_arquitetura_camadas.png` | Arquitetura em camadas (MVC) |
| `Projeto_POOII-2026-11-11-0730.excalidraw` | Desenho do fluxo de telas (abrir em [excalidraw.com](https://excalidraw.com)) |
| `Modelagem_POO2_20261104.excalidraw` | Primeiro rascunho das telas |
| [`Comparativo_IA/`](Comparativo_IA/) | Comparativo com e sem IA do `PedidoService.FinalizarPedido` (versões A e B + discussão crítica) |

## Requisitos do enunciado

- **Herança:** `Pessoa` → `PessoaFisica` / `PessoaJuridica` e `Pagamento` → `PagamentoPix` / `PagamentoBoleto` / `PagamentoCartao` (ambas em TPH)
- **Relacionamentos:** 1×1 (Pedido–Pagamento), 1×N (Pessoa–Pedido, Pessoa–Endereço, Categoria–Produto, Pedido–Itens), N×N (Pedido–Produto via PedidoItem; favoritos)
- **Composição e agregação:** Pedido ◆ PedidoItem, Pessoa ◆ Endereço; Categoria ◇ Produto
- **SQL puro:** tela "Meus Pedidos" (`MeusPedidosDAO`)
- **SOLID e Clean Code:** locais de aplicação descritos nas Seções 8 e 9 do plano

## Uso de IA

O professor liberou o uso de IA sem limite de nível (a regra inicial era o nível IA-2 da AI Assessment Scale).
A IA (Claude Code) foi usada no planejamento, nos diagramas, na geração do código e da documentação, sempre
com revisão humana. O registro de ferramentas e prompts está na Seção 10 do plano, e o comparativo com e sem IA
em [`Comparativo_IA/`](Comparativo_IA/).
