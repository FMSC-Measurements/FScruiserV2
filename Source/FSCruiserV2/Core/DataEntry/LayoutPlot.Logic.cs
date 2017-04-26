using System;
using System.Windows.Forms;
using CruiseDAL.Schema;
using FMSC.Sampling;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;
using FSCruiser.WinForms.DataEntry;
using FScruiser.Core.Services;

namespace FSCruiser.Core.DataEntry
{
    public class LayoutPlotLogic
    {
        IDialogService _dialogService;
        ISoundService _soundService;
        ApplicationSettings _appSettings;

        private bool _disableCheckPlot = false;
        private Plot _prevPlot;
        private System.Windows.Forms.BindingSource _BS_Plots;
        public System.Windows.Forms.BindingSource _BS_Trees;

        public IPlotLayout View { get; set; }

        public IDataEntryDataService DataService { get; protected set; }

        public IViewController ViewController { get; protected set; }

        public bool UserCanAddTrees { get; set; }

        public bool Is3PPNT
        {
            get
            {
                return this.Stratum.Method == CruiseDAL.Schema.CruiseMethods.THREEPPNT;
            }
        }

        public PlotStratum Stratum { get; set; }

        public Plot CurrentPlot
        {
            get
            {
                return _BS_Plots.Current as Plot;
            }
            set
            {
                int i = _BS_Plots.IndexOf(value);
                _BS_Plots.Position = i;
            }
        }

        public Tree CurrentTree
        {
            get
            {
                return this._BS_Trees.Current as Tree;
            }
        }

        public LayoutPlotLogic(PlotStratum stratum
            , LayoutPlot view
            , IDataEntryDataService dataService
            , ISoundService soundService
            , IDialogService dialogService
            , ApplicationSettings settings
            , IViewController viewController)
        {
            this.Stratum = stratum;
            this.View = view;
            DataService = dataService;
            _soundService = soundService;
            _dialogService = dialogService;
            _appSettings = settings;
            this.ViewController = viewController;

            this._BS_Plots = new BindingSource();
            this._BS_Trees = new BindingSource();
            ((System.ComponentModel.ISupportInitialize)(this._BS_Plots)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._BS_Trees)).BeginInit();

            this._BS_Plots.DataSource = typeof(Plot);
            this._BS_Plots.CurrentChanged += new System.EventHandler(this._BS_Plots_CurrentChanged);

            this._BS_Trees.DataSource = typeof(Tree);
            this._BS_Trees.CurrentChanged += new System.EventHandler(this._BS_Trees_CurrentChanged);

            ((System.ComponentModel.ISupportInitialize)(this._BS_Plots)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._BS_Trees)).EndInit();

