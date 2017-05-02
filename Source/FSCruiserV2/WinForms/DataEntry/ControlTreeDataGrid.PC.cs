using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FSCruiser.Core;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;
using FScruiser.Core.Services;
using FSCruiser.WinForms.Controls;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class ControlTreeDataGrid : CustomDataGridView, ITreeView
    {
        BindingSource _BS_trees;

        DataGridViewComboBoxColumn _speciesColumn;
        DataGridViewComboBoxColumn _sgColumn;
        DataGridViewComboBoxColumn _stratumColumn;
        DataGridViewTextBoxColumn _treeNumberColumn;
        DataGridViewButtonColumn _logsColumn;
        DataGridViewTextBoxColumn _errorMessageColumn;
        DataGridViewComboBoxColumn _initialsColoumn;

        ContextMenuStrip _contexMenu;
        ToolStripMenuItem _logToolStripMenuItem;

        //#region Properties

        //public FormDataEntryLogic DataEntryController { get; set; }

        //public bool ViewLoading { get { return _viewLoading; } }

        //public ICollection<Tree> Trees
        //{
        //    get
        //    {
        //        return DataService.NonPlotTrees;
        //    }
        //}

        //#region DataService

        //IDataEntryDataService _dataService;

        //IDataEntryDataService DataService
        //{
        //    get { return _dataService; }
        //    set
        //    {
        //        OnDataServiceChanging();
        //        _dataService = value;
        //        OnDataServiceChanged();
        //    }
        //}

        //void OnDataServiceChanged()
        //{
        //    if (_dataService != null)
        //    {
        //        _dataService.EnableLogGradingChanged += HandleEnableLogGradingChanged;
        //    }
        //}

        //void OnDataServiceChanging()
        //{
        //    if (_dataService != null)
        //    {
        //        _dataService.EnableLogGradingChanged -= HandleEnableLogGradingChanged;
        //    }
        //}

        //void HandleEnableLogGradingChanged(object sender, EventArgs e)
        //{
        //    if (_logsColumn != null)
        //    {
        //        var logGradingEnabled = DataService.EnableLogGrading;
        //        _logsColumn.Visible = logGradingEnabled;

        //        _logToolStripMenuItem.Text = logGradingEnabled ?
        //        "Disable Log Grading" : "Enable Log Grading";
        //    }
        //}

        //#endregion DataService

        //#region AppSettings

        //private ApplicationSettings _appSettings;

        //public ApplicationSettings AppSettings
        //{
        //    get { return _appSettings; }
        //    set
        //    {
        //        OnAppSettingsChanging();
        //        _appSettings = value;
        //        OnAppSettingsChanged();
        //    }
        //}

        //private void OnAppSettingsChanging()
        //{
        //    if (_appSettings != null)
        //    {
        //        _appSettings.CruisersChanged -= Settings_CruisersChanged;
        //    }
        //}

        //private void OnAppSettingsChanged()
        //{
        //    if (_appSettings != null)
        //    {
        //        _appSettings.CruisersChanged += Settings_CruisersChanged;
        //    }
        //}

        //#endregion AppSettings

        //#endregion Properties

        ControlTreeDataGrid()
        {
            UpdatePageText();

            EditMode = DataGridViewEditMode.EditOnEnter;
            AutoGenerateColumns = false;
            AllowUserToDeleteRows = false;
            AllowUserToAddRows = false;

            CellClick += new DataGridViewCellEventHandler(ControlTreeDataGrid_CellClick);

            _BS_trees = new BindingSource();
            ((System.ComponentModel.ISupportInitialize)_BS_trees).BeginInit();
            _BS_trees.DataSource = typeof(Tree);
            DataSource = _BS_trees;
            ((System.ComponentModel.ISupportInitialize)_BS_trees).EndInit();

            ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(ControlTreeDataGrid_ColumnHeaderMouseClick);

            _logToolStripMenuItem = new ToolStripMenuItem();
            _logToolStripMenuItem.Name = "logToolStripMenuItem";
            _logToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            _logToolStripMenuItem.Click += logToolStripMenuItem_Click;

            _contexMenu = new ContextMenuStrip(new System.ComponentModel.Container());
            _contexMenu.SuspendLayout();
            _contexMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _logToolStripMenuItem });
            _contexMenu.Name = "_contexMenu";
            _contexMenu.Size = new System.Drawing.Size(181, 26);

            _contexMenu.ResumeLayout(false);
        }

        public ControlTreeDataGrid(IDataEntryDataService dataService
            , ApplicationSettings appSettings
            , FormDataEntryLogic dataEntryController) : this()
        {
            DataService = dataService;
            DataEntryController = dataEntryController;
            AppSettings = appSettings;

            var fontWidth = (int)Math.Ceiling(CreateGraphics().MeasureString("_", Font).Width);

            var columns = DataService.MakeTreeColumns(fontWidth);
            base.Columns.AddRange(columns.ToArray());

            _speciesColumn = base.Columns["TreeDefaultValue"] as DataGridViewComboBoxColumn;
            _sgColumn = base.Columns["SampleGroup"] as DataGridViewComboBoxColumn;
            _stratumColumn = base.Columns["Stratum"] as DataGridViewComboBoxColumn;
            _treeNumberColumn = base.Columns["TreeNumber"] as DataGridViewTextBoxColumn;
            _initialsColoumn = base.Columns["Initials"] as DataGridViewComboBoxColumn;
            _errorMessageColumn = base.Columns["Error"] as DataGridViewTextBoxColumn;
            _logsColumn = base.Columns["Logs"] as DataGridViewButtonColumn;

            if (_speciesColumn != null)
            {
                _speciesColumn.DataSource = DataService.GetTreeDefaultValuesAll().ToList();
            }
            if (_sgColumn != null)
            {
                _sgColumn.DataSource = DataService.TreeStrata.SelectMany(st => st.SampleGroups).ToList();
            }
            if (_stratumColumn != null)
            {
                _stratumColumn.DataSource = DataService.TreeStrata;
            }
            if (_initialsColoumn != null)
            {
                _initialsColoumn.DataSource = AppSettings.Cruisers.ToArray();
            }
            if (_logsColumn != null)
            {
                _logsColumn.Visible = DataService.EnableLogGrading;
            }
        }

        #region event handlers

        void ControlTreeDataGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (_logToolStripMenuItem == null || _contexMenu == null) { return; }

                _logToolStripMenuItem.Text = DataService.EnableLogGrading ?
                    "Disable Log Grading" : "Enable Log Grading";

                _contexMenu.Show(Cursor.Position);
            }
        }

        void ControlTreeDataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex > Trees.Count) { return; }

            if (_logsColumn != null && e.ColumnIndex == _logsColumn.Index)
            {
                var curTree = Trees.ElementAt(e.RowIndex) as Tree;
                if (curTree != null)
                {
                    ShowLogs(curTree);
                }
            }
        }

        #endregion event handlers

        #region overrides

        protected override void OnDataError(bool displayErrorDialogIfNoHandler, DataGridViewDataErrorEventArgs e)
        {
            displayErrorDialogIfNoHandler = false;
            base.OnDataError(displayErrorDialogIfNoHandler, e);
        }

        protected override void OnCellEnter(DataGridViewCellEventArgs e)
        {
            base.OnCellEnter(e);

            DataGridViewComboBoxCell cell = base[e.ColumnIndex, e.RowIndex] as DataGridViewComboBoxCell;
            if (cell == null) { return; }
            var curTree = _BS_trees[e.RowIndex] as Tree;
            if (curTree == null) { return; }

            if (_sgColumn != null && e.ColumnIndex == _sgColumn.Index)
            {
                UpdateSampleGroupColumn(curTree, cell);
            }

            if (_speciesColumn != null && e.ColumnIndex == _speciesColumn.Index)
            {
                UpdateSpeciesColumn(curTree, cell);
            }
        }

        protected override void OnCellValidating(DataGridViewCellValidatingEventArgs e)
        {
            base.OnCellValidating(e);

            if (e.RowIndex > base.RowCount - 1 || e.RowIndex < 0) { return; }
            if (e.ColumnIndex > base.ColumnCount - 1 || e.ColumnIndex < 0) { return; }

            var cell = base[e.ColumnIndex, e.RowIndex];
            if (cell == null) { return; }
            if (cell.FormattedValue == e.FormattedValue) { return; }//are there any changes?

            Tree curTree = null;
            try
            {
                curTree = _BS_trees[e.RowIndex] as Tree;
                if (curTree == null) { return; }
            }
            catch (ArgumentOutOfRangeException) { return; }//ignore possible out of bound exceptions

            object cellValue = e.FormattedValue;
            cellValue = cell.ParseFormattedValue(cellValue, cell.InheritedStyle, null, null);

            if (_treeNumberColumn != null && e.ColumnIndex == _treeNumberColumn.Index)
            {
                var newTreeNum = (long)cellValue;
                if (curTree.TreeNumber != newTreeNum
                    && !DataService.IsTreeNumberAvalible(newTreeNum))
                {
                    MessageBox.Show("Tree Number already exists");
                    e.Cancel = true;
                }
            }
            else if (_sgColumn != null && e.ColumnIndex == _sgColumn.Index)
            {
                var sg = cellValue as SampleGroup;
                if (curTree.HandleSampleGroupChanging(sg))
                {
                    curTree.SampleGroup = sg;
                    curTree.HandleSampleGroupChanged();
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else if (_speciesColumn != null && e.ColumnIndex == _speciesColumn.Index)
            {
                TreeDefaultValueDO tdv = cellValue as TreeDefaultValueDO;
                e.Cancel = !curTree.HandleSpeciesChanged(tdv);
            }
            else if (_stratumColumn != null && e.ColumnIndex == _stratumColumn.Index)
            {
                var newSt = cellValue as Stratum;
                if (curTree.HandleStratumChanging(newSt))
                {
                    curTree.Stratum = newSt;
                    curTree.HandleStratumChanged();
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        #endregion overrides

        public void UpdateSampleGroupColumn(Tree tree)
        {
            UpdateSampleGroupColumn(tree, CurrentCell as DataGridViewComboBoxCell);
        }

        public void UpdateSpeciesColumn(Tree tree)
        {
            UpdateSpeciesColumn(tree, CurrentCell as DataGridViewComboBoxCell);
        }

        protected void UpdateSampleGroupColumn(Tree tree, DataGridViewComboBoxCell cell)
        {
            if (cell == null) { return; }
            cell.DataSource = tree.ReadValidSampleGroups();
        }

        protected void UpdateSpeciesColumn(Tree tree, DataGridViewComboBoxCell cell)
        {
            if (cell == null) { return; }
            cell.DataSource = tree.ReadValidTDVs();
        }

        #region ITreeView Members

        public bool ErrorColumnVisable
        {
            get
            {
                if (_errorMessageColumn != null)
                {
                    return _errorMessageColumn.Visible;
                }
                else { return false; }
            }
            set
            {
                if (_errorMessageColumn != null)
                {
                    _errorMessageColumn.Visible = value;
                }
            }
        }

        public bool LogColumnVisable
        {
            get
            {
                if (_logsColumn != null)
                {
                    return _logsColumn.Visible;
                }
                else { return false; }
            }
            set
            {
                if (_logsColumn != null)
                {
                    _logsColumn.Visible = value;
                }
            }
        }

        #endregion ITreeView Members

        void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataService.EnableLogGrading = !DataService.EnableLogGrading;

            _logToolStripMenuItem.Text = DataService.EnableLogGrading ?
                "Disable Log Grading" : "Enable Log Grading";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_BS_trees != null)
                {
                    _BS_trees.Dispose();
                    _BS_trees = null;
                }

                AppSettings = null;
                DataService = null;
            }
            base.Dispose(disposing);
        }
    }
}