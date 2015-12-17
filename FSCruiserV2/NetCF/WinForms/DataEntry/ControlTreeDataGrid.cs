using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.WindowsCE.Forms;
using FMSC.Controls;
using CruiseDAL.DataObjects;
using FSCruiser.Core.Models;
using FSCruiser.Core;
using FSCruiser.Core.ViewInterfaces;
using FSCruiser.Core.DataEntry;

namespace FSCruiser.WinForms.DataEntry
{
    public class ControlTreeDataGrid : FMSC.Controls.EditableDataGrid, ITreeView
    {
        private bool _viewLoading = true;
        //private bool _changingTree = false;
        private EditableComboBoxColumn _speciesColumn; //handle to the species column in the datagrid, if displayed otherwise null
        private EditableComboBoxColumn _sgColumn; //handle to the sample group column in the datagird, if displayed otherwise null
        private EditableComboBoxColumn _stratumColumn; //handle to the stratum column in the datagrid, if displayed otherwise null
        private EditableTextBoxColumn _treeNumberColumn;
        private EditableComboBoxColumn _initialsColoumn;
        private DataGridButtonColumn _logsColumn;
        private EditableTextBoxColumn _kpiColumn; 
        private System.Windows.Forms.BindingSource _BS_trees;
        private IApplicationController Controller { get; set; }

        private bool _userCanAddTrees; 
        public bool UserCanAddTrees 
        {
            get { return _userCanAddTrees; }
            set { _userCanAddTrees = value; } 
        }

        public bool ViewLoading { get { return _viewLoading; } }

        public FormDataEntryLogic DataEntryController { get; set; }

        public IList<TreeVM> Trees
        {
            get
            {
                return this.DataEntryController.Unit.NonPlotTrees;
            }
        }

        //public String[] VisableFields
        //{
        //    get
        //    {
        //        return DataGridAdjuster.GetTreeFieldNames(this.Controller._cDal, this.Controller.CurrentUnit, null);
        //    }
        //}


        //public System.Windows.Forms.BindingSource TreeBindingSource
        //{
        //    get { return _BS_trees; }
        //    set
        //    {
        //        if (this._BS_trees == value) { return; }
        //        if (_BS_trees != null)
        //        {
        //            _BS_trees.CurrentChanged -= this._BS_trees_CurrentChanged;
        //        }

        //        _BS_trees = value;
        //        this.DataSource = value;
        //        if (_BS_trees != null)
        //        {
        //            _BS_trees.CurrentChanged += this._BS_trees_CurrentChanged;
        //        }
        //    }
        //}

        public ControlTreeDataGrid(IApplicationController controller, FormDataEntryLogic dataEntryController, InputPanel sip)
        {
            this.Controller = controller;
            this.DataEntryController = dataEntryController;
            DataGridAdjuster.InitializeGrid(this);
            DataGridTableStyle tableStyle = DataGridAdjuster.InitializeTreeColumns(controller._cDal, this, this.DataEntryController.Unit, null,this.Controller.ViewController.EnableLogGrading, this.LogsClicked);

            this.AllowUserToAddRows = false;//don't allow down arrow to add tree
            this.SIP = sip;
            //this.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold);

            //initialize _BS_trees
            this._BS_trees = new System.Windows.Forms.BindingSource();
            ((System.ComponentModel.ISupportInitialize)(this._BS_trees)).BeginInit();
            this._BS_trees.DataSource = typeof(TreeVM);
            //this._BS_trees.AddingNew += new System.ComponentModel.AddingNewEventHandler(this.UserAddTree);
            this._BS_trees.CurrentChanged += new EventHandler(_BS_trees_CurrentChanged);
            this.DataSource = this._BS_trees;
            ((System.ComponentModel.ISupportInitialize)(this._BS_trees)).EndInit();

            _speciesColumn = tableStyle.GridColumnStyles["TreeDefaultValue"] as EditableComboBoxColumn;
            _sgColumn = tableStyle.GridColumnStyles["SampleGroup"] as EditableComboBoxColumn;
            _stratumColumn = tableStyle.GridColumnStyles["Stratum"] as EditableComboBoxColumn;
            _treeNumberColumn = tableStyle.GridColumnStyles["TreeNumber"] as EditableTextBoxColumn;
            _initialsColoumn = tableStyle.GridColumnStyles["Initials"] as EditableComboBoxColumn;
            _logsColumn = tableStyle.GridColumnStyles["LogCount"] as DataGridButtonColumn;
            _kpiColumn = tableStyle.GridColumnStyles["KPI"] as EditableTextBoxColumn;

            //if (_speciesColumn != null)
            //{
            //    _speciesColumn.SelectedValueChanged += new EventHandler(SelectedSpeciesChanged);
            //}

            //if (_sgColumn != null)
            //{
            //    _sgColumn.SelectedValueChanged += new EventHandler(SelectedSampleGroupChanged);
            //}

            //if (_stratumColumn != null)
            //{
            //    //
            //    _stratumColumn.SelectedValueChanged += new EventHandler(SelectedStratumChanged);
            //}

            //if (_treeNumberColumn != null)
            //{
            //    _treeNumberColumn.Validating += new System.ComponentModel.CancelEventHandler(TreeNumberTextBox_Validating);
            //    //_treeNumberColumn.TextChanged += new EventHandler(_treeNumberColumn_TextChanged);
            //}
            //if (_kpiColumn != null)
            //{
            //    _kpiColumn.CellEditBeginning += new CellEditEventHandler(_kpiColumn_CellEditBeginning);
            //}

            if (_initialsColoumn != null)
            {
                _initialsColoumn.DataSource = this.Controller.GetCruiserList();
            }
        }

