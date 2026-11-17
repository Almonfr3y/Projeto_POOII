namespace LojaVirtual.View;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        // Primeiro o login. Se o cliente fechar sem entrar, o programa termina.
        FrmLogin login = new FrmLogin();
        if (login.ShowDialog() != DialogResult.OK || login.ClienteLogado == null)
            return;

        SessaoDoUsuario sessao = new SessaoDoUsuario(login.ClienteLogado);
        Application.Run(new FrmInicial(sessao));
    }
}
