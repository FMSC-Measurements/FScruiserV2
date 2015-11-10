using System;
using System.Collections.Generic;
using CruiseDAL.DataObjects;
using System.ComponentModel;
using System.Windows.Forms;
using FMSC.Sampling;
using CruiseDAL;
using FSCruiser.Core.ViewInterfaces;
using FSCruiser.Core.Models;
using FSCruiser.WinForms.DataEntry;

namespace FSCruiser.Core.DataEntry
{
    public class FormDataEntryLogic
    {
        public const char ESC_KEY_VALUE = char.MaxValue;

        private Dictionary<char, int> _stratumHotKeyLookup; 

        public IApplicationController Controller { get; set; }
        public IDataEntryView View { get; set; }

        public CuttingUnitDO Unit { get { return this.Controller.CurrentUnit; } }
        public List<CountTreeVM> Counts { get; protected set; }

        public DAL Database { get { return this.Controller._cDal; } }
        public IViewController ViewController { get { return this.Controller.ViewController; } }



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

        public FormDataEntryLogic(IApplicationController controller, IDataEntryView view)
        {
            this.Controller = controller;
            this.View = view;
            this.Counts = this.Database.Read<CountTreeVM>((string)null);
        }


        protected bool AskEnterMeasureTreeData()
        {
            this.ViewController.SignalMeasureTree(false);
            return this.ViewController.AskYesNo("Measure Tree\r\n Would you like to enter tree data now?", "Sample", MessageBoxIcon.Question, false);
        }

        /// <summary>
        /// Creates a new plot using the plot info view and adds it to the given stratum's plot collection
        /// </summary>
        /// <param name="stratum">stratum to create plot in</param>
        /// <returns>reference to newly created plot</returns>
        public PlotVM AddPlot(StratumVM stratum)
        {
            PlotVM newPlot = new PlotVM(this.Database);
            newPlot.CuttingUnit = this.Unit;
            newPlot.Stratum = stratum;
            newPlot.PlotNumber = this.GetNextPlotNumber(stratum);

            //PlotInfo plotInfo = new PlotInfo(newPlot, stratum);
            //newPlot.NextPlotTreeNum = 1;
            if (this.ViewController.ShowPlotInfo(newPlot, true) == DialogResult.OK)
            {
                foreach (PlotVM pi in stratum.Plots)
                {
                    if (pi.PlotNumber == newPlot.PlotNumber)
                    {
                        MessageBox.Show(String.Format("Plot Number {0} Already Exists", newPlot.PlotNumber));
                        return this.AddPlot(stratum);
                    }
                }


                newPlot.Save();
                stratum.Plots.Add(newPlot);
                newPlot.CheckDataState();

                if (!String.IsNullOrEmpty(newPlot.IsEmpty) && String.Compare(newPlot.IsEmpty.Trim(), "True", true) == 0)
                {
                    return this.AddPlot(stratum) ?? newPlot;//add plot may return null, in that case return most recently created plot
                }
                else if (newPlot.Stratum.Method == "3PPNT" && newPlot.Trees.Count == 0)
                {
                    return this.AddPlot(stratum) ?? newPlot;//add plot may return null, in that case return most recently created plot
                }
                return newPlot;
            }
            return null;
        }