        //void _treeNumberColumn_TextChanged(object sender, EventArgs e)
        //{
        //    TextBox tb = (TextBox)sender;
        //    try
        //    {
        //        int newTreeNumber = Convert.ToInt32(tb.Text);
        //        if (!this.Controller.SetUnitTreeNumberSeqance(newTreeNumber))
        //        {
        //            tb.Undo();
        //        }
        //    }
        //    catch
        //    {
        //        tb.Undo();
        //    }
        //}

        


        protected override void OnCellValidating(EditableDataGridCellValidatingEventArgs e)
        {
            base.OnCellValidating(e);
            bool cancel = false;
            TreeVM currTree = (TreeVM)this._BS_trees[e.RowIndex];
            if (e.Column == _sgColumn)
            {
                this.DataEntryController.HandleSampleGroupChanging(currTree, e.Value as SampleGroupDO, out cancel);
                //this.HandleSampleGroupChanging(currTree, e.Value as SampleGroupDO, out cancel);
            }
            else if (e.Column == _speciesColumn)
            {
                //no action required
                return;
            }
            else if (e.Column == _stratumColumn)
            {
                this.DataEntryController.HandleStratumChanging(currTree, e.Value as StratumDO, out cancel);
                //this.HandleStratumChanging(currTree, e.Value as StratumDO, out cancel);
            }
            else if (e.Column == _treeNumberColumn)
            {
                try
                {
                    long treeNumber = (long)e.Value;
                    this.HandleTreeNumberChanging(treeNumber, out cancel);
                }
                catch
                {
                    cancel = true;
                }
            }
            else if (e.Column == _kpiColumn)
            {
                this.DataEntryController.HandleKPIChanging(currTree, (float)e.Value, false, out cancel);
                //this.HandleKPIChanging(currTree, (float)e.Value, out cancel);

                //MessageBox.Show("Bam!");
            }

            e.Cancel = cancel;
        }

        protected override void OnCellValueChanged(EditableDataGridCellEventArgs e)
        {
            base.OnCellValueChanged(e);

            TreeVM tree = (TreeVM)this._BS_trees[e.RowIndex];
            if (e.Column == _sgColumn)
            {
                this.DataEntryController.HandleSampleGroupChanged(this, tree);
                //this.HandleSampleGroupChanged(tree);
            }
            else if (e.Column == _speciesColumn)
            {
                EditableComboBoxColumn col = e.Column as EditableComboBoxColumn;
                if (col == null) { return; }

                this.DataEntryController.HandleSpeciesChanged(tree, col.EditComboBox.SelectedItem as TreeDefaultValueDO);
                //this.HandleSpeciesChanged(tree, col.EditComboBox.SelectedItem as TreeDefaultValueDO);
            }
            else if (e.Column == _stratumColumn)
            {
                this.DataEntryController.HandleStratumChanged(this, tree);
                //this.HandleStratumChanged(tree);
            }
            //else if (e.Column == _kpiColumn)
            //{
            //    MessageBox.Show("Bam!");
            //}
        }


