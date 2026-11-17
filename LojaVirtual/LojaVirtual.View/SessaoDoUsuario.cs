using LojaVirtual.Model.Carrinho;
using LojaVirtual.Model.Entidades;

namespace LojaVirtual.View;

// Dados que todas as telas usam enquanto o programa está aberto: o cliente logado e o carrinho.
public class SessaoDoUsuario
{
    public Pessoa Cliente { get; private set; }
    public CarrinhoDeCompras Carrinho { get; private set; }

    public SessaoDoUsuario(Pessoa cliente)
    {
        Cliente = cliente;
        Carrinho = new CarrinhoDeCompras();
    }
}
