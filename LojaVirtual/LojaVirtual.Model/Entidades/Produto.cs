using LojaVirtual.Model.Excecoes;

namespace LojaVirtual.Model.Entidades;

// Peça à venda. Os produtos já vêm cadastrados no banco (carga inicial).
public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public string Especificacoes { get; set; } = "";
    public decimal Preco { get; set; }
    public decimal PesoKg { get; set; }   // usado no frete
    public int Estoque { get; set; }

    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }

    public ICollection<Pessoa> FavoritadoPor { get; set; } = new List<Pessoa>();

    public void BaixarEstoque(int quantidade)
    {
        if (quantidade > Estoque)
            throw new EstoqueInsuficienteException(this, quantidade);

        Estoque = Estoque - quantidade;
    }
}
