using LojaVirtual.Controller;
using LojaVirtual.Model.Excecoes;

namespace LojaVirtual.View;

/// <summary>"Tela de Log In". Clientes de teste: ana@teste.com (PF) / empresa@teste.com (PJ), senha 123456.</summary>
public partial class FrmLogin : Form
{
    private readonly ContaController _contas;
    private readonly SessaoDoUsuario _sessao;

    public FrmLogin(ContaController contas, SessaoDoUsuario sessao)
    {
        _contas = contas;
        _sessao = sessao;
        InitializeComponent(); // cria os controles desenhados no Designer
    }

    private void BtnEntrar_Click(object sender, EventArgs e)
    {
        try
        {
            _sessao.Entrar(_contas.Entrar(txtEmail.Text, txtSenha.Text));
            DialogResult = DialogResult.OK; // fecha o login e avisa o Program que deu certo
        }
        catch (LoginInvalidoException ex)
        {
            Mensagem.Aviso(this, ex.Message);
        }
    }
}
