namespace FSCruiser.WinForms
{
    partial class FormCruiserSelection
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Button button1;
            System.Windows.Forms.Panel panel1;
            this._treeNumLBL = new System.Windows.Forms.Label();
            this._sampleGroupLBL = new System.Windows.Forms.Label();
            this._stratumLBL = new System.Windows.Forms.Label();
            this._crusierSelectPanel = new System.Windows.Forms.Panel();
            button1 = new System.Windows.Forms.Button();
            panel1 = new System.Windows.Forms.Panel();
            panel1.SuspendLayout();
            this._crusierSelectPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            button1.Dock = System.Windows.Forms.DockStyle.Top;
            button1.Location = new System.Drawing.Point(0, 0);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(240, 20);
            button1.TabIndex = 0;
            button1.Text = "#. <init>";
            // 
            // panel1
            // 
            panel1.Controls.Add(this._treeNumLBL);
            panel1.Controls.Add(this._sampleGroupLBL);
            panel1.Controls.Add(this._stratumLBL);
            panel1.Dock = System.Windows.Forms.DockStyle.Top;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(240, 41);
            // 
            // _treeNumLBL
            // 
            this._treeNumLBL.Location = new System.Drawing.Point(4, 0);
            this._treeNumLBL.Name = "_treeNumLBL";
            this._treeNumLBL.Size = new System.Drawing.Size(74, 20);
            this._treeNumLBL.Text = "Tree #: ___";
            // 
            // _sampleGroupLBL
            // 
            this._sampleGroupLBL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._sampleGroupLBL.Location = new System.Drawing.Point(4, 18);
            this._sampleGroupLBL.Name = "_sampleGroupLBL";
            this._sampleGroupLBL.Size = new System.Drawing.Size(236, 20);
            this._sampleGroupLBL.Text = "Sg: ___";
            // 
            // _stratumLBL
            // 
            this._stratumLBL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._stratumLBL.Location = new System.Drawing.Point(84, 0);
            this._stratumLBL.Name = "_stratumLBL";
            this._stratumLBL.Size = new System.Drawing.Size(156, 20);
            this._stratumLBL.Text = "St: ___";
            // 
            // _crusierSelectPanel
            // 
            this._crusierSelectPanel.Controls.Add(button1);
            this._crusierSelectPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._crusierSelectPanel.Location = new System.Drawing.Point(0, 41);
            this._crusierSelectPanel.Name = "_crusierSelectPanel";
            this._crusierSelectPanel.Size = new System.Drawing.Size(240, 253);
            // 
            // FormCruiserSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this._crusierSelectPanel);
            this.Controls.Add(panel1);
            this.MinimizeBox = false;
            this.Name = "FormCruiserSelection";
            this.Text = "Select Cruiser";
            panel1.ResumeLayout(false);
            this._crusierSelectPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label _sampleGroupLBL;
        private System.Windows.Forms.Label _stratumLBL;
        private System.Windows.Forms.Label _treeNumLBL;
        private System.Windows.Forms.Panel _crusierSelectPanel;

    }
}