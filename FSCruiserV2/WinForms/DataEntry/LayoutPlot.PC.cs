using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CruiseDAL;
using CruiseDAL.DataObjects;
using FSCruiser.Core.ViewInterfaces;
using FSCruiser.Core;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;
using FMSC.ORM.Core.SQL;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class LayoutPlot : UserControl , ITallyView , ITreeView, IPlotLayout
    {
        private bool _viewLoading = true;

        private DataEntryMode _mode;

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

        public IList<TreeVM> Trees
        {
            get
            {
                return this.ViewLogicController.CurrentPlotTreeList;
            }
        }


        public LayoutPlot(FormDataEntryLogic dataEntryController, Control parent, PlotStratum stratum)
        {
            this.ViewLogicController = new LayoutPlotLogic(stratum, this, dataEntryController, dataEntryController.ViewController);
            this.Dock = DockStyle.Fill;            
            InitializeComponent();

            this._dataGrid.CellClick += new DataGridViewCellEventHandler(_dataGrid_CellClick);

            this._dataGrid.AutoGenerateColumns = false;
            //this._dataGrid.DataSource = _BS_Trees;
            DataGridViewColumn[] columns = DataGridAdjuster.MakeTreeColumns(this.AppController._cDal, null, stratum, this.AppController.ViewController.EnableLogGrading);
            this._dataGrid.SuspendLayout();
            this._dataGrid.Columns.AddRange(columns);
            this._dataGrid.ResumeLayout();

            _speciesColumn = _dataGrid.Columns["Species"] as DataGridViewComboBoxColumn;
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


            //no need to load tallies....?
            //Controller.PopulateTallies(this.StratumInfo, this._mode, Controller.CurrentUnit, this._tallyListPanel, this);
            this.Parent = parent;

            
        }

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
        #endregion

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


        public bool PreviewKeypress(string key)
        {
            if (_viewLoading) { return false; }
            switch (key)
            {
                case "Add":
                    {
                        this._addPlotButton_Click(null, null);
                        return true;
                    }
                case "Escape":
                    {
                        #warning not implemented
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
            try
            {
                throw new NotImplementedException();
            }
            catch (NotImplementedException)
            { }
        }

        public Control MakeTallyRow(Control container, CountTreeVM count)
        {
            TallyRow row = new TallyRow();
            row.SuspendLayout();

            row.DiscriptionLabel.Text = count.Tally.Description;
            row.TallyButton.Click += new EventHandler(this.HandleTallyButtonClick);
            row.SettingsButton.Click += new EventHandler(this.HandleSettingsButtonClick);
            if (count.Tally.Hotkey != null && count.Tally.Hotkey.Length > 0)
            {
                row.HotKeyLabel.Text = count.Tally.Hotkey.Substring(0, 1);
            }

            row.TallyButton.DataBindings.Add(new Binding("Text", count, "TreeCount"));

            row.Tag = count;
            row.Parent = container;


            row.Dock = DockStyle.Top;
            row.ResumeLayout(true);
            return row;
            
        }

        public Control MakeTallyRow(Control container, SubPop subPop)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (NotImplementedException)
            { }
            return null;
        }

        private void HandleTallyButtonClick(object sender, EventArgs e)
        {
            Control button = (Control)sender;
            TallyRow row = (TallyRow)button.Parent.Parent;
            CountTreeVM count = (CountTreeVM)row.Tag;
            OnTally(count);

        }

        private void HandleSettingsButtonClick(object sender, EventArgs e)
        {
            Button settingsbutton = (Button)sender;
            TallyRow row = (TallyRow)settingsbutton.Parent.Parent;
            CountTreeVM count = (CountTreeVM)row.Tag;
            this.ViewLogicController.ViewController.ShowTallySettings(count);
            row.DiscriptionLabel.Text = count.Tally.Description;
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

        #endregion

        #region ITreeView Members

        public bool UserCanAddTrees
        {
            get { return this.ViewLogicController.UserCanAddTrees; }
            set { this.ViewLogicController.UserCanAddTrees = value; }
        }

        public void HandleLoad()
        {
            _viewLoading = false;
            this.ViewLogicController.HandleViewLoad();
            
        }

        public void ShowHideErrorCol()
        {
            if (this._errorMessageColumn != null)
            {
                this._errorMessageColumn.Visible = !this._errorMessageColumn.Visible;
            }
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

        public void DeleteRow()
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

        public void MoveLast()
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

        #endregion
        #region DataGrid events
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

            if (_sgColumn != null && cell.ColumnIndex == _sgColumn.Index)
            {
                SampleGroupVM sg = cellValue as SampleGroupVM;
                e.Cancel = !ProcessSampleGroupChanging(curTree, sg);

            }
            if (_speciesColumn != null && cell.ColumnIndex == _speciesColumn.Index)
            {
                TreeDefaultValueDO tdv = cellValue as TreeDefaultValueDO;
                e.Cancel = !ProcessSpeciesChanged(curTree, tdv);
            }

        }

        private void _datagrid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewComboBoxCell cell = _dataGrid[e.ColumnIndex, e.RowIndex] as DataGridViewComboBoxCell;
            if (cell == null) { return; }
            TreeVM curTree = this.ViewLogicController.CurrentPlotTreeList[e.RowIndex] as TreeVM;
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

        protected bool ProcessSampleGroupChanging(TreeVM tree, SampleGroupVM newSG)
        {
            if (tree == null || newSG == null) { return false; }
            //if (tree.SampleGroup == newSG) { return false; }
            if (tree.SampleGroup != null)
            {
                if (MessageBox.Show("You are changing the Sample Group of a tree, are you sure you want to do this?", "!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2)
                    == DialogResult.No)
                {
                    return false;
                }
                else
                {

                    this.AppController._cDal.LogMessage(String.Format("Tree Sample Group Changed (Cu:{0} St:{1} Sg:{2} -> {3} Tdv_CN:{4} T#: {5}",
                        tree.CuttingUnit.Code,
                        tree.Stratum.Code,
                        (tree.SampleGroup != null) ? tree.SampleGroup.Code : "?",
                        newSG.Code,
                        (tree.TreeDefaultValue != null) ? tree.TreeDefaultValue.TreeDefaultValue_CN.ToString() : "?",
                        tree.TreeNumber), "high");
                    tree.SampleGroup = newSG;
                }
            }
            if (!tree.SampleGroup.TreeDefaultValues.Contains(tree.TreeDefaultValue))
            {
                tree.SetTreeTDV(null);
            }
            return tree.TrySave();
        }




        protected bool ProcessSpeciesChanged(TreeVM tree, TreeDefaultValueDO tdv)
        {
            if (tree == null) { return true; }
            if (tree.TreeDefaultValue == tdv) { return true; }
            tree.SetTreeTDV(tdv);
            return tree.TrySave();
        }
        #endregion 


        #region IPlotLayout Members


        public bool AskContinueOnCurrnetPlotTreeError()
        {
            return this.AppController.ViewController.AskYesNo("Error(s) found on tree records in current plot, Would you like to continue?", "Continue?", MessageBoxIcon.Question, true);
        }

        public void ShowNoPlotSelectedMessage()
        {
            MessageBox.Show("No Plot Selected");
        }

        public void ShowNullPlotMessage()
        {
            MessageBox.Show("Can't perform action on null plot");
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
            this.toolStripComboBox1.ComboBox.DisplayMember = "Self";
            this.toolStripComboBox1.ComboBox.DataSource = plotBS;
        }

        public void BindTreeData(BindingSource treeBS)
        {
            this._dataGrid.DataSource = treeBS;
        }

        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var stratum = this.ViewLogicController.Stratum;

            this._tallyListPanel.SuspendLayout();
            this._mode = stratum.GetDataEntryMode();
            this.ViewLogicController.DataEntryController.PopulateTallies(stratum, this._mode, DataEntryController.Unit, this._tallyListPanel, this);
            if (stratum.Method == "3PPNT")
            {
                this.IsGridExpanded = true;
            }

            this._tallyListPanel.ResumeLayout();

            this.ViewLogicController.UpdateCurrentPlot();
        }

        //protected override void OnKeyUp(KeyEventArgs e)
        //{
        //    base.OnKeyUp(e);
        //    if (e.Handled == true) { return; }
        //    char key = (char)e.KeyValue;
        //    if (_viewLoading) { return; }
        //    e.Handled = this.ViewLogicController.DataEntryController.ProcessHotKey(key, this);
        //}

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

        
    }
}
