﻿using CruiseDAL.DataObjects;
using FScruiser.Core.Services;
using FScruiser.Services;
using FSCruiser.Core;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class LayoutPlot : UserControl, ITallyView, ITreeView, IPlotLayout
    {
        private delegate void SplitterMovedEventHandler(object sender, int newPosition);

        private DataGridViewComboBoxColumn _initialsColoumn;
        private DataGridViewComboBoxColumn _stColumn;
        private DataGridViewComboBoxColumn _sgColumn;
        private DataGridViewComboBoxColumn _speciesColumn;
        private DataGridViewTextBoxColumn _errorMessageColumn;
        private DataGridViewButtonColumn _logsColumn;
        private DataGridViewTextBoxColumn _treeNumberColumn;

        public LayoutPlot()
        {
            this.Dock = DockStyle.Fill;
            InitializeComponent();

            this.bindingNavigatorAddNewItem.Click += new System.EventHandler(this._addPlotButton_Click);
            this.bindingNavigatorDeleteItem.Click += new System.EventHandler(this._deletePlotButton_Click);
            this._plotInfoBTN.Click += new System.EventHandler(this._plotInfoBTN_Click);

            this._dataGrid.CellClick += new DataGridViewCellEventHandler(_dataGrid_CellClick);
            this._dataGrid.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this._dataGrid_CellValidating);
            this._dataGrid.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this._dataGrid_DataError);
            this._dataGrid.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this._datagrid_CellEnter);

            this._dataGrid.AutoGenerateColumns = false;
        }

        public LayoutPlot(IPlotDataService dataService,
            ISampleSelectorRepository sampleSelectorRepository,
            ApplicationSettings appSettings,
            ISoundService soundService,
            IViewController viewController,
            PlotStratum stratum) : this()
        {
            SampleSelectorRepository = sampleSelectorRepository;
            //Stratum = stratum;
            DataService = dataService;
            AppSettings = appSettings;
            this.ViewLogicController = new LayoutPlotLogic(stratum,
                this,
                dataService,
                soundService,
                DialogService.Instance,
                AppSettings,
                viewController,
                sampleSelectorRepository);

            WireSplitter(stratum);

            InitializeDataGrid(stratum);

            //no need to load tallies....?
            //Controller.PopulateTallies(this.StratumInfo, this._mode, Controller.CurrentUnit, this._tallyListPanel, this);
        }

        private void InitializeDataGrid(PlotStratum stratum)
        {
            this._dataGrid.SuspendLayout();

            var fontWidth = (int)Math.Ceiling(CreateGraphics().MeasureString("_", Font).Width);

            var columns = stratum.MakeTreeColumns(fontWidth);
            this._dataGrid.Columns.AddRange(columns.ToArray());
            this._dataGrid.ResumeLayout();

            _speciesColumn = _dataGrid.Columns["TreeDefaultValue"] as DataGridViewComboBoxColumn;
            _sgColumn = _dataGrid.Columns["SampleGroup"] as DataGridViewComboBoxColumn;
            _treeNumberColumn = _dataGrid.Columns["TreeNumber"] as DataGridViewTextBoxColumn;
            _initialsColoumn = _dataGrid.Columns["Initials"] as DataGridViewComboBoxColumn;
            _errorMessageColumn = _dataGrid.Columns["Error"] as DataGridViewTextBoxColumn;
            _logsColumn = _dataGrid.Columns["Logs"] as DataGridViewButtonColumn;
            _stColumn = _dataGrid.Columns["Stratum"] as DataGridViewComboBoxColumn;

            if (_speciesColumn != null)
            {
                _speciesColumn.DataSource = DataService.GetTreeDefaultValuesAll().ToList();
            }
            if (_sgColumn != null)
            {
                _sgColumn.DataSource = stratum.SampleGroups;
            }

            Settings_CruisersChanged(null, null);//initialize initials column

            if (_logsColumn != null)
            {
                _logsColumn.Visible = DataService.EnableLogGrading;
            }
            if (_stColumn != null)
            {
                _stColumn.DataSource = new PlotStratum[] { this.Stratum };
            }
        }

        private void InitializeTallyPanel(PlotStratum stratum)
        {
            _tallyListPanel.SuspendLayout();

            if (stratum is FixCNTStratum)
            {
                var openFixCNTTallyButton = new Button()
                {
                    Text = "Open Tally Screen"
                    ,
                    Dock = DockStyle.Top
                };
                openFixCNTTallyButton.Click += new EventHandler(openFixCNTTallyButton_Click);
                this._tallyListPanel.Controls.Add(openFixCNTTallyButton);
            }
            else if (stratum.IsSingleStage)
            {
                if (stratum.Method == "3PPNT")
                {
                    //no need to initialize any counts or samplegroup info for 3PPNT
                    IsGridExpanded = false;
                }
                else if (stratum.Counts.Count() > 0)
                {
                    foreach (var count in stratum.Counts)
                    {
                        MakeTallyRow(_tallyListPanel, count);
                    }
                }
                else
                {
                    MakeSGList(stratum.SampleGroups, _tallyListPanel);
                }
            }
            else
            {
                foreach (var count in stratum.Counts)
                {
                    MakeTallyRow(_tallyListPanel, count);
                }
            }

            this._tallyListPanel.ResumeLayout();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var stratum = this.ViewLogicController.Stratum;

            InitializeTallyPanel(stratum);

            this.ViewLogicController.UpdateCurrentPlot();

            logToolStripMenuItem.Text = DataService.EnableLogGrading ?
                "Disable Log Grading" : "Enable Log Grading";
        }

        #region splitter

        private static event SplitterMovedEventHandler SplitterMoved;

        private bool suppressSplitterEvents;

        private void WireSplitter(PlotStratum stratum)
        {
            if (stratum.Is3PPNT == false)
            {
                LayoutPlot.SplitterMoved += new SplitterMovedEventHandler(LayoutPlot_SplitterMoved);
                this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.HandleSplitterMoved);
            }
            else
            {
                this.splitContainer1.Panel1Collapsed = true;
            }
        }

        private void HandleSplitterMoved(object sender, SplitterEventArgs e)
        {
            if (this.ViewLoading
                || suppressSplitterEvents) { return; }
            if (LayoutPlot.SplitterMoved != null)
            {
                LayoutPlot.SplitterMoved(this, splitContainer1.SplitterDistance);
            }
        }

        private void LayoutPlot_SplitterMoved(object sender, int newPosition)
        {
            if (sender == this) { return; }
            else
            {
                suppressSplitterEvents = true;
                try { this.splitContainer1.SplitterDistance = newPosition; }
                finally { suppressSplitterEvents = false; }
            }
        }

        #endregion splitter

        #region Event Handlers

        #region DataGrid events

        private void _dataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= Trees.Count) { return; }

            if (_logsColumn != null && e.ColumnIndex == _logsColumn.Index)
            {
                var curTree = this.Trees.ElementAt(e.RowIndex) as Tree;
                ShowLogs(curTree);
            }
        }

        private void _dataGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void _dataGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //check bounds
            if (e.RowIndex > _dataGrid.RowCount - 1 || e.RowIndex < 0) { return; }
            if (e.ColumnIndex > _dataGrid.ColumnCount - 1 || e.ColumnIndex < 0) { return; }

            var row = _dataGrid.Rows[e.RowIndex];

            var cell = row.Cells[e.ColumnIndex];
            if (cell == null) { return; }
            if (cell.ReadOnly) { return; }//we don't need to validate readonly columns
            if (cell.FormattedValue == e.FormattedValue) { return; }//no changes

            var curTree = _dataGrid.Rows[e.RowIndex].DataBoundItem as Tree;
            if (curTree == null) { return; }

            object cellValue = null;
            try
            {
                cellValue = cell.ParseFormattedValue(e.FormattedValue, cell.InheritedStyle, null, null);
            }
            catch
            {
                e.Cancel = true;
                return;
            }

            if (_treeNumberColumn != null && e.ColumnIndex == _treeNumberColumn.Index)
            {
                e.Cancel = !ValicateTreeNumber(curTree, cellValue);
                return;
            }
            else if (_sgColumn != null && cell.ColumnIndex == _sgColumn.Index)
            {
                e.Cancel = !ValidateSampleGroup(curTree, cellValue);
                return;
            }
            else if (_speciesColumn != null && cell.ColumnIndex == _speciesColumn.Index)
            {
                TreeDefaultValueDO tdv = cellValue as TreeDefaultValueDO;
                e.Cancel = !ProcessSpeciesChanged(curTree, tdv);
            }
        }

        protected bool ProcessSpeciesChanged(Tree tree, TreeDefaultValueDO tdv)
        {
            if (tree == null) { return true; }
            if (tree.TreeDefaultValue == tdv) { return true; }
            tree.SetTreeTDV(tdv);
            return tree.TrySave();
        }

        private static bool ValidateSampleGroup(Tree curTree, object cellValue)
        {
            var sg = cellValue as SampleGroup;
            if (curTree.HandleSampleGroupChanging(sg))
            {
                curTree.SampleGroup = sg;
                curTree.HandleSampleGroupChanged();
                return true;
            }
            else { return false; }
        }

        private bool ValicateTreeNumber(Tree curTree, object cellValue)
        {
            if (cellValue == null || !(cellValue is long))
            { return false; }//if cell value is blank cellValue is an object

            var newTreeNum = (long)cellValue;
            var currPlot = ViewLogicController.CurrentPlot;
            if (curTree.TreeNumber != newTreeNum)
            {
                if (!currPlot.IsTreeNumberAvalible(newTreeNum))
                {
                    MessageBox.Show("Tree Number already exists");
                    return false;
                }
                else if (!ViewLogicController.DataService.CrossStrataIsTreeNumberAvalible(currPlot, newTreeNum))
                {
                    MessageBox.Show("Tree Number already exist, in a different stratum");
                    return false;
                }
                else { return true; }
            }
            else { return true; }
        }

        private void _datagrid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= Trees.Count) { return; }

            DataGridViewComboBoxCell cell = _dataGrid[e.ColumnIndex, e.RowIndex] as DataGridViewComboBoxCell;
            if (cell == null) { return; }
            var curTree = this.Trees.ElementAt(e.RowIndex) as Tree;
            if (curTree == null) { return; }

            if (_sgColumn != null && e.ColumnIndex == _sgColumn.Index)
            {
                this.UpdateSampleGroupColumn(curTree, cell);
            }

            if (_speciesColumn != null && e.ColumnIndex == _speciesColumn.Index)
            {
                this.UpdateSpeciesColumn(curTree, cell);
            }
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

        private void _dataGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                logToolStripMenuItem.Text = DataService.EnableLogGrading ?
                    "Disable Log Grading" : "Enable Log Grading";
                _contexMenu.Show(Cursor.Position);
            }
        }

        #endregion DataGrid events

        private void _plotInfoBTN_Click(object sender, EventArgs e)
        {
            ShowCurrentPlotInfo();
        }

        private void _addPlotButton_Click(object sender, EventArgs e)
        {
            this.ViewLogicController.HandleAddPlot();
        }

        private void _deletePlotButton_Click(object sender, EventArgs e)
        {
            this.ViewLogicController.HandleDeletePlot();
        }

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataService.EnableLogGrading = !DataService.EnableLogGrading;

            logToolStripMenuItem.Text = DataService.EnableLogGrading ?
                "Disable Log Grading" : "Enable Log Grading";
        }

        #endregion Event Handlers

        #region IPlotLayout members

        private bool _isGridExpanded = false;

        public bool IsGridExpanded
        {
            get
            {
                return _isGridExpanded;
            }
            set
            {
                if (value == _isGridExpanded) { return; }
                if (value == true)
                {
                    _tallyListPanel.Visible = false;
                    //_dataGrid.ReadOnly = false;

                    //_expandGridButton.ImageIndex = 3;
                }
                else
                {
                    _tallyListPanel.Visible = true;
                    //_dataGrid.ReadOnly = true;

                    //_expandGridButton.ImageIndex = 0;
                }
                _isGridExpanded = value;
            }
        }

        public void HandleCurrentTreeChanged(Tree tree)
        {/*Do nothing*/ }

        #endregion IPlotLayout members

        #region ITallyView Members

        public bool HotKeyEnabled
        {
            get
            {
                return false; //&& !Stratum.IsSingleStage;
            }
        }

        public void MakeSGList(IEnumerable<SampleGroup> sampleGroups, Panel container)
        {
            foreach (var sg in sampleGroups.Reverse())
            {
                var sgRow = new SampleGroupRow()
                {
                    Text = "SG: " + sg.Code,
                    Dock = DockStyle.Top
                };
                sgRow.SpeciesClicked += new SampleGroupRow.SpeciesClickedEventHandler(sgRow_SpeciesClicked);

                sg.TreeDefaultValues.Populate();
                var subPops = (from TreeDefaultValueDO tdv in sg.TreeDefaultValues
                               select new SubPop(sg, tdv));

                sgRow.AddSupPops(subPops);

                container.Controls.Add(sgRow);
            }
        }

        public Control MakeTallyRow(Control container, CountTree count)
        {
            var row = new PlotTallyRow(count);
            row.SuspendLayout();

            row.TallyButtonClicked += new EventHandler(this.tallyRow_TallyButtonClicked);
            row.SettingsButtonClicked += new EventHandler(this.tallyRow_InfoButtonClicked);

            row.Height = 56;
            row.Parent = container;

            row.Dock = DockStyle.Top;
            row.ResumeLayout(true);
            return row;
        }

        #endregion ITallyView Members

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

        public void ViewEndEdit()
        {
            this._dataGrid.EndEdit();
        }

        public void MoveHomeField()
        {
            _dataGrid.MoveFirstEmptyCell();
        }

        #endregion ITreeView Members

        #region IPlotLayout Members

        public void RefreshTreeView(Plot currentPlot)
        {
            if (currentPlot != null)
            {
                this._dataGrid.Enabled = true;
                //this._dataGrid.Focus();
            }
            else //no plot is selected
            {
                this._dataGrid.Enabled = false;     //disable data grid
            }
        }

        public void BindPlotData(BindingSource plotBS)
        {
            this._bindingNavigator.BindingSource = plotBS;
            this._plotSelect_CB.ComboBox.DisplayMember = "Self";
            this._plotSelect_CB.ComboBox.DataSource = plotBS;
        }

        public void BindTreeData(BindingSource treeBS)
        {
            this._dataGrid.DataSource = treeBS;
        }

        #endregion IPlotLayout Members

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                DataService = null;
                AppSettings = null;
            }
            LayoutPlot.SplitterMoved -= LayoutPlot_SplitterMoved;

            base.Dispose(disposing);
        }
    }
}