namespace FSCruiser.WinForms.DataEntry
{
    partial class FormLogs
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
            this._BS_Logs = new System.Windows.Forms.BindingSource(this.components);
            this._dataGrid = new FMSC.Controls.EditableDataGrid();
            this._treeDesLbl = new System.Windows.Forms.Label();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this._deleteMI = new System.Windows.Forms.MenuItem();
            this._addMI = new System.Windows.Forms.MenuItem();
            this._okBTN = new System.Windows.Forms.Button();
            this._ceControlPanel = new System.Windows.Forms.Panel();
            this._deleteBTN = new System.Windows.Forms.Button();
            this._addBTN = new System.Windows.Forms.Button();
            this._sipPlaceHolder = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this._BS_Logs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dataGrid)).BeginInit();
            this._ceControlPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _BS_Logs
            // 
            this._BS_Logs.DataSource = typeof(CruiseDAL.DataObjects.LogDO);
            this._BS_Logs.AddingNew += new System.ComponentModel.AddingNewEventHandler(this._BS_Logs_AddingNew);
            // 
            // _dataGrid
            // 
            //this._dataGrid.DataSource = this._BS_Logs;
            this._dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dataGrid.Location = new System.Drawing.Point(0, 30);
            this._dataGrid.Name = "_dataGrid";
            this._dataGrid.Size = new System.Drawing.Size(240, 240);
            this._dataGrid.TabIndex = 0;
            // 
            // _treeDesLbl
            // 
            this._treeDesLbl.Dock = System.Windows.Forms.DockStyle.Top;
            this._treeDesLbl.Location = new System.Drawing.Point(0, 0);
            this._treeDesLbl.Name = "_treeDesLbl";
            this._treeDesLbl.Size = new System.Drawing.Size(240, 30);
            this._treeDesLbl.Text = "<Tree Description PlaceHolder>\r\n<Tree Description Line 2>";
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this._deleteMI);
            this.mainMenu1.MenuItems.Add(this._addMI);
            // 
            // _deleteMI
            // 
            this._deleteMI.Text = "Delete";
            this._deleteMI.Click += new System.EventHandler(this._deleteBtn_Click);
            // 
            // _addMI
            // 
            this._addMI.Text = "Add";
            this._addMI.Click += new System.EventHandler(this._addBtn_Click);
            // 
            // _okBTN
            // 
            this._okBTN.Dock = System.Windows.Forms.DockStyle.Fill;
            this._okBTN.Location = new System.Drawing.Point(80, 0);
            this._okBTN.Name = "_okBTN";
            this._okBTN.Size = new System.Drawing.Size(80, 24);
            this._okBTN.TabIndex = 2;
            this._okBTN.Text = "Done";
            this._okBTN.Click += new System.EventHandler(this._doneBtn_Click);
            // 
            // _ceControlPanel
            // 
            this._ceControlPanel.Controls.Add(this._okBTN);
            this._ceControlPanel.Controls.Add(this._deleteBTN);
            this._ceControlPanel.Controls.Add(this._addBTN);
            this._ceControlPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._ceControlPanel.Location = new System.Drawing.Point(0, 270);
            this._ceControlPanel.Name = "_ceControlPanel";
            this._ceControlPanel.Size = new System.Drawing.Size(240, 24);
            this._ceControlPanel.Visible = false;
            // 
            // _deleteBTN
            // 
            this._deleteBTN.Dock = System.Windows.Forms.DockStyle.Left;
            this._deleteBTN.Location = new System.Drawing.Point(0, 0);
            this._deleteBTN.Name = "_deleteBTN";
            this._deleteBTN.Size = new System.Drawing.Size(80, 24);
            this._deleteBTN.TabIndex = 4;
            this._deleteBTN.Text = "Delete";
            this._deleteBTN.Click += new System.EventHandler(this._deleteBtn_Click);
            // 
            // _addBTN
            // 
            this._addBTN.Dock = System.Windows.Forms.DockStyle.Right;
            this._addBTN.Location = new System.Drawing.Point(160, 0);
            this._addBTN.Name = "_addBTN";
            this._addBTN.Size = new System.Drawing.Size(80, 24);
            this._addBTN.TabIndex = 3;
            this._addBTN.Text = "Add";
            this._addBTN.Click += new System.EventHandler(this._addBtn_Click);
            // 
            // _sipPlaceHolder
            // 
            this._sipPlaceHolder.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._sipPlaceHolder.Location = new System.Drawing.Point(0, 294);
            this._sipPlaceHolder.Name = "_sipPlaceHolder";
            this._sipPlaceHolder.Size = new System.Drawing.Size(240, 0);
            // 
            // FormLogs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this._dataGrid);
            this.Controls.Add(this._ceControlPanel);
            this.Controls.Add(this._treeDesLbl);
            this.Controls.Add(this._sipPlaceHolder);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "FormLogs";
            this.Text = "Logs";
            ((System.ComponentModel.ISupportInitialize)(this._BS_Logs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dataGrid)).EndInit();
            this._ceControlPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FMSC.Controls.EditableDataGrid _dataGrid;
        private System.Windows.Forms.BindingSource _BS_Logs;
        private System.Windows.Forms.Label _treeDesLbl;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem _deleteMI;
        private System.Windows.Forms.MenuItem _addMI;
        private System.Windows.Forms.Button _okBTN;
        private System.Windows.Forms.Panel _ceControlPanel;
        private System.Windows.Forms.Button _deleteBTN;
        private System.Windows.Forms.Button _addBTN;
        private System.Windows.Forms.Panel _sipPlaceHolder;
    }
}