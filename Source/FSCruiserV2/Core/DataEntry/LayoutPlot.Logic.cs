﻿using FMSC.Sampling;
using FScruiser.Core.Services;
using FScruiser.Services;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;
using FSCruiser.WinForms.DataEntry;
using System;
using System.Windows.Forms;

namespace FSCruiser.Core.DataEntry
{
    public class LayoutPlotLogic
    {
        private IDialogService _dialogService;
        private ISoundService _soundService;
        private ApplicationSettings _appSettings;

        private bool _disableCheckPlot = false;
        private Plot _prevPlot;
        private BindingSource _BS_Plots;
        private BindingSource _BS_Trees;

        public IPlotLayout View { get; set; }

        public IPlotDataService DataService { get; protected set; }

        public IViewController ViewController { get; protected set; }

        public ISampleSelectorRepository SampleSelectorRepository { get; protected set; }

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
            , IPlotDataService dataService
            , ISoundService soundService
            , IDialogService dialogService
            , ApplicationSettings settings
            , IViewController viewController
            , ISampleSelectorRepository sampleSelectorRepository)
        {
            SampleSelectorRepository = sampleSelectorRepository;
            Stratum = stratum;
            View = view;
            DataService = dataService;
            _soundService = soundService;
            _dialogService = dialogService;
            _appSettings = settings;
            ViewController = viewController;

            _BS_Plots = new BindingSource();
            _BS_Trees = new BindingSource();
            ((System.ComponentModel.ISupportInitialize)(_BS_Plots)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(_BS_Trees)).BeginInit();

            _BS_Plots.DataSource = typeof(Plot);
            _BS_Plots.CurrentChanged += new System.EventHandler(_BS_Plots_CurrentChanged);

            _BS_Trees.DataSource = typeof(Tree);
            _BS_Trees.CurrentChanged += new System.EventHandler(_BS_Trees_CurrentChanged);

            ((System.ComponentModel.ISupportInitialize)(_BS_Plots)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(_BS_Trees)).EndInit();

            _BS_Plots.DataSource = Stratum.Plots;
        }

