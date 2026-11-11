using LojaVirtual.Model.Entidades;

namespace LojaVirtual.Model.Repositorios;

public interface ICategoriaRepository
{
    IReadOnlyList<Categoria> Listar();
}
