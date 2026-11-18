# Contexto do projeto — histórico das sessões com IA

Registro do que foi feito com o Claude Code (Anthropic) neste projeto, para o grupo se situar e para servir de
base à Seção 10 do plano (uso de IA). Última atualização: **18/11/2026** (dia da entrega).

## O projeto em uma página

- **Disciplina:** POO II — Stack EW (UVV), vale 2,5 pontos. O enunciado pede grupos de 3, mas **este trabalho é individual: o grupo é só o Miguel** (informado em 17/11). **Entrega e apresentação: 18/11/2026.**
- **Entrega:** código C# (Visual Studio), documentação e **vídeo único de até 10 min** em que cada integrante
  apresenta uma parte, com o nome completo na tela. **Envio individual no Blog.**
- **Tema:** Loja Virtual de peças de computador, do ponto de vista do **cliente fazendo uma compra**:
  Login → Catálogo (busca/categorias) → Favoritos / Carrinho → Checkout (Endereço → Entrega → Pagamento →
  Revisão → Concluído) → Meus Pedidos.
- **Stack:** C# .NET 10, **WPF** (XAML), MVC em 4 projetos (View, Controller, Model, Data), EF Core 10 Code First
  com MySQL (provedor oficial da Oracle, `MySql.EntityFrameworkCore`), e a tela Meus Pedidos em **SQL puro**
  (ADO.NET com `MySql.Data`, `MeusPedidosDAO`). Até 17/11 era .NET 8 com o provedor Pomelo e telas em Windows Forms.
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
| 17/11 | (1) Commit/push das telas e da migration. (2) Regra de IA liberada. (3) `PedidoService.FinalizarPedido` implementado; comparativo com e sem IA; ajustes de estoque; plano e READMEs atualizados; revisão dos diagramas. (4) Código simplificado e comentado. (5) Este arquivo. (6) **Segunda simplificação, para o nível de um aluno de 3º período** (detalhes abaixo). (7) Commit `9591fd4` enviado ao GitHub; avaliação do nível do trabalho para um aluno sozinho (seção abaixo). (8) Plano ajustado para **trabalho individual**; criado o **material de estudo e roteiro do vídeo** em `Estudo/` (fora do Git). (9) **Comentários linha a linha** em todo o código; **102 perguntas e respostas** do professor em `Estudo/`. |
| 18/11 | **Migração para o .NET 10** e **conversão das telas de Windows Forms para WPF** (detalhes abaixo). |

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

## Trabalho individual: o nível está adequado? (avaliação de 17/11)

**Contexto:** o grupo é só o Miguel, aluno do 3º período de Engenharia de Software (UVV), e a nota é individual.
O professor fará perguntas sobre o código.

**Veredito:** depois da 2ª simplificação, o **código** está no nível de um aluno de POO II: classes, herança,
`abstract`/`override`, interface, `List<>`, `foreach`, `try/catch`, LINQ básico e EF com `using`. O que ainda fica
**acima** do esperado para um aluno sozinho:

1. **Tamanho do projeto para uma pessoa:** 4 projetos, 6 telas, 9 tabelas, 2 heranças, checkout em 5 etapas, em cerca de
   2 semanas. É plausível, mas é bastante. Se perguntarem, a resposta honesta é que a IA ajudou (e isso está documentado).
2. **Fluent API do EF:** `HasDiscriminator` (TPH), `UsingEntity<Dictionary<string, object>>` (tb_favorito),
   `HasData` (seed), `HasConversion<string>`. É o trecho mais difícil de explicar; é o primeiro a estudar.
3. **Padrões com nome:** Strategy (`IDescontoStrategy`) e Template Method (`Pagamento.Processar` →
   `GerarDadosDoPagamento`). São simples, mas é preciso saber explicar com as próprias palavras.
4. **O plano (.docx) tem linguagem de profissional** (21 páginas, tabelas de SOLID/Clean Code, cronograma em fases).
   ✅ Ajustado em 17/11 para trabalho individual (ver abaixo).
5. **Comparativo com e sem IA:** a versão A foi escrita pela IA. Sozinho, não há "outro integrante" que tenha escrito
   sem IA. O mais seguro é o Miguel reescrever a versão A do jeito dele, ou dizer no vídeo que é uma reconstrução.