        //validate plot before switching plots
        protected bool ValidatePlot(Plot plot)
        {
            if (plot != null)
            {
                EndEdit();
                if (!(plot is Plot3PPNT)
                    && !plot.IsNull
                    && (plot.Trees != null && plot.Trees.Count == 0)
                    && _dialogService.AskYesNo("Plot contains no tree records. Make it a NULL plot?","Mark Plot Empty?"))
                {
                    plot.IsNull = true;
                    DataService.DataStore.Save(plot);
                    _BS_Plots.ResetItem(_BS_Plots.IndexOf(plot));//call reset item to cause the binding source to update view
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
            View.BindPlotData(_BS_Plots);
            UpdateCurrentPlot();
            View.BindTreeData(_BS_Trees);
        }

        public void EndEdit()
        {
            View.ViewEndEdit();   //HACK force datagrid to end edits to cause data validation to occur
            _BS_Plots.EndEdit();
            _BS_Trees.EndEdit();
        }

        public bool EnsureCurrentPlotWorkable()
        {
            return EnsurePlotSelected() && EnsureCurrentPlotNotEmpty();
        }

        protected bool EnsurePlotSelected()
        {
            if (View.ViewLoading) { return false; }
            if (CurrentPlot == null)
            {
                View.ShowNoPlotSelectedMessage();
                return false;
            }
            return true;
        }

        protected bool EnsureCurrentPlotNotEmpty()
        {
            if (CurrentPlot.IsNull)
            {
                View.ShowNullPlotMessage();
                return false;
            }
            return true;
        }

        protected bool SavePlotTrees(Plot plot)
        {
            EndEdit();

            return DataService.TrySaveTrees(plot)
                || _dialogService.AskYesNo("Can not save all trees in the plot, Would you like to continue?"
                    , "Continue?", true);
        }

        public bool SavePlotTrees()
        {
            if (this.CurrentPlot == null) { return false; }
            return SavePlotTrees(this.CurrentPlot);
        }

        public void UpdateCurrentPlot()
        {
            if (View.ViewLoading) { return; }
            this.EndEdit();
            _BS_Plots.ResetCurrentItem();

            var currentPlot = CurrentPlot;
            if (currentPlot != null)
            {
                _BS_Trees.DataSource = currentPlot.Trees;
            }
            else
            {
                this._BS_Trees.DataSource = new Tree[0];
            }
            this.View.RefreshTreeView(currentPlot);
        }

        public void HandleAddPlot()
        {
            var currentPlot = CurrentPlot;
            if (currentPlot != null
                && (!SavePlotTrees(currentPlot) || !ValidatePlot(currentPlot)))
            {
                return;
            }

            Plot plotInfo = AddPlot();
            if (plotInfo != null)
            {
                _disableCheckPlot = true;
                try
                {
                    _BS_Plots.ResetBindings(false);
                    _BS_Plots.MoveLast();
                }
                finally
                {
                    _disableCheckPlot = false;
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

        public void OnTally(CountTree count)
        {
            if (!this.EnsureCurrentPlotWorkable()) { return; }

            OnTally(count, this.CurrentPlot);
        }

        protected void OnTally(CountTree count, Plot plot)
        {
            if (plot == null) { throw new ArgumentNullException("plot"); }
            if (count == null) { throw new ArgumentNullException("count"); }

            bool isSingleStage = Stratum.IsSingleStage;//i.e. FIX or PNT
            Tree tree = null;

            var sg = count.SampleGroup;
            //if ((sg.TallyMethod & CruiseDAL.Enums.TallyMode.Manual) == CruiseDAL.Enums.TallyMode.Manual)
            //{
            //    tree = plot.CreateNewTreeEntry(count, true);
            //    tree.TreeCount = sg.SamplingFrequency;
            //}
            //else
            if (isSingleStage)
            {
                //FIX and PNT should always generate a measure tree
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
                tree.TrySave();
                plot.AddTree(tree);

                //single stage trees are always measure so no need to remind the user.
                if (!isSingleStage)
                {
                    if (tree.CountOrMeasure == "M"
                        || tree.CountOrMeasure == "I")
                    {
                        if (tree.CountOrMeasure == "M")
                        {
                            _soundService.SignalMeasureTree();
                            _dialogService.ShowMessage("Measure Tree# " + tree.TreeNumber);
                        }
                        else if (tree.CountOrMeasure == "I")
                        {
                            _soundService.SignalInsuranceTree();
                        }
                    }
                }

                SelectLastTree();
            }
        }

        protected Tree TallyThreeP(Plot plot, CountTree count)
        {
            Tree tree;
            var sg = count.SampleGroup;
            var sgCode = sg.Code;
            var stCode = sg.Stratum.Code;
            var spCode = count.TreeDefaultValue.Species;
            var sampler = SampleSelectorRepository.GetSamplerBySampleGroupCode(stCode, sgCode);

            int kpi = 0;
            int? value = _dialogService.AskKPI((int)sg.MinKPI, (int)sg.MaxKPI, stCode, sgCode, spCode);
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
                var result = ((IThreePSelector)sampler).Sample(kpi);

                tree = DataService.CreateNewTreeEntry(plot, count, (result != SampleResult.C));
                tree.CountOrMeasure = result.ToString();
                tree.KPI = kpi;
            }
            else//tree is sure to measure
            {
                tree = DataService.CreateNewTreeEntry(plot, count, true);
                tree.STM = "Y";
            }

            tree.TreeCount = 1;

            return tree;
        }

        protected Tree TallyStandard(Plot plot, CountTree count)
        {
            var sg = count.SampleGroup;
            var sgCode = sg.Code;
            var stCode = sg.Stratum.Code;
            var sampler = SampleSelectorRepository.GetSamplerBySampleGroupCode(stCode, sgCode);

            Tree tree;
            var freqSampler = sampler as IFrequencyBasedSelecter;
            // sampler maybe null, if it is treat as zero frequency sampler
            var result = (freqSampler != null) ? freqSampler.Sample() : SampleResult.C;
            tree = DataService.CreateNewTreeEntry(plot, count, (result != SampleResult.C));
            tree.CountOrMeasure = result.ToString();
            tree.TreeCount = 1;

            return tree;
        }

        public void AddTree(SubPop subPop)
        {
            Tree tree = DataService.CreateNewTreeEntry(CurrentPlot, subPop);

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

        #region event handlers

        private void _BS_Plots_CurrentChanged(object sender, EventArgs e)
        {
            if (!_disableCheckPlot && _prevPlot != null && _prevPlot != CurrentPlot)
            {
                if (!this.ValidatePlot(_prevPlot) || !this.SavePlotTrees(_prevPlot))
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