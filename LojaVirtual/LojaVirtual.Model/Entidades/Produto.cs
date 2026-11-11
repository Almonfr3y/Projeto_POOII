using LojaVirtual.Model.Excecoes;

namespace LojaVirtual.Model.Entidades;

/// <summary>Peça à venda. Já vem cadastrada no banco (seed).</summary>
public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Especificacoes { get; set; } = string.Empty;
    public decimal Preco { get; set; }

    /// <summary>Base do cálculo de frete (OpcaoEntrega.CalcularFrete).</summary>
    public decimal PesoKg { get; set; }

    public int Estoque { get; set; }

    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }

    /// <summary>Lado inverso do N×N de favoritos.</summary>
    public ICollection<Pessoa> FavoritadoPor { get; set; } = new List<Pessoa>();

    public void BaixarEstoque(int quantidade)
    {
        if (quantidade <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantidade), "A quantidade deve ser maior que zero.");
        if (quantidade > Estoque)
            throw new EstoqueInsuficienteException(this, quantidade);

        Estoque -= quantidade;
    }
}
