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
            this._settingsBTN = new System.Windows.Forms.Button();
            this._tallyBTN = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _settingsBTN
            // 
            this._settingsBTN.Dock = System.Windows.Forms.DockStyle.Right;
            this._settingsBTN.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold);
            this._settingsBTN.Location = new System.Drawing.Point(258, 0);
            this._settingsBTN.Name = "_settingsBTN";
            this._settingsBTN.Size = new System.Drawing.Size(17, 56);
            this._settingsBTN.TabIndex = 2;
            this._settingsBTN.Text = "i";
            // 
            // _tallyBTN
            // 
            this._tallyBTN.Dock = System.Windows.Forms.DockStyle.Fill;
            //this._tallyBTN.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._tallyBTN.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular);
            this._tallyBTN.Location = new System.Drawing.Point(0, 0);
            this._tallyBTN.Name = "_tallyBTN";
            this._tallyBTN.Size = new System.Drawing.Size(258, 56);
            this._tallyBTN.TabIndex = 3;
            this._tallyBTN.Text = "[_] <---->\r\n cnt:###";
            //this._tallyBTN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TallyRow
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this._tallyBTN);
            this.Controls.Add(this._settingsBTN);
            this.Name = "TallyRow";
            this.Size = new System.Drawing.Size(275, 56);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _settingsBTN;
        private System.Windows.Forms.Button _tallyBTN;

    }
}
