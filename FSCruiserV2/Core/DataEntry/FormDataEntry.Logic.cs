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

namespace FSCruiser.Core.DataEntry
{
    public class FormDataEntryLogic
    {
        private Dictionary<char, int> _stratumHotKeyLookup;

        public IApplicationController Controller { get; set; }

        public IDataEntryView View { get; set; }

        public CuttingUnitVM Unit { get; set; }

        public DAL Database { get { return this.Unit.DAL; } }

        public IViewController ViewController { get { return this.Controller.ViewController; } }

        LoadCuttingUnitWorker _loadUnitWorker;

        public bool HotKeyenabled { get; set; }

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

        public FormDataEntryLogic(CuttingUnitVM unit
            , IApplicationController controller
            , IDataEntryView view)
        {
            this.Unit = unit;
            this.Controller = controller;
            this.View = view;

            this.Unit.InitializeStrata();
            //this.Counts = this.Database.Read<CountTreeVM>((string)null);
        }

        public string GetViewTitle()
        {
            return "Unit: " + this.Unit.Code + ", " + this.Unit.Description;
        }

        protected bool AskEnterMeasureTreeData()
        {
            return this.ViewController.AskYesNo("Would you like to enter tree data now?", "Sample", MessageBoxIcon.Question, false);
        }

        public void ShowLogs(TreeVM tree)
        {
            if (tree.TrySave())
            {
                this.ViewController.ShowLogsView(tree.Stratum, tree);
            }
            else
            {
                ViewController.ShowMessage("Unable to save tree. Ensure Tree Number, Sample Group and Stratum are valid"
                    , null, MessageBoxIcon.Hand);
            }
        }

        public bool ShowLimitingDistanceDialog(StratumModel stratum, PlotVM plot)
        {
            string logMessage = String.Empty;
            bool isVariableRadius = Array.IndexOf(CruiseDAL.Schema.CruiseMethods.VARIABLE_RADIUS_METHODS, stratum.Method) > -1;
            float bafOrFixedPlotSize = (isVariableRadius) ? stratum.BasalAreaFactor : stratum.FixedPlotSize;
            DialogResult dResult = ViewController.ShowLimitingDistanceDialog(bafOrFixedPlotSize, isVariableRadius, out logMessage);
            if (dResult == DialogResult.OK)
            {
                plot.Remarks += logMessage;
                return true;
            }
            return false;
        }

        public void OnTally(CountTreeVM count)
        {
            TallyAction action = new TallyAction(count);
            SampleGroupDO sg = count.SampleGroup;

            //if doing a manual tally create a tree and jump out
            //if ((sg.TallyMethod & CruiseDAL.Enums.TallyMode.Manual) == CruiseDAL.Enums.TallyMode.Manual)
            //{
            //    TreeVM newTree;
            //    newTree = Controller.CreateNewTreeEntry(count, null, true); //create measure tree
            //    count.TreeCount += sg.SamplingFrequency;                    //increment tree count on tally
            //    this.Controller.TrySaveTree(newTree);
            //    Controller.AddTallyAction(action);
            //    Controller.OnTally();
            //    return;
            //}

            SampleSelecter sampler = (SampleSelecter)count.SampleGroup.Sampler;
            if (count.SampleGroup.Stratum.Is3P)//threeP sampling
            {
                int kpi = 0;
                int? value = ViewController.AskKPI((int)count.SampleGroup.MinKPI, (int)count.SampleGroup.MaxKPI);
                if (value == null)
                {
                    return;
                }
                else
                {
                    kpi = value.Value;
                }
                if (kpi == -1)  //user enterted sure to measure
                {
                    TreeVM tree;
                    tree = Unit.CreateNewTreeEntry(count);
                    tree.STM = "Y";
                    tree.TrySave();
                    Unit.AddNonPlotTree(tree);
                    action.TreeRecord = tree;
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
                        this.OnSample(action, count, kpi, item.IsInsuranceItem);
                    }
                }
            }
            else//non 3P sampling (STR)
            {
                boolItem item = (boolItem)sampler.NextItem();
                //If we recieve nothing from from the sampler, we don't have a sample
                if (item != null)//&& (item.IsSelected || item.IsInsuranceItem))
                {
                    this.OnSample(action, count, (item != null && item.IsInsuranceItem));
                }
            }

            count.TreeCount++;
            Unit.TallyHistoryBuffer.Add(action);
        }

        private void OnSample(TallyAction action, CountTreeVM count, int kpi, bool isInsurance)
        {
            TreeVM tree = Unit.CreateNewTreeEntry(count, !isInsurance);
            tree.KPI = kpi;
            tree.CountOrMeasure = (isInsurance) ? "I" : "M";
            action.TreeRecord = tree;

            if (!isInsurance)
            {
                this.ViewController.SignalMeasureTree(true);
            }
            else
            {
                this.ViewController.SignalInsuranceTree();
            }

            this.ViewController.ShowCruiserSelection(tree);

            tree.TrySave();
            Unit.AddNonPlotTree(tree);

            if (!isInsurance && View.AskEnterMeasureTreeData())
            {
                this.View.GotoTreePage();
                this.View.TreeViewMoveLast();
            }
        }

