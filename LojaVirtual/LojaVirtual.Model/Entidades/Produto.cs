using LojaVirtual.Model.Excecoes;   // para usar EstoqueInsuficienteException

namespace LojaVirtual.Model.Entidades;

// Peça à venda. Os produtos já vêm cadastrados no banco (carga inicial).
public class Produto
{
    public int Id { get; set; }                       // chave primária
    public string Nome { get; set; } = "";            // ex.: "SSD NVMe 1 TB"
    public string Especificacoes { get; set; } = "";  // detalhes técnicos
    public decimal Preco { get; set; }                // decimal: tipo certo para dinheiro
    public decimal PesoKg { get; set; }   // usado no frete
    public int Estoque { get; set; }                  // quantas unidades existem

    public int CategoriaId { get; set; }              // chave estrangeira da categoria
    public Categoria? Categoria { get; set; }         // navegação para a categoria

    // Outro lado do NxN de favoritos: quais clientes favoritaram este produto.
    public ICollection<Pessoa> FavoritadoPor { get; set; } = new List<Pessoa>();

    // Tira unidades do estoque na hora da compra.
    public void BaixarEstoque(int quantidade)
    {
        if (quantidade > Estoque)   // não tem peças suficientes
            throw new EstoqueInsuficienteException(this, quantidade);   // interrompe e avisa quem chamou

        Estoque = Estoque - quantidade;   // desconta do estoque
    }
}
