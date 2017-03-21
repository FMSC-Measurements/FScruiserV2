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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDataEntry));
            this._deleteTreeBTN = new System.Windows.Forms.Button();
            this._addTreeBTN = new System.Windows.Forms.Button();
            this._mainContenPanel = new System.Windows.Forms.Panel();
            this._pageContainer = new System.Windows.Forms.TabControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._limitingDistanceMI = new System.Windows.Forms.ToolStripMenuItem();
            this._manageCruisersMI = new System.Windows.Forms.ToolStripMenuItem();
            this._settingsMI = new System.Windows.Forms.ToolStripMenuItem();
            this._mainContenPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _deleteTreeBTN
            // 
            this._deleteTreeBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._deleteTreeBTN.AutoSize = true;
            this._deleteTreeBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._deleteTreeBTN.Location = new System.Drawing.Point(676, 429);
            this._deleteTreeBTN.Name = "_deleteTreeBTN";
            this._deleteTreeBTN.Size = new System.Drawing.Size(73, 23);
            this._deleteTreeBTN.TabIndex = 1;
            this._deleteTreeBTN.Text = "Delete Tree";
            this._deleteTreeBTN.UseVisualStyleBackColor = true;
            this._deleteTreeBTN.Click += new System.EventHandler(this._deleteTreeBTN_Click);
            // 
            // _addTreeBTN
            // 
            this._addTreeBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._addTreeBTN.AutoSize = true;
            this._addTreeBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._addTreeBTN.Location = new System.Drawing.Point(609, 429);
            this._addTreeBTN.Name = "_addTreeBTN";
            this._addTreeBTN.Size = new System.Drawing.Size(61, 23);
            this._addTreeBTN.TabIndex = 0;
            this._addTreeBTN.Text = "Add Tree";
            this._addTreeBTN.UseVisualStyleBackColor = true;
            this._addTreeBTN.Click += new System.EventHandler(this._addTreeMI_Click);
            // 
            // _mainContenPanel
            // 
            this._mainContenPanel.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.SetColumnSpan(this._mainContenPanel, 4);
            this._mainContenPanel.Controls.Add(this._pageContainer);
            this._mainContenPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._mainContenPanel.Location = new System.Drawing.Point(3, 3);
            this._mainContenPanel.Name = "_mainContenPanel";
            this._mainContenPanel.Size = new System.Drawing.Size(746, 420);
            this._mainContenPanel.TabIndex = 1;
            // 
            // _pageContainer
            // 
            this._pageContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pageContainer.Location = new System.Drawing.Point(0, 0);
            this._pageContainer.Name = "_pageContainer";
            this._pageContainer.SelectedIndex = 0;
            this._pageContainer.Size = new System.Drawing.Size(746, 420);
            this._pageContainer.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this._deleteTreeBTN, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this._mainContenPanel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._addTreeBTN, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.button1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(752, 466);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 429);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(44, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Menu";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._limitingDistanceMI,
            this._manageCruisersMI,
            this._settingsMI});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(167, 70);
            // 
            // _limitingDistanceMI
            // 
            this._limitingDistanceMI.Name = "_limitingDistanceMI";
            this._limitingDistanceMI.Size = new System.Drawing.Size(166, 22);
            this._limitingDistanceMI.Text = "Limiting Distance";
            this._limitingDistanceMI.Click += new System.EventHandler(this.LimitingDistance_Click);
            // 
            // _manageCruisersMI
            // 
            this._manageCruisersMI.Name = "_manageCruisersMI";
            this._manageCruisersMI.Size = new System.Drawing.Size(166, 22);
            this._manageCruisersMI.Text = "Manage Cruisers";
            this._manageCruisersMI.Click += new System.EventHandler(this._editCruisersMI_Click);
            // 
            // _settingsMI
            // 
            this._settingsMI.Name = "_settingsMI";
            this._settingsMI.Size = new System.Drawing.Size(166, 22);
            this._settingsMI.Text = "Settings";
            this._settingsMI.Click += new System.EventHandler(this._settingsMI_Click);
            // 
            // FormDataEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 466);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDataEntry";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormDataEntry";
            this._mainContenPanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        System.Windows.Forms.Button _addTreeBTN;
        System.Windows.Forms.Button _deleteTreeBTN;
        System.Windows.Forms.TabControl _pageContainer;
        private System.Windows.Forms.Panel _mainContenPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem _limitingDistanceMI;
        private System.Windows.Forms.ToolStripMenuItem _manageCruisersMI;
        private System.Windows.Forms.ToolStripMenuItem _settingsMI;
    }
}