        //protected void HandleKPIChanging(TreeVM tree, float newKPI, out bool cancel)
        //{
        //    if (tree == null) 
        //    {
        //        cancel = true;
        //        return; 
        //    }
        //    if (tree.SampleGroup == null)
        //    {
        //        MessageBox.Show("Select Sample Group before entering KPI");
        //        cancel = true;
        //        return;
        //    }
        //    if (tree.KPI != 0.0F)
        //    {
        //        string message = string.Format("Tree RecID:{0} KPI changed from {1} to {2}", tree.Tree_CN, tree.KPI, newKPI);
        //        this.Controller._cDal.LogMessage(message, "I");
        //    }
        //    cancel = false;
        //}


        protected void HandleTreeNumberChanging(long newTreeNumber, out bool cancel)
        {
            try
            {

                if (!this.DataEntryController.Unit.IsTreeNumberAvalible(newTreeNumber))
                {
                    cancel = true;
                    return;
                }
            }
            catch
            {
                cancel = true;
                return;
            }
            cancel = false;
        }



        //protected void HandleStratumChanging(TreeVM tree, StratumDO st, out bool cancel)
        //{
        //    if (tree == null || st == null) { cancel = true; return; }
        //    if (tree.Stratum != null && tree.Stratum.Stratum_CN == st.Stratum_CN) { cancel = true; return; }
        //    if (tree.Stratum != null)
        //    {
        //        if (MessageBox.Show("You are changing the stratum of a tree, are you sure you want to do this?", "!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2)
        //            == DialogResult.No)
        //        {
        //            cancel = true;//do not change stratum
        //        }
        //        else
        //        {
        //            //log stratum changed
        //            this.Controller._cDal.LogMessage(String.Format("Tree Stratum Changed (Cu:{0} St:{1} -> {2} Sg:{3} Tdv_CN:{4} T#: {5}",
        //                tree.CuttingUnit.Code,
        //                tree.Stratum.Code,
        //                st.Code,
        //                (tree.SampleGroup != null) ? tree.SampleGroup.Code : "?",
        //                (tree.TreeDefaultValue != null) ? tree.TreeDefaultValue.TreeDefaultValue_CN.ToString() : "?",
        //                tree.TreeNumber), "high");
        //        }
        //    }
        //    cancel = false;
        //}

        //protected void HandleStratumChanged(TreeVM tree)
        //{
        //    if (tree == null) { return; }

        //    tree.Species = null;
        //    tree.SampleGroup = null;
        //    this.Controller.SetTreeTDV(tree, null);
        //    this.UpdateSampleGroupColumn(tree);
        //    this.UpdateSpeciesColumn(tree);
        //    this.Controller.TrySaveTree(tree);
        //}

         

         //protected void HandleSampleGroupChanging(TreeVM tree, SampleGroupDO newSG, out bool cancel)
         //{
         //    if (tree == null || newSG == null) { cancel = true; return; }
         //    if (tree.SampleGroup != null && tree.SampleGroup_CN == newSG.SampleGroup_CN) { cancel = true; return; } 
         //    if (tree.SampleGroup != null)
         //    {
         //        if (MessageBox.Show("You are changing the Sample Group of a tree, are you sure you want to do this?", "!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2)
         //            == DialogResult.No)
         //        {
         //            cancel = true;//disregard changes
         //            return;
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
         //        }
         //    }
         //    cancel = false;
         //}

        //protected void HandleSampleGroupChanged(TreeVM tree)
        //{
        //    if(tree == null) { return; }
        //    if (!tree.SampleGroup.TreeDefaultValues.Contains(tree.TreeDefaultValue))
        //    {
        //        this.Controller.SetTreeTDV(tree, null);
        //    }
        //    this.UpdateSpeciesColumn(tree);
        //    this.Controller.TrySaveTree(tree);
        //}