**O que estudar para as perguntas do professor (por prioridade):**
- O caminho de uma compra: `FrmCheckout.ConfirmarCompra` → `PedidoController.Finalizar` → `Produto.BaixarEstoque`,
  `OpcaoEntrega.CalcularFrete`, `Pedido.AplicarDesconto`, `Pagamento.Processar` → `SaveChanges`.
  Por que nada é gravado se der erro no meio (o `SaveChanges` só acontece no fim).
- Herança TPH: por que PF e PJ ficam na mesma tabela (`tb_pessoa`, coluna `TipoPessoa`), e o mesmo para pagamentos.
- Os relacionamentos: 1×1 (Pedido–Pagamento), 1×N, N×N (PedidoItem com chave composta × tb_favorito sem classe),
  composição (Cascade) × agregação (Restrict).
- SQL puro no `MeusPedidosDAO`: JOIN, LEFT JOIN, GROUP BY, SUM, parâmetro `@clienteId` contra SQL injection.
- OCP na prática: trocar `new DescontoQuantidade()` por `new SemDesconto()` no `FrmCheckout`.
- Code First: o que é migration, `Add-Migration` e `Update-Database`.

## Plano ajustado para trabalho individual (17/11)

- **Capa:** "Autor: 【Seu nome completo】 — trabalho individual, envio no Blog" (**falta preencher o nome**).
- **Texto:** Sumário Executivo, 2.1 ("Este trabalho é individual..."), 2.2, 3.1, 4.1, 6.1, 8.2, 10.1, 10.4 e 11 sem
  "grupo/integrantes". Seção 7 renomeada para "Gestão do Projeto e Organização do Trabalho"; 7.2 virou
  "Organização do Trabalho Individual", com a **Tabela 5 = três frentes de trabalho** (Modelagem e telas; Banco e
  checkout; SQL puro, documentação e vídeo) e regras de trabalho de uma pessoa só.
- **Tabela 1:** Trello e Git sem "atribuição por membro"/"revisão entre integrantes". **Tabela 8:** vídeo "com o nome
  completo do autor na tela". **Tabela 9 (roteiro):** a coluna "Apresentador" virou "O que mostrar na tela", com
  8 blocos (Abertura, Modelagem, Arquitetura, Execução, Banco, SQL puro, SOLID/Clean Code, IA/gestão/encerramento).
- O curso na capa continua "Engenharia da Computação" (é o que está no enunciado). O Miguel disse que é de Engenharia
  de Software: conferir qual nome usar.
- PDF regerado com o Sumário atualizado.

## Material de estudo e roteiro do vídeo (`Estudo/`, fora do Git)

`Estudo/Material_de_Estudo_e_Roteiro.md` (e `.pdf`, 18 páginas). Fica fora do repositório público porque tem as
respostas preparadas para o professor. Conteúdo:
1. Plano de estudo de cerca de 3 horas · 2. Fala de 1 minuto sobre o projeto · 3. Mapa dos arquivos · 4. Conceitos do
enunciado no código (OO, herança, relacionamentos, coleções, MVC, padrões, exceções) · 5. Caminho de uma compra
linha a linha · 6. Banco (Code First, TPH, N×N, seed) · 7. SQL puro · 8. SOLID e Clean Code (com o ponto fraco do DIP
e a resposta) · 9. Como falar do uso de IA · 10. **29 perguntas prováveis com respostas** · 11. Preparação do PC
(VS, MySQL, `Update-Database`, `Drop-Database` para zerar antes de gravar) · 12. **Roteiro do vídeo com as falas**
e a sequência exata da demonstração · 13. Cola de 1 página.

**Dados da demonstração (conferidos pelo código):** login errado → "E-mail ou senha inválidos."; PC Gamer (estoque 4)
com quantidade 5 → "Estoque insuficiente..."; 5 × Mouse gamer + entrega Padrão + Cartão 3x →
subtotal R$ 749,50, frete R$ 20,70, desconto R$ 37,48, **total R$ 732,72**; estoque do mouse 40 → 35.

