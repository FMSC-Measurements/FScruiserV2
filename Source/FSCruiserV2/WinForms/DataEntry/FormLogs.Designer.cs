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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogs));
            this._dataGrid = new FSCruiser.WinForms.Controls.CustomDataGridView();
            this._BS_Logs = new System.Windows.Forms.BindingSource(this.components);
            this._treeDesLbl = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.logCNDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.logGUIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.treeCNDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.logNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gradeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seenDefectDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.percentRecoverableDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lengthDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exportGradeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.smallEndDiameterDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.largeEndDiameterDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grossBoardFootDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.netBoardFootDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grossCubicFootDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.netCubicFootDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.boardFootRemovedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cubicFootRemovedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dIBClassDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.barkThicknessDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modifiedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.errorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dALDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.propertyChangedEventsDisabledDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isPersistedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isDeletedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.hasChangesDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isChangedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this._dataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._BS_Logs)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _dataGrid
            // 
            this._dataGrid.AllowUserToAddRows = false;
            this._dataGrid.AllowUserToDeleteRows = false;
            this._dataGrid.AutoGenerateColumns = false;
            this._dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._dataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.logCNDataGridViewTextBoxColumn,
            this.logGUIDDataGridViewTextBoxColumn,
            this.treeCNDataGridViewTextBoxColumn,
            this.logNumberDataGridViewTextBoxColumn,
            this.gradeDataGridViewTextBoxColumn,
            this.seenDefectDataGridViewTextBoxColumn,
            this.percentRecoverableDataGridViewTextBoxColumn,
            this.lengthDataGridViewTextBoxColumn,
            this.exportGradeDataGridViewTextBoxColumn,
            this.smallEndDiameterDataGridViewTextBoxColumn,
            this.largeEndDiameterDataGridViewTextBoxColumn,
            this.grossBoardFootDataGridViewTextBoxColumn,
            this.netBoardFootDataGridViewTextBoxColumn,
            this.grossCubicFootDataGridViewTextBoxColumn,
            this.netCubicFootDataGridViewTextBoxColumn,
            this.boardFootRemovedDataGridViewTextBoxColumn,
            this.cubicFootRemovedDataGridViewTextBoxColumn,
            this.dIBClassDataGridViewTextBoxColumn,
            this.barkThicknessDataGridViewTextBoxColumn,
            this.createdByDataGridViewTextBoxColumn,
            this.createdDateDataGridViewTextBoxColumn,
            this.modifiedByDataGridViewTextBoxColumn,
            this.errorDataGridViewTextBoxColumn,
            this.dALDataGridViewTextBoxColumn,
            this.propertyChangedEventsDisabledDataGridViewCheckBoxColumn,
            this.isPersistedDataGridViewCheckBoxColumn,
            this.isDeletedDataGridViewCheckBoxColumn,
            this.hasChangesDataGridViewCheckBoxColumn,
            this.isChangedDataGridViewCheckBoxColumn});
            this._dataGrid.DataSource = this._BS_Logs;
            this._dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dataGrid.Location = new System.Drawing.Point(0, 55);
            this._dataGrid.Name = "_dataGrid";
            this._dataGrid.Size = new System.Drawing.Size(428, 231);
            this._dataGrid.TabIndex = 2;
            this._dataGrid.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this._dataGrid_DataError);
            // 
            // _BS_Logs
            // 
            this._BS_Logs.DataSource = typeof(FSCruiser.Core.Models.Log);
            // 
            // _treeDesLbl
            // 
            this._treeDesLbl.Dock = System.Windows.Forms.DockStyle.Top;
            this._treeDesLbl.Location = new System.Drawing.Point(0, 0);
            this._treeDesLbl.Name = "_treeDesLbl";
            this._treeDesLbl.Size = new System.Drawing.Size(428, 30);
            this._treeDesLbl.TabIndex = 4;
            this._treeDesLbl.Text = "<Tree Description PlaceHolder>\r\n<Tree Description Line 2>";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 30);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(428, 25);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.RightToLeftAutoMirrorImage = true;
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Add new";
            this.toolStripButton1.Click += new System.EventHandler(this._BTN_add_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.RightToLeftAutoMirrorImage = true;
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "Delete";
            this.toolStripButton2.Click += new System.EventHandler(this._BTN_delete_Click);
            // 
            // logCNDataGridViewTextBoxColumn
            // 
            this.logCNDataGridViewTextBoxColumn.DataPropertyName = "Log_CN";
            this.logCNDataGridViewTextBoxColumn.HeaderText = "Log_CN";
            this.logCNDataGridViewTextBoxColumn.Name = "logCNDataGridViewTextBoxColumn";
            // 
            // logGUIDDataGridViewTextBoxColumn
            // 
            this.logGUIDDataGridViewTextBoxColumn.DataPropertyName = "Log_GUID";
            this.logGUIDDataGridViewTextBoxColumn.HeaderText = "Log_GUID";
            this.logGUIDDataGridViewTextBoxColumn.Name = "logGUIDDataGridViewTextBoxColumn";
            // 
            // treeCNDataGridViewTextBoxColumn
            // 
            this.treeCNDataGridViewTextBoxColumn.DataPropertyName = "Tree_CN";
            this.treeCNDataGridViewTextBoxColumn.HeaderText = "Tree_CN";
            this.treeCNDataGridViewTextBoxColumn.Name = "treeCNDataGridViewTextBoxColumn";
            // 
            // logNumberDataGridViewTextBoxColumn
            // 
            this.logNumberDataGridViewTextBoxColumn.DataPropertyName = "LogNumber";
            this.logNumberDataGridViewTextBoxColumn.HeaderText = "LogNumber";
            this.logNumberDataGridViewTextBoxColumn.Name = "logNumberDataGridViewTextBoxColumn";
            // 
            // gradeDataGridViewTextBoxColumn
            // 
            this.gradeDataGridViewTextBoxColumn.DataPropertyName = "Grade";
            this.gradeDataGridViewTextBoxColumn.HeaderText = "Grade";
            this.gradeDataGridViewTextBoxColumn.Name = "gradeDataGridViewTextBoxColumn";
            // 
            // seenDefectDataGridViewTextBoxColumn
            // 
            this.seenDefectDataGridViewTextBoxColumn.DataPropertyName = "SeenDefect";
            this.seenDefectDataGridViewTextBoxColumn.HeaderText = "SeenDefect";
            this.seenDefectDataGridViewTextBoxColumn.Name = "seenDefectDataGridViewTextBoxColumn";
            // 
            // percentRecoverableDataGridViewTextBoxColumn
            // 
            this.percentRecoverableDataGridViewTextBoxColumn.DataPropertyName = "PercentRecoverable";
            this.percentRecoverableDataGridViewTextBoxColumn.HeaderText = "PercentRecoverable";
            this.percentRecoverableDataGridViewTextBoxColumn.Name = "percentRecoverableDataGridViewTextBoxColumn";
            // 
            // lengthDataGridViewTextBoxColumn
            // 
            this.lengthDataGridViewTextBoxColumn.DataPropertyName = "Length";
            this.lengthDataGridViewTextBoxColumn.HeaderText = "Length";
            this.lengthDataGridViewTextBoxColumn.Name = "lengthDataGridViewTextBoxColumn";
            // 
            // exportGradeDataGridViewTextBoxColumn
            // 
            this.exportGradeDataGridViewTextBoxColumn.DataPropertyName = "ExportGrade";
            this.exportGradeDataGridViewTextBoxColumn.HeaderText = "ExportGrade";
            this.exportGradeDataGridViewTextBoxColumn.Name = "exportGradeDataGridViewTextBoxColumn";
            // 
            // smallEndDiameterDataGridViewTextBoxColumn
            // 
            this.smallEndDiameterDataGridViewTextBoxColumn.DataPropertyName = "SmallEndDiameter";
            this.smallEndDiameterDataGridViewTextBoxColumn.HeaderText = "SmallEndDiameter";
            this.smallEndDiameterDataGridViewTextBoxColumn.Name = "smallEndDiameterDataGridViewTextBoxColumn";
            // 
            // largeEndDiameterDataGridViewTextBoxColumn
            // 
            this.largeEndDiameterDataGridViewTextBoxColumn.DataPropertyName = "LargeEndDiameter";
            this.largeEndDiameterDataGridViewTextBoxColumn.HeaderText = "LargeEndDiameter";
            this.largeEndDiameterDataGridViewTextBoxColumn.Name = "largeEndDiameterDataGridViewTextBoxColumn";
            // 
            // grossBoardFootDataGridViewTextBoxColumn
            // 
            this.grossBoardFootDataGridViewTextBoxColumn.DataPropertyName = "GrossBoardFoot";
            this.grossBoardFootDataGridViewTextBoxColumn.HeaderText = "GrossBoardFoot";
            this.grossBoardFootDataGridViewTextBoxColumn.Name = "grossBoardFootDataGridViewTextBoxColumn";
            // 
            // netBoardFootDataGridViewTextBoxColumn
            // 
            this.netBoardFootDataGridViewTextBoxColumn.DataPropertyName = "NetBoardFoot";
            this.netBoardFootDataGridViewTextBoxColumn.HeaderText = "NetBoardFoot";
            this.netBoardFootDataGridViewTextBoxColumn.Name = "netBoardFootDataGridViewTextBoxColumn";
            // 
            // grossCubicFootDataGridViewTextBoxColumn
            // 
            this.grossCubicFootDataGridViewTextBoxColumn.DataPropertyName = "GrossCubicFoot";
            this.grossCubicFootDataGridViewTextBoxColumn.HeaderText = "GrossCubicFoot";
            this.grossCubicFootDataGridViewTextBoxColumn.Name = "grossCubicFootDataGridViewTextBoxColumn";
            // 
            // netCubicFootDataGridViewTextBoxColumn
            // 
            this.netCubicFootDataGridViewTextBoxColumn.DataPropertyName = "NetCubicFoot";
            this.netCubicFootDataGridViewTextBoxColumn.HeaderText = "NetCubicFoot";
            this.netCubicFootDataGridViewTextBoxColumn.Name = "netCubicFootDataGridViewTextBoxColumn";
            // 
            // boardFootRemovedDataGridViewTextBoxColumn
            // 
            this.boardFootRemovedDataGridViewTextBoxColumn.DataPropertyName = "BoardFootRemoved";
            this.boardFootRemovedDataGridViewTextBoxColumn.HeaderText = "BoardFootRemoved";
            this.boardFootRemovedDataGridViewTextBoxColumn.Name = "boardFootRemovedDataGridViewTextBoxColumn";
            // 
            // cubicFootRemovedDataGridViewTextBoxColumn
            // 
            this.cubicFootRemovedDataGridViewTextBoxColumn.DataPropertyName = "CubicFootRemoved";
            this.cubicFootRemovedDataGridViewTextBoxColumn.HeaderText = "CubicFootRemoved";
            this.cubicFootRemovedDataGridViewTextBoxColumn.Name = "cubicFootRemovedDataGridViewTextBoxColumn";
            // 
            // dIBClassDataGridViewTextBoxColumn
            // 
            this.dIBClassDataGridViewTextBoxColumn.DataPropertyName = "DIBClass";
            this.dIBClassDataGridViewTextBoxColumn.HeaderText = "DIBClass";
            this.dIBClassDataGridViewTextBoxColumn.Name = "dIBClassDataGridViewTextBoxColumn";
            // 
            // barkThicknessDataGridViewTextBoxColumn
            // 
            this.barkThicknessDataGridViewTextBoxColumn.DataPropertyName = "BarkThickness";
            this.barkThicknessDataGridViewTextBoxColumn.HeaderText = "BarkThickness";
            this.barkThicknessDataGridViewTextBoxColumn.Name = "barkThicknessDataGridViewTextBoxColumn";
            // 
            // createdByDataGridViewTextBoxColumn
            // 
            this.createdByDataGridViewTextBoxColumn.DataPropertyName = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn.HeaderText = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn.Name = "createdByDataGridViewTextBoxColumn";
            // 
            // createdDateDataGridViewTextBoxColumn
            // 
            this.createdDateDataGridViewTextBoxColumn.DataPropertyName = "CreatedDate";
            this.createdDateDataGridViewTextBoxColumn.HeaderText = "CreatedDate";
            this.createdDateDataGridViewTextBoxColumn.Name = "createdDateDataGridViewTextBoxColumn";
            // 
            // modifiedByDataGridViewTextBoxColumn
            // 
            this.modifiedByDataGridViewTextBoxColumn.DataPropertyName = "ModifiedBy";
            this.modifiedByDataGridViewTextBoxColumn.HeaderText = "ModifiedBy";
            this.modifiedByDataGridViewTextBoxColumn.Name = "modifiedByDataGridViewTextBoxColumn";
            // 
            // errorDataGridViewTextBoxColumn
            // 
            this.errorDataGridViewTextBoxColumn.DataPropertyName = "Error";
            this.errorDataGridViewTextBoxColumn.HeaderText = "Error";
            this.errorDataGridViewTextBoxColumn.Name = "errorDataGridViewTextBoxColumn";
            this.errorDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // dALDataGridViewTextBoxColumn
            // 
            this.dALDataGridViewTextBoxColumn.DataPropertyName = "DAL";
            this.dALDataGridViewTextBoxColumn.HeaderText = "DAL";
            this.dALDataGridViewTextBoxColumn.Name = "dALDataGridViewTextBoxColumn";
            // 
            // propertyChangedEventsDisabledDataGridViewCheckBoxColumn
            // 
            this.propertyChangedEventsDisabledDataGridViewCheckBoxColumn.DataPropertyName = "PropertyChangedEventsDisabled";
            this.propertyChangedEventsDisabledDataGridViewCheckBoxColumn.HeaderText = "PropertyChangedEventsDisabled";
            this.propertyChangedEventsDisabledDataGridViewCheckBoxColumn.Name = "propertyChangedEventsDisabledDataGridViewCheckBoxColumn";
            this.propertyChangedEventsDisabledDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // isPersistedDataGridViewCheckBoxColumn
            // 
            this.isPersistedDataGridViewCheckBoxColumn.DataPropertyName = "IsPersisted";
            this.isPersistedDataGridViewCheckBoxColumn.HeaderText = "IsPersisted";
            this.isPersistedDataGridViewCheckBoxColumn.Name = "isPersistedDataGridViewCheckBoxColumn";
            this.isPersistedDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // isDeletedDataGridViewCheckBoxColumn
            // 
            this.isDeletedDataGridViewCheckBoxColumn.DataPropertyName = "IsDeleted";
            this.isDeletedDataGridViewCheckBoxColumn.HeaderText = "IsDeleted";
            this.isDeletedDataGridViewCheckBoxColumn.Name = "isDeletedDataGridViewCheckBoxColumn";
            this.isDeletedDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // hasChangesDataGridViewCheckBoxColumn
            // 
            this.hasChangesDataGridViewCheckBoxColumn.DataPropertyName = "HasChanges";
            this.hasChangesDataGridViewCheckBoxColumn.HeaderText = "HasChanges";
            this.hasChangesDataGridViewCheckBoxColumn.Name = "hasChangesDataGridViewCheckBoxColumn";
            this.hasChangesDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // isChangedDataGridViewCheckBoxColumn
            // 
            this.isChangedDataGridViewCheckBoxColumn.DataPropertyName = "IsChanged";
            this.isChangedDataGridViewCheckBoxColumn.HeaderText = "IsChanged";
            this.isChangedDataGridViewCheckBoxColumn.Name = "isChangedDataGridViewCheckBoxColumn";
            this.isChangedDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // FormLogs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 286);
            this.Controls.Add(this._dataGrid);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this._treeDesLbl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FormLogs";
            this.Text = "Logs";
            ((System.ComponentModel.ISupportInitialize)(this._dataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._BS_Logs)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.CustomDataGridView _dataGrid;
        private System.Windows.Forms.BindingSource _BS_Logs;
        private System.Windows.Forms.Label _treeDesLbl;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.DataGridViewTextBoxColumn logCNDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn logGUIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn treeCNDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn logNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn gradeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn seenDefectDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn percentRecoverableDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lengthDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn exportGradeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn smallEndDiameterDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn largeEndDiameterDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn grossBoardFootDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn netBoardFootDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn grossCubicFootDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn netCubicFootDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn boardFootRemovedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cubicFootRemovedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dIBClassDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn barkThicknessDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modifiedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn errorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dALDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn propertyChangedEventsDisabledDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isPersistedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isDeletedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn hasChangesDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isChangedDataGridViewCheckBoxColumn;
    }
}