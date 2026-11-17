using LojaVirtual.Model.Carrinho;     // para usar CarrinhoDeCompras
using LojaVirtual.Model.Entidades;    // para usar Pessoa

namespace LojaVirtual.View;

// Dados que todas as telas usam enquanto o programa está aberto: o cliente logado e o carrinho.
// O mesmo objeto é passado de tela em tela.
public class SessaoDoUsuario
{
    public Pessoa Cliente { get; private set; }               // quem fez login (PF ou PJ)
    public CarrinhoDeCompras Carrinho { get; private set; }   // o carrinho desta sessão

    // Construtor: chamado logo depois do login.
    public SessaoDoUsuario(Pessoa cliente)
    {
        Cliente = cliente;                       // guarda o cliente logado
        Carrinho = new CarrinhoDeCompras();      // começa com o carrinho vazio
    }
}
