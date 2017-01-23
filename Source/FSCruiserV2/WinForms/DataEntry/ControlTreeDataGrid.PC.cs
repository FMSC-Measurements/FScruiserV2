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

        public IList<Tree> Trees
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

            ApplicationSettings.Instance.CruisersChanged += new EventHandler(Settings_CruisersChanged);

            CellClick += new DataGridViewCellEventHandler(ControlTreeDataGrid_CellClick);

            _BS_trees = new BindingSource();
            ((System.ComponentModel.ISupportInitialize)_BS_trees).BeginInit();
            _BS_trees.DataSource = typeof(Tree);
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

            var columns = DataEntryController.Unit.MakeTreeColumns();
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
                _speciesColumn.DataSource = Controller.DataStore.From<TreeDefaultValueDO>().Read().ToList();
            }
            if (_sgColumn != null)
            {
                _sgColumn.DataSource = DataEntryController.Unit.TreeSampleGroups.ToList();
            }
            if (_stratumColumn != null)
            {
                _stratumColumn.DataSource = DataEntryController.Unit.TreeStrata;
            }
            if (_initialsColoumn != null)
            {
                _initialsColoumn.DataSource = ApplicationSettings.Instance.Cruisers.ToArray();
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
                var curTree = this.Trees[e.RowIndex] as Tree;
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
            if (cell == null) { return; }
            var curTree = this._BS_trees[e.RowIndex] as Tree;
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

            if (e.RowIndex > base.RowCount - 1 || e.RowIndex < 0) { return; }
            if (e.ColumnIndex > base.ColumnCount - 1 || e.ColumnIndex < 0) { return; }

            var cell = base[e.ColumnIndex, e.RowIndex];
            if (cell == null) { return; }
            if (cell.FormattedValue == e.FormattedValue) { return; }//are there any changes?

            Tree curTree = null;
            try
            {
                curTree = this._BS_trees[e.RowIndex] as Tree;
                if (curTree == null) { return; }
            }
            catch (ArgumentOutOfRangeException) { return; }//ignore posible out of bound exceptions

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
                e.Cancel = !this.DataEntryController.HandleSpeciesChanged(curTree, tdv);
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

        public void UpdateSampleGroupColumn(Tree tree)
        {
            this.UpdateSampleGroupColumn(tree, this.CurrentCell as DataGridViewComboBoxCell);
        }

        public void UpdateSpeciesColumn(Tree tree)
        {
            this.UpdateSpeciesColumn(tree, this.CurrentCell as DataGridViewComboBoxCell);
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
            if (ea.KeyData != Keys.None) { return false; }

            var settings = ApplicationSettings.Instance;

            if (ea.KeyData == settings.JumpTreeTallyKey)
            {
                this.DataEntryController.View.GoToTallyPage();
                return true;
            }
            else if (ea.KeyData == settings.AddTreeKey)
            {
                return UserAddTree() != null;
            }
            else
            {
                return false;
            }
        }

        public void HandleLoad()
        {
            this._BS_trees.DataSource = DataEntryController.Unit.NonPlotTrees;

            _viewLoading = false;
        }

        public void HandleEnableLogGradingChanged()
        {
            if (this._logsColumn != null)
            {
                this._logsColumn.Visible = this.Controller.ViewController.EnableLogGrading;
            }
        }

        void Settings_CruisersChanged(object sender, EventArgs e)
        {
            if (this._initialsColoumn != null)
            {
                this._initialsColoumn.DataSource = ApplicationSettings.Instance.Cruisers.ToArray();
            }
        }

        public void DeleteSelectedTree()
        {
            var curTree = this._BS_trees.Current as Tree;
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

        public void MoveLastTree()
        {
            this._BS_trees.MoveLast();
        }

        public void MoveHomeField()
        {
            var row = CurrentRow;
            if (row == null) { return; }
            try
            {
                var cells = row.Cells.OfType<DataGridViewCell>()
                    .Where(c => !c.ReadOnly && c.Visible && !(c is DataGridViewButtonCell));
                foreach (var cell in cells)
                {
                    var cellValue = cell.Value;
                    var cellType = cell.ValueType;
                    if (cellValue == null
                        || (cellValue is String && string.IsNullOrEmpty(cellValue as string)) //is empty string
                        || (cellType.IsValueType && cellValue.Equals(Activator.CreateInstance(cellType)))) //is the default value of a value type
                    {
                        CurrentCell = cell;
                        break;
                    }
                }
            }
            catch
            { }
        }

        public Tree UserAddTree()
        {
            if (_viewLoading) { return null; }
            EndEdit();
            var newTree = DataEntryController.Unit.UserAddTree();
            if (newTree != null)
            {
                this.MoveLastTree();
                this.MoveHomeField();
            }
            return newTree;
        }

        public void NotifyEnter()
        {

        }

        #endregion ITreeView Members

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
                if (_BS_trees != null)
                {
                    _BS_trees.Dispose();
                    _BS_trees = null;
                }

                try
                {
                    ApplicationSettings.Instance.CruisersChanged -= Settings_CruisersChanged;
                }
                catch { }
            }
            base.Dispose(disposing);
        }
    }
}