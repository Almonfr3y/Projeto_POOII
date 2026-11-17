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
| 17/11 | (1) Commit/push das telas e da migration. (2) Regra de IA liberada. (3) `PedidoService.FinalizarPedido` implementado; comparativo com e sem IA; ajustes de estoque; plano e READMEs atualizados; revisão dos diagramas. (4) Código simplificado e comentado. (5) Este arquivo. |

## O que foi feito em 17/11 (detalhes)

### Finalização da compra — `PedidoService.FinalizarPedido` (versão B, com IA)
1. Monta o pedido relendo cada produto do banco (estoque atual, preço congelado em `PrecoUnitario`).
2. Confere o estoque de **todos** os itens antes de alterar qualquer um.
3. Calcula frete (`IFreteService`) e desconto (`IDescontoStrategy`).
4. Processa o pagamento (Pix, Boleto ou Cartão — polimorfismo).
5. Baixa o estoque, finaliza e grava tudo num único `SaveChanges` (uma transação).

A Revisão do checkout passou a mostrar desconto e total reais (`SimularDesconto`), e o Concluído mostra o
número do pedido. Desconto confirmado: **5% a partir de 5 peças**.

### Correções encontradas na revisão
- O carrinho usa o mesmo `DbContext` a sessão inteira e via estoque antigo depois de uma compra →
  `IProdutoRepository.ObterParaConsulta` (sem rastreamento) e `AsNoTracking()` no catálogo.
- O catálogo mostrava a entidade crua na grade → agora mostra só Peça, Categoria, Especificações, Preço e
  Estoque, e é atualizado ao voltar de outra tela.

### Comparativo com e sem IA (`Comparativo_IA/`)
- **Versão A (sem IA)** — `VersaoA_SemIA.cs.txt`: um método de ~45 linhas. Funciona no caso feliz, mas baixa o
  estoque pela metade quando um item falha, lança `Exception` genérica (a tela não trata), ignora a
  `IDescontoStrategy` (quebra o OCP), duplica o cálculo de frete (quebra o DRY) e gera 4 avisos de nulidade.
- **Versão B (com IA)** — a do sistema. Discussão completa em `Comparativo_IA/README.md` e na Tabela 7a do plano.
- ⚠️ A versão A foi **escrita pela IA a pedido do grupo**, imitando uma primeira versão sem IA. Recomendação:
  alguém do grupo revisar e ajustar com as próprias palavras, ou dizer no vídeo que é uma reconstrução.

### Documentação atualizada
- **Plano (.docx/.pdf):** Seção 10 reescrita para a nova regra (ferramenta: Claude Code; prompts reais);
  Tabela 7 (registro de IA) preenchida; Tabela 7a (comparativo); checklist com Concluído/Pendente.
  O PDF foi gerado com o Sumário atualizado (no Word: botão direito no Sumário → Atualizar campo).
- **READMEs** da raiz e de `LojaVirtual/` atualizados.

### Simplificação e comentários do código
- Mantido tudo o que o enunciado exige e o plano cita como evidência de SOLID/MVC: interfaces de repositório e
  serviço, `IDescontoStrategy`, heranças, `FabricaDeControladores`, `UnidadeDeTrabalho`, `MeusPedidosDAO`.
- Removido/simplificado:
  - `LojaVirtual.Data/Configuracoes/` (12 classes em 3 arquivos) → Fluent API inteira dentro do `LojaDbContext`,
    em 4 seções. Verificado com `dotnet ef migrations has-pending-model-changes`: o modelo do banco não mudou.
  - `ClienteLogado` → o login devolve a própria `Pessoa`.
  - `SessaoDoUsuario.EstaLogado` (não era usado).
  - Novo `View/Mensagem.cs` no lugar do `MessageBox` repetido em todas as telas.
  - `FrmCheckout`: validação do cartão mais curta e "4 últimos dígitos" num só lugar.
