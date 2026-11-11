using LojaVirtual.Controller;
using LojaVirtual.Model.Servicos;

namespace LojaVirtual.View;

/// <summary>Estado da sessão: o cliente logado e o carrinho atual.</summary>
public class SessaoDoUsuario
{
    private ClienteLogado? _cliente;

    public SessaoDoUsuario(CarrinhoController carrinho) => Carrinho = carrinho;

    public CarrinhoController Carrinho { get; }

    public ClienteLogado Cliente => _cliente ?? throw new InvalidOperationException("Nenhum cliente logado.");

    public bool EstaLogado => _cliente is not null;

    public void Entrar(ClienteLogado cliente) => _cliente = cliente;
}
