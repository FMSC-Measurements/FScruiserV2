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
            this.SuspendLayout();
            // 
            // FixCNTForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.fixCntTallyControl1);
            this.Name = "FixCNTForm";
            this.ResumeLayout(false);

        }

        #endregion

        private FixCntTallyControl fixCntTallyControl1;

    }
}