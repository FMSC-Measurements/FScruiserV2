namespace FSCruiser.WinForms.DataEntry
{
    partial class FormDataEntry
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDataEntry));
            this.panel1 = new System.Windows.Forms.Panel();
            this._deleteTreeBTN = new System.Windows.Forms.Button();
            this._addTreeBTN = new System.Windows.Forms.Button();
            this._mainContenPanel = new System.Windows.Forms.Panel();
            this._pageContainer = new System.Windows.Forms.TabControl();
            this.panel1.SuspendLayout();
            this._mainContenPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._deleteTreeBTN);
            this.panel1.Controls.Add(this._addTreeBTN);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 432);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(752, 34);
            this.panel1.TabIndex = 0;
            // 
            // _deleteTreeBTN
            // 
            this._deleteTreeBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._deleteTreeBTN.Location = new System.Drawing.Point(570, 8);
            this._deleteTreeBTN.Name = "_deleteTreeBTN";
            this._deleteTreeBTN.Size = new System.Drawing.Size(75, 23);
            this._deleteTreeBTN.TabIndex = 1;
            this._deleteTreeBTN.Text = "Delete Tree";
            this._deleteTreeBTN.UseVisualStyleBackColor = true;
            this._deleteTreeBTN.Click += new System.EventHandler(this._deleteTreeBTN_Click);
            // 
            // _addTreeBTN
            // 
            this._addTreeBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._addTreeBTN.Location = new System.Drawing.Point(651, 8);
            this._addTreeBTN.Name = "_addTreeBTN";
            this._addTreeBTN.Size = new System.Drawing.Size(89, 23);
            this._addTreeBTN.TabIndex = 0;
            this._addTreeBTN.Text = "Add Tree (F3)";
            this._addTreeBTN.UseVisualStyleBackColor = true;
            this._addTreeBTN.Click += new System.EventHandler(this._addTreeMI_Click);
            // 
            // _mainContenPanel
            // 
            this._mainContenPanel.BackColor = System.Drawing.SystemColors.Control;
            this._mainContenPanel.Controls.Add(this._pageContainer);
            this._mainContenPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._mainContenPanel.Location = new System.Drawing.Point(0, 0);
            this._mainContenPanel.Name = "_mainContenPanel";
            this._mainContenPanel.Size = new System.Drawing.Size(752, 432);
            this._mainContenPanel.TabIndex = 1;
            // 
            // _pageContainer
            // 
            this._pageContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pageContainer.Location = new System.Drawing.Point(0, 0);
            this._pageContainer.Name = "_pageContainer";
            this._pageContainer.SelectedIndex = 0;
            this._pageContainer.Size = new System.Drawing.Size(752, 432);
            this._pageContainer.TabIndex = 0;
            // 
            // FormDataEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 466);
            this.Controls.Add(this._mainContenPanel);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDataEntry";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormDataEntry";
            this.panel1.ResumeLayout(false);
            this._mainContenPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        System.Windows.Forms.Button _addTreeBTN;
        System.Windows.Forms.Button _deleteTreeBTN;
        System.Windows.Forms.TabControl _pageContainer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel _mainContenPanel;
    }
}