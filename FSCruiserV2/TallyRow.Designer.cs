namespace FSCruiserV2
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
            this._settingsButton = new System.Windows.Forms.Button();
            this._discriptionLabel = new System.Windows.Forms.Label();
            this._hotKeyLabel = new System.Windows.Forms.Label();
            this._tallyButton = new FMSC.Controls.Mobile.ButtonPanel();
            this.SuspendLayout();
            // 
            // _settingsButton
            // 
            this._settingsButton.Dock = System.Windows.Forms.DockStyle.Right;
            this._settingsButton.Location = new System.Drawing.Point(143, 0);
            this._settingsButton.Name = "_settingsButton";
            this._settingsButton.Size = new System.Drawing.Size(15, 23);
            this._settingsButton.TabIndex = 1;
            this._settingsButton.Text = "i";
            // 
            // _discriptionLabel
            // 
            this._discriptionLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this._discriptionLabel.Location = new System.Drawing.Point(0, 0);
            this._discriptionLabel.Name = "_discriptionLabel";
            this._discriptionLabel.Size = new System.Drawing.Size(31, 23);
            this._discriptionLabel.Text = "____";
            // 
            // _hotKeyLabel
            // 
            this._hotKeyLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._hotKeyLabel.Font = new System.Drawing.Font("Tahoma", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this._hotKeyLabel.Location = new System.Drawing.Point(31, 0);
            this._hotKeyLabel.Name = "_hotKeyLabel";
            this._hotKeyLabel.Size = new System.Drawing.Size(58, 23);
            this._hotKeyLabel.Text = "*";
            this._hotKeyLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // _tallyButton
            // 
            this._tallyButton.Dock = System.Windows.Forms.DockStyle.Right;
            this._tallyButton.Location = new System.Drawing.Point(89, 0);
            this._tallyButton.Name = "_tallyButton";
            this._tallyButton.Size = new System.Drawing.Size(54, 23);
            this._tallyButton.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular);
            // 
            // TallyRow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._hotKeyLabel);
            this.Controls.Add(this._tallyButton);
            this.Controls.Add(this._discriptionLabel);
            this.Controls.Add(this._settingsButton);
            this.Name = "TallyRow";
            this.Size = new System.Drawing.Size(158, 23);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _settingsButton;
        private System.Windows.Forms.Label _discriptionLabel;
        public System.Windows.Forms.Label _hotKeyLabel;
        private FMSC.Controls.Mobile.ButtonPanel _tallyButton;
    }
}
