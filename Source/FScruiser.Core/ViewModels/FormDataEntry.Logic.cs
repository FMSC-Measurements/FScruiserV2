﻿using FMSC.Sampling;
using FScruiser.Core.Services;
using FScruiser.Sampling;
using FScruiser.Services;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace FSCruiser.Core.DataEntry
{
    public class FormDataEntryLogic
    {
        private Dictionary<char, IDataEntryPage> _stratumHotKeyLookup;

        public IApplicationController Controller { get; set; }

        public IDataEntryView View { get; set; }

        public IViewController ViewController { get { return this.Controller.ViewController; } }

        public bool HotKeyenabled { get; set; }

        private IDataEntryDataService DataService
        { get { return _dataService; } }

        public Dictionary<char, IDataEntryPage> StratumHotKeyLookup
        {
            get
            {
                return _stratumHotKeyLookup ?? (_stratumHotKeyLookup = new Dictionary<char, IDataEntryPage>());
            }
        }

        private IDataEntryDataService _dataService;
        private IDialogService _dialogService;
        private ISoundService _soundService;
        private IApplicationSettings _appSettings;
        private ISampleSelectorRepository _sampleSelectorRepo;

        public FormDataEntryLogic(IApplicationController controller,
            IDialogService dialogService,
            ISoundService soundService,
            IDataEntryDataService dataService,
            IApplicationSettings settings,
            IDataEntryView view,
            ISampleSelectorRepository sampleSelectorRepository)
        {
            this.Controller = controller;
            this.View = view;

            _dialogService = dialogService;
            _soundService = soundService;
            _dataService = dataService;
            _appSettings = settings;
            _sampleSelectorRepo = sampleSelectorRepository;
        }

        public string GetViewTitle()
        {
            return "Unit: " + DataService.CuttingUnit.Code + ", " + DataService.CuttingUnit.Description;
        }

        public void OnTally(CountTree count)
        {
            OnTally(count,
                DataService, DataService.TallyHistory,
                _appSettings, View,
                _dialogService, _soundService, _sampleSelectorRepo);

            //TallyAction action = null;
            //SampleGroupDO sg = count.SampleGroup;

            ////if doing a manual tally create a tree and jump out
            //if (sg.SampleSelectorType == CruiseDAL.Schema.CruiseMethods.CLICKER_SAMPLER_TYPE)
            //{
            //    action = new TallyAction(count);
            //    var newTree = DataService.CreateNewTreeEntry(count, true); //create measure tree
            //    newTree.TreeCount = sg.SamplingFrequency;     //increment tree count on tally
            //    action.TreeRecord = newTree;
            //}
            //else if (count.SampleGroup.Stratum.Is3P)//threeP sampling
            //{
            //    action = TallyThreeP(count);
            //}
            //else//non 3P sampling (STR)
            //{
            //    action = TallyStandard(count);
            //}

            ////action may be null if cruising 3P and user doesn't enter a kpi
            //if (action != null)
            //{
            //    try
            //    {
            //        count.Save();
            //        _soundService.SignalTally();
            //        var tree = action.TreeRecord;
            //        if (tree != null)
            //        {
            //            if (tree.CountOrMeasure == "M")
            //            {
            //                _soundService.SignalMeasureTree();
            //            }
            //            else if (tree.CountOrMeasure == "I")
            //            {
            //                _soundService.SignalInsuranceTree();
            //            }

            //            if (_appSettings.EnableCruiserPopup)
            //            {
            //                _dialogService.AskCruiser(tree);
            //            }
            //            else
            //            {
            //                var sampleType = (tree.CountOrMeasure == "M") ? "Measure Tree" :
            //                         (tree.CountOrMeasure == "I") ? "Insurance Tree" : String.Empty;
            //                _dialogService.ShowMessage("Tree #" + tree.TreeNumber.ToString(), sampleType);
            //            }

            //            tree.TrySave();
            //            DataService.AddNonPlotTree(tree);

            //            if (tree.CountOrMeasure == "M" && AskEnterMeasureTreeData())
            //            {
            //                this.View.GotoTreePage();
            //                //this.View.TreeViewMoveLast();
            //            }
            //        }
            //        DataService.CuttingUnit.TallyHistoryBuffer.Add(action);
            //    }
            //    catch(FMSC.ORM.SQLException e) //count save fail
            //    {
            //        _dialogService.ShowMessage("File error");
            //    }
            //}
        }

        public static void OnTally(CountTree count,
            IDataEntryDataService dataService, ICollection<TallyAction> tallyHistory,
            IApplicationSettings appSettings, IDataEntryView view,
            IDialogService dialogService, ISoundService soundService, 
            ISampleSelectorRepository sampleSelectorRepository)
        {
            TallyAction action = null;
            SampleGroup sg = count.SampleGroup;
            var sampler = sampleSelectorRepository.GetSamplerBySampleGroupCode(sg.Stratum.Code, sg.Code);

            
            if(sampler == null)
            {

            }
            // if doing a manual tally create a tree and jump out
            else if (sampler is ExternalSampleSelectorPlaceholder)
            {
                try
                {
                    action = new TallyAction(count);
                    var newTree = dataService.CreateNewTreeEntry(count, true); //create measure tree
                    newTree.TreeCount = sg.SamplingFrequency;     //increment tree count on tally
                    action.TreeRecord = newTree;
                }
                catch (FMSC.ORM.SQLException) //count save fail
                {
                    dialogService.ShowMessage("File error");
                }
            }
            else if (count.SampleGroup.Stratum.Is3P)//threeP sampling
            {
                action = TallyThreeP(count, sampler, sg, dataService, dialogService);
            }
            else//non 3P sampling (STR)
            {
                action = TallyStandard(count, sampler, dataService, dialogService);
            }

            //action may be null if cruising 3P and user doesn't enter a kpi
            if (action != null)
            {
                dataService.SaveTallyAction(action);
                soundService.SignalTally();

                var tree = action.TreeRecord;
                if (tree != null)
                {
                    if (tree.CountOrMeasure == "M")
                    {
                        soundService.SignalMeasureTree();
                    }
                    else if (tree.CountOrMeasure == "I")
                    {
                        soundService.SignalInsuranceTree();
                    }

                    if (appSettings.EnableCruiserPopup)
                    {
                        dialogService.AskCruiser(tree);
                        tree.TrySave();
                    }
                    else
                    {
                        var sampleType = (tree.CountOrMeasure == "M") ? "Measure Tree" :
                                 (tree.CountOrMeasure == "I") ? "Insurance Tree" : String.Empty;
                        dialogService.ShowMessage("Tree #" + tree.TreeNumber.ToString(), sampleType);
                    }

                    if (tree.CountOrMeasure == "M" && AskEnterMeasureTreeData(appSettings, dialogService))
                    {
                        view.GotoTreePage();
                        //this.View.TreeViewMoveLast();
                    }
                }
                tallyHistory.Add(action);
            }
        }

        //ViewController (askKPI) //TODO use Dialog Service instead
        //DataService (CreateNewTreeEntry)
        //DAL (LogTreeEstimate) //should be dataservice instead
        //SampleGroup (MinKPI/MaxKPI)
        public static TallyAction TallyThreeP(CountTree count, ISampleSelector sampler, SampleGroup sg, ITreeDataService dataService, IDialogService dialogService)
        {
            TallyAction action = new TallyAction(count)
            {
                TreeCount = 1,
            };

            var sgCode = sg.Code;
            var stCode = sg.Stratum.Code;
            var spCode = count.TreeDefaultValue.Species;
            int kpi = 0;
            int? value = dialogService.AskKPI((int)sg.MinKPI, (int)sg.MaxKPI, stCode, sgCode, spCode);
            if (value == null)
            {
                return null;
            }
            else
            {
                kpi = value.Value;
            }

            if (kpi == -1)  //user entered sure to measure
            {
                var tree = dataService.CreateNewTreeEntry(count);
                tree.STM = "Y";
                action.TreeRecord = tree;
            }
            else
            {
                action.TreeEstimate = dataService.LogTreeEstimate(count, kpi);
                action.KPI = kpi;

                var result = ((IThreePSelector)sampler).Sample(kpi);
                if (result != SampleResult.C)
                {
                    var tree = dataService.CreateNewTreeEntry(count);
                    tree.KPI = kpi;
                    tree.CountOrMeasure = result.ToString();
                    action.TreeRecord = tree;
                }
            }

            return action;
        }

        //DataService (CreateNewTreeEntry)
        //
        public static TallyAction TallyStandard(CountTree count, ISampleSelector sampleSelecter, ITreeDataService dataService, IDialogService dialogService)
        {
            TallyAction action = new TallyAction(count)
            {
                TreeCount = 1,
            };

            var result = ((IFrequencyBasedSelecter)sampleSelecter).Sample();

            //If we receive nothing from the sampler, we don't have a sample
            if (result != SampleResult.C)//&& (item.IsSelected || item.IsInsuranceItem))
            {
                var tree = dataService.CreateNewTreeEntry(count);
                tree.CountOrMeasure = result.ToString();
                action.TreeRecord = tree;
            }

            return action;
        }

        protected static bool AskEnterMeasureTreeData(IApplicationSettings appSettings, IDialogService dialogService)
        {
            if (!appSettings.EnableAskEnterTreeData) { return false; }

            return dialogService.AskYesNo("Would you like to enter tree data now?", "Sample", false);
        }

        public void RegisterStratumHotKey(string hk, IDataEntryPage page)
        {
            if (string.IsNullOrEmpty(hk)) { return; }
            char stratumHotKey = char.ToUpper(hk[0]);
            this.StratumHotKeyLookup.Add(stratumHotKey, page);
        }

        public bool HandleKeyPress(string keyStr)
        {
            var view = this.View.FocusedLayout;

            if (view != null)
            {
                if (view.PreviewKeypress(keyStr))
                {
                    return true;
                }
                else if (keyStr.Length == 1)
                {
                    var key = keyStr.FirstOrDefault();
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
                            this.View.GoToPage(this.StratumHotKeyLookup[key]);
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

        private bool IsHotkeyKey(char c)
        {
            return Array.IndexOf(Constants.HOTKEY_KEYS, c) != -1;
        }

        //public void HandleKPIChanging(Tree tree, float newKPI, bool doSample, out bool cancel)
        //{
        //    if (tree == null)
        //    {
        //        cancel = true;
        //        return;
        //    }
        //    if (tree.SampleGroup == null)
        //    {
        //        _dialogService.ShowMessage("Select Sample Group before entering KPI");
        //        cancel = true;
        //        return;
        //    }
        //    if (!tree.KPI.EqualsEx(0.0F))
        //    {
        //        string message = string.Format("Tree RecID:{0} KPI changed from {1} to {2}"
        //            , tree.Tree_CN
        //            , tree.KPI
        //            , newKPI);
        //        DataService.LogMessage(message, "I");
        //    }
        //    else if (doSample)
        //    {
        //        CountTree count = tree.FindCountRecord();
        //        if (count != null && count.SampleGroup.Sampler is ThreePSelecter)
        //        {
        //            ThreePItem item = (ThreePItem)count.SampleGroup.Sampler.NextItem();
        //            if (item.KPI < newKPI)
        //            {
        //                tree.CountOrMeasure = "M";
        //                _soundService.SignalMeasureTree();
        //                _dialogService.ShowMessage("Measure Tree");
        //            }
        //            else
        //            {
        //                tree.CountOrMeasure = "C";
        //            }
        //            //count.SumKPI += (long)newKPI;
        //        }
        //    }
        //    cancel = false;
        //}

        public void HandleViewClosing(CancelEventArgs e)
        {
            ViewController.ShowWait();

            try
            {
                ITreeView view;
                //Go through all the tree views and validate
                //if a tree view has invalid trees lets ask the user if they want to continue
                if (!this.ValidateTreeViews(out view)
                    && !_dialogService.AskYesNo("Error(s) found on tree records. Would you like to continue", "Continue?", true))
                {
                    e.Cancel = true;
                    this.View.GoToPage(view);
                    return;
                }

                if (DataService.Region == 10)
                {
                    foreach (var treeView in View.Layouts.OfType<ITreeView>())
                    {
                        var treeNums = treeView.Trees.OrEmpty()
                            .Where(t => t.CountOrMeasure == "M" && t.LogCountActual == 0)
                            .Select(t => t.TreeNumber.ToString()).ToArray();
                        if (treeNums.Length > 0)
                        {
                            if (!_dialogService.AskYesNo("Tree(s) " + String.Join(", ", treeNums) + " have no logs", "Continue?", true))
                            {
                                e.Cancel = true;
                                //View.GoToPageIndex(View.Layouts.IndexOf(treeView));
                                View.GoToPage(treeView);
                                return;
                            }
                        }
                    }
                }
            }
            finally
            {
                ViewController.HideWait();
            }
        }

        /// <summary>
        /// performs validation on each tree view and returns ture if validation passes
        /// </summary>
        /// <param name="invalidViewIndex">index to first invalid view</param>
        /// <returns></returns>
        protected bool ValidateTreeViews(out ITreeView invalidView)
        {
            invalidView = null;
            bool validationPass = true;

            foreach (var view in View.Layouts.OfType<ITreeView>())
            {
                if (view.Trees != null)
                {
                    view.EndEdit();
                    var worker = new TreeValidationWorker(view.Trees);
                    if (!worker.ValidateTrees())
                    {
                        invalidView = view;
                        validationPass = false;
                    }
                }
            }

            return validationPass;
        }
    }//end class deff
}