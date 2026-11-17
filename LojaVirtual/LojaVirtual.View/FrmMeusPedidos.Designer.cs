namespace LojaVirtual.View;

partial class FrmMeusPedidos
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
        dgvPedidos = new DataGridView();
        lblResumo = new Label();
        ((System.ComponentModel.ISupportInitialize)dgvPedidos).BeginInit();
        SuspendLayout();
        //
        // dgvPedidos
        //
        dgvPedidos.AllowUserToAddRows = false;
        dgvPedidos.AllowUserToDeleteRows = false;
        dgvPedidos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvPedidos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvPedidos.Location = new Point(12, 12);
        dgvPedidos.MultiSelect = false;
        dgvPedidos.ReadOnly = true;
        dgvPedidos.RowHeadersVisible = false;
        dgvPedidos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvPedidos.Size = new Size(876, 488);
        dgvPedidos.Name = "dgvPedidos";
        dgvPedidos.TabIndex = 0;
        //
        // lblResumo
        //
        lblResumo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        lblResumo.AutoSize = true;
        lblResumo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblResumo.Location = new Point(12, 518);
        lblResumo.Name = "lblResumo";
        lblResumo.TabIndex = 1;
        //
        // FrmMeusPedidos
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(900, 556);
        Controls.Add(dgvPedidos);
        Controls.Add(lblResumo);
        Name = "FrmMeusPedidos";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Meus Pedidos";
        Load += FrmMeusPedidos_Load;
        ((System.ComponentModel.ISupportInitialize)dgvPedidos).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private DataGridView dgvPedidos;
    private Label lblResumo;
}
