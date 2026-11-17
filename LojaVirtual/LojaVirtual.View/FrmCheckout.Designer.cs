namespace LojaVirtual.View;

partial class FrmCheckout
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
        lblEtapa = new Label();
        pnlEndereco = new Panel();
        lblEndereco = new Label();
        lstEnderecos = new ListBox();
        pnlEntrega = new Panel();
        lblEntrega = new Label();
        lstEntregas = new ListBox();
        pnlPagamento = new Panel();
        rdoPix = new RadioButton();
        rdoBoleto = new RadioButton();
        rdoCartao = new RadioButton();
        grpCartao = new GroupBox();
        lblTitular = new Label();
        txtTitular = new TextBox();
        lblNumeroCartao = new Label();
        txtNumeroCartao = new TextBox();
        lblParcelas = new Label();
        nudParcelas = new NumericUpDown();
        pnlRevisao = new Panel();
        txtRevisao = new TextBox();
        pnlConcluido = new Panel();
        txtConcluido = new TextBox();
        btnVoltar = new Button();
        btnAvancar = new Button();
        pnlEndereco.SuspendLayout();
        pnlEntrega.SuspendLayout();
        pnlPagamento.SuspendLayout();
        grpCartao.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudParcelas).BeginInit();
        pnlRevisao.SuspendLayout();
        pnlConcluido.SuspendLayout();
        SuspendLayout();
        //
        // lblEtapa
        //
        lblEtapa.AutoSize = true;
        lblEtapa.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblEtapa.Location = new Point(12, 12);
        lblEtapa.Name = "lblEtapa";
        lblEtapa.TabIndex = 0;
        //
        // pnlEndereco
        //
        pnlEndereco.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pnlEndereco.Controls.Add(lblEndereco);
        pnlEndereco.Controls.Add(lstEnderecos);
        pnlEndereco.Location = new Point(12, 48);
        pnlEndereco.Size = new Size(876, 452);
        pnlEndereco.Name = "pnlEndereco";
        pnlEndereco.TabIndex = 1;
        //
        // lblEndereco
        //
        lblEndereco.AutoSize = true;
        lblEndereco.Location = new Point(0, 0);
        lblEndereco.Text = "Escolha o endereço de entrega:";
        lblEndereco.Name = "lblEndereco";
        //
        // lstEnderecos
        //
        lstEnderecos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lstEnderecos.IntegralHeight = false;
        lstEnderecos.Location = new Point(0, 24);
        lstEnderecos.Size = new Size(876, 428);
        lstEnderecos.Name = "lstEnderecos";
        lstEnderecos.TabIndex = 0;
        //
        // pnlEntrega
        //
        pnlEntrega.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pnlEntrega.Controls.Add(lblEntrega);
        pnlEntrega.Controls.Add(lstEntregas);
        pnlEntrega.Location = new Point(12, 48);
        pnlEntrega.Size = new Size(876, 452);
        pnlEntrega.Name = "pnlEntrega";
        pnlEntrega.TabIndex = 2;
        //
        // lblEntrega
        //
        lblEntrega.AutoSize = true;
        lblEntrega.Location = new Point(0, 0);
        lblEntrega.Text = "Escolha a forma de entrega (frete calculado pelo peso do carrinho):";
        lblEntrega.Name = "lblEntrega";
        //
        // lstEntregas
        //
        lstEntregas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lstEntregas.IntegralHeight = false;
        lstEntregas.Location = new Point(0, 24);
        lstEntregas.Size = new Size(876, 428);
        lstEntregas.Name = "lstEntregas";
        lstEntregas.TabIndex = 0;
        //
        // pnlPagamento
        //
        pnlPagamento.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pnlPagamento.Controls.Add(rdoPix);
        pnlPagamento.Controls.Add(rdoBoleto);
        pnlPagamento.Controls.Add(rdoCartao);
        pnlPagamento.Controls.Add(grpCartao);
        pnlPagamento.Location = new Point(12, 48);
        pnlPagamento.Size = new Size(876, 452);
        pnlPagamento.Name = "pnlPagamento";
        pnlPagamento.TabIndex = 3;
        //
        // rdoPix
        //
        rdoPix.AutoSize = true;
        rdoPix.Location = new Point(0, 0);
        rdoPix.Text = "Pix";
        rdoPix.CheckedChanged += FormaDePagamento_CheckedChanged;
        rdoPix.Name = "rdoPix";
        rdoPix.TabIndex = 0;
        //
        // rdoBoleto
        //
        rdoBoleto.AutoSize = true;
        rdoBoleto.Location = new Point(0, 30);
        rdoBoleto.Text = "Boleto bancário";
        rdoBoleto.CheckedChanged += FormaDePagamento_CheckedChanged;
        rdoBoleto.Name = "rdoBoleto";
        rdoBoleto.TabIndex = 1;
        //
        // rdoCartao
        //
        rdoCartao.AutoSize = true;
        rdoCartao.Location = new Point(0, 60);
        rdoCartao.Text = "Cartão de crédito";
        rdoCartao.CheckedChanged += FormaDePagamento_CheckedChanged;
        rdoCartao.Name = "rdoCartao";
        rdoCartao.TabIndex = 2;
        //
        // grpCartao
        //
        grpCartao.Controls.Add(lblTitular);
        grpCartao.Controls.Add(txtTitular);
        grpCartao.Controls.Add(lblNumeroCartao);
        grpCartao.Controls.Add(txtNumeroCartao);
        grpCartao.Controls.Add(lblParcelas);
        grpCartao.Controls.Add(nudParcelas);
        grpCartao.Enabled = false;
        grpCartao.Location = new Point(0, 92);
        grpCartao.Size = new Size(420, 150);
        grpCartao.Text = "Dados do cartão";
        grpCartao.Name = "grpCartao";
        grpCartao.TabIndex = 3;
        //
        // lblTitular
        //
        lblTitular.AutoSize = true;
        lblTitular.Location = new Point(12, 30);
        lblTitular.Text = "Nome do titular:";
        lblTitular.Name = "lblTitular";
        //
        // txtTitular
        //
        txtTitular.Location = new Point(130, 27);
        txtTitular.Size = new Size(270, 23);
        txtTitular.Name = "txtTitular";
        txtTitular.TabIndex = 0;
        //
        // lblNumeroCartao
        //
        lblNumeroCartao.AutoSize = true;
        lblNumeroCartao.Location = new Point(12, 68);
        lblNumeroCartao.Text = "Número:";
        lblNumeroCartao.Name = "lblNumeroCartao";
        //
        // txtNumeroCartao
        //
        txtNumeroCartao.Location = new Point(130, 65);
        txtNumeroCartao.MaxLength = 23;
        txtNumeroCartao.Size = new Size(270, 23);
        txtNumeroCartao.Name = "txtNumeroCartao";
        txtNumeroCartao.TabIndex = 1;
        //
        // lblParcelas
        //
        lblParcelas.AutoSize = true;
        lblParcelas.Location = new Point(12, 106);
        lblParcelas.Text = "Parcelas:";
        lblParcelas.Name = "lblParcelas";
        //
        // nudParcelas
        //
        nudParcelas.Location = new Point(130, 103);
        nudParcelas.Minimum = 1;
        nudParcelas.Value = 1;
        nudParcelas.Size = new Size(60, 23);
        nudParcelas.Name = "nudParcelas";
        nudParcelas.TabIndex = 2;
        //
        // pnlRevisao
        //
        pnlRevisao.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pnlRevisao.Controls.Add(txtRevisao);
        pnlRevisao.Location = new Point(12, 48);
        pnlRevisao.Size = new Size(876, 452);
        pnlRevisao.Name = "pnlRevisao";
        pnlRevisao.TabIndex = 4;
        //
        // txtRevisao
        //
        txtRevisao.Dock = DockStyle.Fill;
        txtRevisao.Font = new Font("Consolas", 10F);
        txtRevisao.Multiline = true;
        txtRevisao.ReadOnly = true;
        txtRevisao.ScrollBars = ScrollBars.Vertical;
        txtRevisao.Name = "txtRevisao";
        txtRevisao.TabIndex = 0;
        //
        // pnlConcluido
        //
        pnlConcluido.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pnlConcluido.Controls.Add(txtConcluido);
        pnlConcluido.Location = new Point(12, 48);
        pnlConcluido.Size = new Size(876, 452);
        pnlConcluido.Name = "pnlConcluido";
        pnlConcluido.TabIndex = 5;
        //
        // txtConcluido
        //
        txtConcluido.Dock = DockStyle.Fill;
        txtConcluido.Font = new Font("Segoe UI", 12F);
        txtConcluido.Multiline = true;
        txtConcluido.ReadOnly = true;
        txtConcluido.Name = "txtConcluido";
        txtConcluido.TabIndex = 0;
        //
        // btnVoltar
        //
        btnVoltar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnVoltar.Location = new Point(562, 512);
        btnVoltar.Size = new Size(140, 32);
        btnVoltar.Text = "Voltar";
        btnVoltar.UseVisualStyleBackColor = true;
        btnVoltar.Click += BtnVoltar_Click;
        btnVoltar.Name = "btnVoltar";
        btnVoltar.TabIndex = 6;
        //
        // btnAvancar
        //
        btnAvancar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnAvancar.Location = new Point(708, 512);
        btnAvancar.Size = new Size(180, 32);
        btnAvancar.Text = "Avançar";
        btnAvancar.UseVisualStyleBackColor = true;
        btnAvancar.Click += BtnAvancar_Click;
        btnAvancar.Name = "btnAvancar";
        btnAvancar.TabIndex = 7;
        //
        // FrmCheckout
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(900, 556);
        Controls.Add(lblEtapa);
        Controls.Add(pnlEndereco);
        Controls.Add(pnlEntrega);
        Controls.Add(pnlPagamento);
        Controls.Add(pnlRevisao);
        Controls.Add(pnlConcluido);
        Controls.Add(btnVoltar);
        Controls.Add(btnAvancar);
        Name = "FrmCheckout";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Finalizar compra";
        Load += FrmCheckout_Load;
        pnlEndereco.ResumeLayout(false);
        pnlEndereco.PerformLayout();
        pnlEntrega.ResumeLayout(false);
        pnlEntrega.PerformLayout();
        pnlPagamento.ResumeLayout(false);
        pnlPagamento.PerformLayout();
        grpCartao.ResumeLayout(false);
        grpCartao.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudParcelas).EndInit();
        pnlRevisao.ResumeLayout(false);
        pnlRevisao.PerformLayout();
        pnlConcluido.ResumeLayout(false);
        pnlConcluido.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblEtapa;
    private Panel pnlEndereco;
    private Label lblEndereco;
    private ListBox lstEnderecos;
    private Panel pnlEntrega;
    private Label lblEntrega;
    private ListBox lstEntregas;
    private Panel pnlPagamento;
    private RadioButton rdoPix;
    private RadioButton rdoBoleto;
    private RadioButton rdoCartao;
    private GroupBox grpCartao;
    private Label lblTitular;
    private TextBox txtTitular;
    private Label lblNumeroCartao;
    private TextBox txtNumeroCartao;
    private Label lblParcelas;
    private NumericUpDown nudParcelas;
    private Panel pnlRevisao;
    private TextBox txtRevisao;
    private Panel pnlConcluido;
    private TextBox txtConcluido;
    private Button btnVoltar;
    private Button btnAvancar;
}
