namespace FSCruiser.WinForms.DataEntry
{
    partial class SampleGroupRow
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
            this._expand_BTN = new System.Windows.Forms.Button();
            this._speciesContainer = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // _expand_BTN
            // 
            this._expand_BTN.BackColor = System.Drawing.Color.SkyBlue;
            this._expand_BTN.Dock = System.Windows.Forms.DockStyle.Top;
            this._expand_BTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._expand_BTN.Location = new System.Drawing.Point(0, 0);
            this._expand_BTN.Name = "_expand_BTN";
            this._expand_BTN.Size = new System.Drawing.Size(50, 30);
            this._expand_BTN.TabIndex = 0;
            this._expand_BTN.Text = "<SG_Code>";
            this._expand_BTN.UseVisualStyleBackColor = false;
            // 
            // _speciesContainer
            // 
            this._speciesContainer.AutoSize = true;
            this._speciesContainer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._speciesContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._speciesContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._speciesContainer.Location = new System.Drawing.Point(0, 30);
            this._speciesContainer.Name = "_speciesContainer";
            this._speciesContainer.Size = new System.Drawing.Size(50, 2);
            this._speciesContainer.TabIndex = 1;
            // 
            // SampleGroupRow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this._speciesContainer);
            this.Controls.Add(this._expand_BTN);
            this.MinimumSize = new System.Drawing.Size(50, 30);
            this.Name = "SampleGroupRow";
            this.Size = new System.Drawing.Size(50, 32);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _expand_BTN;
        private System.Windows.Forms.Panel _speciesContainer;
    }
}