- Comentários: de 131 para 325 linhas. Cada classe tem resumo; linhas não óbvias explicadas; marcados no
  código os conceitos do enunciado (HERANÇA, COMPOSIÇÃO, AGREGAÇÃO, 1×1, N×N, OCP, DIP, LSP).
- Não comentados: `*.Designer.cs` (o Visual Studio regera) e as Migrations (geradas pelo EF).

## Verificação

- Compilação: **0 avisos, 0 erros** (no Linux, com `-p:EnableWindowsTargeting=true`).
- Teste de ponta a ponta contra **MySQL 8 real** (container temporário), chamando os controladores como as
  telas chamam — **22 verificações, todas OK**: compra com Pix; desconto com 5 peças e Cartão 3x; falha de
  estoque no 2º item seguida de nova tentativa (1º item baixado uma única vez); boleto; carrinho vazio
  rejeitado; carrinho e catálogo vendo o estoque atual; cliente PJ (CNPJ); Meus Pedidos em SQL puro com o
  mesmo total do EF.
- **Não testado:** a interface gráfica. WinForms só roda no Windows — abram cada tela no Visual Studio.

## Revisão das modelagens (diagramas)

Sem erros estruturais: classes, atributos, multiplicidades e tabelas batem com o código. Pontos a corrigir
(os PNGs não têm arquivo-fonte, então corrigir exige redesenhar):

1. **Casos de uso:** "Filtrar por categoria e buscar peça" deveria ser **«extend»** (é opcional), não «include».
2. **Casos de uso:** "Aplicar desconto" e "Baixar estoque" são passos internos do sistema, não objetivos do
   cliente — remover ou estar pronto para justificar (o desconto, por ser condicional, também seria «extend»).
3. **Classes — navegabilidade** (conceito citado no enunciado): Pessoa↔Pedido, Pedido↔Pagamento,
   Pedido↔PedidoItem e Endereco↔Pessoa são bidirecionais no código, mas o diagrama tem seta só de um lado.
4. **Classes:** `StatusPedido.Cancelado` não é usado (está comentado no código como reservado).
5. **Classes (opcional):** incluir `CarrinhoDeCompras` e `PedidoService` (centrais no fluxo e no tema coleções).
6. **Arquitetura:** o Controller referencia a Data, mas só na `FabricaDeControladores` — vale uma nota.
7. **Excalidraw:** as telas de Cadastro e Perfil e o "Produto em Promoção" estão desenhados mas fora do escopo —
   marcar como "fora do escopo" ou explicar no vídeo.

## Pendências do grupo (para 18/11)

- [ ] Abrir cada tela no Visual Studio (Windows) e conferir o layout.
- [ ] Criar o banco: `Update-Database` no Package Manager Console (projeto padrão `LojaVirtual.Data`).
- [ ] Revisar o código e o comparativo (o professor fará perguntas sobre o código).
- [ ] Nomes dos integrantes nas Tabelas 5 e 9 do plano.
- [ ] Copiar a Tabela 7 (registro de IA) para o Trello.
- [ ] (Opcional) Redesenhar os diagramas com as correções acima.
- [ ] Gravar o vídeo (roteiro na Seção 11.2 do plano) e enviar individualmente no Blog.

## Notas técnicas para quem continuar

- Conexão: variável de ambiente `LOJA_VIRTUAL_DB`; sem ela, usa
  `server=localhost;port=3306;database=loja_pecas;user=root;password=;` (`LojaVirtual.Data/ConfiguracaoBanco.cs`).
- Mudou alguma entidade ou o `LojaDbContext`? `Add-Migration <Nome>` e depois `Update-Database`.
- Para trocar a regra de desconto (demonstrar o OCP): em `FabricaDeControladores.CriarPedidoController`,
  troque `new DescontoQuantidade()` por `new SemDesconto()` ou por uma nova classe que implemente `IDescontoStrategy`.
- Ao editar o `.docx` do plano no Linux, gere o PDF de forma que o Sumário seja atualizado; não deixe o
  LibreOffice salvar por cima do `.docx` (pode estragar a formatação do Word).
