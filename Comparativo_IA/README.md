# Comparativo com e sem IA — `PedidoController.Finalizar`

O enunciado pede para comparar duas versões do mesmo trecho, uma feita com IA e outra sem, e discutir as diferenças.
O trecho escolhido é o `PedidoController.Finalizar`, que fecha a compra: monta o pedido, calcula frete e desconto,
processa o pagamento, baixa o estoque e grava tudo.

| Versão | Arquivo |
|---|---|
| **A — sem IA** | [`VersaoA_SemIA.cs.txt`](VersaoA_SemIA.cs.txt) (fica fora da compilação) |
| **B — com IA** | [`../LojaVirtual/LojaVirtual.Controller/PedidoController.cs`](../LojaVirtual/LojaVirtual.Controller/PedidoController.cs) (a usada no sistema) |

## Como cada versão funciona

**Versão A:** um laço lê cada produto, confere e baixa o estoque, soma peso e quantidade. Depois calcula o frete
e o desconto ali mesmo (`if (qtd >= 5) ... * 0.05m`), processa o pagamento, muda o `Status` direto e grava.

**Versão B:** faz os mesmos passos, na mesma ordem, mas usa o que já existe nas outras classes:
`produto.BaixarEstoque(...)`, `CalcularFrete(...)` (o mesmo da etapa Entrega), `pedido.AplicarDesconto(_desconto)`
e `pedido.Finalizar()`.

## Discussão

| Critério | Versão A (sem IA) | Versão B (com IA) |
|---|---|---|
| Falta de estoque | `throw new Exception(...)`. O `FrmCheckout` só trata `EstoqueInsuficienteException`, então **o programa fecha com erro** | `produto.BaixarEstoque` lança `EstoqueInsuficienteException` e a tela mostra a mensagem |
| Desconto | Regra copiada no método (`5` e `0.05m`). O `IDescontoStrategy` recebido no construtor é **ignorado**: trocar para `SemDesconto` não muda nada (quebra o OCP) | `pedido.AplicarDesconto(_desconto)`: a regra fica só em `DescontoQuantidade` |
| Centavos | Desconto com 3 casas (ex.: R$ 37,475). O banco grava R$ 37,48 e o total da tela fica diferente do total do pedido em "Meus Pedidos" | `DescontoQuantidade` arredonda para 2 casas |
| Frete | Recalcula peso × valor por kg na mão, repetindo o que `OpcaoEntrega.CalcularFrete` já faz (quebra o DRY) | Usa `CalcularFrete`, o mesmo método da etapa Entrega |
| Produto inexistente | `Find` pode devolver `null` e o código usa direto (`NullReferenceException`) | Confere o `null` e mostra mensagem |
| Carrinho vazio | Grava um pedido sem itens, cobrando só o frete | Rejeita com "O carrinho está vazio." |
| Encapsulamento | Muda `pedido.Status` direto, sem passar pelas validações | Chama `pedido.Finalizar()` |
| Conexão | `new LojaDbContext()` sem `using`: a conexão não é fechada | `using`: fecha a conexão no fim |
| Avisos do compilador | 2 (CS8600 e CS8602, possível `null`) | 0 |

### O que foi conferido na revisão humana

- **Arredondamento do desconto.** O teste contra o MySQL mostrou que o total calculado na memória era diferente do
  total lido pela consulta em SQL puro. A causa era o desconto com 3 casas decimais; a correção foi `Math.Round(..., 2)`
  em `DescontoQuantidade`.
- **Estoque baixado antes do pagamento.** Na versão B o estoque é baixado dentro do laço, antes de processar o
  pagamento. Isso é seguro porque nada vai para o banco antes do `SaveChanges`, no final: se der erro (falta de
  estoque, parcelas inválidas), o contexto é descartado e nada é gravado. Isso foi testado.

### Testes

A versão B foi testada em 17/11/2026 contra um MySQL 8 real, chamando os controladores como as telas chamam
(31 verificações): login PF e PJ, catálogo e filtros, favoritos, compra com Pix, desconto com 5 peças e cartão em 3x,
parcelas inválidas, falta de estoque no 2º item seguida de nova tentativa, boleto, carrinho vazio e Meus Pedidos em
SQL puro com o mesmo total do pedido.

### Conclusão

As duas versões funcionam no caso normal. A versão A tem defeitos que só aparecem nos casos de erro (programa
fechando por falta de estoque, centavos diferentes, desconto que não respeita a regra configurada) e repete regras
que já existiam em outras classes. A versão B tem praticamente o mesmo tamanho, mas reaproveita os métodos das
entidades e deixa cada regra em um lugar só. A IA ajudou a enxergar esses casos de erro, mas o problema dos centavos
só apareceu no teste: o código gerado precisa ser testado e entendido pelo grupo.
