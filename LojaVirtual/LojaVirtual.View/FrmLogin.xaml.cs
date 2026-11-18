using System.Windows;                 // classes do WPF (Window, MessageBox...)
using LojaVirtual.Controller;         // para usar ContaController
using LojaVirtual.Model.Entidades;    // para usar Pessoa

namespace LojaVirtual.View;

// Tela de login. Clientes de teste: ana@teste.com (PF) e empresa@teste.com (PJ), senha 123456.
// "partial": a outra parte da classe vem do FrmLogin.xaml (os controles desenhados).
public partial class FrmLogin : Window
{
    private ContaController _conta = new ContaController();   // controlador desta tela

    // Cliente que entrou. O App lê esta propriedade depois que a tela fecha.
    public Pessoa? ClienteLogado { get; private set; }

    public FrmLogin()
    {
        InitializeComponent();   // cria os controles desenhados no XAML
    }

    // Clique no botão "Entrar".
    private void BtnEntrar_Click(object sender, RoutedEventArgs e)
    {
        ClienteLogado = _conta.Entrar(txtEmail.Text, txtSenha.Password);   // pede para o controlador conferir

        if (ClienteLogado == null)   // e-mail ou senha errados
        {
            MessageBox.Show("E-mail ou senha inválidos.");   // caixa de mensagem
            return;                                          // continua na tela de login
        }

        DialogResult = true;   // fecha o login avisando que deu certo
    }
}
