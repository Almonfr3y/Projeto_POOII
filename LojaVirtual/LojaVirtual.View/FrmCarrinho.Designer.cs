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
        SuspendLayout();
        // 
        // FrmCarrinho
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(900, 560);
        Name = "FrmCarrinho";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Carrinho";
        ResumeLayout(false);
    }

    #endregion
}
