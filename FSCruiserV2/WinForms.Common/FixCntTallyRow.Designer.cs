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
        private void InitializeComponent()
        {
            System.Windows.Forms.Panel panel1;
            this._tallyContainer_PNL = new System.Windows.Forms.Panel();
            this._speciesName_LBL = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            panel1 = new System.Windows.Forms.Panel();
            panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tallyContainer_PNL
            // 
            this._tallyContainer_PNL.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tallyContainer_PNL.Location = new System.Drawing.Point(62, 0);
            this._tallyContainer_PNL.Name = "_tallyContainer_PNL";
            this._tallyContainer_PNL.Size = new System.Drawing.Size(336, 62);
            this._tallyContainer_PNL.TabIndex = 0;
            // 
            // _speciesName_LBL
            // 
            this._speciesName_LBL.Dock = System.Windows.Forms.DockStyle.Fill;
            this._speciesName_LBL.Location = new System.Drawing.Point(0, 13);
            this._speciesName_LBL.Name = "_speciesName_LBL";
            this._speciesName_LBL.Size = new System.Drawing.Size(62, 49);
            this._speciesName_LBL.TabIndex = 1;
            this._speciesName_LBL.Text = "<sp>";
            // 
            // panel1
            // 
            panel1.Controls.Add(this._speciesName_LBL);
            panel1.Controls.Add(this.label1);
            panel1.Dock = System.Windows.Forms.DockStyle.Left;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(62, 62);
            panel1.TabIndex = 0;
            // 
            // label1
            // 
#if !NetCF
            this.label1.AutoSize = true;
#endif
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Species:";
            // 
            // FixCntTallyRow
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this._tallyContainer_PNL);
            this.Controls.Add(panel1);
            this.Name = "FixCntTallyRow";
            this.Size = new System.Drawing.Size(398, 62);
            panel1.ResumeLayout(true);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _tallyContainer_PNL;
        private System.Windows.Forms.Label _speciesName_LBL;
        private System.Windows.Forms.Label label1;


    }
}
