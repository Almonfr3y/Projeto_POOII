namespace LojaVirtual.View;

partial class FrmCarrinho
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
        dgvItens = new DataGridView();
        lblSubtotal = new Label();
        lblQuantidade = new Label();
        nudQuantidade = new NumericUpDown();
        btnAlterar = new Button();
        btnRemover = new Button();
        btnFinalizar = new Button();
        ((System.ComponentModel.ISupportInitialize)dgvItens).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudQuantidade).BeginInit();
        SuspendLayout();
        //
        // dgvItens
        //
        dgvItens.AllowUserToAddRows = false;
        dgvItens.AllowUserToDeleteRows = false;
        dgvItens.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvItens.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvItens.Location = new Point(12, 12);
        dgvItens.MultiSelect = false;
        dgvItens.ReadOnly = true;
        dgvItens.RowHeadersVisible = false;
        dgvItens.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvItens.Size = new Size(876, 488);
        dgvItens.SelectionChanged += DgvItens_SelectionChanged;
        dgvItens.Name = "dgvItens";
        dgvItens.TabIndex = 0;
        //
        // lblSubtotal
        //
        lblSubtotal.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        lblSubtotal.AutoSize = true;
        lblSubtotal.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblSubtotal.Location = new Point(12, 517);
        lblSubtotal.Name = "lblSubtotal";
        lblSubtotal.TabIndex = 1;
        //
        // lblQuantidade
        //
        lblQuantidade.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        lblQuantidade.AutoSize = true;
        lblQuantidade.Location = new Point(298, 520);
        lblQuantidade.Text = "Quantidade:";
        lblQuantidade.Name = "lblQuantidade";
        lblQuantidade.TabIndex = 2;
        //
        // nudQuantidade
        //
        nudQuantidade.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        nudQuantidade.Location = new Point(374, 517);
        nudQuantidade.Minimum = 1;
        nudQuantidade.Maximum = 99;
        nudQuantidade.Value = 1;
        nudQuantidade.Size = new Size(56, 23);
        nudQuantidade.Name = "nudQuantidade";
        nudQuantidade.TabIndex = 3;
        //
        // btnAlterar
        //
        btnAlterar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnAlterar.Location = new Point(436, 512);
        btnAlterar.Size = new Size(110, 32);
        btnAlterar.Text = "Alterar";
        btnAlterar.UseVisualStyleBackColor = true;
        btnAlterar.Click += BtnAlterar_Click;
        btnAlterar.Name = "btnAlterar";
        btnAlterar.TabIndex = 4;
        //
        // btnRemover
        //
        btnRemover.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnRemover.Location = new Point(552, 512);
        btnRemover.Size = new Size(150, 32);
        btnRemover.Text = "Remover do carrinho";
        btnRemover.UseVisualStyleBackColor = true;
        btnRemover.Click += BtnRemover_Click;
        btnRemover.Name = "btnRemover";
        btnRemover.TabIndex = 5;
        //
        // btnFinalizar
        //
        btnFinalizar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnFinalizar.Location = new Point(708, 512);
        btnFinalizar.Size = new Size(180, 32);
        btnFinalizar.Text = "Finalizar Compra";
        btnFinalizar.UseVisualStyleBackColor = true;
        btnFinalizar.Click += BtnFinalizar_Click;
        btnFinalizar.Name = "btnFinalizar";
        btnFinalizar.TabIndex = 6;
        //
        // FrmCarrinho
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(900, 556);
        Controls.Add(dgvItens);
        Controls.Add(lblSubtotal);
        Controls.Add(lblQuantidade);
        Controls.Add(nudQuantidade);
        Controls.Add(btnAlterar);
        Controls.Add(btnRemover);
        Controls.Add(btnFinalizar);
        Name = "FrmCarrinho";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Carrinho";
        Load += FrmCarrinho_Load;
        ((System.ComponentModel.ISupportInitialize)dgvItens).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudQuantidade).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private DataGridView dgvItens;
    private Label lblSubtotal;
    private Label lblQuantidade;
    private NumericUpDown nudQuantidade;
    private Button btnAlterar;
    private Button btnRemover;
    private Button btnFinalizar;
}
