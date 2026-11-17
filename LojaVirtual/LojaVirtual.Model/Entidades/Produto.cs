using LojaVirtual.Model.Excecoes;

namespace LojaVirtual.Model.Entidades;

/// <summary>Peça à venda. Vem cadastrada no banco pela carga inicial.</summary>
public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Especificacoes { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public decimal PesoKg { get; set; }      // usado no cálculo do frete
    public int Estoque { get; set; }

    public int CategoriaId { get; set; }     // chave estrangeira
    public Categoria? Categoria { get; set; }

    public ICollection<Pessoa> FavoritadoPor { get; set; } = new List<Pessoa>(); // outro lado do N×N de favoritos

    /// <summary>Tira peças do estoque; impede estoque negativo.</summary>
    public void BaixarEstoque(int quantidade)
    {
        if (quantidade <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantidade), "A quantidade deve ser maior que zero.");
        if (quantidade > Estoque)
            throw new EstoqueInsuficienteException(this, quantidade);

        Estoque -= quantidade;
    }
}
