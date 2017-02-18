﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using CruiseDAL;
using CruiseDAL.DataObjects;
using FMSC.Sampling;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;
using FSCruiser.WinForms.DataEntry;
using FScruiser.Core.Services;

namespace FSCruiser.Core.DataEntry
{
    public class FormDataEntryLogic
    {
        private Dictionary<char, int> _stratumHotKeyLookup;

        public IApplicationController Controller { get; set; }

        public IDataEntryView View { get; set; }

        public CuttingUnit Unit { get { return _dataService.CuttingUnit; } }

        public DAL Database { get { return this.Unit.DAL; } }

        public IViewController ViewController { get { return this.Controller.ViewController; } }

        LoadCuttingUnitWorker _loadUnitWorker;

        public bool HotKeyenabled { get; set; }

        IDataEntryDataService DataService { get { return _dataService; } }

        public Dictionary<char, int> StratumHotKeyLookup
        {
            get
            {
                if (_stratumHotKeyLookup == null)
                {
                    _stratumHotKeyLookup = new Dictionary<char, int>();
                }
                return _stratumHotKeyLookup;
            }
        }

        IDataEntryDataService _dataService;
        IDialogService _dialogService;
        ISoundService _soundService;

        public FormDataEntryLogic(IApplicationController controller
            , IDialogService dialogService
            , ISoundService soundService
            , IDataEntryDataService dataService
            , IDataEntryView view)
        {
            this.Controller = controller;
            this.View = view;

            _dialogService = dialogService;
            _soundService = soundService;
            _dataService = dataService;
        }

        public string GetViewTitle()
        {
            return "Unit: " + this.Unit.Code + ", " + this.Unit.Description;
        }

        public void ShowLogs(Tree tree)
        {
            if (tree.TrySave())
            {
                this.ViewController.ShowLogsView(tree.Stratum, tree);
            }
            else
            {
                _dialogService.ShowMessage("Unable to save tree. Ensure Tree Number, Sample Group and Stratum are valid"
                    , null);
            }
        }

        public void OnTally(CountTree count)
        {
            TallyAction action = null;
            SampleGroupDO sg = count.SampleGroup;

            //if doing a manual tally create a tree and jump out
            if (sg.SampleSelectorType == CruiseDAL.Schema.CruiseMethods.CLICKER_SAMPLER_TYPE)
            {
                action = new TallyAction(count);
                var newTree = DataService.CreateNewTreeEntry(count, true); //create measure tree
                newTree.TreeCount = sg.SamplingFrequency;     //increment tree count on tally
                action.TreeRecord = newTree;
            }
            else if (count.SampleGroup.Stratum.Is3P)//threeP sampling
            {
                action = TallyThreeP(count);
            }
            else//non 3P sampling (STR)
            {
                action = TallyStandard(count);
            }

            if (action != null)
            {
                _soundService.SignalTally();
                var tree = action.TreeRecord;
                if (tree != null)
                {
                    if (tree.CountOrMeasure == "M")
                    {
                        _soundService.SignalMeasureTree(false);
                    }
                    else if (tree.CountOrMeasure == "I")
                    {
                        _soundService.SignalInsuranceTree();
                    }

                    _dialogService.AskCruiser(tree);
                    if (!ApplicationSettings.Instance.EnableCruiserPopup)
                    {
                        var sampleType = (tree.CountOrMeasure == "M") ? "Measure Tree" :
                    (tree.CountOrMeasure == "I") ? "Insurance Tree" : String.Empty;
                        _dialogService.ShowMessage("Tree #" + tree.TreeNumber.ToString(), sampleType);
                    }

                    tree.TrySave();
                    DataService.AddNonPlotTree(tree);

                    if (tree.CountOrMeasure == "M" && AskEnterMeasureTreeData())
                    {
                        this.View.GotoTreePage();
                        //this.View.TreeViewMoveLast();
                    }
                }
                Unit.TallyHistoryBuffer.Add(action);
            }
        }

