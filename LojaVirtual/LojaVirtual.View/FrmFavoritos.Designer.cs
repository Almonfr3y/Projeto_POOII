namespace LojaVirtual.View;

partial class FrmFavoritos
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
        txtBusca = new TextBox();
        btnBuscar = new Button();
        dgvFavoritos = new DataGridView();
        btnDesfavoritar = new Button();
        btnAdicionar = new Button();
        ((System.ComponentModel.ISupportInitialize)dgvFavoritos).BeginInit();
        SuspendLayout();
        //
        // txtBusca
        //
        txtBusca.Location = new Point(12, 12);
        txtBusca.PlaceholderText = "pesquise favorito";
        txtBusca.Size = new Size(330, 23);
        txtBusca.Name = "txtBusca";
        txtBusca.TabIndex = 0;
        //
        // btnBuscar
        //
        btnBuscar.Location = new Point(348, 11);
        btnBuscar.Size = new Size(80, 25);
        btnBuscar.Text = "Buscar";
        btnBuscar.UseVisualStyleBackColor = true;
        btnBuscar.Click += BtnBuscar_Click;
        btnBuscar.Name = "btnBuscar";
        btnBuscar.TabIndex = 1;
        //
        // dgvFavoritos
        //
        dgvFavoritos.AllowUserToAddRows = false;
        dgvFavoritos.AllowUserToDeleteRows = false;
        dgvFavoritos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvFavoritos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvFavoritos.Location = new Point(12, 46);
        dgvFavoritos.MultiSelect = false;
        dgvFavoritos.ReadOnly = true;
        dgvFavoritos.RowHeadersVisible = false;
        dgvFavoritos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvFavoritos.Size = new Size(876, 454);
        dgvFavoritos.Name = "dgvFavoritos";
        dgvFavoritos.TabIndex = 2;
        //
        // btnDesfavoritar
        //
        btnDesfavoritar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnDesfavoritar.Location = new Point(562, 512);
        btnDesfavoritar.Size = new Size(140, 32);
        btnDesfavoritar.Text = "Desfavoritar";
        btnDesfavoritar.UseVisualStyleBackColor = true;
        btnDesfavoritar.Click += BtnDesfavoritar_Click;
        btnDesfavoritar.Name = "btnDesfavoritar";
        btnDesfavoritar.TabIndex = 3;
        //
        // btnAdicionar
        //
        btnAdicionar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnAdicionar.Location = new Point(708, 512);
        btnAdicionar.Size = new Size(180, 32);
        btnAdicionar.Text = "Adicionar ao carrinho";
        btnAdicionar.UseVisualStyleBackColor = true;
        btnAdicionar.Click += BtnAdicionar_Click;
        btnAdicionar.Name = "btnAdicionar";
        btnAdicionar.TabIndex = 4;
        //
        // FrmFavoritos
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(900, 556);
        Controls.Add(txtBusca);
        Controls.Add(btnBuscar);
        Controls.Add(dgvFavoritos);
        Controls.Add(btnDesfavoritar);
        Controls.Add(btnAdicionar);
        AcceptButton = btnBuscar;
        Name = "FrmFavoritos";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Favoritos";
        Load += FrmFavoritos_Load;
        ((System.ComponentModel.ISupportInitialize)dgvFavoritos).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TextBox txtBusca;
    private Button btnBuscar;
    private DataGridView dgvFavoritos;
    private Button btnDesfavoritar;
    private Button btnAdicionar;
}