        //protected void HandleSpeciesChanging(TreeDefaultValueDO newTDV, out bool cancel)
        //{
        //    //no action required
        //}

        //protected void HandleSpeciesChanged(TreeVM tree, TreeDefaultValueDO tdv)
        //{
        //    if (tree == null) { return; }
        //    this.Controller.SetTreeTDV(tree, tdv);
        //    this.Controller.TrySaveTree(tree);
        //}


        void LogsClicked(ButtonCellClickEventArgs e)
        {
            TreeVM tree = this._BS_trees[e.RowNumber] as TreeVM;
            if (tree == null) { return; }
            this.DataEntryController.ShowLogs(tree);
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

            var newTree = this.DataEntryController.Unit.UserAddTree(prevTree
                , assumedSt
                , this.DataEntryController.ViewController);
            //this.DataEntryController.Controller.OnTally();
            return newTree;

            //return Controller.UserAddTree(prevTree, assumedSt, null);
        }

        public TreeVM UserAddTree()
        {
            if (_viewLoading) { return null; }
            TreeVM t = this.GetNewTree();
            if (t != null)
            {
                t.TreeCount = 1;

                this._BS_trees.MoveLast();
                this.MoveFirstEmptyCell();                
            }
            return t;
        }


        private void _BS_trees_CurrentChanged(object sender, EventArgs e)
        {

            TreeVM tree = _BS_trees.Current as TreeVM;

            //_changingTree = true;
            UpdateSpeciesColumn(tree);
            UpdateSampleGroupColumn(tree);

            //_changingTree = false;
        }

        public void UpdateSpeciesColumn(TreeVM tree)
        {
            if (_speciesColumn != null)
            {
                _speciesColumn.DataSource = tree.ReadValidTDVs();
            }
        }

        public void UpdateSampleGroupColumn(TreeVM tree)
        {
            if (_sgColumn != null)
            {
                _sgColumn.DataSource = tree.ReadValidSampleGroups();
            }
        }

