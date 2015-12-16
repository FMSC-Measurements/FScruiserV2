using System;

using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;
using FSCruiser.WinForms.DataEntry;

namespace FSCruiser.Core.DataEntry
{
    public class LayoutPlotLogic
    {
        private bool _disableCheckPlot = false; 
        private PlotVM _prevPlot;
        private System.Windows.Forms.BindingSource _BS_Plots;
        public System.Windows.Forms.BindingSource _BS_Trees;


        public IPlotLayout View { get; set; }
        public IApplicationController Controller { get { return this.DataEntryController.Controller; } }
        public FormDataEntryLogic DataEntryController { get; protected set; }

        public bool UserCanAddTrees { get; set; }

        public StratumVM StratumInfo { get; set; }

        public IList<PlotVM> Plots 
        {
            get
            {
                return (IList<PlotVM>)_BS_Plots.List;
            }
        }

        public PlotVM CurrentPlot
        {
            get
            {
                return _BS_Plots.Current as PlotVM;
            }
            set
            {
                int i = _BS_Plots.IndexOf(value);
                _BS_Plots.Position = i;
            }
        }

        public BindingList<TreeVM> CurrentPlotTreeList
        {
            get
            {
                PlotVM plotInfo = _BS_Plots.Current as PlotVM;
                if (plotInfo != null)
                {
                    return plotInfo.Trees;
                }
                return null;
            }
        }

        public TreeVM CurrentTree
        {
            get
            {
                return this._BS_Trees.Current as TreeVM;
            }
        }

        public LayoutPlotLogic(StratumVM stratum, LayoutPlot view, FormDataEntryLogic dataEntryController)
        {
            this.StratumInfo = stratum;
            this.View = view;
            this.DataEntryController = dataEntryController;


            this._BS_Plots = new BindingSource();
            this._BS_Trees = new BindingSource();
            ((System.ComponentModel.ISupportInitialize)(this._BS_Plots)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._BS_Trees)).BeginInit();

            this._BS_Plots.DataSource = typeof(PlotVM);
            this._BS_Plots.CurrentChanged += new System.EventHandler(this._BS_Plots_CurrentChanged);

            this._BS_Trees.DataSource = typeof(TreeVM);
            this._BS_Trees.CurrentChanged += new System.EventHandler(this._BS_Trees_CurrentChanged);

            ((System.ComponentModel.ISupportInitialize)(this._BS_Plots)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._BS_Trees)).EndInit();

