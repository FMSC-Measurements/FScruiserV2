namespace FSCruiser.WinForms.DataEntry
{
    partial class TallyRow
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                UnWireCount();
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
            this._tallyBTN = new FSCruiser.WinForms.DataEntry.TallyRow.TallyRowButton();
            this._settingsBTN = new FSCruiser.WinForms.DataEntry.TallyRow.TallyRowButton();
            this.SuspendLayout();
            // 
            // _tallyBTN
            // 
            this._tallyBTN.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tallyBTN.Font = new System.Drawing.Font("Tahoma", 12.25F, System.Drawing.FontStyle.Bold);
            this._tallyBTN.Location = new System.Drawing.Point(0, 0);
            this._tallyBTN.Name = "_tallyBTN";
            this._tallyBTN.Size = new System.Drawing.Size(143, 35);
            this._tallyBTN.TabIndex = 3;
            this._tallyBTN.Text = "[_] <----> ###";
            // 
            // _settingsBTN
            // 
            this._settingsBTN.Dock = System.Windows.Forms.DockStyle.Right;
            this._settingsBTN.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold);
            this._settingsBTN.Location = new System.Drawing.Point(143, 0);
            this._settingsBTN.Name = "_settingsBTN";
            this._settingsBTN.Size = new System.Drawing.Size(17, 35);
            this._settingsBTN.TabIndex = 2;
            this._settingsBTN.Text = "i";
            // 
            // TallyRow
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this._tallyBTN);
            this.Controls.Add(this._settingsBTN);
            this.Name = "TallyRow";
            this.Size = new System.Drawing.Size(160, 35);
            this.ResumeLayout(false);

        }

        #endregion

        private TallyRowButton _settingsBTN;
        private TallyRowButton _tallyBTN;

    }
}
