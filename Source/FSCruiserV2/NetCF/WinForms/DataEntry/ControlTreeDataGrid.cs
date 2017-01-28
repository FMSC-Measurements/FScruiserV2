﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FMSC.Controls;
using FSCruiser.Core;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;
using Microsoft.WindowsCE.Forms;

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
        private DataGridTextBoxColumn _errorsColumn;
        private System.Windows.Forms.BindingSource _BS_trees;

        private IApplicationController Controller { get; set; }

        private bool _userCanAddTrees;

        public bool UserCanAddTrees
        {
            get { return _userCanAddTrees; }
            set { _userCanAddTrees = value; }
        }
        public FormDataEntryLogic DataEntryController { get; set; }

        public IList<Tree> Trees
        {
            get
            {
                return this.DataEntryController.Unit.NonPlotTrees;
            }
        }

        public ControlTreeDataGrid(IApplicationController controller, FormDataEntryLogic dataEntryController, InputPanel sip)
        {
            this.Controller = controller;
            this.DataEntryController = dataEntryController;
            DataGridAdjuster.InitializeGrid(this);
            DataGridTableStyle tableStyle = DataEntryController.Unit.InitializeTreeColumns(this);

            ApplicationSettings.Instance.CruisersChanged += new EventHandler(Settings_CruisersChanged);

            this.AllowUserToAddRows = false;//don't allow down arrow to add tree
            this.SIP = sip;
            //this.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold);

            //initialize _BS_trees
            this._BS_trees = new System.Windows.Forms.BindingSource();
            ((System.ComponentModel.ISupportInitialize)(this._BS_trees)).BeginInit();
            this._BS_trees.DataSource = typeof(Tree);
            this._BS_trees.CurrentChanged += new EventHandler(_BS_trees_CurrentChanged);
            this.DataSource = this._BS_trees;
            ((System.ComponentModel.ISupportInitialize)(this._BS_trees)).EndInit();

            _speciesColumn = tableStyle.GridColumnStyles["TreeDefaultValue"] as EditableComboBoxColumn;
            _sgColumn = tableStyle.GridColumnStyles["SampleGroup"] as EditableComboBoxColumn;
            _stratumColumn = tableStyle.GridColumnStyles["Stratum"] as EditableComboBoxColumn;
            _treeNumberColumn = tableStyle.GridColumnStyles["TreeNumber"] as EditableTextBoxColumn;
            _initialsColoumn = tableStyle.GridColumnStyles["Initials"] as EditableComboBoxColumn;
            _logsColumn = tableStyle.GridColumnStyles["LogCountActual"] as DataGridButtonColumn;
            _kpiColumn = tableStyle.GridColumnStyles["KPI"] as EditableTextBoxColumn;
            _errorsColumn = tableStyle.GridColumnStyles["Errors"] as DataGridTextBoxColumn;

            if (_logsColumn != null)
            {
                _logsColumn.Click += this.LogsClicked;
            }

            if (_initialsColoumn != null)
            {
                _initialsColoumn.DataSource = ApplicationSettings.Instance.Cruisers.ToArray();
            }
        }

        

        protected override void OnCellValidating(EditableDataGridCellValidatingEventArgs e)
        {
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
                    && !this.DataEntryController.Unit.IsTreeNumberAvalible(newTreeNum))
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
            else if (e.Column == _kpiColumn)
            {
                bool cancel = false;

                this.DataEntryController.HandleKPIChanging(tree, (float)e.Value, false, out cancel);
                //this.HandleKPIChanging(currTree, (float)e.Value, out cancel);
            }
        }

        protected override void OnCellValueChanged(EditableDataGridCellEventArgs e)
        {
            base.OnCellValueChanged(e);

            Tree tree = (Tree)this._BS_trees[e.RowIndex];
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

                this.DataEntryController.HandleSpeciesChanged(tree, col.EditComboBox.SelectedItem as TreeDefaultValueDO);
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
            this.DataEntryController.ShowLogs(tree);
        }

        public Tree UserAddTree()
        {
            if (_viewLoading
                || this.UserCanAddTrees == false) { return null; }

            var newTree = DataEntryController.Unit.UserAddTree();

            if (newTree != null)
            {
                this._BS_trees.MoveLast();
                this.MoveFirstEmptyCell();
            }
            return newTree;
        }

        private void _BS_trees_CurrentChanged(object sender, EventArgs e)
        {
            Tree tree = _BS_trees.Current as Tree;

            //_changingTree = true;
            UpdateSpeciesColumn(tree);
            UpdateSampleGroupColumn(tree);

            //_changingTree = false;
        }

        public void UpdateSpeciesColumn(Tree tree)
        {
            if (_speciesColumn != null)
            {
                _speciesColumn.DataSource = tree.ReadValidTDVs();
            }
        }

        public void UpdateSampleGroupColumn(Tree tree)
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
                _stratumColumn.DataSource = DataEntryController.Unit.TreeStrata;
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
                try
                {
                    ApplicationSettings.Instance.CruisersChanged -= Settings_CruisersChanged;
                }
                catch (NullReferenceException) { }
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

        public void MoveLastTree()
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



        public void DeleteSelectedTree()
        {
            Tree curTree = this._BS_trees.Current as Tree;
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

        #endregion ITreeView Members

        #region IDataEntryPage

        public bool ViewLoading { get { return _viewLoading; } }

        public bool PreviewKeypress(KeyEventArgs ea)
        {
            if (ea.KeyData == Keys.None) { return false; }

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

        public void HandleEnableLogGradingChanged()
        {
            if (_logsColumn == null) { return; }
            if ((_logsColumn.Width > 0) == this.Controller.ViewController.EnableLogGrading) { return; }
            if (this.Controller.ViewController.EnableLogGrading)
            {
                _logsColumn.Width = Constants.LOG_COLUMN_WIDTH;
            }
            else
            {
                _logsColumn.Width = -1;
            }
        }

        void Settings_CruisersChanged(object sender, EventArgs e)
        {
            if (this._initialsColoumn != null)
            {
                this._initialsColoumn.DataSource = ApplicationSettings.Instance.Cruisers.ToArray();
            }
        }

        public void NotifyEnter()
        {
            
            MoveLastTree();
            MoveHomeField();
            Edit();
        }
        #endregion
    }
}