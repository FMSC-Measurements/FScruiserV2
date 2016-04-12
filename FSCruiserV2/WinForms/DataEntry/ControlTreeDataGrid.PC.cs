using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CruiseDAL.DataObjects;
using CruiseDAL; 
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;
using FSCruiser.Core;
using FSCruiser.Core.DataEntry;
using FMSC.ORM.Core.SQL;
using System.Drawing;

namespace FSCruiser.WinForms.DataEntry
{
    public class ControlTreeDataGrid : DataGridView, ITreeView
    {
        bool _userCanAddTrees;
        private bool _viewLoading = true;
        private BindingSource _BS_trees;
        private DataGridViewComboBoxColumn _speciesColumn;
        private DataGridViewComboBoxColumn _sgColumn;
        private DataGridViewComboBoxColumn _stratumColumn;
        private DataGridViewTextBoxColumn _treeNumberColumn;
        private DataGridViewButtonColumn _logsColumn;
        private DataGridViewTextBoxColumn _errorMessageColumn;
        private DataGridViewComboBoxColumn _initialsColoumn;

        private ContextMenuStrip _contexMenu;
        private ToolStripMenuItem logToolStripMenuItem;

        //private BindingSource _BS_TreeSampleGroups;
        //private BindingSource _BS_TreeSpecies; 

        public IApplicationController Controller { get; protected set; }
        public FormDataEntryLogic DataEntryController { get; set; }

        public bool ViewLoading { get { return _viewLoading; } }

        public int HomeColumnIndex { get; set; }


        public String[] VisableFields
        {
            get
            {
                return DataGridAdjuster.GetTreeFieldNames(this.Controller._cDal, this.DataEntryController.Unit, null);
            }
        }

        public IList<TreeVM> Trees
        {
            get
            {
                return this.DataEntryController.Unit.NonPlotTrees;
            }
        }

        public ControlTreeDataGrid(IApplicationController controller, FormDataEntryLogic dataEntryController)
        {
            EditMode = DataGridViewEditMode.EditOnEnter;
            AutoGenerateColumns = false;
            AllowUserToDeleteRows = false;
            AllowUserToAddRows = false;
            Controller = controller;
            DataEntryController = dataEntryController;

            CellClick += new DataGridViewCellEventHandler(ControlTreeDataGrid_CellClick);

            _BS_trees = new BindingSource();
            ((System.ComponentModel.ISupportInitialize)_BS_trees).BeginInit();
            _BS_trees.DataSource = typeof(TreeVM);
            DataSource = _BS_trees;
            ((System.ComponentModel.ISupportInitialize)_BS_trees).EndInit();

            //_BS_TreeSpecies = new BindingSource();
            //((System.ComponentModel.ISupportInitialize)_BS_TreeSpecies).BeginInit();
            //_BS_TreeSpecies.DataSource = typeof(TreeDefaultValueDO);
            //((System.ComponentModel.ISupportInitialize)_BS_TreeSpecies).EndInit();

            //_BS_TreeSampleGroups = new BindingSource();
            //((System.ComponentModel.ISupportInitialize)_BS_TreeSampleGroups).BeginInit();
            //_BS_TreeSampleGroups.DataSource = typeof(SampleGroupDO);
            //((System.ComponentModel.ISupportInitialize)_BS_TreeSampleGroups).EndInit();

            DataGridViewColumn[] columns = DataGridAdjuster.MakeTreeColumns(controller._cDal, DataEntryController.Unit, null, Controller.ViewController.EnableLogGrading);
            base.Columns.AddRange(columns);

            _speciesColumn = base.Columns["Species"] as DataGridViewComboBoxColumn;
            _sgColumn = base.Columns["SampleGroup"] as DataGridViewComboBoxColumn;
            _stratumColumn = base.Columns["Stratum"] as DataGridViewComboBoxColumn;
            _treeNumberColumn = base.Columns["TreeNumber"] as DataGridViewTextBoxColumn;
            _initialsColoumn = base.Columns["Initials"] as DataGridViewComboBoxColumn;
            _errorMessageColumn = base.Columns["Error"] as DataGridViewTextBoxColumn;
            _logsColumn = base.Columns["Logs"] as DataGridViewButtonColumn;

            if (_speciesColumn != null)
            {
                _speciesColumn.DataSource = Controller._cDal.From<TreeDefaultValueDO>().Read().ToList();
            }
            if (_sgColumn != null)
            {
                _sgColumn.DataSource = Controller._cDal.From<SampleGroupVM>().Read().ToList();
            }
            if (_stratumColumn != null)
            {
                _stratumColumn.DataSource = DataEntryController.Unit.GetTreeBasedStrata();
            }
            if (_initialsColoumn != null)
            {
                _initialsColoumn.DataSource = Controller.Settings.Cruisers.ToArray();
            }
            if (_logsColumn != null)
            {
                _logsColumn.Visible = Controller.ViewController.EnableLogGrading;
            }

            _contexMenu = new ContextMenuStrip(new System.ComponentModel.Container());
            logToolStripMenuItem = new ToolStripMenuItem();
            _contexMenu.SuspendLayout();

            this.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(ControlTreeDataGrid_ColumnHeaderMouseClick);

            _contexMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { logToolStripMenuItem });
            _contexMenu.Name = "_contexMenu";
            _contexMenu.Size = new System.Drawing.Size(181, 26);
            logToolStripMenuItem.Name = "logToolStripMenuItem";
            logToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            logToolStripMenuItem.Text = Controller.ViewController.EnableLogGrading ?
                "Disable Log Grading" : "Enable Log Grading";
            logToolStripMenuItem.Click += logToolStripMenuItem_Click;
            _contexMenu.ResumeLayout(false);
        }

