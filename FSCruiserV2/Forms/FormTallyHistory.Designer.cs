namespace FSCruiserV2.Forms
{
    partial class FormTallyHistory
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
            this.components = new System.ComponentModel.Container();
            this._BS_TallyActions = new System.Windows.Forms.BindingSource(this.components);
            this._tallyActionLB = new System.Windows.Forms.ListBox();
            this._untallyButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._BS_TallyActions)).BeginInit();
            this.SuspendLayout();
            // 
            // _BS_TallyActions
            // 
            this._BS_TallyActions.DataSource = typeof(FSCruiserV2.Logic.TallyAction);
            this._BS_TallyActions.Position = 0;
            // 
            // _tallyActionLB
            // 
            this._tallyActionLB.DataSource = this._BS_TallyActions;
            this._tallyActionLB.Dock = System.Windows.Forms.DockStyle.Top;
            this._tallyActionLB.Location = new System.Drawing.Point(0, 0);
            this._tallyActionLB.Name = "_tallyActionLB";
            this._tallyActionLB.Size = new System.Drawing.Size(240, 254);
            this._tallyActionLB.TabIndex = 0;
            // 
            // _untallyButton
            // 
            this._untallyButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this._untallyButton.Location = new System.Drawing.Point(0, 254);
            this._untallyButton.Name = "_untallyButton";
            this._untallyButton.Size = new System.Drawing.Size(240, 40);
            this._untallyButton.TabIndex = 1;
            this._untallyButton.Text = "Untally";
            this._untallyButton.Click += new System.EventHandler(this._untallyButton_Click);
            // 
            // FormTallyHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this._untallyButton);
            this.Controls.Add(this._tallyActionLB);
            this.Name = "FormTallyHistory";
            this.Text = "FormTallyHistory";
            ((System.ComponentModel.ISupportInitialize)(this._BS_TallyActions)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox _tallyActionLB;
        private System.Windows.Forms.BindingSource _BS_TallyActions;
        private System.Windows.Forms.Button _untallyButton;
    }
}