        public void UpdateStratumColumn()
        {
            if (_stratumColumn != null)
            {
                _stratumColumn.DataSource = DataEntryController.Unit.GetTreeBasedStrata();
            }
        }
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
            }
        }

        #region ITreeView Members

        public void MoveLast()
        {            
            this._BS_trees.MoveLast();
            this.MoveHomeField();
        }

        public void MoveHomeField()
        {
            this.MoveFirstEmptyCell();
        }

        public void HandleLoad()
        {
            UpdateStratumColumn();
            this._BS_trees.DataSource = DataEntryController.Unit.NonPlotTrees;
            
            _viewLoading = false;
        }

        public bool HandleEscKey()
        {
                this.DataEntryController.View.GoToTallyPage();
                return true;
        }

        public void HandleEnableLogGradingChanged()
        {
            if (_logsColumn == null) { return; }
            if ((_logsColumn.Width > 0) == this.Controller.ViewController.EnableLogGrading) { return; }
            if (this.Controller.ViewController.EnableLogGrading)
            {
                _logsColumn.Click += this.LogsClicked;
                _logsColumn.Width = Constants.LOG_COLUMN_WIDTH;
            }
            else
            {
                _logsColumn.Click -= this.LogsClicked;
                _logsColumn.Width = -1;
            }
        }

        public void HandleCruisersChanged()
        {
            if (this._initialsColoumn != null)
            {
                this._initialsColoumn.DataSource = this.Controller.GetCruiserList();
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
                    //curTree.Delete();
                    //Controller.DeleteTree(curTree);
                }
            }
            
        }


        public void ShowHideErrorCol()
        {
            DataGridAdjuster.ShowHideErrorCol(this);
        }

        #endregion

        //protected void TreeNumberTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    TextBox tb = (TextBox)sender;
        //    try
        //    {
        //        int newTreeNumber = Convert.ToInt32(tb.Text);
        //        if (!this.Controller.SetUnitTreeNumberSeqance(newTreeNumber))
        //        {
        //            tb.Undo();
        //        }
        //    }
        //    catch
        //    {
        //        tb.Undo();
        //    }
        //}

        //void SelectedStratumChanged(object sender, EventArgs e)
        //{
        //    if (!_changingTree)
        //    {
        //        TreeVM tree = _BS_trees.Current as TreeVM;
        //        StratumDO stratum = null;
        //        if (_stratumColumn != null)
        //        {
        //            stratum = _stratumColumn.EditComboBox.SelectedItem as StratumDO;
        //        }
        //        if (tree == null || stratum == null) { return; }
        //        if (tree.Stratum == stratum) { return; }
        //        if (tree.Stratum != null)
        //        {
        //            if (MessageBox.Show("You are changing the stratum of a tree, are you sure you want to do this?", "!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2)
        //                == DialogResult.No)
        //            {
        //                return;//do not change stratum
        //            }
        //            else
        //            {
        //                //log stratum changed
        //                this.Controller._cDal.LogMessage(String.Format("Tree Stratum Changed (Cu:{0} St:{1} -> {2} Sg:{3} Tdv_CN:{4} T#: {5}",
        //                    tree.CuttingUnit.Code,
        //                    tree.Stratum.Code,
        //                    stratum.Code,
        //                    (tree.SampleGroup != null) ? tree.SampleGroup.Code : "?",
        //                    (tree.TreeDefaultValue != null) ? tree.TreeDefaultValue.TreeDefaultValue_CN.ToString() : "?",
        //                    tree.TreeNumber), "high");
        //            }
        //        }


        //        tree.Species = null;
        //        tree.SampleGroup = null;
        //        ApplicationController.SetTreeTDV(tree, null);
        //        tree.Stratum = stratum;
        //        this.UpdateSampleGroupColumn(tree);
        //        this.UpdateSpeciesColumn(tree);
        //        this.Controller.TrySaveTree(tree);


        //    }
        //}

        //void SelectedSampleGroupChanged(object sender, EventArgs e)
        //{
        //    if (!_changingTree)
        //    {
        //        TreeVM tree = _BS_trees.Current as TreeVM;
        //        SampleGroupDO sg = null;
        //        if (_sgColumn != null)
        //        {
        //            sg = _sgColumn.EditComboBox.SelectedItem as SampleGroupDO;
        //        }
        //        if (tree == null || sg == null) { return; }
        //        if (tree.SampleGroup == sg) { return; }
        //        if (tree.SampleGroup != null)
        //        {
        //            if (MessageBox.Show("You are changing the Sample Group of a tree, are you sure you want to do this?", "!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2)
        //                == DialogResult.No)
        //            {
        //                return;//disregard changes
        //            }
        //            else
        //            {

        //                this.Controller._cDal.LogMessage(String.Format("Tree Sample Group Changed (Cu:{0} St:{1} Sg:{2} -> {3} Tdv_CN:{4} T#: {5}",
        //                    tree.CuttingUnit.Code,
        //                    tree.Stratum.Code,
        //                    (tree.SampleGroup != null) ? tree.SampleGroup.Code : "?",
        //                    sg.Code,
        //                    (tree.TreeDefaultValue != null) ? tree.TreeDefaultValue.TreeDefaultValue_CN.ToString() : "?",
        //                    tree.TreeNumber), "high");
        //            }
        //        }

        //        tree.SampleGroup = sg;
        //        if (!sg.TreeDefaultValues.Contains(tree.TreeDefaultValue))
        //        {
        //            ApplicationController.SetTreeTDV(tree, null);
        //        }
        //        this.UpdateSpeciesColumn(tree);
        //        this.Controller.TrySaveTree(tree);
        //    }
        //}

        //private void SelectedSpeciesChanged(object sender, EventArgs e)
        //{
        //    if (!_changingTree)
        //    {
        //        TreeVM tree = _BS_trees.Current as TreeVM;
        //        TreeDefaultValueDO tdv = null;
        //        if (_speciesColumn != null)
        //        {
        //            tdv = _speciesColumn.EditComboBox.SelectedItem as TreeDefaultValueDO;
        //        }
        //        if (tree == null || tdv == null) { return; }
        //        ApplicationController.SetTreeTDV(tree, tdv);
        //        this.Controller.TrySaveTree(tree);
        //    }
        //}
    }
}