## Comentários linha a linha e perguntas do professor (17/11, noite)

- **Código comentado:** todos os arquivos `.cs` escritos à mão (Model, Controller, Data e as telas) têm agora um
  comentário em quase toda linha ou método, em linguagem simples (o que a linha faz e por quê), incluindo
  explicações de C# (`abstract`, `override`, `protected`, `?`, `using`, `Include`, `?:`, `as`...). São 613 linhas com
  comentário. Ficaram de fora os `*.Designer.cs` (o Visual Studio regera; em 18/11 eles saíram com a conversão para WPF) e as Migrations (geradas pelo EF).
- **Garantia de que o código não mudou:** um script remove os comentários das versões antes e depois e compara;
  o resultado foi idêntico. A compilação continua com 0 avisos e 0 erros.
- **`Estudo/Perguntas_e_Respostas.md` / `.pdf`** (14 páginas, fora do Git): 102 perguntas em 17 blocos — projeto,
  OO, herança/polimorfismo, relacionamentos, coleções/LINQ, MVC, EF/Code First, SQL puro, regras de negócio, exceções,
  SOLID, Clean Code, padrões, Windows Forms, "e se...?", IA/gestão e pegadinhas (senha em texto puro, «include»
  × «extend», DIP parcial, carrinho só na memória, `Cancelado` sem uso). Cada resposta diz em que arquivo está.

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

## Migração para o .NET 10 (18/11)

Pedido: "reescreva o código em .NET 10, mantendo a integridade do projeto".

- **Projetos:** `net8.0` → `net10.0` (Model, Controller, Data) e `net8.0-windows` → `net10.0-windows` (View).
- **Provedor do MySQL:** o Pomelo não tem versão para o EF Core 10 (a última é a 9.0.0). Por escolha do Miguel, foi
  trocado pelo provedor oficial da Oracle, **`MySql.EntityFrameworkCore` 10.0.12**, com EF Core **10.0.12**.
  - `LojaDbContext.OnConfiguring`: `UseMySql(conexão, new MySqlServerVersion(...))` → `UseMySQL(conexão)`.
  - `MeusPedidosDAO`: `using MySqlConnector;` → `using MySql.Data.MySqlClient;` (mesmas classes `MySqlConnection`,
    `MySqlCommand` e `MySqlDataReader`; o SQL não mudou).
  - **Migration gerada de novo:** a antiga (`20261112131405_CriacaoInicial`) tinha código próprio do Pomelo e não
    compilava. A nova é `20261118110714_CriacaoInicial`, com o mesmo nome e as mesmas 9 tabelas, colunas, chaves e
    seed. **Quem já tinha criado o banco com a migration antiga deve rodar `Drop-Database` e depois `Update-Database`.**
- **Pacote novo `Microsoft.EntityFrameworkCore.Tools`:** os comandos `Add-Migration`/`Update-Database`/`Drop-Database` do
  Console do Gerenciador de Pacotes dependem dele, e ele **faltava desde o início** (sem ele, o `Update-Database`
  das instruções não funcionaria no Visual Studio).
- **Avisos novos:** o .NET 10 marcou `DataGridViewCell.Value` e `Columns["..."]` como possivelmente nulos (9 avisos).
  Corrigidos em `FrmInicial`, `FrmFavoritos` e `FrmCarrinho` com `Convert.ToInt32(...)` no lugar de `(int)...` e
  `Columns["Id"]!` (comentado). Compilação: **0 avisos, 0 erros**.
- **Teste:** a migration foi aplicada num MySQL 8.0 (podman), seguida de um teste de ponta a ponta com 16 verificações:
  login PF/PJ/errado, catálogo, busca, favoritos, estoque insuficiente, subtotal R$ 749,50, frete R$ 20,70,
  desconto R$ 37,48, pedido gravado, estoque 40 → 35, Meus Pedidos em SQL puro com total **R$ 732,72**, e pedido PJ
  com Pix. Todas passaram. `has-pending-model-changes`: sem mudanças.
