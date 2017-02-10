using FSCruiser.Core.Models;
namespace FSCruiser.WinForms.Common
{
    partial class FixCNTForm
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
            this._tallyControl = new FSCruiser.WinForms.Common.FixCNTTallyControl();
            this.SuspendLayout();
            // 
            // _tallyControl
            // 
            this._tallyControl.AutoScroll = true;
            this._tallyControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tallyControl.Location = new System.Drawing.Point(0, 0);
            this._tallyControl.Name = "_tallyControl";
            this._tallyControl.Size = new System.Drawing.Size(338, 281);
            this._tallyControl.TabIndex = 0;
            // 
            // FixCNTForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(338, 317);
            this.Controls.Add(this._tallyControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FixCNTForm";
            this.ResumeLayout(false);

        }

        #endregion

        private FixCNTTallyControl _tallyControl;
        private System.Windows.Forms.Panel panel1;
    }
}