namespace FSCruiser.WinForms.Common
{
    partial class FixCNTTallyButton
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
            this._bucketValue_LBL = new System.Windows.Forms.Label();
            this._tallyCount_LBL = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _bucketValue_LBL
            // 
            this._bucketValue_LBL.Dock = System.Windows.Forms.DockStyle.Top;
            this._bucketValue_LBL.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this._bucketValue_LBL.Location = new System.Drawing.Point(0, 0);
            this._bucketValue_LBL.Name = "_bucketValue_LBL";
            this._bucketValue_LBL.Size = new System.Drawing.Size(77, 23);
            this._bucketValue_LBL.TabIndex = 1;
            this._bucketValue_LBL.Text = "<int>\"";
            this._bucketValue_LBL.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // _tallyCount_LBL
            // 
            this._tallyCount_LBL.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tallyCount_LBL.Location = new System.Drawing.Point(0, 23);
            this._tallyCount_LBL.Name = "_tallyCount_LBL";
            this._tallyCount_LBL.Size = new System.Drawing.Size(68, 13);
            this._tallyCount_LBL.TabIndex = 0;
            this._tallyCount_LBL.Text = "Count: <cnt>";
            this._tallyCount_LBL.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // FixCNTTallyButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this._tallyCount_LBL);
            this.Controls.Add(this._bucketValue_LBL);
            this.Name = "FixCNTTallyButton";
            this.Size = new System.Drawing.Size(96, 78);
            this.ResumeLayout(true);

        }

        #endregion

        private System.Windows.Forms.Label _bucketValue_LBL;
        private System.Windows.Forms.Label _tallyCount_LBL;
    }
}
