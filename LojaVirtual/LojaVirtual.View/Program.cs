using LojaVirtual.Controller.Composicao;

namespace LojaVirtual.View;

internal static class Program
{
    /// <summary>O aplicativo começa no login; com o cliente logado, abre a Tela Inicial.</summary>
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        var fabrica = new FabricaDeControladores();
        var sessao = new SessaoDoUsuario(fabrica.CriarCarrinhoController());

        using (var login = new FrmLogin(fabrica.CriarContaController(), sessao))
        {
            if (login.ShowDialog() != DialogResult.OK)
                return;
        }

        Application.Run(new FrmInicial(fabrica, sessao));
    }
}