        protected TallyAction TallyThreeP(CountTree count)
        {
            TallyAction action = new TallyAction(count);
            var sampler = count.SampleGroup.Sampler;

            int kpi = 0;
            int? value = ViewController.AskKPI((int)count.SampleGroup.MinKPI, (int)count.SampleGroup.MaxKPI);
            if (value == null)
            {
                return null;
            }
            else
            {
                kpi = value.Value;
            }

            Tree tree = null;
            if (kpi == -1)  //user enterted sure to measure
            {
                tree = DataService.CreateNewTreeEntry(count);
                tree.STM = "Y";
            }
            else
            {
                action.TreeEstimate = count.LogTreeEstimate(kpi);
                action.KPI = kpi;
                count.SumKPI += kpi;

                ThreePItem item = (ThreePItem)((ThreePSelecter)sampler).NextItem();
                if (item != null && kpi > item.KPI)
                {
                    if (sampler.IsSelectingITrees)
                    {
                        item.IsInsuranceItem = sampler.InsuranceCounter.Next();
                    }
                    tree = DataService.CreateNewTreeEntry(count);
                    tree.KPI = kpi;
                    tree.CountOrMeasure = (item.IsInsuranceItem) ? "I" : "M";
                }
            }

            action.TreeRecord = tree;
            count.TreeCount++;

            return action;
        }

        protected TallyAction TallyStandard(CountTree count)
        {
            TallyAction action = new TallyAction(count);

            boolItem item = (boolItem)count.SampleGroup.Sampler.NextItem();
            //If we receive nothing from the sampler, we don't have a sample
            if (item != null)//&& (item.IsSelected || item.IsInsuranceItem))
            {
                Tree tree = DataService.CreateNewTreeEntry(count);
                tree.CountOrMeasure = (item.IsInsuranceItem) ? "I" : "M";
                action.TreeRecord = tree;
            }

            count.TreeCount++;

            return action;
        }

        protected bool AskEnterMeasureTreeData()
        {
            if (!ApplicationSettings.Instance.EnableAskEnterTreeData) { return false; }

            return _dialogService.AskYesNo("Would you like to enter tree data now?", "Sample", false);
        }

        public void RegisterStratumHotKey(string hk, int pageIndex)
        {
            if (string.IsNullOrEmpty(hk)) { return; }
            char stratumHotKey = char.ToUpper(hk[0]);
            this.StratumHotKeyLookup.Add(stratumHotKey, pageIndex);
        }

