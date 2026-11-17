using LojaVirtual.Controller.Composicao;

namespace LojaVirtual.View;

internal static class Program
{
    /// <summary>Ponto de entrada: abre o login e, se o cliente entrar, a Tela Inicial.</summary>
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize(); // configurações padrão do Windows Forms (fonte, DPI)

        var fabrica = new FabricaDeControladores();                            // cria os controladores
        var sessao = new SessaoDoUsuario(fabrica.CriarCarrinhoController());   // um carrinho para a sessão toda

        using (var login = new FrmLogin(fabrica.CriarContaController(), sessao))
        {
            if (login.ShowDialog() != DialogResult.OK) // fechou o login sem entrar: encerra o programa
                return;
        }

        Application.Run(new FrmInicial(fabrica, sessao)); // fechar a Tela Inicial encerra o programa
    }
}