        void ControlTreeDataGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                logToolStripMenuItem.Text = Controller.ViewController.EnableLogGrading ?
                    "Disable Log Grading" : "Enable Log Grading";

                _contexMenu.Show(Cursor.Position);
            }
        }

        void ControlTreeDataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_logsColumn != null && e.RowIndex > -1 && e.ColumnIndex == _logsColumn.Index)
            {
                TreeVM curTree = this.Trees[e.RowIndex] as TreeVM;
                if (curTree != null)
                {
                    this.DataEntryController.ShowLogs(curTree);
                }
            }
        }

        protected override void OnDataError(bool displayErrorDialogIfNoHandler, DataGridViewDataErrorEventArgs e)
        {
            displayErrorDialogIfNoHandler = false;
            base.OnDataError(displayErrorDialogIfNoHandler, e);
            
        }

        protected override void OnCellEnter(DataGridViewCellEventArgs e)
        {
            base.OnCellEnter(e);

            DataGridViewComboBoxCell cell = base[e.ColumnIndex, e.RowIndex] as DataGridViewComboBoxCell;
            if(cell == null) { return; }
            TreeVM curTree = this._BS_trees[e.RowIndex] as TreeVM;
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

        //protected override void OnCurrentCellDirtyStateChanged(EventArgs e)
        //{
        //    base.OnCurrentCellDirtyStateChanged(e);

        //    DataGridViewComboBoxCell cell = base.CurrentCell as DataGridViewComboBoxCell;
        //    if (cell == null) { return; }
            
        //    TreeVM curTree = null;
        //    try
        //    {
        //        curTree = this._BS_trees[cell.RowIndex] as TreeVM;
        //    }
        //    catch (SystemException) { return; }//ignore posible out of bound exceptions

        //    object cellValue = cell.EditedFormattedValue;
        //    cellValue = cell.ParseFormattedValue(cellValue, cell.InheritedStyle, null, null);
        //    if (curTree == null) { return; }

        //    if (_sgColumn != null && cell.ColumnIndex == _sgColumn.Index)
        //    {
        //        SampleGroupDO sg = cellValue as SampleGroupDO;
        //        HandleSampleGroupChanging(curTree, sg);

        //    }
        //    if (_speciesColumn != null && cell.ColumnIndex == _speciesColumn.Index)
        //    {
        //        TreeDefaultValueDO tdv = cellValue as TreeDefaultValueDO;
        //        HandleSpeciesChanged(curTree, tdv);
        //    }
        //}

        protected override void OnCellValidating(DataGridViewCellValidatingEventArgs e)
        {
            base.OnCellValidating(e);
            var cell = base[e.ColumnIndex,e.RowIndex];
            if (cell == null) { return; }
            if (cell.FormattedValue == e.FormattedValue) { return; }//are there any changes? 

            TreeVM curTree = null;
            try
            {
                curTree = this._BS_trees[e.RowIndex] as TreeVM;
            }
            catch (ArgumentOutOfRangeException) { return; }//ignore posible out of bound exceptions
            if (curTree == null) { return; }

            object cellValue = e.FormattedValue;
            cellValue = cell.ParseFormattedValue(cellValue, cell.InheritedStyle, null, null);

            if (_treeNumberColumn != null && e.ColumnIndex == _treeNumberColumn.Index)
            {
                var newTreeNum = (long)cellValue;
                if (curTree.TreeNumber != newTreeNum
                    && !this.DataEntryController.Unit.IsTreeNumberAvalible(newTreeNum))
                {
                    MessageBox.Show("Tree Number already exists");
                    e.Cancel = true;
                }
            }
            else if (_sgColumn != null && e.ColumnIndex == _sgColumn.Index)
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

                //SampleGroupVM sg = cellValue as SampleGroupVM;
                //bool cancel = e.Cancel;
                //this.DataEntryController.HandleSampleGroupChanging(curTree, sg, out cancel);
                //e.Cancel = cancel;

            }
            else if (_speciesColumn != null && e.ColumnIndex == _speciesColumn.Index)
            {
                TreeDefaultValueDO tdv = cellValue as TreeDefaultValueDO;
                e.Cancel = !this.DataEntryController.HandleSpeciesChanged(curTree, tdv);
            }
            else if (_stratumColumn != null && e.ColumnIndex == _stratumColumn.Index)
            {
                StratumVM newSt = cellValue as StratumVM;
                if (curTree.HandleStratumChanging(newSt, this))
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



        //protected bool ProcessSampleGroupChanging(TreeVM tree, SampleGroupVM newSG)
        //{
        //    if (tree == null || newSG == null) {  return true; }
        //    //if (tree.SampleGroup == newSG) { return false; }
        //    if (tree.SampleGroup != null)
        //    {
        //        if (MessageBox.Show("You are changing the Sample Group of a tree, are you sure you want to do this?", "!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2)
        //            == DialogResult.No)
        //        {
        //            return false;
        //        }
        //        else
        //        {

        //            this.Controller._cDal.LogMessage(String.Format("Tree Sample Group Changed (Cu:{0} St:{1} Sg:{2} -> {3} Tdv_CN:{4} T#: {5}",
        //                tree.CuttingUnit.Code,
        //                tree.Stratum.Code,
        //                (tree.SampleGroup != null) ? tree.SampleGroup.Code : "?",
        //                newSG.Code,
        //                (tree.TreeDefaultValue != null) ? tree.TreeDefaultValue.TreeDefaultValue_CN.ToString() : "?",
        //                tree.TreeNumber), "high");
        //            tree.SampleGroup = newSG;
        //        }
        //    }
        //    if (!tree.SampleGroup.TreeDefaultValues.Contains(tree.TreeDefaultValue))
        //    {
        //        tree.SetTreeTDV(null);
        //    }
        //    return tree.TrySave();
        //}




        //protected bool ProcessSpeciesChanged(TreeVM tree, TreeDefaultValueDO tdv)
        //{
        //    if (tree == null) { return true; }
        //    if (tree.TreeDefaultValue == tdv) { return true; }
        //    tree.SetTreeTDV(tdv);
        //    return tree.TrySave();
        //}

        public void UpdateSampleGroupColumn(TreeVM tree)
        {
            this.UpdateSampleGroupColumn(tree, this.CurrentCell as DataGridViewComboBoxCell);
        }

        public void UpdateSpeciesColumn(TreeVM tree)
        {
            this.UpdateSpeciesColumn(tree, this.CurrentCell as DataGridViewComboBoxCell);
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

        //private void UpdateSampleGroupColumn(TreeVM tree, DataGridViewComboBoxEditingControl editControl)
        //{
        //    object sel = editControl.SelectedItem;
        //    if(editControl.DataSource != this._BS_TreeSampleGroups)//generaly always true
        //    {
        //        editControl.DataSource = this._BS_TreeSampleGroups;
        //    }

        //    this._BS_TreeSampleGroups.SuspendBinding();
        //    this._BS_TreeSampleGroups.DataSource = Controller.GetTreeSGList(tree);
        //    this._BS_TreeSampleGroups.ResumeBinding();

        //    this._BS_TreeSampleGroups.Position = this._BS_TreeSampleGroups.IndexOf(sel);

            
        //}



        #region ITreeView Members

        
        public bool UserCanAddTrees
        {
            get
            {
                return _userCanAddTrees;
                //return this.AllowUserToAddRows;
            }
            set
            {
                _userCanAddTrees = value;
                //this.AllowUserToAddRows = value;
            }
        }

        public bool PreviewKeypress(KeyEventArgs ea)
        {
            switch (ea.KeyCode)
            {
                case Keys.Escape: //esc
                    {
                        this.DataEntryController.View.GoToTallyPage();
                        return true;
                    }
                default:
                    { return false; }
            }
        }

        //public bool HandleEscKey()
        //{
        //    this.DataEntryController.View.GoToTallyPage();
        //    return true;
        //}

        public void HandleLoad()
        {
            this._BS_trees.DataSource = DataEntryController.Unit.NonPlotTrees;

            _viewLoading = false;
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
                this._logsColumn.Visible = this.Controller.ViewController.EnableLogGrading;
            }
        }

        public void HandleCruisersChanged()
        {
            if (this._initialsColoumn != null)
            {
                this._initialsColoumn.DataSource = this.Controller.Settings.Cruisers.ToArray();
            }
        }

        public void DeleteRow()
        {
            TreeVM curTree = this._BS_trees.Current as TreeVM;
            if (curTree == null)
            {
                MessageBox.Show("No Tree Selected");
            }
            else
            {
                if (DialogResult.Yes == MessageBox.Show("Delete Tree #" + curTree.TreeNumber.ToString() + "?",
                    "Delete Tree?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2))
                {
                    DataEntryController.Unit.DeleteTree(curTree);
                }
            }
        }

        public new void EndEdit()
        {
            base.EndEdit();
        }

        public void MoveLast()
        {
            this._BS_trees.MoveLast();
        }

        public void MoveHomeField()
        {
            if (this.CurrentCellAddress.Y == -1) { return; }
            try
            {
                this.CurrentCell = this[0, this.CurrentCellAddress.Y];
            }
            catch
            { }
        }

        public TreeVM UserAddTree()
        {
            if (_viewLoading) { return null; }
            TreeVM t = this.GetNewTree();
            if (t != null)
            {
                //t.TreeCount = 1; //for pc dont set tree count to 1
                //this._BS_trees.Add(t);
                this.MoveLast();
                this.MoveHomeField();
            }
            return t;

        }

        private TreeVM GetNewTree()
        {
            if (this.UserCanAddTrees == false) { return null; }
            TreeVM prevTree = null;
            StratumVM assumedSt = DataEntryController.Unit.DefaultStratum;
            if (_BS_trees.Count > 0)
            {
                prevTree = (TreeVM)_BS_trees[_BS_trees.Count - 1];
                assumedSt = prevTree.Stratum;
            }

            return DataEntryController.Unit.UserAddTree(prevTree, assumedSt, DataEntryController.ViewController);
        }

        #endregion

        

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Controller.ViewController.EnableLogGrading = !Controller.ViewController.EnableLogGrading;

            logToolStripMenuItem.Text = Controller.ViewController.EnableLogGrading ?
                "Disable Log Grading" : "Enable Log Grading";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(_BS_trees != null)
                {
                    _BS_trees.Dispose();
                    _BS_trees = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}
