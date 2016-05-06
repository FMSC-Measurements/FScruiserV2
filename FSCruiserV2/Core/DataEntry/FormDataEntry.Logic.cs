using System;
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

        //public IDataEntryPage FocusedLayout { get; set; }

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

        public bool ShowLimitingDistanceDialog(StratumVM stratum, PlotVM plot, TreeVM optTree)
        {
            string logMessage = String.Empty;
            bool isVariableRadius = Array.IndexOf(CruiseDAL.Schema.CruiseMethods.VARIABLE_RADIUS_METHODS, stratum.Method) > -1;
            float bafOrFixedPlotSize = (isVariableRadius) ? stratum.BasalAreaFactor : stratum.FixedPlotSize;
            DialogResult dResult = ViewController.ShowLimitingDistanceDialog(bafOrFixedPlotSize, isVariableRadius, optTree, out logMessage);
            if (dResult == DialogResult.OK)
            {
                plot.Remarks += logMessage;
                return true;
            }
            return false;

        }

        public int? ShowNumericValueInput(int? min, int? max, int? initialValue, bool acceptNullInput)
        {
            ViewController.NumPadDialog.ShowDialog(min, max, initialValue, acceptNullInput);
            return ViewController.NumPadDialog.UserEnteredValue;
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
            DataEntryMode mode = count.SampleGroup.Stratum.GetDataEntryMode();


            if ((mode & DataEntryMode.ThreeP) == DataEntryMode.ThreeP)//threeP sampling
            {
                
                int kpi = 0;
                int? value = ViewController.AskKPI((int)count.SampleGroup.MinKPI, (int)count.SampleGroup.MaxKPI);
                if (value == null)
                {
                    this.ViewController.ShowMessage("No Value Entered", null, MessageBoxIcon.None);
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
                if (item != null )//&& (item.IsSelected || item.IsInsuranceItem))
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

            if (!isInsurance && this.AskEnterMeasureTreeData())
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

            if(!isInsurance)
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

            if (!isInsurance && this.AskEnterMeasureTreeData())
            {
                this.View.GotoTreePage();
                this.View.TreeViewMoveLast();
            }
        }

        public void OnTally(CountTreeVM count, PlotVM plot, ITreeView treeView)
        {
            System.Diagnostics.Debug.Assert(plot != null);

            SampleGroupDO sg = count.SampleGroup;
            //if ((sg.TallyMethod & CruiseDAL.Enums.TallyMode.Manual) == CruiseDAL.Enums.TallyMode.Manual)
            //{
            //    TreeVM newTree;
            //    newTree = Controller.CreateNewTreeEntry(count, plot, true);
            //    count.TreeCount += sg.SamplingFrequency;
            //    this.Controller.TrySaveTree(newTree);
            //    Controller.OnTally();
            //    return;
            //}

            SampleSelecter sampler = (SampleSelecter)count.SampleGroup.Sampler;
            TreeVM tree = null;
            DataEntryMode mode = count.SampleGroup.Stratum.GetDataEntryMode();
            if ((mode & DataEntryMode.ThreeP) == DataEntryMode.ThreeP)
            {
                int kpi = 0;
                int? value = ViewController.AskKPI((int)count.SampleGroup.MinKPI, (int)count.SampleGroup.MaxKPI);
                if (value == null)
                {
                    this.ViewController.ShowMessage("No Value Entered", null, MessageBoxIcon.None);
                    return;
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
                            this.ViewController.SignalInsuranceTree();
                            tree = plot.CreateNewTreeEntry(count, true);
                            tree.CountOrMeasure = "I";
                        }
                        else
                        {
                            this.ViewController.SignalMeasureTree(true);
                            tree = plot.CreateNewTreeEntry(count, true);
                            //tree.CountOrMeasure = "M";

                        }
                    }
                    else
                    {
                        tree = plot.CreateNewTreeEntry(count, false);
                        //tree.CountOrMeasure = "C";
                    }
                    tree.KPI = kpi;
                }
                else
                {
                    tree = plot.CreateNewTreeEntry(count, true);
                    tree.STM = "Y";
                }

            }
            else
            {
                //count.TreeCount++; tree count doesn't get incremented for plots


                boolItem item = (sampler != null) ? (boolItem)sampler.NextItem() : (boolItem)null;
                if (item != null && !item.IsInsuranceItem)
                {
                    this.ViewController.SignalMeasureTree(true);
                    tree = plot.CreateNewTreeEntry(count, true);
                    //tree.CountOrMeasure = "M";

                }
                else if (item != null && item.IsInsuranceItem)
                {
                    this.ViewController.SignalInsuranceTree();
                    tree = plot.CreateNewTreeEntry(count, true);
                    tree.CountOrMeasure = "I";
                }
                else
                {
                    tree = plot.CreateNewTreeEntry(count, false);
                }
                

            }

            tree.TreeCount = 1;
            tree.TrySave();

            plot.AddTree(tree);

            //treeView.Trees.Add(tree);
            treeView.MoveLastTree();
            //this._dataGrid.CurrentColumnIndex = this._dataGrid.HomeColumnIndex;

            //count.TreeCount++;//TODO double check the rules for tree counts on plots 
            //this.Controller.OnTally();

        }

        public void AddStratumHotKey(string hk, int pageIndex)
        {
            if (string.IsNullOrEmpty(hk)) { return; }
            char stratumHotKey = char.ToUpper(hk[0]);
            this.StratumHotKeyLookup.Add(stratumHotKey, pageIndex);
        }

        
        public void PopulateTallies(StratumVM stratum, CuttingUnitVM unit, Panel container, ITallyView view)
        {
            var stratumMode = stratum.GetDataEntryMode();
            if (stratum is FixCNTStratum)
            {
                //don't initialize tallies for FixCNT
            }
            else if ((stratumMode & DataEntryMode.OneStagePlot) == DataEntryMode.OneStagePlot)
            {
                if (stratum.Method == "3PPNT")
                {
                    //no need to initialize any counts or samplegroup info for 3PPNT
                }
                else
                {
                    List<SampleGroupVM> sgList = this.Database.From<SampleGroupVM>()
                        .Where("Stratum_CN = ?")
                        .Read(stratum.Stratum_CN).ToList();
                    view.MakeSGList(sgList, container);
                }
            }
            else
            {
                var counts = new List<CountTreeVM>();
                var tallySettings = this.Database.From<TallySettingsDO>()
                    .Join("SampleGroup" ,"USING (SampleGroup_CN)")
                    .Where("SampleGroup.Stratum_CN = ?")
                    .GroupBy("CountTree.SampleGroup_CN", "CountTree.TreeDefaultValue_CN", "CountTree.Tally_CN")
                    .Read(stratum.Stratum_CN);

                foreach (TallySettingsDO ts in tallySettings)
                {
                    CountTreeVM count = this.Database.From<CountTreeVM>()
                        .Where("CuttingUnit_CN = ? AND SampleGroup_CN = ? AND Tally_CN = ?")
                        .Read(this.Unit.CuttingUnit_CN
                        , ts.SampleGroup_CN
                        , ts.Tally_CN).FirstOrDefault();
                    if (count == null)
                    {
                        count = new CountTreeVM(this.Database);
                        count.CuttingUnit = this.Unit;
                        count.SampleGroup_CN = ts.SampleGroup_CN;
                        count.TreeDefaultValue_CN = ts.TreeDefaultValue_CN;
                        count.Tally_CN = ts.Tally_CN;

                        count.Save();
                        //this.Unit.Counts.Add(count);
                    }
                    counts.Add(count);
                }

                stratum.Counts = counts;
                stratum.PopulateHotKeyLookup();

                MakeCountTallyRowHadler f = new MakeCountTallyRowHadler(view.MakeTallyRow);
                System.Threading.ThreadPool.QueueUserWorkItem((t) =>
                {
                    foreach (CountTreeVM count in counts)
                    {
                        container.Invoke(f, container, count);
                    }
                    view.HandleStratumLoaded(container);
                });
            }
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
                    var tallyView = view as ITallyView;
                    if (tallyView == null) { return false; }
                    if (tallyView.HotKeyEnabled == false) { return false; }

                    var key = PlatformHelper.KeyToChar(ea.KeyData);
                    if (key == char.MinValue) { return false; }
                    key = char.ToUpper(key);
                    if (!IsHotkeyKey(key)) { return false; }

                    //if valid stratm hot key, go to view that stratum belongs to
                    if (this.StratumHotKeyLookup.ContainsKey(key))
                    {
                        this.View.GoToPageIndex(this.StratumHotKeyLookup[key]);
                        return true;
                    }
                    else if (tallyView.HotKeyLookup != null && tallyView.HotKeyLookup.ContainsKey(key))//maybe a tally hotkey
                    {
                        CountTreeVM count = tallyView.HotKeyLookup[key];
                        tallyView.OnTally(count);
                        return true;
                    }
                    else//not valid hotkey, get grumpy
                    {
                        this.ViewController.SignalInvalidAction();
                        return true;
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
            else if(doSample)
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
                this.ViewController.ShowMessage("Something went wrong while saving the tally count for this unit", null, MessageBoxIcon.Asterisk);
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
