using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FSCruiser.Core.ViewInterfaces;
using FSCruiser.Core;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class LayoutPlot : UserControl , ITallyView , ITreeView, IPlotLayout
    {
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

        public IList<TreeVM> Trees
        {
            get
            {
                return this.ViewLogicController.CurrentPlotTreeList;
            }
        }


        public LayoutPlot(FormDataEntryLogic dataEntryController, Control parent, StratumVM stratum)
        {
            this.ViewLogicController = new LayoutPlotLogic(stratum, this, dataEntryController);
            this.Dock = DockStyle.Fill;            
            InitializeComponent();
            
            

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

            if (_speciesColumn != null)
            {
                _speciesColumn.DataSource = AppController._cDal.Read<TreeDefaultValueDO>("TreeDefaultValue", null);
            }
            if (_sgColumn != null)
            {
                _sgColumn.DataSource = AppController._cDal.Read<SampleGroupVM>("SampleGroup", null);
            }
            if (_initialsColoumn != null)
            {
                _initialsColoumn.DataSource = this.AppController.GetCruiserList();
            }

            //no need to load tallies....?
            //Controller.PopulateTallies(this.StratumInfo, this._mode, Controller.CurrentUnit, this._tallyListPanel, this);
            this.Parent = parent;
            this.ViewLogicController.UpdateCurrentPlot();
        }


        #region ITallyView Members
        //TODO implement ITallyView members 
        public bool HotKeyEnabled
        {
            get
            {
                return true;
            }
        }
        public Dictionary<char, CountTreeVM> HotKeyLookup
        {
            get
            {
                return this.ViewLogicController.StratumInfo.HotKeyLookup;
            }

        }

        public bool HandleHotKeyFirst(char key)
        {
            return false;//TODO not implemented
        }

        public bool HandleEscKey()
        {
            return false;//TODO not implemented
        }


        public void MakeSGList(List<SampleGroupVM> list, Panel container)
        {
            throw new NotImplementedException();
        }

        public Control MakeTallyRow(Control container, CountTreeVM count)
        {
            throw new NotImplementedException();
        }

        public Control MakeTallyRow(Control container, SubPop subPop)
        {
            throw new NotImplementedException();
        }

        public void OnTally(CountTreeVM count)
        {
            throw new NotImplementedException();
        }

        public void HandleStratumLoaded(Control container)
        {
            //do nothing 
            return;
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
                this._initialsColoumn.DataSource = this.AppController.GetCruiserList();
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
            this._dataGrid.CurrentCell = this._dataGrid[0, this._dataGrid.CurrentCellAddress.Y];
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
            DataGridViewComboBoxCell cell = _dataGrid[e.ColumnIndex, e.RowIndex] as DataGridViewComboBoxCell;
            if (cell == null) { return; }
            if (cell.FormattedValue == e.FormattedValue) { return; }//are there any changes 


            bool cancel = false;
            TreeVM curTree = null;
            try
            {
                curTree = this.ViewLogicController.CurrentTree;
                if (curTree == null) { return; }
            }
            catch
            {
                e.Cancel = true;
                return;
            }

            //TreeVM curTree = null;
            //try
            //{

            //    curTree = this._BS_Trees[e.RowIndex] as TreeVM;
            //}
            //catch (SystemException) { return; }//ignore posible out of bound exceptions
            //if (curTree == null) { return; }

            object cellValue = e.FormattedValue;
            cellValue = cell.ParseFormattedValue(cellValue, cell.InheritedStyle, null, null);

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
            cell.DataSource = AppController.GetTreeSGList(tree);
        }

        protected void UpdateSpeciesColumn(TreeVM tree, DataGridViewComboBoxCell cell)
        {
            if (cell == null) { return; }
            cell.DataSource = AppController.GetTreeTDVList(tree);
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
                this.AppController.SetTreeTDV(tree, null);
            }
            return this.AppController.TrySaveTree(tree);
        }




        protected bool ProcessSpeciesChanged(TreeVM tree, TreeDefaultValueDO tdv)
        {
            if (tree == null) { return true; }
            if (tree.TreeDefaultValue == tdv) { return true; }
            this.AppController.SetTreeTDV(tree, tdv);
            return this.AppController.TrySaveTree(tree);
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
            if (this.ViewLogicController.CurrentPlotInfo == null)
            {
                ShowNoPlotSelectedMessage();
                return;
            }

            this.ViewLogicController.Controller.ShowLimitingDistanceDialog(this.ViewLogicController.StratumInfo, this.ViewLogicController.CurrentPlotInfo, null);
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
        }

        public void BindTreeData(BindingSource treeBS)
        {
            this._dataGrid.DataSource = treeBS;
        }

        #endregion

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.Handled == true) { return; }
            char key = (char)e.KeyValue;
            if (_viewLoading) { return; }
            e.Handled = this.ViewLogicController.DataEntryController.ProcessHotKey(key, this);
        }

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