        public int GetNextPlotNumber(StratumDO stratum)
        {
            try
            {
                int highestInUnit = 0;
                int highestInStratum = 0;

                {
                    string query = string.Format("Select Max(PlotNumber) FROM Plot WHERE CuttingUnit_CN = {0}", this.Unit.CuttingUnit_CN);
                    long? result = Database.ExecuteScalar(query) as long?;
                    highestInUnit = (result != null) ? (int)result.Value : 0;
                }
       
                {
                    string query = string.Format("Select Max(PlotNumber) FROM Plot WHERE CuttingUnit_CN = {0} AND Stratum_CN = {1}", this.Unit.CuttingUnit_CN, stratum.Stratum_CN);
                    long? result = Database.ExecuteScalar(query) as long?;
                    highestInStratum = (result != null) ? (int)result.Value : 0;
                }


                if (highestInUnit > highestInStratum && highestInUnit > 0)
                {
                    return highestInUnit;
                }
                return highestInUnit + 1;
            }
            catch (Exception e)
            {
                Logger.Log.E("Unable to establish next plot number", e);
                return 0;
            }
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
            DataEntryMode mode = Controller.GetStrataDataEntryMode(count.SampleGroup.Stratum);


            if ((mode & DataEntryMode.ThreeP) == DataEntryMode.ThreeP)//threeP sampling
            {
                
                int kpi = 0;
                int? value = Controller.GetKPI((int)count.SampleGroup.MinKPI, (int)count.SampleGroup.MaxKPI);
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
                    tree = Controller.CreateNewTreeEntry(count);
                    tree.STM = "Y";
                    Controller.TrySaveTree(tree);
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
            Controller.AddTallyAction(action);
            //this._BS_tallyHistory.MoveLast();
            Controller.OnTally();

        }


        private void OnSample(TallyAction action, CountTreeVM count, int kpi, bool isInsurance)
        {
            TreeVM tree;
            tree = Controller.CreateNewTreeEntry(count, null, !isInsurance);
            tree.KPI = kpi;
            tree.CountOrMeasure = (isInsurance) ? "I" : "M";
            this.Controller.TrySaveTree(tree);
            action.TreeRecord = tree;

            if (!isInsurance && this.AskEnterMeasureTreeData())
            {
                this.View.GotoTreePage();
                this.View.TreeViewMoveLast();
            }
            else if(isInsurance)
            {
                this.Controller.ViewController.SignalInsuranceTree();
            }
        }

        private void OnSample(TallyAction action, CountTreeVM count, bool isInsurance)
        {
            TreeVM tree;
            tree = Controller.CreateNewTreeEntry(count);
            action.TreeRecord = tree;
            tree.CountOrMeasure = (isInsurance) ? "I" : "M";
            this.Controller.TrySaveTree(tree);

            if (!isInsurance && this.AskEnterMeasureTreeData())
            {
                this.View.GotoTreePage();
                this.View.TreeViewMoveLast();
            }
            else if (isInsurance)
            {
                this.ViewController.SignalInsuranceTree();
            }
        }


        public void OnTally(CountTreeVM count, PlotVM plot, ITreeView treeView)
        {
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
            DataEntryMode mode = Controller.GetStrataDataEntryMode(count.SampleGroup.Stratum);
            if ((mode & DataEntryMode.ThreeP) == DataEntryMode.ThreeP)
            {
                int kpi = 0;
                int? value = Controller.GetKPI((int)count.SampleGroup.MinKPI, (int)count.SampleGroup.MaxKPI);
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
                            tree = Controller.CreateNewTreeEntry(count, plot, true);
                            tree.CountOrMeasure = "I";
                        }
                        else
                        {
                            this.ViewController.SignalMeasureTree(true);
                            tree = Controller.CreateNewTreeEntry(count, plot, true);
                            //tree.CountOrMeasure = "M";

                        }
                    }
                    else
                    {
                        tree = Controller.CreateNewTreeEntry(count, plot, false);
                        //tree.CountOrMeasure = "C";
                    }
                    tree.KPI = kpi;
                }
                else
                {
                    tree = Controller.CreateNewTreeEntry(count, plot, true);
                    tree.STM = "Y";
                }
                //tree.TreeNumber = CurrentPlotInfo.NextPlotTreeNum++;

            }
            else
            {
                //count.TreeCount++; tree count doesn't get incremented for plots


                boolItem item = (sampler != null) ? (boolItem)sampler.NextItem() : (boolItem)null;
                if (item != null && !item.IsInsuranceItem)
                {
                    this.ViewController.SignalMeasureTree(true);
                    tree = Controller.CreateNewTreeEntry(count, plot, true);
                    //tree.CountOrMeasure = "M";

                }
                else if (item != null && item.IsInsuranceItem)
                {
                    this.ViewController.SignalInsuranceTree();
                    tree = Controller.CreateNewTreeEntry(count, plot, true);
                    tree.CountOrMeasure = "I";
                }
                else
                {
                    tree = Controller.CreateNewTreeEntry(count, plot, false);
                }
                

            }

