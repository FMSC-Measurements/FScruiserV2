using System;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FMSC.Controls;
using FScruiser.Core.Services;
using FSCruiser.Core;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class ControlTreeDataGrid : FMSC.Controls.EditableDataGrid, ITreeView
    {
        //private bool _changingTree = false;
        private EditableComboBoxColumn _speciesColumn; //handle to the species column in the datagrid, if displayed otherwise null

        private EditableComboBoxColumn _sgColumn; //handle to the sample group column in the datagird, if displayed otherwise null
        private EditableComboBoxColumn _stratumColumn; //handle to the stratum column in the datagrid, if displayed otherwise null
        private EditableTextBoxColumn _treeNumberColumn;
        private EditableComboBoxColumn _initialsColoumn;
        private DataGridButtonColumn _logsColumn;
        private EditableTextBoxColumn _kpiColumn;
        private DataGridTextBoxColumn _errorsColumn;
        private System.Windows.Forms.BindingSource _BS_trees;

        public ControlTreeDataGrid()
        {
            UpdatePageText();

            DataGridAdjuster.InitializeGrid(this);

            this.AllowUserToAddRows = false;//don't allow down arrow to add tree
            //this.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold);

            //initialize _BS_trees
            this._BS_trees = new System.Windows.Forms.BindingSource();
            ((System.ComponentModel.ISupportInitialize)(this._BS_trees)).BeginInit();
            this._BS_trees.DataSource = typeof(Tree);
            this._BS_trees.CurrentChanged += new EventHandler(_BS_trees_CurrentChanged);
            this.DataSource = this._BS_trees;
            ((System.ComponentModel.ISupportInitialize)(this._BS_trees)).EndInit();
        }

        public ControlTreeDataGrid(IDataEntryDataService dataService
            , ApplicationSettings appSettings
            , FormDataEntryLogic dataEntryController)
            : this()
        {
            DataService = dataService;
            DataEntryController = dataEntryController;
            AppSettings = appSettings;

            DataGridTableStyle tableStyle = dataService.InitializeTreeColumns(this);

            _speciesColumn = tableStyle.GridColumnStyles["TreeDefaultValue"] as EditableComboBoxColumn;
            _sgColumn = tableStyle.GridColumnStyles["SampleGroup"] as EditableComboBoxColumn;
            _stratumColumn = tableStyle.GridColumnStyles["Stratum"] as EditableComboBoxColumn;
            _treeNumberColumn = tableStyle.GridColumnStyles["TreeNumber"] as EditableTextBoxColumn;
            _initialsColoumn = tableStyle.GridColumnStyles["Initials"] as EditableComboBoxColumn;
            _logsColumn = tableStyle.GridColumnStyles["LogCountActual"] as DataGridButtonColumn;
            _kpiColumn = tableStyle.GridColumnStyles["KPI"] as EditableTextBoxColumn;
            _errorsColumn = tableStyle.GridColumnStyles["Errors"] as DataGridTextBoxColumn;

            Settings_CruisersChanged(null, null);//initialize initials column

            if (_logsColumn != null)
            {
                _logsColumn.Click += this.LogsClicked;
                LogColumnVisable = DataService.EnableLogGrading;
            }
            if (_stratumColumn != null)
            {
                _stratumColumn.DataSource = dataService.TreeStrata;
            }
        }

        protected override void OnCellValidating(EditableDataGridCellValidatingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _BS_trees.Count) 
            { e.Cancel = true; return; }            

            base.OnCellValidating(e);
            Tree tree = (Tree)this._BS_trees[e.RowIndex];
            if (e.Column == _sgColumn)
            {
                var newSG = e.Value as SampleGroupDO;
                e.Cancel = !tree.HandleSampleGroupChanging(newSG);

                //this.DataEntryController.HandleSampleGroupChanging(curTree, e.Value as SampleGroupDO, out cancel);
            }
            else if (e.Column == _speciesColumn)
            {
                //no action required
                return;
            }
            else if (e.Column == _stratumColumn)
            {
                var newSt = e.Value as StratumDO;
                e.Cancel = !tree.HandleStratumChanging(newSt);

                //this.DataEntryController.HandleStratumChanging(tree, e.Value as StratumDO, out cancel);
                //this.HandleStratumChanging(currTree, e.Value as StratumDO, out cancel);
            }
            else if (e.Column == _treeNumberColumn)
            {
                try
                {
                    long newTreeNum = (long)e.Value;

                    if (tree.TreeNumber != newTreeNum
                    && !DataService.IsTreeNumberAvalible(newTreeNum))
                    {
                        MessageBox.Show("Tree Number already exists");
                        e.Cancel = true;
                    }
                }
                catch
                {
                    e.Cancel = true;
                }
            }
            //else if (e.Column == _kpiColumn)
            //{
            //    bool cancel = false;

            //    this.DataEntryController.HandleKPIChanging(tree, (float)e.Value, false, out cancel);
            //    //this.HandleKPIChanging(currTree, (float)e.Value, out cancel);
            //}
        }

        protected override void OnCellValueChanged(EditableDataGridCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _BS_trees.Count) { return; }

            base.OnCellValueChanged(e);

            Tree tree = _BS_trees[e.RowIndex] as Tree;
            if (tree == null) { return; }

            if (e.Column == _sgColumn)
            {
                tree.HandleSampleGroupChanged();
                this.UpdateSpeciesColumn(tree);

                //this.DataEntryController.HandleSampleGroupChanged(this, tree);
            }
            else if (e.Column == _speciesColumn)
            {
                EditableComboBoxColumn col = e.Column as EditableComboBoxColumn;
                if (col == null) { return; }

                tree.HandleSpeciesChanged(col.EditComboBox.SelectedItem as TreeDefaultValueDO);
            }
            else if (e.Column == _stratumColumn)
            {
                tree.HandleStratumChanged();
                this.UpdateSampleGroupColumn(tree);
                this.UpdateSpeciesColumn(tree);

                //this.DataEntryController.HandleStratumChanged(this, tree);
            }
        }

        void LogsClicked(ButtonCellClickEventArgs e)
        {
            Tree tree = this._BS_trees[e.RowNumber] as Tree;
            if (tree == null) { return; }

            ShowLogs(tree);
        }

        void _BS_trees_CurrentChanged(object sender, EventArgs e)
        {
            Tree tree = _BS_trees.Current as Tree;

            //_changingTree = true;
            UpdateSpeciesColumn(tree);
            UpdateSampleGroupColumn(tree);

            //_changingTree = false;
        }

        void UpdateSpeciesColumn(Tree tree)
        {
            if (_speciesColumn != null)
            {
                _speciesColumn.DataSource = tree.ReadValidTDVs();
            }
        }

        void UpdateSampleGroupColumn(Tree tree)
        {
            if (_sgColumn != null)
            {
                _sgColumn.DataSource = tree.ReadValidSampleGroups();
            }
        }

        #region ITreeView Members

        public bool ErrorColumnVisable
        {
            get
            {
                if (_errorsColumn != null)
                {
                    return _errorsColumn.Width > 0;
                }
                else { return false; }
            }
            set
            {
                if (_errorsColumn != null)
                {
                    _errorsColumn.Width = (value) ? Screen.PrimaryScreen.WorkingArea.Width : -1;
                }
            }
        }

        public bool LogColumnVisable
        {
            get
            {
                if (_logsColumn != null)
                {
                    return _logsColumn.Width > 0;
                }
                else { return false; }
            }
            set
            {
                if (_logsColumn != null)
                {
                    _logsColumn.Width = (value) ? Constants.LOG_COLUMN_WIDTH : -1;
                }
            }
        }

        #endregion ITreeView Members

        #region IDataEntryPage

        #endregion IDataEntryPage

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (this._BS_trees != null)
                {
                    this._BS_trees.Dispose();
                    this._BS_trees = null;
                }
                DataService = null;
                AppSettings = null;
            }
        }
    }
}