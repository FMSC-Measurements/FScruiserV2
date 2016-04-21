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
            this._tallyContainer_PNL = new System.Windows.Forms.Panel();
            this._speciesName_LBL = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _tallyContainer_PNL
            // 
            this._tallyContainer_PNL.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tallyContainer_PNL.Location = new System.Drawing.Point(71, 0);
            this._tallyContainer_PNL.Name = "_tallyContainer_PNL";
            this._tallyContainer_PNL.Size = new System.Drawing.Size(327, 62);
            // 
            // _speciesName_LBL
            // 
            this._speciesName_LBL.Dock = System.Windows.Forms.DockStyle.Left;
            this._speciesName_LBL.Location = new System.Drawing.Point(0, 0);
            this._speciesName_LBL.Name = "_speciesName_LBL";
            this._speciesName_LBL.Size = new System.Drawing.Size(71, 62);
            this._speciesName_LBL.Text = "label1";
            // 
            // FixCntTallyRow
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this._tallyContainer_PNL);
            this.Controls.Add(this._speciesName_LBL);
            this.Name = "FixCntTallyRow";
            this.Size = new System.Drawing.Size(398, 62);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _tallyContainer_PNL;
        private System.Windows.Forms.Label _speciesName_LBL;


    }
}