            tree.TreeCount = 1;
            this.Controller.TrySaveTree(tree);
            treeView.Trees.Add(tree);
            treeView.MoveLast();
            //this._dataGrid.CurrentColumnIndex = this._dataGrid.HomeColumnIndex;

            //count.TreeCount++;//TODO double check the rules for tree counts on plots 
            this.Controller.OnTally();

        }

        public void AddStratumHotKey(string hk, int pageIndex)
        {
            if (string.IsNullOrEmpty(hk)) { return; }
            char stratumHotKey = char.ToUpper(hk[0]);
            this.StratumHotKeyLookup.Add(stratumHotKey, pageIndex);
        }

        public string GetViewTitle()
        {
            return "Unit: " + this.Unit.Code + ", " + this.Unit.Description;
        }

        public void PopulateTallies(StratumVM stratum, DataEntryMode stratumMode, CuttingUnitVM unit, Panel container, ITallyView view)
        {
            //StratumDO stratum = stratumInfo.Stratum;

            if ((stratumMode & DataEntryMode.OneStagePlot) == DataEntryMode.OneStagePlot)
            {
                if (stratum.Method == "3PPNT")
                {
                    //no need to initialize any counts or samplegroup info for 3PPNT

                }
                else
                {
                    List<SampleGroupVM> sgList = this.Database.Read<SampleGroupVM>("SampleGroup", "WHERE Stratum_CN = ?", stratum.Stratum_CN);
                    view.MakeSGList(sgList, container);
                }
            }
            else
            {
                List<CountTreeVM> counts = new List<CountTreeVM>();
                List<TallySettingsDO> tallySettings = this.Database.Read<TallySettingsDO>("CountTree",
                    @"JOIN SampleGroup USING (SampleGroup_CN) 
                    WHERE SampleGroup.Stratum_CN = ? 
                    GROUP BY CountTree.SampleGroup_CN, CountTree.TreeDefaultValue_CN, CountTree.Tally_CN",
                    stratum.Stratum_CN);

                foreach (TallySettingsDO ts in tallySettings)
                {
                    CountTreeVM count = this.Database.ReadSingleRow<CountTreeVM>("CountTree", "WHERE CuttingUnit_CN = ? AND SampleGroup_CN = ? AND Tally_CN = ?", this.Unit.CuttingUnit_CN, ts.SampleGroup_CN, ts.Tally_CN);
                    if (count == null)
                    {
                        count = new CountTreeVM(this.Database);
                        count.CuttingUnit = this.Unit;
                        count.SampleGroup_CN = ts.SampleGroup_CN;
                        count.TreeDefaultValue_CN = ts.TreeDefaultValue_CN;
                        count.Tally_CN = ts.Tally_CN;

                        count.Save();
                        this.Counts.Add(count);
                    }
                    counts.Add(count);
                }

                //List<CountTreeVM> counts = _cDal.Read<CountTreeVM>(CruiseDAL.Schema.COUNTTREE._NAME, "JOIN SampleGroup WHERE CountTree.SampleGroup_CN = SampleGroup.SampleGroup_CN AND CuttingUnit_CN = ? AND SampleGroup.Stratum_CN = ?", unit.CuttingUnit_CN, stratum.Stratum_CN);
                //Counts.AddRange(counts);

                stratum.HotKeyLookup.Clear();
                foreach (CountTreeVM count in counts)
                {
                    try
                    {
                        char hotkey = count.Tally.Hotkey[0];
                        hotkey = char.ToUpper(hotkey);
                        stratum.HotKeyLookup.Add(hotkey, count);
                    }
                    catch
                    { }
                }

                MakeCountTallyRowHadler f = new MakeCountTallyRowHadler(view.MakeTallyRow);
                System.Threading.ThreadPool.QueueUserWorkItem((t) =>
                {
                    foreach (CountTreeVM count in counts)
                    {
                        //MakeSampleSelecter(count, stratumMode);
                        //if (container.InvokeRequired)
                        //{
                        //    container.Invoke(f, container, count); //== view.MakeTallyRow(container, count);
                        //}
                        //else
                        //{
                        //    view.MakeTallyRow(container, count);
                        //}
                        container.Invoke(f, container, count);
                    }
                    view.HandleStratumLoaded(container);
                });
            }
        }

        public bool ProcessHotKey(char key, ITallyView view)
        {
            //if tally view is active but blocking hotkeys, jump out
            if (view != null && view.HotKeyEnabled == false)
            {
                return false;
            }
            else if (view != null && view.HandleHotKeyFirst(key))//pass off to tally view to handle
            {
                return true; //if handled return
            }

            //if valid stratm hot key, go to view that stratum belongs to
            if (this.StratumHotKeyLookup.ContainsKey(key))
            {
                this.View.GoToPageIndex(this.StratumHotKeyLookup[key]);
                return true;
            }
            else if (view != null)//not a stratum hotkey 
            {
                if (view.HotKeyLookup != null && view.HotKeyLookup.ContainsKey(key))//maybe a tally hotkey
                {
                    CountTreeVM count = view.HotKeyLookup[key];
                    view.OnTally(count);
                    return true;
                }
                else//not valid hotkey, get angry
                {
                    this.ViewController.SignalInvalidAction();
                    return true;
                }
            }
            else//not a stratum hotkey, give up
            {
                return false;
            }
        }

        public bool HandleHotKey(char key)
        {
            ITallyView view = this.View.FocusedLayout as ITallyView;
            return this.ProcessHotKey(key, view);
        }

        public bool HandleEscKey()
        {
            IDataEntryPage view = this.View.FocusedLayout;
            if (view != null)
            {
                return view.HandleEscKey();
            }
            return false;
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
                CountTreeVM count = this.Controller.GetCountRecord(tree);
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

        public void HandleSampleGroupChanged(ITreeView view, TreeVM tree)
        {
            if (tree == null) { return; }
            if (!tree.SampleGroup.TreeDefaultValues.Contains(tree.TreeDefaultValue))
            {
                this.Controller.SetTreeTDV(tree, null);
            }
            view.UpdateSpeciesColumn(tree);
            this.Controller.TrySaveTree(tree);
        }

        public void HandleSampleGroupChanging(TreeVM tree, SampleGroupDO newSG, out bool cancel)
        {
            if (tree == null || newSG == null) { cancel = true; return; }
            if (tree.SampleGroup != null && tree.SampleGroup_CN == newSG.SampleGroup_CN) { cancel = true; return; }
            if (tree.SampleGroup != null)
            {
                if (MessageBox.Show("You are changing the Sample Group of a tree, are you sure you want to do this?", "!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2)
                    == DialogResult.No)
                {
                    cancel = true;//disregard changes
                    return;
                }
                else
                {

                    this.Database.LogMessage(String.Format("Tree Sample Group Changed (Cu:{0} St:{1} Sg:{2} -> {3} Tdv_CN:{4} T#: {5} P#:{6}",
                        tree.CuttingUnit.Code,
                        tree.Stratum.Code,
                        (tree.SampleGroup != null) ? tree.SampleGroup.Code : "?",
                        newSG.Code,
                        (tree.TreeDefaultValue != null) ? tree.TreeDefaultValue.TreeDefaultValue_CN.ToString() : "?",
                        tree.TreeNumber,
                        (tree.Plot != null)? tree.Plot.PlotNumber.ToString() : "-"), "high");
                }
            }
            cancel = false;
        }

        public bool HandleSpeciesChanged(TreeVM tree, TreeDefaultValueDO tdv)
        {
            if (tree == null) { return true; }
            //if (tree.TreeDefaultValue == tdv) { return true; }
            this.Controller.SetTreeTDV(tree, tdv);
            return this.Controller.TrySaveTree(tree);
        }

        public void HandleStratumChanged(ITreeView view, TreeVM tree)
        {
            if (tree == null) { return; }

            tree.Species = null;
            tree.SampleGroup = null;
            this.Controller.SetTreeTDV(tree, null);
            view.UpdateSampleGroupColumn(tree);
            view.UpdateSpeciesColumn(tree);
            this.Controller.TrySaveTree(tree);
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

//        public IList<StratumInfo> GetUnitPlotStrata()
//        {
//            CuttingUnitDO unit = this.Controller.CurrentUnit;
//            IList<StratumInfo> list = this.Controller._cDal.Read<StratumInfo>("Stratum",
//@"JOIN CuttingUnitStratum USING (Stratum_CN) 
//WHERE CuttingUnitStratum.CuttingUnit_CN = ? 
//AND Stratum.Method IN ( 'FIX', 'FCM', 'F3P', 'PNT', 'PCM', 'P3P', 'P3PNT')", unit.CuttingUnit_CN);
//            foreach (StratumInfo s in list)
//            {
//                if (s.Plots == null)
//                {
//                    s.Plots = this.Controller._cDal.Read<PlotInfo>("Plot", "WHERE Stratum_CN = ? AND CuttingUnit_CN = ? ORDER BY PlotNumber", s.Stratum_CN, unit.CuttingUnit_CN);
//                }
//                if (s.Method == "3PPNT")
//                {
//                    if (s.KZ3PPNT <= 0)
//                    {
//                        MessageBox.Show("error 3PPNT missing KZ value, please return to Cruise System Manger and fix");
//                        return null;
//                    }
//                    s.SampleSelecter = new FMSC.Sampling.ThreePSelecter((int)s.KZ3PPNT, 1000000, 0);
//                }
//            }
//            return list;
//        }

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
                layout.DeleteRow();
            }
        }

        public void HandleShowHideErrorCol()
        {
            ITreeView view = this.View.FocusedLayout as ITreeView;
            if (view == null) { return; }
            view.ShowHideErrorCol();
        }

        public void HandleShowHideLogCol()
        {
            this.ViewController.EnableLogGrading = !this.ViewController.EnableLogGrading;
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
            Controller.AsyncLoadCuttingUnitData();
        }

        public void SaveCounts()
        {
                foreach (CountTreeVM count in Counts)
                {
                    count.Save();
                }
        }


        

        public void HandleViewClosing(CancelEventArgs e)
        {
            
            //bool haveAskedToContinue = false;
            //for (int i = 0; i < this.View.Layouts.Count; i++)
            //{
            //    ITreeView view = this.View.Layouts[i] as ITreeView;
            //    if (view != null && view.Trees != null)
            //    {
            //        view.EndEdit();
            //        if (!Controller.ValidateTrees(view.Trees))
            //        {
            //            //if a view fails tree validation the user is alearted.
            //            //if they decide not to continue they are taken to the tree view containing the invalid trees
            //            //if (MessageBox.Show("Error(s) found on tree records Would you like to continue", "Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            //            if (!haveAskedToContinue && this.Controller.ViewController.AskYesNo("Error(s) found on tree records Would you like to continue", "Continue?", MessageBoxIcon.Question, true) == false)
            //            {
            //                e.Cancel = true;
            //                this.View.GoToPageIndex(i);
            //                return;
            //            }
            //            else
            //            {
            //                haveAskedToContinue = true;// do not keep asking the same question
            //            }
            //        }
            //    }
            //}


            //Go through all the tree views and validate 
            //if a tree view has invalid trees lets ask the user if they want to continue
            int viewIndex;
            if (!this.ValidateTreeViews(out viewIndex)
                && this.ViewController.AskYesNo("Error(s) found on tree records Would you like to continue", "Continue?", MessageBoxIcon.Question, true) == false)
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

            try
            {
                this.SaveCounts();
            }
            catch (Exception)
            {
                e.Cancel = true;
                this.ViewController.ShowMessage("Something went wrong while saving the tally count for this unit",null, MessageBoxIcon.Asterisk);
            }
            this.Controller.OnLeavingCurrentUnit(e);
        }

        /// <summary>
        /// performs validation on each tree view and retruns ture if validation passes
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
                    if (!Controller.ValidateTrees(view.Trees))
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
