namespace FSCruiserV2.Forms
{
    partial class FormTally
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.treeDOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._leftContentPanel = new System.Windows.Forms.Panel();
            this._ringBufferListBox = new System.Windows.Forms.ListBox();
            this._untallyButton = new System.Windows.Forms.Button();
            this._rightContentPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.treeDOBindingSource)).BeginInit();
            this._rightContentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeDOBindingSource
            // 
            this.treeDOBindingSource.DataSource = typeof(CruiseDAL.DataObjects.TreeDO);
            // 
            // _leftContentPanel
            // 
            this._leftContentPanel.AutoScroll = true;
            this._leftContentPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this._leftContentPanel.Location = new System.Drawing.Point(0, 0);
            this._leftContentPanel.Name = "_leftContentPanel";
            this._leftContentPanel.Size = new System.Drawing.Size(127, 268);
            // 
            // _ringBufferListBox
            // 
            this._ringBufferListBox.DataSource = this.treeDOBindingSource;
            this._ringBufferListBox.DisplayMember = "Species";
            this._ringBufferListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ringBufferListBox.Location = new System.Drawing.Point(0, 0);
            this._ringBufferListBox.Name = "_ringBufferListBox";
            this._ringBufferListBox.Size = new System.Drawing.Size(113, 268);
            this._ringBufferListBox.TabIndex = 3;
            // 
            // _untallyButton
            // 
            this._untallyButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._untallyButton.Location = new System.Drawing.Point(0, 248);
            this._untallyButton.Name = "_untallyButton";
            this._untallyButton.Size = new System.Drawing.Size(113, 20);
            this._untallyButton.TabIndex = 4;
            this._untallyButton.Text = "Untally";
            // 
            // _rightContentPanel
            // 
            this._rightContentPanel.Controls.Add(this._untallyButton);
            this._rightContentPanel.Controls.Add(this._ringBufferListBox);
            this._rightContentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rightContentPanel.Location = new System.Drawing.Point(127, 0);
            this._rightContentPanel.Name = "_rightContentPanel";
            this._rightContentPanel.Size = new System.Drawing.Size(113, 268);
            // 
            // FormTally
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this._rightContentPanel);
            this.Controls.Add(this._leftContentPanel);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "FormTally";
            this.Text = "Tally";
            ((System.ComponentModel.ISupportInitialize)(this.treeDOBindingSource)).EndInit();
            this._rightContentPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource treeDOBindingSource;
        private System.Windows.Forms.Panel _leftContentPanel;
        private System.Windows.Forms.ListBox _ringBufferListBox;
        private System.Windows.Forms.Button _untallyButton;
        private System.Windows.Forms.Panel _rightContentPanel;

    }
}