namespace LojaVirtual.View;

partial class FrmLogin
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        lblTitulo = new Label();
        txtEmail = new TextBox();
        txtSenha = new TextBox();
        btnEntrar = new Button();
        SuspendLayout();
        // 
        // lblTitulo
        // 
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblTitulo.Location = new Point(24, 20);
        lblTitulo.Text = "Entre na sua Conta";
        lblTitulo.Name = "lblTitulo";
        lblTitulo.TabIndex = 0;
        // 
        // txtEmail
        // 
        txtEmail.Location = new Point(24, 64);
        txtEmail.PlaceholderText = "e-mail";
        txtEmail.Size = new Size(292, 23);
        txtEmail.Name = "txtEmail";
        txtEmail.TabIndex = 1;
        // 
        // txtSenha
        // 
        txtSenha.Location = new Point(24, 100);
        txtSenha.PlaceholderText = "Senha";
        txtSenha.Size = new Size(292, 23);
        txtSenha.UseSystemPasswordChar = true;
        txtSenha.Name = "txtSenha";
        txtSenha.TabIndex = 2;
        // 
        // btnEntrar
        // 
        btnEntrar.Location = new Point(24, 140);
        btnEntrar.Size = new Size(292, 32);
        btnEntrar.Text = "Entrar";
        btnEntrar.UseVisualStyleBackColor = true;
        btnEntrar.Click += BtnEntrar_Click;
        btnEntrar.Name = "btnEntrar";
        btnEntrar.TabIndex = 3;
        // 
        // FrmLogin
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(340, 196);
        Controls.Add(lblTitulo);
        Controls.Add(txtEmail);
        Controls.Add(txtSenha);
        Controls.Add(btnEntrar);
        AcceptButton = btnEntrar;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        Name = "FrmLogin";
        Text = "Log In";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblTitulo;
    private TextBox txtEmail;
    private TextBox txtSenha;
    private Button btnEntrar;
}
