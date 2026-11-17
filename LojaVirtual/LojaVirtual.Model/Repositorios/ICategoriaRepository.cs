using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Repositorios;

// Interfaces pequenas, uma por assunto (ISP). O Model só conhece a interface;
// a implementação com Entity Framework fica na camada Data (DIP).
public interface ICategoriaRepository
{
    IReadOnlyList<Categoria> Listar();
}
