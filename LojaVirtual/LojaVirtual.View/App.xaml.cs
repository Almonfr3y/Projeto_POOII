using System.Windows;   // classes do WPF (Application, Window, MessageBox...)

namespace LojaVirtual.View;

// Parte em C# do App.xaml. "partial": a outra parte é gerada a partir do XAML.
public partial class App : Application
{
    // Roda quando o programa abre (ligado pelo Startup="App_Startup" do App.xaml).
    private void App_Startup(object sender, StartupEventArgs e)
    {
        // Primeiro o login. Se o cliente fechar sem entrar, o programa termina.
        FrmLogin login = new FrmLogin();
        if (login.ShowDialog() != true || login.ClienteLogado == null)   // ShowDialog: espera a tela fechar
        {
            Shutdown();   // encerra o programa
            return;
        }

        SessaoDoUsuario sessao = new SessaoDoUsuario(login.ClienteLogado);   // cria a sessão com o cliente

        // A Tela Inicial vira a janela principal: fechar ela encerra o programa.
        FrmInicial inicial = new FrmInicial(sessao);
        MainWindow = inicial;
        ShutdownMode = ShutdownMode.OnMainWindowClose;
        inicial.Show();
    }
}
