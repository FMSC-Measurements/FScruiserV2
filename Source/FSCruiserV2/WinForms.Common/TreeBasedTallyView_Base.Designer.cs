namespace FSCruiser.WinForms
{
    partial class TreeBasedTallyView_Base
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
            this.components = new System.ComponentModel.Container();
            this._BS_tallyHistory = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this._BS_tallyHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // _BS_tallyHistory
            // 
            this._BS_tallyHistory.DataSource = typeof(FSCruiser.Core.Models.TallyAction);
            // 
            // TreeBasedTallyView_Base
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Name = "TreeBasedTallyView_Base";
            ((System.ComponentModel.ISupportInitialize)(this._BS_tallyHistory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.BindingSource _BS_tallyHistory;

    }
}