        public bool HandleKeyPress(KeyEventArgs ea)
        {
            var view = this.View.FocusedLayout;

            if (view != null)
            {
                if (view.PreviewKeypress(ea))
                {
                    return true;
                }
                else
                {
                    var key = PlatformHelper.KeyToChar(ea.KeyData);
                    if (key == char.MinValue) { return false; }
                    key = char.ToUpper(key);
                    if (!IsHotkeyKey(key)) { return false; }

                    var tallyView = view as ITallyView;
                    if (tallyView != null)
                    {
                        if (tallyView.HotKeyEnabled == false) { return false; }

                        if (tallyView.HotKeyLookup != null && tallyView.HotKeyLookup.ContainsKey(key))//maybe a tally hotkey
                        {
                            CountTree count = tallyView.HotKeyLookup[key];
                            tallyView.OnTally(count);
                            return true;
                        }

                        //if valid stratm hot key, go to view that stratum belongs to
                        if (this.StratumHotKeyLookup.ContainsKey(key))
                        {
                            this.View.GoToPageIndex(this.StratumHotKeyLookup[key]);
                            return true;
                        }
                        else//not valid hotkey, get grumpy
                        {
                            _soundService.SignalInvalidAction();
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        bool IsHotkeyKey(char c)
        {
            return Array.IndexOf(Constants.HOTKEY_KEYS, c) != -1;
        }

        public void HandleKPIChanging(Tree tree, float newKPI, bool doSample, out bool cancel)
        {
            if (tree == null)
            {
                cancel = true;
                return;
            }
            if (tree.SampleGroup == null)
            {
                MessageBox.Show("Select Sample Group before entering KPI");
                cancel = true;
                return;
            }
            if (!tree.KPI.EqualsEx(0.0F))
            {
                string message = string.Format("Tree RecID:{0} KPI changed from {1} to {2}", tree.Tree_CN, tree.KPI, newKPI);
                this.Database.LogMessage(message, "I");
            }
            else if (doSample)
            {
                CountTree count = tree.FindCountRecord();
                if (count != null && count.SampleGroup.Sampler is ThreePSelecter)
                {
                    ThreePItem item = (ThreePItem)count.SampleGroup.Sampler.NextItem();
                    if (item.KPI < newKPI)
                    {
                        tree.CountOrMeasure = "M";
                        _soundService.SignalMeasureTree(true);
                    }
                    else
                    {
                        tree.CountOrMeasure = "C";
                    }
                    //count.SumKPI += (long)newKPI;
                }
            }
            cancel = false;
        }

        public bool HandleSpeciesChanged(Tree tree, TreeDefaultValueDO tdv)
        {
            if (tree == null) { return true; }
            //if (tree.TreeDefaultValue == tdv) { return true; }
            tree.SetTreeTDV(tdv);
            return tree.TrySave();
        }

        public void HandleViewLoading()
        {
            //TODO check to see if strata are loaded for this unit, otherwise display error message
            //Controller.AsyncLoadCuttingUnitData();

            this._loadUnitWorker = new LoadCuttingUnitWorker(this.Unit);
            this._loadUnitWorker.DoneLoading += new EventHandler(_loadUnitWorker_DoneLoading);
            this._loadUnitWorker.AsyncLoadCuttingUnitData();
        }

        void _loadUnitWorker_DoneLoading(object sender, EventArgs e)
        {
            this.View.HandleCuttingUnitDataLoaded();
        }

        public void HandleViewClosing(CancelEventArgs e)
        {
            ViewController.ShowWait();

            try
            {
                //Go through all the tree views and validate
                //if a tree view has invalid trees lets ask the user if they want to continue
                int viewIndex;
                if (!this.ValidateTreeViews(out viewIndex)
                    && _dialogService.AskYesNo("Error(s) found on tree records. Would you like to continue", "Continue?", true) == false)
                {
                    e.Cancel = true;
                    this.View.GoToPageIndex(viewIndex);
                    return;
                }

                //save all the plot views, this will save all trees and plots in them
                foreach (var view in View.Layouts.OfType<IPlotLayout>())
                {
                    view.ViewLogicController.Save();
                }

                if (!DataService.TrySaveCounts())
                {
                    e.Cancel = true;
                    _dialogService.ShowMessage("Something went wrong while saving the tally count for this unit", null);
                }

                if (!DataService.SaveFieldData())
                {
                    e.Cancel = true;
                    _dialogService.ShowMessage("Something went wrong saving the data for this unit, check trees for errors and try again", null);
                }
            }
            finally
            {
                ViewController.HideWait();
            }

            this.Controller.OnLeavingCurrentUnit(e);
        }

        /// <summary>
        /// performs validation on each tree view and returns ture if validation passes
        /// </summary>
        /// <param name="invalidViewIndex">index to first invalid view</param>
        /// <returns></returns>
        protected bool ValidateTreeViews(out int invalidViewIndex)
        {
            bool validationPass = true;
            invalidViewIndex = -1;
            foreach (var view in View.Layouts.OfType<ITreeView>())
            {
                if (view.Trees != null)
                {
                    view.EndEdit();
                    var worker = new TreeValidationWorker(view.Trees);
                    if (!worker.ValidateTrees())
                    {
                        if (invalidViewIndex != -1)
                        {
                            invalidViewIndex = View.Layouts.IndexOf(view);
                        }
                        validationPass = false;
                    }
                }
            }

            return validationPass;
        }
    }//end class deff
}