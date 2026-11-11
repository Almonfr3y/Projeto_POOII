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
        SuspendLayout();
        // 
        // FrmCheckout
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(900, 560);
        Name = "FrmCheckout";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Finalizar compra";
        ResumeLayout(false);
    }

    #endregion
}
