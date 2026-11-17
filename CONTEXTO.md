# Contexto do projeto — histórico das sessões com IA

Registro do que foi feito com o Claude Code (Anthropic) neste projeto, para o grupo se situar e para servir de
base à Seção 10 do plano (uso de IA). Última atualização: **17/11/2026** (véspera da entrega).

## O projeto em uma página

- **Disciplina:** POO II — Stack EW (UVV), grupo de 3, vale 2,5 pontos. **Entrega e apresentação: 18/11/2026.**
- **Entrega:** código C# (Visual Studio), documentação e **vídeo único de até 10 min** em que cada integrante
  apresenta uma parte, com o nome completo na tela. **Envio individual no Blog.**
- **Tema:** Loja Virtual de peças de computador, do ponto de vista do **cliente fazendo uma compra**:
  Login → Catálogo (busca/categorias) → Favoritos / Carrinho → Checkout (Endereço → Entrega → Pagamento →
  Revisão → Concluído) → Meus Pedidos.
- **Stack:** C# .NET 8, Windows Forms, MVC em 4 projetos (View, Controller, Model, Data), EF Core 8 Code First
  com MySQL (Pomelo), e a tela Meus Pedidos em **SQL puro** (ADO.NET, `MeusPedidosDAO`).
- **Fora do escopo:** cadastro de produtos/clientes/endereços (tudo vem da carga inicial), área do operador,
  relatórios, segurança de senha (texto simples), pagamento e frete reais (simulados).
- **Logins de teste:** `ana@teste.com` (pessoa física) e `empresa@teste.com` (pessoa jurídica), senha `123456`.
- **Repositório:** https://github.com/Almonfr3y/Projeto_POOII (branch `main`).

## Regra de uso de IA

- No início, o enunciado limitava a IA ao **nível IA-2** (IA só para explorar e planejar).
- Em **17/11/2026 o professor liberou o uso de IA sem limite**. Continuam valendo: documentar ferramentas e
  prompts, e **comparar duas versões de um trecho, com e sem IA** (feito em `Comparativo_IA/`).

## Linha do tempo

| Data | O que foi feito |
|---|---|
| 04/11 | Primeiros diagramas e plano (originais guardados em `_versao_original_20261104/`, fora do Git). |
| 11/11 | Diagramas e plano corrigidos; escopo simplificado para o fluxo de compra do cliente; heranças `Pessoa → PF/PJ` e `Pagamento → Pix/Boleto/Cartão` (TPH) trazidas de volta; solution criada; repositório publicado. |
| 12/11 | Telas Favoritos, Carrinho, Checkout (assistente em 5 etapas) e Meus Pedidos; migration `CriacaoInicial`; teste contra MySQL 8. |
| 17/11 | (1) Commit/push das telas e da migration. (2) Regra de IA liberada. (3) `PedidoService.FinalizarPedido` implementado; comparativo com e sem IA; ajustes de estoque; plano e READMEs atualizados; revisão dos diagramas. (4) Código simplificado e comentado. (5) Este arquivo. (6) **Segunda simplificação, para o nível de um aluno de 3º período** (detalhes abaixo). |

## Como o código está agora (segunda simplificação, 17/11 à tarde)