- **Documentos atualizados:** README, README da solution (requisitos), diagrama de arquitetura (HTML e PNG), plano
  (Tabela 1 da stack, Seção 5.3, nova linha na Tabela 7 de registro de IA, Figura 2, PDF com o Sumário atualizado;
  o plano passou de 21 para 22 páginas) e `Estudo/` (requisitos do PC, `OnConfiguring` e a nova pergunta 40b).
- **Visual Studio:** o .NET 10 precisa do **Visual Studio 2026** (versão 18); o VS 2022 não tem suporte a ele.

## Conversão para WPF (18/11)

Pedido: "é um projeto WPF com .NET 10, faça as alterações necessárias para que ele seja compatível". Só a camada
**View** mudou; Model, Controller e Data ficaram iguais.

- **Projeto:** `LojaVirtual.View.csproj` com `<UseWPF>true</UseWPF>` no lugar de `UseWindowsForms`.
- **Ponto de entrada:** saiu o `Program.cs`; entrou o **`App.xaml` / `App.xaml.cs`**. O `App_Startup` abre o login
  (`ShowDialog`); se entrar, cria a `SessaoDoUsuario` e abre a `FrmInicial` como `MainWindow`. O `ShutdownMode` começa
  em `OnExplicitShutdown` (fechar o login não encerra o programa antes da hora) e passa a `OnMainWindowClose`.
- **Telas:** os 6 `*.Designer.cs` saíram; cada tela agora tem **`.xaml` (layout) + `.xaml.cs` (eventos)**, com os
  **mesmos nomes** (`FrmLogin`, `FrmInicial`, `FrmFavoritos`, `FrmCarrinho`, `FrmCheckout`, `FrmMeusPedidos`), os
  mesmos textos, botões, fluxo e mensagens. Layout com `Grid`/`DockPanel`/`StackPanel` (redimensiona sozinho).
- **Trocas de controles:** `DataGridView` → `DataGrid` (`ItemsSource`, colunas automáticas); `NumericUpDown` (não
  existe no WPF) → `TextBox` + `int.TryParse` na quantidade do carrinho e `ComboBox` 1..10 nas parcelas; senha com
  `PasswordBox`; `Panel` → `DockPanel`/`StackPanel`/`TextBox` com `Visibility`; `Load` → `Loaded`; `.Text` de botão
  → `.Content`; `Enabled` → `IsEnabled`; `Checked` → `IsChecked == true`; `DialogResult.OK` → `DialogResult = true`;
  `PlaceholderText` → rótulo (`TextBlock`) acima do campo.
- **Linha selecionada:** antes a grade tinha uma coluna `Id` escondida. Agora a tela guarda a lista original
  (`_produtos`, `_favoritosDaTela`, `Carrinho.Itens`) e usa `SelectedIndex` para achar o item. A ordenação por clique
  na coluna foi desligada (`CanUserSortColumns="False"`) para a posição continuar valendo.
- **Compilação:** 0 avisos, 0 erros (XAML compilado). **As telas ainda não foram abertas**: WPF só roda no Windows.
- **Documentos:** READMEs, diagrama de arquitetura (HTML/PNG e Figura 2 do plano), plano (Sumário Executivo, Tabela 1,
  3.2, 4.1, 4.2, 8.1, Tabela 10, capa e nova linha na Tabela 7; PDF regerado, 23 páginas) e `Estudo/` (perguntas
  31, 79, 79b, 80, 82, 83 e o mapa de arquivos).

## Como abrir e rodar no Windows (passo a passo)

**1. Programas (só na primeira vez)**
- **Visual Studio 2026** (Community é gratuito); o VS 2022 não abre projetos .NET 10. No instalador, marque a carga
  **"Desenvolvimento para desktop com .NET"** (instala o .NET 10 e o WPF).
- **MySQL Server 8**, usuário `root` **sem senha**, porta 3306. Se o root tiver senha, troque `password=;` por
  `password=SUA_SENHA;` em `LojaVirtual.Data/LojaDbContext.cs`.

**2. Abrir**
1. Se baixou o ZIP do GitHub, **extraia primeiro** (botão direito → Extrair tudo); não abra de dentro do ZIP.
2. Abra a pasta `Projeto_POOII\LojaVirtual` e dê dois cliques em **`LojaVirtual.sln`** (ou, no Visual Studio:
   Arquivo → Abrir → Projeto/Solução).
