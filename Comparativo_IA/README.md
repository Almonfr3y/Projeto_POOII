# Comparativo com e sem IA — `PedidoService.FinalizarPedido`

O enunciado pede a comparação de duas versões de um mesmo trecho, uma com IA e outra sem, para discussão crítica.
O trecho escolhido é `PedidoService.FinalizarPedido`, que fecha a compra: monta o pedido, calcula frete e desconto,
processa o pagamento, baixa o estoque e grava tudo.

| Versão | Arquivo | Situação |
|---|---|---|
| **A — sem IA** | [`VersaoA_SemIA.cs.txt`](VersaoA_SemIA.cs.txt) | Primeira versão, direta, num único método. Fica fora da compilação. |
| **B — com IA** | [`../LojaVirtual/LojaVirtual.Model/Servicos/PedidoService.cs`](../LojaVirtual/LojaVirtual.Model/Servicos/PedidoService.cs) | Versão usada no sistema, gerada com Claude Code e revisada. |

## Versão A — sem IA (resumo)

Um método de ~45 linhas que faz tudo em sequência: para cada item do carrinho, lê o produto, confere o estoque,
**já baixa o estoque**, cria o item e soma o peso; depois calcula o frete na mão, calcula o desconto na mão
(`if (qtd >= 5) ... * 0.05m`), processa o pagamento e muda `Status` direto para `Finalizado`.

## Versão B — com IA (resumo)

```csharp
public Pedido FinalizarPedido(DadosCheckout dados, IReadOnlyList<PedidoItem> itensDoCarrinho)
{
    Pedido pedido = MontarPedido(dados, itensDoCarrinho);
    GarantirEstoqueDeTodosOsItens(pedido);
    CalcularValores(pedido, dados.OpcaoEntregaId);
    RegistrarPagamento(pedido, dados.Pagamento);

    BaixarEstoque(pedido);
    pedido.Finalizar();
    _pedidos.Adicionar(pedido);
    _unidadeDeTrabalho.SalvarAlteracoes();
    return pedido;
}
```

O método público lê como a lista de passos da regra de negócio; cada passo é um método privado pequeno.

## Discussão crítica

| Critério | Versão A (sem IA) | Versão B (com IA) |
|---|---|---|
| Legibilidade | Um bloco único; nomes curtos (`i`, `p`, `op`, `qtd`) | Método principal de 10 linhas que descreve o processo; nomes do domínio |
| Desconto | Regra copiada no método (`5` e `0.05m`); **ignora a `IDescontoStrategy`** recebida no construtor — trocar o desconto na `FabricaDeControladores` não tem efeito (quebra o OCP) | Usa `pedido.AplicarDesconto(_desconto)`: a regra fica só em `DescontoQuantidade` |
| Frete | Recalcula peso × valor por kg na mão, duplicando o `FreteService` (quebra o DRY) | Usa `IFreteService`, o mesmo da etapa Entrega |
| Estoque | Baixa o estoque **dentro do laço**: se o 2º item não tiver estoque, o 1º já foi baixado na memória do `DbContext`; um novo clique em "Confirmar" desconta de novo | Confere **todos** os itens antes de alterar qualquer um; só baixa depois de pagamento validado |
| Erros | `throw new Exception(...)`: o `FrmCheckout` só trata `EstoqueInsuficienteException`, então o programa fecha com erro | Lança `EstoqueInsuficienteException` / `InvalidOperationException`, tratadas na tela com mensagem |
| Valores nulos | `ObterPorId` pode devolver `null` e o código usa direto (`NullReferenceException`); o compilador emite avisos de nulidade | Trata `null` com mensagem clara (`?? throw`) |
| Carrinho vazio | Grava um pedido sem itens, cobrando só o frete | Rejeita (`O carrinho está vazio.`) |
| Encapsulamento | Muda `pedido.Status` direto, pulando as validações de `Pedido.Finalizar()` | Chama `pedido.Finalizar()` |
| Tamanho | 1 método, ~45 linhas | 1 método público de 10 linhas + 6 privados de 3 a 12 linhas |
| Avisos do compilador | 4 (CS8600/CS8602, possível `null`) | 0 |

### Verificação

A versão B foi testada em 17/11/2026 contra um MySQL 8 real, chamando os controladores como as telas chamam:
compra com Pix, desconto com 5 peças e Cartão em 3x, falha de estoque no 2º item seguida de nova tentativa no
mesmo checkout (o 1º item foi baixado uma única vez), boleto, carrinho vazio, cliente PJ e a tela Meus Pedidos
em SQL puro com o mesmo total calculado pelo EF. A versão A foi compilada à parte para contar os avisos.

### O que a revisão humana precisou conferir na versão B

- **Estoque desatualizado no carrinho.** Na revisão apareceu um problema fora do método: o `CarrinhoController`
  usa o mesmo `DbContext` a sessão inteira, e o EF devolve o produto já carregado sem reler o estoque. Depois de
  uma compra, o carrinho continuaria vendo o estoque antigo. Correção: `IProdutoRepository.ObterParaConsulta`
  (leitura sem rastreamento) no carrinho e `AsNoTracking()` na listagem do catálogo.
- **Operador `!` (`item.Produto!`)**: seguro porque `MontarPedido` sempre preenche o produto, mas é uma
  suposição que o grupo precisa entender para explicar no vídeo.
- **Transação**: a garantia de "tudo ou nada" vem do `SaveChanges` do EF. Se o banco falhar no `SaveChanges`,
  as entidades na memória já foram alteradas; como o checkout é fechado e reaberto com um contexto novo, isso
  não afeta a próxima compra.

### Conclusão

As duas versões entregam o pedido no caso feliz, mas a versão A tem três defeitos reais (baixa de estoque pela
metade, exceção genérica que derruba a tela e regra de desconto que ignora a estratégia configurada) e repete
regras que já existiam em outras classes. A versão B é mais curta no método principal, reaproveita as abstrações
do projeto (SOLID) e deixa cada regra em um único lugar (Clean Code). Em compensação, ela só é melhor porque foi
revisada: a IA gera código plausível rápido, e a responsabilidade de entender cada linha continua com o grupo.
