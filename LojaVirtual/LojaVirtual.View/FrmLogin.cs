using LojaVirtual.Controller;
using LojaVirtual.Model.Entidades;

namespace LojaVirtual.View;

// Tela de login. Clientes de teste: ana@teste.com (PF) e empresa@teste.com (PJ), senha 123456.
public partial class FrmLogin : Form
{
    private ContaController _conta = new ContaController();

    public Pessoa? ClienteLogado { get; private set; }

    public FrmLogin()
    {
        InitializeComponent();
    }

    private void BtnEntrar_Click(object sender, EventArgs e)
    {
        ClienteLogado = _conta.Entrar(txtEmail.Text, txtSenha.Text);

        if (ClienteLogado == null)
        {
            MessageBox.Show("E-mail ou senha inválidos.");
            return;
        }

        DialogResult = DialogResult.OK;   // fecha o login
    }
}
