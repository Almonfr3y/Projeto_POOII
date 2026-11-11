namespace LojaVirtual.View;

partial class FrmInicial
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
        btnCarrinho = new Button();
        btnFavoritos = new Button();
        btnMeusPedidos = new Button();
        cmbCategoria = new ComboBox();
        lblCliente = new Label();
        dgvProdutos = new DataGridView();
        btnFavoritar = new Button();
        btnAdicionar = new Button();
        ((System.ComponentModel.ISupportInitialize)dgvProdutos).BeginInit();
        SuspendLayout();
        // 
        // txtBusca
        // 
        txtBusca.Location = new Point(12, 12);
        txtBusca.PlaceholderText = "pesquise aqui";
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
        // btnCarrinho
        // 
        btnCarrinho.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnCarrinho.Location = new Point(562, 11);
        btnCarrinho.Size = new Size(100, 25);
        btnCarrinho.Text = "🛒 Carrinho";
        btnCarrinho.UseVisualStyleBackColor = true;
        btnCarrinho.Click += BtnCarrinho_Click;
        btnCarrinho.Name = "btnCarrinho";
        btnCarrinho.TabIndex = 2;
        // 
        // btnFavoritos
        // 
        btnFavoritos.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnFavoritos.Location = new Point(668, 11);
        btnFavoritos.Size = new Size(100, 25);
        btnFavoritos.Text = "⭐ Favoritos";
        btnFavoritos.UseVisualStyleBackColor = true;
        btnFavoritos.Click += BtnFavoritos_Click;
        btnFavoritos.Name = "btnFavoritos";
        btnFavoritos.TabIndex = 3;
        // 
        // btnMeusPedidos
        // 
        btnMeusPedidos.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnMeusPedidos.Location = new Point(774, 11);
        btnMeusPedidos.Size = new Size(114, 25);
        btnMeusPedidos.Text = "Meus Pedidos";
        btnMeusPedidos.UseVisualStyleBackColor = true;
        btnMeusPedidos.Click += BtnMeusPedidos_Click;
        btnMeusPedidos.Name = "btnMeusPedidos";
        btnMeusPedidos.TabIndex = 4;
        // 
        // cmbCategoria
        // 
        cmbCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCategoria.Location = new Point(12, 46);
        cmbCategoria.Size = new Size(220, 23);
        cmbCategoria.SelectedIndexChanged += CmbCategoria_SelectedIndexChanged;
        cmbCategoria.Name = "cmbCategoria";
        cmbCategoria.TabIndex = 5;
        // 
        // lblCliente
        // 
        lblCliente.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblCliente.AutoSize = false;
        lblCliente.Location = new Point(562, 46);
        lblCliente.Size = new Size(326, 23);
        lblCliente.TextAlign = ContentAlignment.MiddleRight;
        lblCliente.Name = "lblCliente";
        lblCliente.TabIndex = 6;
        // 
        // dgvProdutos
        // 
        dgvProdutos.AllowUserToAddRows = false;
        dgvProdutos.AllowUserToDeleteRows = false;
        dgvProdutos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvProdutos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvProdutos.Location = new Point(12, 80);
        dgvProdutos.MultiSelect = false;
        dgvProdutos.ReadOnly = true;
        dgvProdutos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvProdutos.Size = new Size(876, 420);
        dgvProdutos.Name = "dgvProdutos";
        dgvProdutos.TabIndex = 7;
        // 
        // btnFavoritar
        // 
        btnFavoritar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnFavoritar.Location = new Point(562, 512);
        btnFavoritar.Size = new Size(140, 32);
        btnFavoritar.Text = "⭐ Favoritar";
        btnFavoritar.UseVisualStyleBackColor = true;
        btnFavoritar.Click += BtnFavoritar_Click;
        btnFavoritar.Name = "btnFavoritar";
        btnFavoritar.TabIndex = 8;
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
        btnAdicionar.TabIndex = 9;
        // 
        // FrmInicial
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(900, 556);
        Controls.Add(txtBusca);
        Controls.Add(btnBuscar);
        Controls.Add(btnCarrinho);
        Controls.Add(btnFavoritos);
        Controls.Add(btnMeusPedidos);
        Controls.Add(cmbCategoria);
        Controls.Add(lblCliente);
        Controls.Add(dgvProdutos);
        Controls.Add(btnFavoritar);
        Controls.Add(btnAdicionar);
        StartPosition = FormStartPosition.CenterScreen;
        Name = "FrmInicial";
        Text = "Loja Virtual de Peças";
        Load += FrmInicial_Load;
        ((System.ComponentModel.ISupportInitialize)dgvProdutos).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TextBox txtBusca;
    private Button btnBuscar;
    private Button btnCarrinho;
    private Button btnFavoritos;
    private Button btnMeusPedidos;
    private ComboBox cmbCategoria;
    private Label lblCliente;
    private DataGridView dgvProdutos;
    private Button btnFavoritar;
    private Button btnAdicionar;
}
