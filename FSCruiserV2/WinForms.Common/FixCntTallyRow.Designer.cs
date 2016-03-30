using System.Drawing;
namespace FSCruiser.WinForms.Common
{
    partial class FixCntTallyRow
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent(int width, int height, IFixCntObject obj)
        {
            int panelContentWidth = width - lblTitle.Width;
            int panelButtonHeight = height - 25;

            this.lblTitle = new System.Windows.Forms.Label();
            this.panel = new System.Windows.Forms.Panel();
            this.pnlHeaders = new System.Windows.Forms.Panel();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(50, height);
            this.lblTitle.Text = obj.Name;
            this.lblTitle.Font = new Font(FontFamily.GenericSansSerif, 25);
            // 
            // panel
            // 
            this.panel.Controls.Add(this.pnlButtons);
            this.panel.Controls.Add(this.pnlHeaders);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(50, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(panelContentWidth, height);
            // 
            // pnlHeaders
            // 
            //this.pnlHeaders.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pnlHeaders.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeaders.Location = new System.Drawing.Point(0, 0);
            this.pnlHeaders.Name = "pnlHeaders";
            this.pnlHeaders.Size = new System.Drawing.Size(panelContentWidth, 25);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlButtons.Location = new System.Drawing.Point(0, 25);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(panelContentWidth, panelButtonHeight);

            // 
            // FixCntTallyRow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel);
            this.Controls.Add(this.lblTitle);
            this.Name = "FixCntTallyRow";
            this.Size = new System.Drawing.Size(width, height);
            this.panel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Panel pnlHeaders;

    }
}
