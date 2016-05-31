using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FSCruiser.Core;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class LayoutPlot : UserControl, ITallyView, ITreeView, IPlotLayout
    {
        private delegate void SplitterMovedEventHandler(object sender, int newPosition);

        private bool _viewLoading = true;

        private DataGridViewComboBoxColumn _initialsColoumn;
        private DataGridViewComboBoxColumn _sgColumn;
        private DataGridViewComboBoxColumn _speciesColumn;
        private DataGridViewTextBoxColumn _errorMessageColumn;
        private DataGridViewButtonColumn _logsColumn;
        private DataGridViewTextBoxColumn _treeNumberColumn;

        public IApplicationController AppController { get { return this.ViewLogicController.Controller; } }

        public FormDataEntryLogic DataEntryController { get { return this.ViewLogicController.DataEntryController; } }

        public LayoutPlotLogic ViewLogicController { get; set; }

        public bool ViewLoading { get { return _viewLoading; } }

        public PlotStratum Stratum { get; set; }

        public IList<TreeVM> Trees
        {
            get
            {
                var curPlot = this.ViewLogicController.CurrentPlot;
                if (curPlot != null)
                {
                    return curPlot.Trees;
                }
                return null;
            }
        }

        public LayoutPlot(FormDataEntryLogic dataEntryController, Control parent, PlotStratum stratum)
        {
            Stratum = stratum;
            this.ViewLogicController = new LayoutPlotLogic(stratum, this, dataEntryController, dataEntryController.ViewController);
            this.Dock = DockStyle.Fill;
            InitializeComponent();

            WireSplitter(stratum);

            this.bindingNavigatorAddNewItem.Click += new System.EventHandler(this._addPlotButton_Click);
            this.bindingNavigatorDeleteItem.Click += new System.EventHandler(this._deletePlotButton_Click);
            this._plotInfoBTN.Click += new System.EventHandler(this._plotInfoBTN_Click);

            //this._dataGrid.DataSource = _BS_Trees;

            this._dataGrid.SuspendLayout();

            this._dataGrid.CellClick += new DataGridViewCellEventHandler(_dataGrid_CellClick);
            this._dataGrid.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this._dataGrid_CellValidating);
            this._dataGrid.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this._dataGrid_DataError);
            this._dataGrid.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this._datagrid_CellEnter);

            this._dataGrid.AutoGenerateColumns = false;
            var columns = stratum.MakeTreeColumns();
            this._dataGrid.Columns.AddRange(columns.ToArray());
            this._dataGrid.ResumeLayout();

            _speciesColumn = _dataGrid.Columns["TreeDefaultValue"] as DataGridViewComboBoxColumn;
            _sgColumn = _dataGrid.Columns["SampleGroup"] as DataGridViewComboBoxColumn;
            _treeNumberColumn = _dataGrid.Columns["TreeNumber"] as DataGridViewTextBoxColumn;
            _initialsColoumn = _dataGrid.Columns["Initials"] as DataGridViewComboBoxColumn;
            _errorMessageColumn = _dataGrid.Columns["Error"] as DataGridViewTextBoxColumn;
            _logsColumn = _dataGrid.Columns["Logs"] as DataGridViewButtonColumn;

            if (_speciesColumn != null)
            {
                _speciesColumn.DataSource = AppController._cDal.From<TreeDefaultValueDO>().Read().ToList();
            }
            if (_sgColumn != null)
            {
                _sgColumn.DataSource = AppController._cDal.From<SampleGroupVM>().Read().ToList();
            }
            if (_initialsColoumn != null)
            {
                _initialsColoumn.DataSource = this.AppController.Settings.Cruisers.ToArray();
            }
            if (_logsColumn != null)
            {
                _logsColumn.Visible = AppController.ViewController.EnableLogGrading;
            }

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

            //no need to load tallies....?
            //Controller.PopulateTallies(this.StratumInfo, this._mode, Controller.CurrentUnit, this._tallyListPanel, this);
            this.Parent = parent;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var stratum = this.ViewLogicController.Stratum;

            this._tallyListPanel.SuspendLayout();
            this.ViewLogicController.DataEntryController.PopulateTallies(stratum, DataEntryController.Unit, this._tallyListPanel, this);
            if (stratum.Method == "3PPNT")
            {
                this.IsGridExpanded = true;
            }

            this._tallyListPanel.ResumeLayout();

            this.ViewLogicController.UpdateCurrentPlot();

            logToolStripMenuItem.Text = AppController.ViewController.EnableLogGrading ?
                "Disable Log Grading" : "Enable Log Grading";
        }

        #region splitter

        static event SplitterMovedEventHandler SplitterMoved;

        void WireSplitter(PlotStratum stratum)
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

        void HandleSplitterMoved(object sender, SplitterEventArgs e)
        {
            if (this.ViewLoading
                || suppressSplitterEvents) { return; }
            if (LayoutPlot.SplitterMoved != null)
            {
                LayoutPlot.SplitterMoved(this, splitContainer1.SplitterDistance);
            }
        }

        bool suppressSplitterEvents;

        void LayoutPlot_SplitterMoved(object sender, int newPosition)
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

        void _dataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_logsColumn != null && e.ColumnIndex == _logsColumn.Index)
            {
                TreeVM curTree = this.Trees[e.RowIndex] as TreeVM;
                if (curTree != null)
                {
                    this.DataEntryController.ShowLogs(curTree);
                }
            }
        }

        private void _dataGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //?do nothing
        }

        private void _dataGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var cell = _dataGrid[e.ColumnIndex, e.RowIndex];
            if (cell == null) { return; }
            if (cell.FormattedValue == e.FormattedValue) { return; }//are there any changes

            TreeVM curTree = null;
            object cellValue = null;
            try
            {
                curTree = this.ViewLogicController.CurrentTree;
                if (curTree == null) { return; }

                cellValue = cell.ParseFormattedValue(e.FormattedValue, cell.InheritedStyle, null, null);
            }
            catch
            {
                e.Cancel = true;
                return;
            }

            if (_treeNumberColumn != null && e.ColumnIndex == _treeNumberColumn.Index)
            {
                var newTreeNum = (long)cellValue;
                if (curTree.TreeNumber != newTreeNum
                    && !this.ViewLogicController.CurrentPlot.IsTreeNumberAvalible(newTreeNum))
                {
                    MessageBox.Show("Tree Number already exists");
                    e.Cancel = true;
                }
            }
            else if (_sgColumn != null && cell.ColumnIndex == _sgColumn.Index)
            {
                SampleGroupVM sg = cellValue as SampleGroupVM;
                if (curTree.HandleSampleGroupChanging(sg, this))
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
            else if (_speciesColumn != null && cell.ColumnIndex == _speciesColumn.Index)
            {
                TreeDefaultValueDO tdv = cellValue as TreeDefaultValueDO;
                e.Cancel = !ProcessSpeciesChanged(curTree, tdv);
            }
        }

        private void _datagrid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewComboBoxCell cell = _dataGrid[e.ColumnIndex, e.RowIndex] as DataGridViewComboBoxCell;
            if (cell == null) { return; }
            TreeVM curTree = this.Trees[e.RowIndex] as TreeVM;
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

        public void HandleCurrentTreeChanged(TreeVM tree)
        {/*Do nothing*/ }

        public void UpdateSampleGroupColumn(TreeVM tree)
        {
            this.UpdateSampleGroupColumn(tree, this._dataGrid.CurrentCell as DataGridViewComboBoxCell);
        }

        public void UpdateSpeciesColumn(TreeVM tree)
        {
            this.UpdateSpeciesColumn(tree, this._dataGrid.CurrentCell as DataGridViewComboBoxCell);
        }

        protected void UpdateSampleGroupColumn(TreeVM tree, DataGridViewComboBoxCell cell)
        {
            if (cell == null) { return; }
            cell.DataSource = tree.ReadValidSampleGroups();
        }

        protected void UpdateSpeciesColumn(TreeVM tree, DataGridViewComboBoxCell cell)
        {
            if (cell == null) { return; }
            cell.DataSource = tree.ReadValidTDVs();
        }

        //protected bool ProcessSampleGroupChanging(TreeVM tree, SampleGroupVM newSG)
        //{
        //    if (tree == null || newSG == null) { return false; }

        //    if (tree.SampleGroup != null)
        //    {
        //        if (MessageBox.Show("You are changing the Sample Group of a tree, are you sure you want to do this?", "!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2)
        //            == DialogResult.No)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            this.AppController._cDal.LogMessage(String.Format("Tree Sample Group Changed (Cu:{0} St:{1} Sg:{2} -> {3} Tdv_CN:{4} T#: {5}",
        //                tree.CuttingUnit.Code,
        //                tree.Stratum.Code,
        //                (tree.SampleGroup != null) ? tree.SampleGroup.Code : "?",
        //                newSG.Code,
        //                (tree.TreeDefaultValue != null) ? tree.TreeDefaultValue.TreeDefaultValue_CN.ToString() : "?",
        //                tree.TreeNumber), "high");
        //            tree.SampleGroup = newSG;
        //        }
        //    }
        //    else
        //    {
        //        tree.SampleGroup = newSG;
        //    }
        //    if (tree.TreeDefaultValue != null)
        //    {
        //        var hasTDV = tree.DAL.ExecuteScalar<bool>("SELECT count(1) " +
        //            "FROM SampleGroupTreeDefaultValue " +
        //            "WHERE TreeDefaultValue_CN = ? AND SampleGroup_CN"
        //            , tree.TreeDefaultValue_CN, newSG.SampleGroup_CN);

        //        if (!hasTDV)
        //        {
        //            tree.SetTreeTDV(null);
        //        }
        //    }
        //    return tree.TrySave();
        //}

        protected bool ProcessSpeciesChanged(TreeVM tree, TreeDefaultValueDO tdv)
        {
            if (tree == null) { return true; }
            if (tree.TreeDefaultValue == tdv) { return true; }
            tree.SetTreeTDV(tdv);
            return tree.TrySave();
        }

        #endregion DataGrid events

        private void _plotInfoBTN_Click(object sender, EventArgs e)
        {
            this.ViewLogicController.ShowCurrentPlotInfo();
            //this.AppController.ViewController.ShowPlotInfo(this.ViewLogicController.CurrentPlotInfo, false);
        }

        private void _addPlotButton_Click(object sender, EventArgs e)
        {
            this.ViewLogicController.HandleAddPlot();
        }

        private void _deletePlotButton_Click(object sender, EventArgs e)
        {
            this.ViewLogicController.HandleDeletePlot();
        }

        void openFixCNTTallyButton_Click(object sender, EventArgs e)
        {
            var stratum = Stratum as FixCNTStratum;
            var currentPlot = ViewLogicController.CurrentPlot as FixCNTPlot;
            if (currentPlot == null) { ShowNoPlotSelectedMessage(); return; }
            if (stratum == null || currentPlot == null) { return; }
            using (var view = new FSCruiser.WinForms.Common.FixCNTForm(stratum))
            {
                view.ShowDialog(currentPlot);
            }
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

        #endregion IPlotLayout members

        #region ITallyView Members

        //TODO implement ITallyView members
        public bool HotKeyEnabled
        {
            get
            {
                return !this._dataGrid.IsCurrentCellInEditMode;
            }
        }

        public Dictionary<char, CountTreeVM> HotKeyLookup
        {
            get
            {
                return this.ViewLogicController.Stratum.HotKeyLookup;
            }
        }

        public bool PreviewKeypress(KeyEventArgs ea)
        {
            if (_viewLoading) { return false; }
            switch (ea.KeyCode)
            {
                case Keys.Add:
                    {
                        this._addPlotButton_Click(null, null);
                        return true;
                    }
                case Keys.Escape:
                    {
                        //#warning not implemented
                        return false;
                        //IsGridExpanded = !IsGridExpanded;
                        //return true;
                    }
                default:
                    return false;
            }
        }

        public void MakeSGList(List<SampleGroupVM> list, Panel container)
        {
            foreach (SampleGroupVM sg in list.Reverse<SampleGroupVM>())
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

        void sgRow_SpeciesClicked(object sender, SubPop sp)
        {
            if (!this.ViewLogicController.EnsureCurrentPlotWorkable()) { return; }

            this.ViewLogicController.AddTree(sp.SG, sp.TDV);
        }

        public Control MakeTallyRow(Control container, CountTreeVM count)
        {
            TallyRow row = new TallyRow(count);
            row.SuspendLayout();

            row.TallyButtonClicked += new EventHandler(this.tallyRow_TallyButtonClicked);
            row.SettingsButtonClicked += new EventHandler(this.tallyRow_InfoButtonClicked);

            row.Height = 56;
            row.Parent = container;

            row.Dock = DockStyle.Top;
            row.ResumeLayout(true);
            return row;
        }

        public Control MakeTallyRow(Control container, SubPop subPop)
        {
            return null;
        }

        private void tallyRow_TallyButtonClicked(object sender, EventArgs e)
        {
            if (!this.ViewLogicController.EnsureCurrentPlotWorkable()) { return; }

            var row = (TallyRow)sender;
            var count = row.Count;
            OnTally(count);
        }

        private void tallyRow_InfoButtonClicked(object sender, EventArgs e)
        {
            TallyRow row = (TallyRow)sender;
            CountTreeVM count = row.Count;
            this.ViewLogicController.SavePlotTrees();
            this.ViewLogicController.ViewController.ShowTallySettings(count);
            //row.DiscriptionLabel.Text = count.Tally.Description;
        }

        public void OnTally(CountTreeVM count)
        {
            this.ViewLogicController.OnTally(count);
        }

        public void HandleStratumLoaded(Control container)
        {
            //do nothing
            return;
        }

        public void SaveCounts()
        {
            this.ViewLogicController.SaveCounts();
        }

        public bool TrySaveCounts()
        {
            if (!this.ViewLogicController.TrySaveCounts())
            {
                MessageBox.Show("Stratum:" + this.ViewLogicController.Stratum.Code
                    + " Unable to save Counts");
                return false;
            }
            return true;
        }

        #endregion ITallyView Members

        #region ITreeView Members

        public bool UserCanAddTrees
        {
            get { return this.ViewLogicController.UserCanAddTrees; }
            set { this.ViewLogicController.UserCanAddTrees = value; }
        }

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

        public void HandleLoad()
        {
            _viewLoading = false;
            this.ViewLogicController.HandleViewLoad();
        }

        public void HandleEnableLogGradingChanged()
        {
            if (this._logsColumn != null)
            {
                this._logsColumn.Visible = this.AppController.ViewController.EnableLogGrading;
            }
        }

        public void HandleCruisersChanged()
        {
            if (this._initialsColoumn != null)
            {
                this._initialsColoumn.DataSource = this.AppController.Settings.Cruisers.ToArray();
            }
        }

        public void DeleteSelectedTree()
        {
            this.ViewLogicController.HandleDeleteCurrentTree();
        }

        public void EndEdit()
        {
            this.ViewLogicController.EndEdit();
        }

        public void ViewEndEdit()
        {
            this._dataGrid.EndEdit();
        }

        public void MoveLastTree()
        {
            this.ViewLogicController.SelectLastTree();
        }

        public void MoveHomeField()
        {
            if (this._dataGrid.CurrentCellAddress.Y == -1) { return; }
            try
            {
                this._dataGrid.CurrentCell = this._dataGrid[0, this._dataGrid.CurrentCellAddress.Y];
            }
            catch
            { }
        }

        public TreeVM UserAddTree()
        {
            return this.ViewLogicController.UserAddTree();
        }

        #endregion ITreeView Members

        #region IPlotLayout Members

        public bool AskContinueOnCurrnetPlotTreeError()
        {
            return this.AskYesNo("Error(s) found on tree records in current plot, Would you like to continue?", "Continue?", MessageBoxIcon.Question, true);
        }

        public void ShowNoPlotSelectedMessage()
        {
            this.ShowMessage("No Plot Selected");
        }

        public void ShowNullPlotMessage()
        {
            this.ShowMessage("Can't perform action on null plot");
        }

        public void ShowLimitingDistanceDialog()
        {
            if (this.ViewLogicController.CurrentPlot == null)
            {
                ShowNoPlotSelectedMessage();
                return;
            }

            this.DataEntryController.ShowLimitingDistanceDialog(this.ViewLogicController.Stratum, this.ViewLogicController.CurrentPlot, null);
        }

        public void RefreshTreeView(PlotVM currentPlot)
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

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppController.ViewController.EnableLogGrading = !AppController.ViewController.EnableLogGrading;

            logToolStripMenuItem.Text = AppController.ViewController.EnableLogGrading ?
                "Disable Log Grading" : "Enable Log Grading";
        }

        private void _dataGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                logToolStripMenuItem.Text = AppController.ViewController.EnableLogGrading ?
                    "Disable Log Grading" : "Enable Log Grading";
                _contexMenu.Show(Cursor.Position);
            }
        }

        #region ITreeView Members

        #endregion ITreeView Members
    }

    //protected override void OnKeyUp(KeyEventArgs e)
    //{
    //    base.OnKeyUp(e);
    //    if (e.Handled == true) { return; }
    //    char key = (char)e.KeyValue;
    //    if (_viewLoading) { return; }
    //    e.Handled = this.ViewLogicController.DataEntryController.ProcessHotKey(key, this);
    //}
}