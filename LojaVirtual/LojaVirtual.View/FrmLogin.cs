using LojaVirtual.Controller;         // para usar ContaController
using LojaVirtual.Model.Entidades;    // para usar Pessoa

namespace LojaVirtual.View;

// Tela de login. Clientes de teste: ana@teste.com (PF) e empresa@teste.com (PJ), senha 123456.
// "partial": a outra parte da classe está no FrmLogin.Designer.cs (os controles desenhados).
public partial class FrmLogin : Form
{
    private ContaController _conta = new ContaController();   // controlador desta tela

    // Cliente que entrou. O Program lê esta propriedade depois que a tela fecha.
    public Pessoa? ClienteLogado { get; private set; }

    public FrmLogin()
    {
        InitializeComponent();   // cria os botões e caixas de texto desenhados no Designer
    }

    // Clique no botão "Entrar".
    private void BtnEntrar_Click(object sender, EventArgs e)
    {
        ClienteLogado = _conta.Entrar(txtEmail.Text, txtSenha.Text);   // pede para o controlador conferir

        if (ClienteLogado == null)   // e-mail ou senha errados
        {
            MessageBox.Show("E-mail ou senha inválidos.");   // caixa de mensagem
            return;                                          // continua na tela de login
        }

        DialogResult = DialogResult.OK;   // fecha o login
    }
}
