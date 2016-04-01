namespace FSCruiser.WinForms.DataEntry
{
    partial class LayoutPlot
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
            LayoutPlot.SplitterMoved -= LayoutPlot_SplitterMoved;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayoutPlot));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this._tallyListPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this._dataGrid = new System.Windows.Forms.DataGridView();
            this._contexMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.logToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._bindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this._plotSelect_CB = new System.Windows.Forms.ToolStripComboBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this._plotInfoBTN = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dataGrid)).BeginInit();
            this._contexMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._bindingNavigator)).BeginInit();
            this._bindingNavigator.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this._tallyListPanel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(484, 283);
            this.splitContainer1.SplitterDistance = 149;
            this.splitContainer1.TabIndex = 3;
            // 
            // _tallyListPanel
            // 
            this._tallyListPanel.AutoScroll = true;
            this._tallyListPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tallyListPanel.Location = new System.Drawing.Point(0, 0);
            this._tallyListPanel.Margin = new System.Windows.Forms.Padding(0);
            this._tallyListPanel.Name = "_tallyListPanel";
            this._tallyListPanel.Size = new System.Drawing.Size(149, 283);
            this._tallyListPanel.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._dataGrid);
            this.panel1.Controls.Add(this._bindingNavigator);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(331, 283);
            this.panel1.TabIndex = 1;
            // 
            // _dataGrid
            // 
            this._dataGrid.AllowUserToAddRows = false;
            this._dataGrid.AllowUserToDeleteRows = false;
            this._dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dataGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this._dataGrid.Location = new System.Drawing.Point(0, 25);
            this._dataGrid.Margin = new System.Windows.Forms.Padding(0);
            this._dataGrid.Name = "_dataGrid";
            this._dataGrid.Size = new System.Drawing.Size(331, 258);
            this._dataGrid.TabIndex = 0;
            this._dataGrid.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this._dataGrid_ColumnHeaderMouseClick);
            // 
            // _contexMenu
            // 
            this._contexMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logToolStripMenuItem});
            this._contexMenu.Name = "_contexMenu";
            this._contexMenu.Size = new System.Drawing.Size(181, 26);
            // 
            // logToolStripMenuItem
            // 
            this.logToolStripMenuItem.Name = "logToolStripMenuItem";
            this.logToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.logToolStripMenuItem.Text = "Disable Log Grading";
            this.logToolStripMenuItem.Click += logToolStripMenuItem_Click;
            // 
            // _bindingNavigator
            // 
            this._bindingNavigator.AddNewItem = null;
            this._bindingNavigator.CountItem = null;
            this._bindingNavigator.DeleteItem = null;
            this._bindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this._plotSelect_CB,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem,
            this._plotInfoBTN});
            this._bindingNavigator.Location = new System.Drawing.Point(0, 0);
            this._bindingNavigator.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this._bindingNavigator.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this._bindingNavigator.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this._bindingNavigator.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this._bindingNavigator.Name = "_bindingNavigator";
            this._bindingNavigator.PositionItem = null;
            this._bindingNavigator.Size = new System.Drawing.Size(331, 25);
            this._bindingNavigator.TabIndex = 1;
            this._bindingNavigator.Text = "bindingNavigator1";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // _plotSelect_CB
            // 
            this._plotSelect_CB.AccessibleName = "Position";
            this._plotSelect_CB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._plotSelect_CB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._plotSelect_CB.Name = "_plotSelect_CB";
            this._plotSelect_CB.Size = new System.Drawing.Size(75, 25);
            this._plotSelect_CB.ToolTipText = "Current Plot";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorAddNewItem.Text = "Add new";
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorDeleteItem.Text = "Delete";
            // 
            // _plotInfoBTN
            // 
            this._plotInfoBTN.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._plotInfoBTN.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._plotInfoBTN.Name = "_plotInfoBTN";
            this._plotInfoBTN.Size = new System.Drawing.Size(35, 22);
            this._plotInfoBTN.Text = " Info";
            // 
            // LayoutPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.splitContainer1);
            this.Name = "LayoutPlot";
            this.Size = new System.Drawing.Size(484, 283);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dataGrid)).EndInit();
            this._contexMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._bindingNavigator)).EndInit();
            this._bindingNavigator.ResumeLayout(false);
            this._bindingNavigator.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel _tallyListPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView _dataGrid;
        private System.Windows.Forms.BindingNavigator _bindingNavigator;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripComboBox _plotSelect_CB;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton _plotInfoBTN;
        private System.Windows.Forms.ContextMenuStrip _contexMenu;
        private System.Windows.Forms.ToolStripMenuItem logToolStripMenuItem;

        //private System.Windows.Forms.BindingSource _BS_Trees;
        //private System.Windows.Forms.BindingSource _BS_Plots;
    }
}
