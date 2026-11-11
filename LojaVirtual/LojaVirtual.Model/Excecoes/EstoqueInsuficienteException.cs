using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Excecoes;

public class EstoqueInsuficienteException : Exception
{
    public EstoqueInsuficienteException(Produto produto, int quantidadeSolicitada)
        : base($"Estoque insuficiente para \"{produto.Nome}\": solicitado {quantidadeSolicitada}, disponível {produto.Estoque}.")
    {
    }
}
