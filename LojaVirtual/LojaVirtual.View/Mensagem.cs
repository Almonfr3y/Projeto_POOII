namespace LojaVirtual.View;

/// <summary>Atalhos para as caixas de mensagem usadas em todas as telas (evita repetir o MessageBox).</summary>
internal static class Mensagem
{
    public static void Info(Form tela, string texto) =>
        MessageBox.Show(tela, texto, tela.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

    public static void Aviso(Form tela, string texto) =>
        MessageBox.Show(tela, texto, tela.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);

    public static void Erro(Form tela, string texto) =>
        MessageBox.Show(tela, texto, tela.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
}