            _BS_Plots.DataSource = this.Stratum.Plots;
        }

        public bool CheckCurrentPlot()
        {
            return this.CheckPlot(this.CurrentPlot);
        }

        public bool CheckPlot(Plot plot)
        {
            if (plot != null)
            {
                this.EndEdit();
                if (!(plot is Plot3PPNT)
                    && !plot.IsNull
                    && (plot.Trees != null && plot.Trees.Count == 0))
                {
                    plot.IsNull |= _dialogService.AskYesNo("Plot contains no tree records. Make it a NULL plot?",
                        "Mark Plot Empty?");
                    _BS_Plots.ResetItem(_BS_Plots.IndexOf(plot));
                }

                string error;
                if (!plot.ValidatePlot(out error))
                {
                    return _dialogService.AskYesNo(error
                        , "Continue?", true);
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
            this.View.ViewEndEdit();   //HACK force datagrid to end edits to cause data validation to occur
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

        public bool SavePlotTrees(Plot plot)
        {
            bool goOn = true;
            this.EndEdit();
            try
            {
                DataService.SaveTrees(plot);
            }
            catch (FMSC.ORM.SQLException)
            {
                if (!_dialogService.AskYesNo("Can not save all trees in the plot, Would you like to continue?"
                    , "Continue?", true))
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
            _BS_Plots.ResetCurrentItem();
            if (CurrentPlot != null)
            {
                CurrentPlot.PopulateTrees();
                this._BS_Trees.DataSource = CurrentPlot.Trees;
            }
            else
            {
                this._BS_Trees.DataSource = new Tree[0];
            }
            this.View.RefreshTreeView(this.CurrentPlot);
        }

        public void HandleAddPlot()
        {
            if (this.CurrentPlot != null
                && (!this.SavePlotTrees(this.CurrentPlot) || !this.CheckCurrentPlot()))
            {
                return;
            }

            Plot plotInfo = this.AddPlot();
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

        /// <summary>
        /// Creates a new plot using the plot info view and adds it to the given stratum's plot collection
        /// </summary>
        /// <returns>reference to newly created plot</returns>
        protected Plot AddPlot()
        {
            var newPlot = Stratum.MakePlot(DataService.CuttingUnit);

            if (View.ShowPlotInfo(DataService, newPlot, Stratum, true))
            {
                newPlot.Save();
                this.Stratum.Plots.Add(newPlot);

                if (newPlot.IsNull)
                {
                    return this.AddPlot() ?? newPlot;//add plot may return null, in that case return most recently created plot
                }
                else if (Stratum.Is3PPNT && newPlot.Trees.Count == 0)
                {
                    return this.AddPlot() ?? newPlot;//add plot may return null, in that case return most recently created plot
                }
                return newPlot;
            }
            return null;
        }

        public void HandleDeletePlot()
        {
            if (CurrentPlot == null)
            {
                this.View.ShowNoPlotSelectedMessage();
                return;
            }

            if (_dialogService.AskYesNo("Are you sure you want to delete this plot?"
                , String.Empty, true))
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

            Tree curTree = this._BS_Trees.Current as Tree;
            if (curTree != null)
            {
                if (!_dialogService.AskCancel("Delete Tree #" + curTree.TreeNumber.ToString(),
                    "Delete Tree", true))
                {
                    //this._BS_Trees.Remove(curTree);
                    //Controller.DeleteTree(curTree);
                    this.CurrentPlot.DeleteTree(curTree);
                }
            }
            else
            {
                _soundService.SignalInvalidAction();
            }
        }

        //public void HandleTreeNumberChanging(long newTreeNumber, out bool cancel)
        //{
        //    try
        //    {
        //        if (!this.CurrentPlot.IsTreeNumberAvalible(newTreeNumber))
        //        {
        //            cancel = true;
        //            return;
        //        }
        //    }
        //    catch
        //    {
        //        cancel = true;
        //        return;
        //    }
        //    cancel = false;
        //}

        public void OnTally(CountTree count)
        {
            if (!this.EnsureCurrentPlotWorkable()) { return; }

            this.OnTally(count, this.CurrentPlot);
        }

        protected void OnTally(CountTree count, Plot plot)
        {
            if (plot == null) { throw new ArgumentNullException("plot"); }
            if (count == null) { throw new ArgumentNullException("count"); }
            Tree tree = null;

            var sg = count.SampleGroup;
            //if ((sg.TallyMethod & CruiseDAL.Enums.TallyMode.Manual) == CruiseDAL.Enums.TallyMode.Manual)
            //{
            //    tree = plot.CreateNewTreeEntry(count, true);
            //    tree.TreeCount = sg.SamplingFrequency;
            //}
            //else
            if (Stratum.Method == CruiseMethods.FIX
            || Stratum.Method == CruiseMethods.PNT)
            {
                tree = DataService.CreateNewTreeEntry(plot, count, true);
                tree.TreeCount = 1;
            }
            else if (sg.Stratum.Is3P)
            {
                tree = TallyThreeP(plot, count);
            }
            else
            {
                tree = TallyStandard(plot, count);
            }

            _soundService.SignalTally();

            //tree may be null if user didn't enter kpi
            if (tree != null)
            {
                if (tree.CountOrMeasure == "M"
                    || tree.CountOrMeasure == "I")
                {
                    if (tree.CountOrMeasure == "M")
                    {
                        _soundService.SignalMeasureTree();
                    }
                    else if (tree.CountOrMeasure == "I")
                    {
                        _soundService.SignalInsuranceTree();
                    }

                    //if (_appSettings.EnableCruiserPopup)
                    //{
                    //    _dialogService.AskCruiser(tree);
                    //}
                    //else
                    //{
                    //    var sampleType = (tree.CountOrMeasure == "M") ? "Measure Tree" :
                    //             (tree.CountOrMeasure == "I") ? "Insurance Tree" : String.Empty;
                    //    _dialogService.ShowMessage("Tree #" + tree.TreeNumber.ToString(), sampleType);
                    //}
                }

                tree.TrySave();
                plot.AddTree(tree);
                SelectLastTree();
            }
        }

        protected Tree TallyThreeP(Plot plot, CountTree count)
        {
            Tree tree;
            var sg = count.SampleGroup;
            var sampler = sg.Sampler;

            int kpi = 0;
            int? value = ViewController.AskKPI((int)sg.MinKPI, (int)sg.MaxKPI);
            if (value == null)
            {
                return null; //user didn't enter valid value
            }
            else
            {
                kpi = value.Value;
            }

            //if kpi == -1 then tree is sure to measure
            if (kpi != -1)
            {
                ThreePItem item = (ThreePItem)((ThreePSelecter)sampler).NextItem();

                if (item != null && kpi > item.KPI)
                {
                    //because the three p sample selector doesn't select insurance trees for us
                    //we need to select them our selves
                    if (sampler.IsSelectingITrees)
                    {
                        item.IsInsuranceItem = sampler.InsuranceCounter.Next();
                    }
                    if (item.IsInsuranceItem)
                    {
                        _soundService.SignalInsuranceTree();
                        tree = DataService.CreateNewTreeEntry(plot, count, true);
                        tree.CountOrMeasure = "I";
                    }
                    else
                    {
                        _soundService.SignalMeasureTree();
                        _dialogService.ShowMessage("Measure Tree");
                        tree = DataService.CreateNewTreeEntry(plot, count, true);
                        //tree.CountOrMeasure = "M";
                    }
                }
                else
                {
                    tree = DataService.CreateNewTreeEntry(plot, count, true);
                    //tree.CountOrMeasure = "C";
                }
                tree.KPI = kpi;
            }
            else
            {
                tree = DataService.CreateNewTreeEntry(plot, count, true);
                tree.STM = "Y";
            }

            tree.TreeCount = 1;

            return tree;
        }

        protected Tree TallyStandard(Plot plot, CountTree count)
        {
            var sampler = count.SampleGroup.Sampler;
            Tree tree;

            boolItem item = (sampler != null) ? (boolItem)sampler.NextItem() : (boolItem)null;
            if (item != null && !item.IsInsuranceItem)
            {
                tree = DataService.CreateNewTreeEntry(plot, count, true);
                //tree.CountOrMeasure = "M";
            }
            else if (item != null && item.IsInsuranceItem)
            {
                tree = DataService.CreateNewTreeEntry(plot, count, true);
                tree.CountOrMeasure = "I";
            }
            else
            {
                tree = DataService.CreateNewTreeEntry(plot, count, false);
            }

            tree.TreeCount = 1;

            return tree;
        }

        //TODO rename method
        public void AddTree(SubPop subPop)
        {
            Tree tree = DataService.CreateNewTreeEntry(CurrentPlot, subPop);

            _dialogService.AskCruiser(tree);

            tree.TrySave();
            CurrentPlot.AddTree(tree);

            SelectLastTree();
        }

        public Tree UserAddTree()
        {
            if (!this.EnsureCurrentPlotWorkable()) { return null; }
            if (!this.UserCanAddTrees) { return null; }
            EndEdit();
            Tree prevTree = null;
            if (_BS_Trees.Count > 0)
            {
                prevTree = (Tree)_BS_Trees[_BS_Trees.Count - 1];
            }

            var newTree = DataService.UserAddTree(CurrentPlot, prevTree);

            if (newTree != null)
            {
                this.SelectLastTree();
            }

            return newTree;
        }

        public bool ResequenceTreeNumbers()
        {
            if (EnsureCurrentPlotWorkable()
                && DialogService.AskYesNo("This will renumber all trees in the plot starting at 1"
                    , "Continue?"
                    , false))
            {
                CurrentPlot.ResequenceTreeNumbers();
                return true;
            }
            else
            {
                return false;
            }
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

        public void Save()
        {
            foreach (Plot p in Stratum.Plots)
            {
                p.Save();

                try
                {
                    DataService.TrySaveTrees(p);
                }
                catch (FMSC.ORM.SQLException e)
                {
                    _dialogService.ShowMessage(e.Message
                        , "Stratum " + this.Stratum.Code + "Plot " + p.PlotNumber.ToString());
                }
            }
        }

        public void SaveCounts()
        {
            this.Stratum.SaveCounts();
        }

        public bool TrySaveCounts()
        {
            return this.Stratum.TrySaveCounts();
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
            Tree tree = this.CurrentTree;

            this.View.HandleCurrentTreeChanged(tree);
        }

        #endregion event handlers
    }
}