            _BS_Plots.DataSource = this.StratumInfo.Plots;
        }

        public bool CheckCurrentPlot()
        {
            return this.CheckPlot(this.CurrentPlot);
        }

        public bool CheckPlot(PlotVM pInfo)
        {
            if (pInfo != null)
            {
                this.EndEdit();
                var worker = new TreeValidationWorker(pInfo.Trees);
                if (!worker.ValidateTrees())
                {
                    return this.View.AskContinueOnCurrnetPlotTreeError();
                }
            }
            return true;
        }

        public void HandleViewLoad()
        {
            
            this.View.BindPlotData(this._BS_Plots);
            this.UpdateCurrentPlot();
            this.View.BindTreeData(this._BS_Trees);
        }

        public void EndEdit()
        {
            this.View.ViewEndEdit();   //HACK force datagrid to end edits to cause data validation to occure  
            this._BS_Plots.EndEdit();
            this._BS_Trees.EndEdit();
        }

        public bool EnsureCurrentPlotWorkable()
        {
            return EnsurePlotSelected() && EnsureCurrentPlotNotEmpty();
        }

        public bool EnsurePlotSelected()
        {
            if (this.View.ViewLoading) { return false; }
            if (this.CurrentPlot == null)
            {
                this.View.ShowNoPlotSelectedMessage();
                return false;
            }
            return true;
        }

        public bool EnsureCurrentPlotNotEmpty()
        {
            if (this.CurrentPlot.IsNull)
            {
                this.View.ShowNullPlotMessage();
                return false;
            }
            return true;
        }

        public bool SavePlotTrees(PlotVM plot)
        {
            bool goOn = true;
            this.EndEdit();
            try
            {
                plot.SaveTrees();
            }
            catch (FMSC.ORM.SQLException)
            {
                if (!this.Controller.ViewController.AskYesNo("Can not save all trees in the plot, Would you like to continue?", "Continue?", MessageBoxIcon.Asterisk, true))
                {
                    goOn = false;
                }
            }
            return goOn;

        }

        public bool SavePlotTrees()
        {
            if (this.CurrentPlot == null) { return false; }
            return this.SavePlotTrees(this.CurrentPlot);
        }

        public void UpdateCurrentPlot()
        {
            if (View.ViewLoading) { return; }
            this.EndEdit();
            if (CurrentPlot != null)
            {
                CurrentPlot.PopulateTreeData();
                this._BS_Trees.DataSource = CurrentPlot.Trees;
            }
            else
            {
                this._BS_Trees.DataSource = new TreeVM[0];
            }
            this.View.RefreshTreeView(this.CurrentPlot);

        }


        public void HandleAddPlot()
        {
            if (this.CurrentPlot != null 
                &&(!this.SavePlotTrees(this.CurrentPlot) || !this.CheckCurrentPlot()))
            {
                return;
            }

            PlotVM plotInfo = this.DataEntryController.AddPlot(this.StratumInfo);
            if (plotInfo != null)
            {
                this._disableCheckPlot = true;
                try
                {
                    this._BS_Plots.ResetBindings(false);
                    this._BS_Plots.MoveLast();
 
                }
                finally
                {
                    this._disableCheckPlot = false;
                }
            }
        }

        public void HandleDeletePlot()
        {            
            if (CurrentPlot == null)
            {
                this.View.ShowNoPlotSelectedMessage();
                return;
            }

            if (this.Controller.ViewController.AskYesNo("Are you sure you want to delete this plot?", "", MessageBoxIcon.Question, true))
            {
                this._disableCheckPlot = true;
                CurrentPlot.Delete();
                _BS_Plots.Remove(CurrentPlot);
                this._disableCheckPlot = false; 
            }
        }

        public void HandleDeleteCurrentTree()
        {
            if (!this.EnsureCurrentPlotWorkable())
            {
                return;
            }

            TreeVM curTree = this._BS_Trees.Current as TreeVM;
            if (curTree != null)
            {

                if (!this.Controller.ViewController.AskCancel("Delete Tree #" + curTree.TreeNumber.ToString(), 
                    "Delete Tree", MessageBoxIcon.Question, true))
                {
                    //this._BS_Trees.Remove(curTree);
                    //Controller.DeleteTree(curTree);
                    this.CurrentPlot.DeleteTree(curTree);
                }
            }
            else
            {
                this.Controller.ViewController.SignalInvalidAction();
            }
        }

        public void HandleTreeNumberChanging(long newTreeNumber, out bool cancel)
        {
            try
            {

                if (!this.CurrentPlot.IsTreeNumberAvalible(newTreeNumber))
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

        public void OnTally(CountTreeVM count)
        {
            if (!this.EnsureCurrentPlotWorkable()) { return; }

            this.DataEntryController.OnTally(count, this.CurrentPlot, this.View);
            this.SelectLastTree();
        }

        //TODO rename method
        public void AddTree(SampleGroupVM sg, CruiseDAL.DataObjects.TreeDefaultValueDO tdv)
        {
            TreeVM tree;
            tree = this.CurrentPlot.CreateNewTreeEntry(sg, tdv, true);
            tree.TreeCount = 1;

            this.Controller.ViewController.ShowCruiserSelection(tree);

            tree.TrySave();
            this.CurrentPlot.AddTree(tree);

            this.SelectLastTree();
        }

        //protected TreeVM GetNewTree()
        //{

        //    if (!this.EnsureCurrentPlotWorkable())// if no plot is selected cancel action
        //    {
        //        return null;
        //    }

        //    if (this.UserCanAddTrees == false) { return null; }

        //    //adding trees can be allowed in some cases for 3PPNT
        //    //if (this.StratumInfo.Stratum.Method == "3PPNT")// if this is a 3PPNT stratum users aren't allowed to manualy enter trees
        //    //{
        //    //    return null;
        //    //}

        //    TreeVM prevTree = null;
        //    if (_BS_Trees.Count > 0)
        //    {
        //        prevTree = (TreeVM)_BS_Trees[_BS_Trees.Count - 1];
        //    }

        //    var newTree = this.CurrentPlot.UserAddTree(prevTree, this.DataEntryController.ViewController);
        //    //DataEntryController.Controller.OnTally();
        //    return newTree;

        //    //return Controller.UserAddTree(prevTree, this.StratumInfo, this.CurrentPlotInfo);
        //}

        public TreeVM UserAddTree()
        {
            if (!this.EnsureCurrentPlotWorkable()) { return null; }
            if (!this.UserCanAddTrees) { return null; }

            TreeVM prevTree = null;
            if (_BS_Trees.Count > 0)
            {
                prevTree = (TreeVM)_BS_Trees[_BS_Trees.Count - 1];
            }

            var newTree = this.CurrentPlot.UserAddTree(prevTree, this.DataEntryController.ViewController);

            if (newTree != null)
            {
                this.SelectLastTree();
            }

            return newTree;
        }

        public void SelectFirstPlot()
        {
            this._BS_Plots.MoveFirst();
        }

        public void SelectPreviousPlot()
        {
            this._BS_Plots.MovePrevious();
        }

        public void SelectNextPlot()
        {
            this._BS_Plots.MoveNext();
        }

        public void SelectLastPlot()
        {
            this._BS_Plots.MoveLast();
        }

        public void SelectLastTree()
        {
            this._BS_Trees.MoveLast();
            this.View.MoveHomeField();
        }


        public void ShowCurrentPlotInfo()
        {
            if (CurrentPlot == null)
            {
                this.View.ShowNoPlotSelectedMessage();
                return;
            }
            if (Controller.ViewController.ShowPlotInfo(CurrentPlot, false) == DialogResult.OK)
            {
                CurrentPlot.Save();
                this._BS_Plots.ResetCurrentItem();
                this.UpdateCurrentPlot();
            }
        }


        public void Save()
        {
            foreach (PlotVM p in this.StratumInfo.Plots)
            {
                p.Save();
            }
        }

        #region event handlers 
        private void _BS_Plots_CurrentChanged(object sender, EventArgs e)
        {
            if (!_disableCheckPlot && _prevPlot != null && _prevPlot != CurrentPlot)
            {
                if (!this.CheckPlot(_prevPlot) && !this.SavePlotTrees(_prevPlot))
                {
                    this.CurrentPlot = _prevPlot;
                }
            }
            this.UpdateCurrentPlot();
            _prevPlot = CurrentPlot;
        }

        private void _BS_Trees_CurrentChanged(object sender, EventArgs e)
        {
            TreeVM tree = this.CurrentTree;

            this.View.HandleCurrentTreeChanged(tree);
        }

        #endregion 
    }
}
