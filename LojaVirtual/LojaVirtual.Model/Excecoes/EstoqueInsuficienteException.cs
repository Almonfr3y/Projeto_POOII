using LojaVirtual.Model.Entidades;   // para usar Produto

namespace LojaVirtual.Model.Excecoes;

// Exceção própria para falta de estoque. As telas mostram a mensagem para o cliente.
// ": Exception" = herda da exceção padrão do C#, então pode ser lançada com throw e pega com catch.
public class EstoqueInsuficienteException : Exception
{
    // Construtor: recebe o produto e a quantidade e monta a mensagem.
    public EstoqueInsuficienteException(Produto produto, int quantidadePedida)
        : base("Estoque insuficiente para \"" + produto.Nome + "\": pedido " + quantidadePedida + ", disponível " + produto.Estoque + ".")
    {
        // ": base(...)" passa a mensagem para o construtor de Exception (vira ex.Message).
    }
}
