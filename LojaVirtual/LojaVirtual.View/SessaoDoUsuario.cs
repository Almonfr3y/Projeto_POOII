using LojaVirtual.Controller;
using LojaVirtual.Model.Entidades;

namespace LojaVirtual.View;

/// <summary>O que as telas compartilham enquanto o programa está aberto: o cliente logado e o carrinho.</summary>
public class SessaoDoUsuario
{
    private Pessoa? _cliente; // null até o login dar certo

    public SessaoDoUsuario(CarrinhoController carrinho) => Carrinho = carrinho;

    public CarrinhoController Carrinho { get; }

    // As telas só abrem depois do login, então aqui o cliente sempre existe.
    public Pessoa Cliente => _cliente ?? throw new InvalidOperationException("Nenhum cliente logado.");

    public void Entrar(Pessoa cliente) => _cliente = cliente;
}
