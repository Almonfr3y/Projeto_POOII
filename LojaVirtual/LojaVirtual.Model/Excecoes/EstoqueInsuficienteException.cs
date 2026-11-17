using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Excecoes;

// Exceção própria para falta de estoque. As telas mostram a mensagem para o cliente.
public class EstoqueInsuficienteException : Exception
{
    public EstoqueInsuficienteException(Produto produto, int quantidadePedida)
        : base("Estoque insuficiente para \"" + produto.Nome + "\": pedido " + quantidadePedida + ", disponível " + produto.Estoque + ".")
    {
    }
}