Pedido do grupo: "não quero um trabalho elaborado, quero que ele seja simples e funcione". O código anterior
tinha arquitetura e sintaxe de profissional (serviços, repositórios, unidade de trabalho, fábrica de
controladores, sintaxe C# 12). Foi reescrito mantendo **tudo o que o enunciado exige** e o diagrama de classes.

**O que saiu:** `PedidoService`, `FreteService`, `AutenticacaoService` e suas interfaces; os 5 repositórios e suas
interfaces; `IUnidadeDeTrabalho`/`UnidadeDeTrabalho`; `FabricaDeControladores`; `CarrinhoController`;
`ConfiguracaoBanco` e `LojaDbContextFactory`; `DadosCheckout`; `IMeusPedidosConsulta`; `LoginInvalidoException`;
`Mensagem.cs`. Ao todo, 1215 linhas removidas e 850 adicionadas.

**Como ficou:**
- Cada controlador abre `using (LojaDbContext db = new LojaDbContext())` em cada método e usa LINQ direto.
  Isso também resolve sozinho o problema antigo do estoque desatualizado no carrinho, porque cada operação lê
  o banco de novo.
- A string de conexão fica em `LojaDbContext.StringDeConexao` (`OnConfiguring`).
- As telas criam os controladores com `new`. O login devolve `null` se errar a senha.
- As grades usam tipos anônimos (`Select(p => new { p.Id, Peça = p.Nome, ... })`), com a coluna Id escondida.
- `FrmCheckout` controla as etapas com um `int` e um vetor de painéis.
- A sintaxe ficou básica: `if`/`foreach`, concatenação de strings, sem `=>` em métodos nem `?? throw`.
- **Mantido igual ao diagrama de classes:** entidades, heranças (Template Method do `Pagamento`),
  `IDescontoStrategy` (com `SemDesconto`/`DescontoQuantidade`), `CarrinhoDeCompras`, `StatusPedido`,
  `EstoqueInsuficienteException`. O modelo do banco **não mudou** (`has-pending-model-changes` sem mudanças):
  a migration `CriacaoInicial` continua valendo.
- **SOLID agora (Seção 8 do plano):** SRP (um controlador por tela, regras nas entidades), OCP (`IDescontoStrategy`),
  LSP (Pagamento e Pessoa), ISP (interface de desconto pequena, controladores separados), DIP (`PedidoController`
  recebe `IDescontoStrategy` pelo construtor).
- **Bug corrigido:** o desconto de 5% podia ter 3 casas decimais (ex.: R$ 37,475). O banco gravava R$ 37,48 e o total
  da tela "Concluído" ficava diferente do total em "Meus Pedidos". Agora `DescontoQuantidade` arredonda para 2 casas.

### Comparativo com e sem IA (`Comparativo_IA/`)
- O trecho comparado agora é o `PedidoController.Finalizar`.
- **Versão A** (`VersaoA_SemIA.cs.txt`): usa `throw new Exception` (derruba a tela), ignora a `IDescontoStrategy` (OCP),
  não arredonda centavos, recalcula o frete na mão (DRY), não confere `null` nem carrinho vazio, muda o `Status`
  direto e não usa `using`. Gera 2 avisos de compilação.
- **Versão B:** a do sistema. Tabela 7a do plano e `Comparativo_IA/README.md`.
- ⚠️ A versão A foi **escrita pela IA a pedido do grupo**, imitando uma primeira versão sem IA. Recomendação:
  alguém do grupo reescrever com as próprias palavras, ou dizer no vídeo que é uma reconstrução.

### Documentação atualizada nesta etapa
- **Plano (.docx/.pdf):** Seções 3.1, 3.3, 4.1, 4.2, 8 (SOLID), 9 (Clean Code, Tabela 6), 10.2 (novo prompt),
  Tabela 7 (nova linha) e 10.4/Tabela 7a reescritas; Figura 2 trocada; Sumário atualizado no PDF.
- **Diagrama de arquitetura** redesenhado: `diagrama_arquitetura_camadas.png`, com o arquivo-fonte
  `diagrama_arquitetura_camadas.html` (abrir no navegador para editar).
- READMEs da raiz, de `LojaVirtual/` e de `Comparativo_IA/`.

## Revisão das modelagens (diagramas)

Sem erros estruturais: classes, atributos, multiplicidades e tabelas batem com o código. Pontos a corrigir
(os PNGs não têm arquivo-fonte, então corrigir exige redesenhar):

1. **Casos de uso:** "Filtrar por categoria e buscar peça" deveria ser **«extend»** (é opcional), não «include».
2. **Casos de uso:** "Aplicar desconto" e "Baixar estoque" são passos internos do sistema, não objetivos do
   cliente — remover ou estar pronto para justificar (o desconto, por ser condicional, também seria «extend»).
3. **Classes — navegabilidade** (conceito citado no enunciado): Pessoa↔Pedido, Pedido↔Pagamento,
   Pedido↔PedidoItem e Endereco↔Pessoa são bidirecionais no código, mas o diagrama tem seta só de um lado.
4. **Classes:** `StatusPedido.Cancelado` não é usado (está comentado no código como reservado).
5. **Classes (opcional):** incluir `CarrinhoDeCompras` (central no fluxo e no tema coleções).
6. **Arquitetura:** ✅ redesenhado em 17/11 (sem fábrica, serviços e repositórios).
7. **Excalidraw:** as telas de Cadastro e Perfil e o "Produto em Promoção" estão desenhados mas fora do escopo —
   marcar como "fora do escopo" ou explicar no vídeo.

## Pendências do grupo (para 18/11)

- [ ] Abrir cada tela no Visual Studio (Windows) e conferir o layout.
- [ ] Criar o banco: `Update-Database -Project LojaVirtual.Data -StartupProject LojaVirtual.Data` no Package Manager Console.
- [ ] Revisar o código e o comparativo (o professor fará perguntas sobre o código).
- [ ] Nomes dos integrantes nas Tabelas 5 e 9 do plano.
- [ ] Copiar a Tabela 7 (registro de IA) para o Trello.
- [ ] (Opcional) Redesenhar os diagramas com as correções acima.
- [ ] Gravar o vídeo (roteiro na Seção 11.2 do plano) e enviar individualmente no Blog.

## Notas técnicas para quem continuar

- Conexão: constante `LojaDbContext.StringDeConexao`
  (`server=localhost;port=3306;database=loja_pecas;user=root;password=;`). O `MeusPedidosDAO` usa a mesma.
- Mudou alguma entidade ou o `LojaDbContext`? `Add-Migration <Nome>` e depois `Update-Database`
  (ambos com `-Project LojaVirtual.Data -StartupProject LojaVirtual.Data`).
- Para trocar a regra de desconto (demonstrar o OCP): em `FrmCheckout`, troque `new DescontoQuantidade()` por
  `new SemDesconto()` ou por uma nova classe que implemente `IDescontoStrategy`.
- O projeto `LojaVirtual.Data` está com `<Nullable>disable</Nullable>` (evita avisos nos `DbSet`); o Model continua
  com `enable`, e isso não pode mudar sem gerar migration nova (o EF usa a nulidade para decidir NOT NULL).
- Ao editar o `.docx` do plano no Linux, gere o PDF de forma que o Sumário seja atualizado; não deixe o
  LibreOffice salvar por cima do `.docx` (pode estragar a formatação do Word).