        private void OnSample(TallyAction action, CountTreeVM count, bool isInsurance)
        {
            TreeVM tree = Unit.CreateNewTreeEntry(count);
            tree.CountOrMeasure = (isInsurance) ? "I" : "M";

            action.TreeRecord = tree;

            if (!isInsurance)
            {
                this.ViewController.SignalMeasureTree(true);
            }
            else
            {
                this.ViewController.SignalInsuranceTree();
            }

            this.ViewController.ShowCruiserSelection(tree);

            tree.TrySave();
            Unit.AddNonPlotTree(tree);

            if (!isInsurance && View.AskEnterMeasureTreeData())
            {
                this.View.GotoTreePage();
                this.View.TreeViewMoveLast();
            }
        }

        public void AddStratumHotKey(string hk, int pageIndex)
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
                            CountTreeVM count = tallyView.HotKeyLookup[key];
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
                            this.ViewController.SignalInvalidAction();
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

        public void HandleKPIChanging(TreeVM tree, float newKPI, bool doSample, out bool cancel)
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
            if (tree.KPI != 0.0F)
            {
                string message = string.Format("Tree RecID:{0} KPI changed from {1} to {2}", tree.Tree_CN, tree.KPI, newKPI);
                this.Database.LogMessage(message, "I");
            }
            else if (doSample)
            {
                CountTreeVM count = tree.FindCountRecord();
                if (count != null && count.SampleGroup.Sampler is ThreePSelecter)
                {
                    ThreePItem item = (ThreePItem)count.SampleGroup.Sampler.NextItem();
                    if (item.KPI < newKPI)
                    {
                        tree.CountOrMeasure = "M";
                        this.ViewController.SignalMeasureTree(true);
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

        public bool HandleSpeciesChanged(TreeVM tree, TreeDefaultValueDO tdv)
        {
            if (tree == null) { return true; }
            //if (tree.TreeDefaultValue == tdv) { return true; }
            tree.SetTreeTDV(tdv);
            return tree.TrySave();
        }

        public void HandleStratumChanging(TreeVM tree, StratumDO st, out bool cancel)
        {
            if (tree == null || st == null) { cancel = true; return; }
            if (tree.Stratum != null && tree.Stratum.Stratum_CN == st.Stratum_CN) { cancel = true; return; }
            if (tree.Stratum != null)
            {
                if (!this.ViewController.AskYesNo("You are changing the stratum of a tree, are you sure you want to do this?", "!", MessageBoxIcon.Asterisk))
                {
                    cancel = true;//do not change stratum
                }
                else
                {
                    //log stratum changed
                    this.Database.LogMessage(String.Format("Tree Stratum Changed (Cu:{0} St:{1} -> {2} Sg:{3} Tdv_CN:{4} T#: {5} P#:{6}",
                        tree.CuttingUnit.Code,
                        tree.Stratum.Code,
                        st.Code,
                        (tree.SampleGroup != null) ? tree.SampleGroup.Code : "?",
                        (tree.TreeDefaultValue != null) ? tree.TreeDefaultValue.TreeDefaultValue_CN.ToString() : "?",
                        tree.TreeNumber,
                        (tree.Plot != null) ? tree.Plot.PlotNumber.ToString() : "-"), "I");
                }
            }
            cancel = false;
        }

        public void HandleAddTreeClick()
        {
            ITreeView view = this.View.FocusedLayout as ITreeView;
            if (view == null) { return; }
            view.UserAddTree();
        }

        public void HandleDeleteRowButtonClick()
        {
            ITreeView layout = this.View.FocusedLayout as ITreeView;
            if (layout != null)
            {
                layout.DeleteSelectedTree();
            }
        }

        public void HandleShowHideErrorCol()
        {
            ITreeView view = this.View.FocusedLayout as ITreeView;
            if (view == null) { return; }
            view.ToggleErrorColumn();
        }

        public void HandleShowHideLogCol()
        {
            ITreeView view = this.View.FocusedLayout as ITreeView;
            if (view == null) { return; }
            view.ToggleLogColumn();
        }

        public void HandleDisplayLimitingDistance()
        {
            LayoutPlot view = this.View.FocusedLayout as LayoutPlot;
            if (view == null) { return; }
            view.ShowLimitingDistanceDialog();
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
                    && this.ViewController.AskYesNo("Error(s) found on tree records. Would you like to continue", "Continue?", MessageBoxIcon.Question, true) == false)
                {
                    e.Cancel = true;
                    this.View.GoToPageIndex(viewIndex);
                    return;
                }

                //save all the plot views, this will save all trees and plots in them
                for (int i = 0; i < this.View.Layouts.Count; i++)
                {
                    IPlotLayout view = this.View.Layouts[i] as IPlotLayout;
                    if (view != null)
                    {
                        view.ViewLogicController.Save();
                    }
                }

                if (!Unit.TrySaveCounts())
                {
                    e.Cancel = true;
                    ViewController.ShowMessage("Something went wrong while saving the tally count for this unit", null, MessageBoxIcon.Asterisk);
                }

                if (!Unit.SaveFieldData())
                {
                    e.Cancel = true;
                    ViewController.ShowMessage("Something went wrong saving the data for this unit, check trees for errors and try again", null, MessageBoxIcon.Asterisk);
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
            for (int i = 0; i < this.View.Layouts.Count; i++)
            {
                ITreeView view = this.View.Layouts[i] as ITreeView;
                if (view != null && view.Trees != null)
                {
                    view.EndEdit();
                    var worker = new TreeValidationWorker(view.Trees);
                    if (!worker.ValidateTrees())
                    {
                        if (invalidViewIndex != -1)
                        {
                            invalidViewIndex = i;
                        }
                        validationPass = false;
                    }
                }
            }

            return validationPass;
        }
    }//end class deff
}