3. Espere o Visual Studio restaurar os pacotes NuGet (barra de baixo).

**3. Preparar**
1. No **Gerenciador de Soluções**, botão direito em **`LojaVirtual.View`** → **Definir como Projeto de Inicialização**.
2. **Ferramentas → Gerenciador de Pacotes do NuGet → Console do Gerenciador de Pacotes**:
   ```
   Update-Database -Project LojaVirtual.Data -StartupProject LojaVirtual.Data
   ```
   Isso cria o banco `loja_pecas` com as tabelas e os produtos. Se já existir um `loja_pecas` antigo, rode antes:
   ```
   Drop-Database -Project LojaVirtual.Data -StartupProject LojaVirtual.Data
   ```

**4. Rodar**
- **F5**. Login: **ana@teste.com** (pessoa física) ou **empresa@teste.com** (pessoa jurídica), senha **123456**.

**Se der erro**

| Problema | Solução |
|---|---|
| "SDK do .NET 10 não encontrado" ou projeto "não carregado" | Está no VS 2022: instale o VS 2026. |
| `Update-Database` não é reconhecido | Feche e abra o Visual Studio, ou Compilar → Recompilar Solução e tente de novo. |
| Erro de conexão / "Access denied" no MySQL | Confira se o MySQL está ligado e a senha do root em `LojaDbContext.cs`. |
| Tela com layout estranho | Abra o `.xaml` daquela tela no Visual Studio e ajuste. |

## Pendências (para 18/11)

- [ ] Abrir cada tela WPF no Visual Studio (Windows) e conferir o layout e o fluxo completo.
- [ ] Instalar o Visual Studio 2026 com o .NET 10 (se ainda for o VS 2022).
- [ ] Criar o banco: `Update-Database -Project LojaVirtual.Data -StartupProject LojaVirtual.Data` no Package Manager Console
  (se já existir um `loja_pecas` criado pela migration antiga, rode `Drop-Database` antes).
- [ ] Revisar o código e reescrever (ou assumir no vídeo) a versão A do comparativo.
- [x] Ajustar o plano para trabalho individual.
- [ ] Preencher o nome completo na capa do plano (Word) e exportar o PDF de novo (atualizar o Sumário antes).
- [ ] Confirmar com o professor que o trabalho individual é aceito (o enunciado fala em grupos de 3).
- [ ] Estudar com `Estudo/Material_de_Estudo_e_Roteiro.pdf` e `Estudo/Perguntas_e_Respostas.pdf` e ensaiar o vídeo uma vez cronometrando.
- [ ] Copiar a Tabela 7 (registro de IA) para o Trello.
- [ ] (Opcional) Redesenhar os diagramas com as correções acima.
- [ ] Gravar o vídeo (roteiro na Seção 11.2 do plano) e enviar no Blog.

## Notas técnicas para quem continuar

- Conexão: constante `LojaDbContext.StringDeConexao`
  (`server=localhost;port=3306;database=loja_pecas;user=root;password=;`). O `MeusPedidosDAO` usa a mesma.
- Mudou alguma entidade ou o `LojaDbContext`? `Add-Migration <Nome>` e depois `Update-Database`
  (ambos com `-Project LojaVirtual.Data -StartupProject LojaVirtual.Data`).
- Para trocar a regra de desconto (demonstrar o OCP): em `FrmCheckout`, troque `new DescontoQuantidade()` por
  `new SemDesconto()` ou por uma nova classe que implemente `IDescontoStrategy`.
- Provedor do MySQL: `MySql.EntityFrameworkCore` (Oracle). Ao trocar de provedor, a migration precisa ser gerada de novo.
- O projeto `LojaVirtual.Data` está com `<Nullable>disable</Nullable>` (evita avisos nos `DbSet`); o Model continua
  com `enable`, e isso não pode mudar sem gerar migration nova (o EF usa a nulidade para decidir NOT NULL).
- Ao editar o `.docx` do plano no Linux, gere o PDF de forma que o Sumário seja atualizado; não deixe o
  LibreOffice salvar por cima do `.docx` (pode estragar a formatação do Word).
