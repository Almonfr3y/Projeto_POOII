namespace LojaVirtual.View;

// Ponto de entrada: é por aqui que o programa começa.
internal static class Program
{
    [STAThread]   // exigido pelo Windows Forms
    private static void Main()
    {
        ApplicationConfiguration.Initialize();   // configurações padrão do Windows Forms (fonte, DPI)

        // Primeiro o login. Se o cliente fechar sem entrar, o programa termina.
        FrmLogin login = new FrmLogin();
        if (login.ShowDialog() != DialogResult.OK || login.ClienteLogado == null)   // ShowDialog: espera a tela fechar
            return;   // sai do Main = encerra o programa

        SessaoDoUsuario sessao = new SessaoDoUsuario(login.ClienteLogado);   // cria a sessão com o cliente
        Application.Run(new FrmInicial(sessao));   // abre a Tela Inicial; fechar ela encerra o programa
    }
}
