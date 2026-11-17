using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Excecoes;

/// <summary>Exceção específica: as telas capturam só ela e mostram a mensagem ao cliente.</summary>
public class EstoqueInsuficienteException : Exception
{
    public EstoqueInsuficienteException(Produto produto, int quantidadeSolicitada)
        : base($"Estoque insuficiente para \"{produto.Nome}\": solicitado {quantidadeSolicitada}, disponível {produto.Estoque}.")
    {
    }
}
