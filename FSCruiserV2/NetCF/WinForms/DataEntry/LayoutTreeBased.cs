using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FMSC.Sampling;
using FSCruiser.Core.ViewInterfaces;
using FSCruiser.Core;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class LayoutTreeBased : UserControl, ITallyView
    {
        private Panel _visableTallyPanel;
        private bool _viewLoading = true;

        public IApplicationController Controller { get; protected set; }
        public FormDataEntryLogic DataEntryController { get; protected set; }
        public Dictionary<char, CountTreeVM> HotKeyLookup 
        {
            get
            {
                if (SelectedStratum != null)
                {
                    return SelectedStratum.HotKeyLookup;
                }
                return null;
            }
        }
        public Dictionary<char, StratumVM> StrataHotKeyLookup { get; protected set; }
        public IList<StratumVM> Strata { get; protected set; }
        public StratumVM SelectedStratum { get; protected set; }
        public bool HotKeyEnabled 
        {
            get { return true; }
        }

        public bool ViewLoading { get { return _viewLoading; } }
        //public List<CountTreeDO> Counts { get; set; }

        public IDataEntryView DataEntryForm
        {
            get
            {
                return (FormDataEntry)this.TopLevelControl;
            }
        }

        public LayoutTreeBased()
        {
            //this.HotKeyLookup = new Dictionary<char, CountTreeDO>();
            this.StrataHotKeyLookup = new Dictionary<char, StratumVM>();
            InitializeComponent();
        }

        public LayoutTreeBased(IApplicationController controller, FormDataEntryLogic dataEntryController): this()
        {
            this.DataEntryController = dataEntryController;
            this.Controller = controller;

            this.SuspendLayout();
            this.PopulateStrata(dataEntryController.Unit);

            //if there is only one strata in the unit 
            //display the counts for that stratum
            if (this.Strata.Count == 1)
            {
                this.DisplayTallyPanel(this.Strata[0]);
            }

            this.ResumeLayout(false);

            
        }


        private void PopulateStrata(CuttingUnitVM unit)
        {
            //unit.Strata.Populate();
            Strata = DataEntryController.Unit.GetTreeBasedStrata();
            foreach (StratumVM stratum in this.Strata)
            {
                if (stratum.Method == CruiseDAL.Schema.Constants.CruiseMethods.H_PCT) { continue; }
                //if ((Controller.GetStrataDataEntryMode(stratum) & DataEntryMode.Plot) 
                //    == DataEntryMode.Plot) { continue; }

                Button strataButton = new Button();
                Panel tallyContainer = new Panel();
                //StratumInfo stratumInfo = new StratumInfo(stratum);
                stratum.TallyContainer = tallyContainer;
                //Strata.Add(stratumInfo);
                //tallyContainer.SuspendLayout();
                //strataButton.SuspendLayout();

                tallyContainer.Dock = DockStyle.Top;
                tallyContainer.Visible = false;
                tallyContainer.Parent = _leftContentPanel;
                tallyContainer.Tag = stratum;
                
                strataButton.Height = 25;
                strataButton.BackColor = System.Drawing.Color.FromArgb(0x2F, 0x4F, 0x4F); //Color.DarkSlateGray;// DarkGray;// Green;System.Drawing.Color.FromArgb(0x2F, 0x4F, 0x4F);
                strataButton.ForeColor = Color.White;
                strataButton.Text = stratum.GetDescriptionShort();
                if (stratum.Hotkey != null && stratum.Hotkey.Length > 0)
                {
                    strataButton.Text += "[" + stratum.Hotkey.Substring(0, 1) + "]";
                }

                strataButton.Click += new EventHandler(strataButton_Click);
                strataButton.Dock = DockStyle.Top;
                FMSC.Controls.DpiHelper.AdjustControl(strataButton);
                strataButton.Parent = _leftContentPanel;
                strataButton.Tag = stratum;

                DataEntryMode mode = stratum.GetDataEntryMode();
                this.DataEntryController.PopulateTallies(stratum, mode, unit, tallyContainer, this);
                //AdjustPanelHeight(tallyContainer);

                if (string.IsNullOrEmpty(stratum.Hotkey) == false)
                {
                    StrataHotKeyLookup.Add(char.ToUpper(stratum.Hotkey[0]), stratum);
                }
            }

            //foreach (Control c in _leftContentPanel.Controls)
            //{
            //    c.ResumeLayout(false);
            //}
        }

        //private void PopulateTallies(Panel control, CuttingUnitDO unit, StratumDO stratum)
        //{
        //    List<CountTreeDO> counts = Controller._cDal.Read<CountTreeDO>(CruiseDAL.Schema.COUNTTREE._NAME, "JOIN SampleGroup WHERE CountTree.SampleGroup_CN = SampleGroup.SampleGroup_CN AND CuttingUnit_CN = ? AND SampleGroup.Stratum_CN = ?", unit.CuttingUnit_CN, stratum.Stratum_CN);
        //    foreach (CountTreeDO count in counts)
        //    {
        //        MakeSampleSelecter(count);
        //        MakeTallyRow(control, count);
        //    }

        //    this.Counts.AddRange(counts);
        //}

        public void HandleLoad()
        {
           

            _BS_tallyHistory.DataSource = DataEntryController.Unit.TallyHistoryBuffer;
            this._viewLoading = false;
        }

        private void DisplayTallyPanel(StratumVM stratumInfo)
        {
            Panel tallyContainer = (Panel)stratumInfo.TallyContainer;

            if (_visableTallyPanel != null && _visableTallyPanel == tallyContainer && tallyContainer.Visible == true)
            {
                tallyContainer.Visible = false;
                this.SelectedStratum = null; 
                _visableTallyPanel = null;
                return;
            }
            else if (_visableTallyPanel != null && _visableTallyPanel != tallyContainer)
            {
                _visableTallyPanel.Visible = false;

            }

            if (tallyContainer == _visableTallyPanel) { return; }
            _visableTallyPanel = tallyContainer;
            //HotKeyLookup.Clear();
            //foreach (Control c in tallyContainer.Controls)
            //{
            //    TallyRow row = c as TallyRow;
            //    if (row != null)
            //    {
            //        CountTreeDO count = (CountTreeDO)row.Tag;
            //        char hotkey = count.Tally.Hotkey[0];
            //        HotKeyLookup.Add(hotkey, count);
            //    }
            //}
            //if (tallyContainer.Controls.Count == 0)
            //{
            //    StratumDO stratum = (StratumDO)tallyContainer.Tag;
            //    tallyContainer.SuspendLayout();
            //    PopulateTallies(tallyContainer, Unit, stratum);
            
            //AdjustPanelHeight(tallyContainer);
            //    tallyContainer.ResumeLayout(true);
            //}
            this.SelectedStratum = stratumInfo;
            tallyContainer.Visible = true;
        }

        public void HandleStratumLoaded(Control container)
        {
            if (container.InvokeRequired)
            {
                Action<Panel> a = new Action<Panel>(AdjustPanelHeight);
                container.Invoke(a, container);
            }
            else
            {
                this.AdjustPanelHeight((Panel)container);
            }
        }

        private void AdjustPanelHeight(Panel panel)
        {
            int totalChildHeight = 0;
            foreach (Control c in panel.Controls)
            {
                totalChildHeight += c.Height;
            }
            panel.Height = totalChildHeight;
        }


        //public SampleSelecter MakeSampleSelecter(CountTreeDO count, DataEntryMode mode)
        //{
        //    SampleSelecter selecter;
        //    if (((mode & DataEntryMode.ThreeP) == DataEntryMode.ThreeP) || //threeP state doesn't need to be deserialized
        //        String.IsNullOrEmpty(count.SampleSelectorState))
        //    {
        //        int iFreq = (int)count.SampleGroup.InsuranceFrequency;
        //        if ((mode & DataEntryMode.ThreeP) == DataEntryMode.ThreeP)
        //        {
        //            int kz = (int)count.SampleGroup.KZ;
        //            int maxKPI = 100000;
        //            selecter = new FMSC.Sampling.ThreePSelecter(kz, maxKPI, iFreq);
        //        }
        //        else
        //        {
        //            int frequency = (int)count.SampleGroup.SamplingFrequency;
        //            selecter = new FMSC.Sampling.BlockSelecter(frequency, iFreq);
        //        }
        //        selecter.Count = (int)count.TreeCount;
        //    }
        //    else
        //    {
        //        selecter = ApplicationController.DeserializeCountSampleState(count);
        //        selecter.Count = (int)count.TreeCount;
        //    }
        //    if (selecter.Count != count.TreeCount)
        //    {
        //        try
        //        {
        //            throw new InvalidOperationException("selecter.Count != count.TreeCount");
        //        }
        //        catch { }
        //    }

        //    count.Tag = selecter;
        //    return selecter;
        //}

        public Control MakeTallyRow(Control container, CountTreeVM count)
        {
            TallyRow row = new TallyRow();
            row.SuspendLayout();
            row.DiscriptionLabel.Text = count.Tally.Description;
            row.TallyButton.Click += new EventHandler(TallyButton_Click);
            row.SettingsButton.Click += new EventHandler(SettingsButton_Click);
            if (count.Tally.Hotkey != null && count.Tally.Hotkey.Length > 0)
            {
                row.HotKeyLabel.Text = count.Tally.Hotkey.Substring(0, 1);
            }

            row.TallyButton.DataBindings.Add(new Binding("Text", count, "TreeCount"));

            row.Count = count;
            row.Parent = container;


            row.Dock = DockStyle.Top;
            row.ResumeLayout(false);
            return row;
        }

        public Control MakeTallyRow(Control container, SubPop subPop)
        {
            Button tallyButton = new Button();
            tallyButton.SuspendLayout();
            tallyButton.Text = subPop.TDV.Species;
            tallyButton.Click += new EventHandler(SpeciesButton_Click);
            tallyButton.Tag = subPop;
            tallyButton.Parent = container;
            tallyButton.Dock = DockStyle.Top;
            tallyButton.ResumeLayout(false);
            return tallyButton;
        }

        public void MakeSGList(List<SampleGroupVM> list, Panel container)
        {
            if (list.Count == 1)
            {
                SampleGroupVM sg = list[0];

                if (sg.TreeDefaultValues.IsPopulated == false)
                {
                    sg.TreeDefaultValues.Populate();
                }
                foreach (TreeDefaultValueDO tdv in sg.TreeDefaultValues)
                {
                    SubPop subPop = new SubPop(sg, tdv);
                    MakeTallyRow(container, subPop);
                }
            }
            else
            {
                foreach (SampleGroupVM sg in list)
                {
                    Button sgButton = new Button();
                    Panel spContainer = new Panel();

                    if (sg.TreeDefaultValues.IsPopulated == false)
                    {
                        sg.TreeDefaultValues.Populate();
                    }
                    foreach (TreeDefaultValueDO tdv in sg.TreeDefaultValues)
                    {
                        SubPop subPop = new SubPop(sg, tdv);
                        MakeTallyRow(spContainer, subPop);
                    }

                    spContainer.Parent = container;
                    spContainer.Dock = DockStyle.Top;
                    spContainer.Visible = false;

                    sgButton.Parent = container;
                    sgButton.Dock = DockStyle.Top;
                    sgButton.Click += new EventHandler(sgButton_Click);
                }
            }
        }

        void sgButton_Click(object sender, EventArgs e)
        {
            Button sgButton = (Button)sender;
            Panel spContainer = (Panel)sgButton.Tag;
            spContainer.Visible = !spContainer.Visible;
        }

        void SpeciesButton_Click(object sender, EventArgs e)
        {
            if (_viewLoading) { return; }
            Button button = (Button)sender;
            SubPop subPop = (SubPop)button.Tag;

            var tree = DataEntryController.Unit.CreateNewTreeEntry(subPop.SG.Stratum, subPop.SG, subPop.TDV, true);
            tree.TreeCount = 1;

            this.Controller.ViewController.ShowCruiserSelection(tree);

            DataEntryController.Unit.AddNonPlotTree(tree);
            DataEntryForm.GotoTreePage();
        }

        public bool HandleHotKeyFirst(char key)
        {
            key = char.ToUpper(key);
            if (StrataHotKeyLookup.ContainsKey(key))
            {
                DisplayTallyPanel(StrataHotKeyLookup[key]);
                return true;
            }
            return false;

            
            //return Controller.ProcessHotKey(key, this);
        }

        public bool HandleEscKey()
        {
            this.DataEntryController.View.GotoTreePage();
            return true; 
        }

        //public bool HandleKeyUp(char key)
        //{
        //    return this.HandleHotKeyFirst(key);
        //}

        //public bool HandleKeyDown(char key)
        //{
        //    return false;
        //}

        //protected override void OnKeyDown(KeyEventArgs e)
        //{
        //    base.OnKeyDown(e);

        //    char key = (char)e.KeyValue;
        //    e.Handled = this.HandleKeyDown(key);
            
        //}

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            char key = (char)e.KeyValue;
            e.Handled = this.DataEntryController.ProcessHotKey(key, this);
        }



        //public bool ProcessHotKey(char key)
        //{
        //    if (HotKeyEnabled && HotKeyLookup.ContainsKey(key))
        //    {
        //        CountTreeDO count = HotKeyLookup[key];
        //        OnTally(count);
        //        return true;
        //    }
        //    return false;

        //}

        private void _untallyButton_Click(object sender, EventArgs e)
        {
            if (_viewLoading) { return; }
            if (MessageBox.Show("Are you sure you want to untally the selected record?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
                == DialogResult.No) { return; }

            TallyAction selectedAction = _BS_tallyHistory.Current as TallyAction;
            DataEntryController.Unit.TallyHistoryBuffer.Remove(selectedAction);
        }

        void strataButton_Click(object sender, EventArgs e)
        {
            Button strataButton = (Button)sender;
            StratumVM stratumInfo = (StratumVM)strataButton.Tag;
            

            //if (_visableTallyPanel != null && _visableTallyPanel == tallyContainer && tallyContainer.Visible == true)
            //{
            //    tallyContainer.Visible = false;
            //    _visableTallyPanel = null;
            //    return;
            //}
            //else if (_visableTallyPanel != null && _visableTallyPanel != tallyContainer)
            //{
            //    _visableTallyPanel.Visible = false;

            //}

            DisplayTallyPanel(stratumInfo);

        }

        void SettingsButton_Click(object sender, EventArgs e)
        {
            Button settingsbutton = (Button)sender; 
            TallyRow row = (TallyRow)settingsbutton.Parent;
            CountTreeVM count = (CountTreeVM)row.Count;
            Controller.ViewController.ShowTallySettings(count);
            row.DiscriptionLabel.Text = count.Tally.Description;
        }

        void TallyButton_Click(object sender, EventArgs e)
        {
            Control button = (Control)sender;
            TallyRow row = (TallyRow)button.Parent;
            CountTreeVM count = (CountTreeVM)row.Count;
            OnTally(count);
        }

        public void OnTally(CountTreeVM count)
        {
            if (_viewLoading) { return; }
            this.DataEntryController.OnTally(count);
            this._BS_tallyHistory.MoveLast();

            //SampleSelecter sampler = (SampleSelecter)count.SampleGroup.Tag;
            //TallyAction action = new TallyAction(count);

            //DataEntryMode mode = Controller.GetStrataDataEntryMode(count.SampleGroup.Stratum);
            //if ((mode & DataEntryMode.ThreeP) == DataEntryMode.ThreeP)
            //{

            //    int kpi = 0;
            //    int? value = Controller.GetKPI((int)count.SampleGroup.MinKPI, (int)count.SampleGroup.MaxKPI);
            //    if (value == null)
            //    {
            //        MessageBox.Show("No Value Entered");
            //        return;
            //    }
            //    else
            //    {
            //        kpi = value.Value;
            //    }
            //    if (kpi == -1)
            //    {
            //        TreeVM tree;
            //        tree = Controller.CreateNewTreeEntry(count);
            //        tree.STM = "Y";
            //        Controller.TrySaveTree(tree);
            //        action.TreeRecord = tree;
            //    }
            //    else
            //    {
            //        action.KPI = kpi;
            //        count.SumKPI += kpi;
            //        ThreePItem item = (ThreePItem)((ThreePSelecter)sampler).NextItem();
            //        if (item != null && kpi > item.KPI)
            //        {
            //            if (sampler.IsSelectingITrees)
            //            {
            //                item.IsInsuranceItem = sampler.InsuranceCounter.Next();
            //            }
            //            if (item.IsInsuranceItem)
            //            {
            //                Controller.ViewController.SignalInsuranceTree();

            //                TreeVM tree;
            //                tree = Controller.CreateNewTreeEntry(count);
            //                tree.KPI = kpi;
            //                tree.CountOrMeasure = "I";
            //                Controller.TrySaveTree(tree);
            //                action.TreeRecord = tree;
            //            }
            //            else
            //            {
            //                OnSample(action, count, kpi);
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    boolItem item = (boolItem)sampler.NextItem();
            //    if (item != null && item.IsInsuranceItem)
            //    {
            //        Controller.ViewController.SignalInsuranceTree();
            //        TreeVM tree;
            //        tree = Controller.CreateNewTreeEntry(count);
            //        tree.CountOrMeasure = "I";
            //        Controller.TrySaveTree(tree);
            //        action.TreeRecord = tree; 
            //    }
            //    else if (item != null)
            //    {
            //        OnSample(action, count);
            //    }
            //}

            //count.TreeCount++;
            //Controller.AddTallyAction(action);
            //this._BS_tallyHistory.MoveLast();
            //Controller.OnTally();
        }


        //void OnSample(TallyAction action, CountTreeDO count, int kpi)
        //{
        //    this.Controller.ViewController.SignalMeasureTree();

        //    TreeVM tree;
        //    tree = Controller.CreateNewTreeEntry(count, null, true);
        //    tree.KPI = kpi;
        //    this.Controller.TrySaveTree(tree);
        //    action.TreeRecord = tree;
            

        //    if (MessageBox.Show("Would You Like To Enter Tree Data?", "Sample", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
        //            == DialogResult.Yes)
        //    {
        //        DataEntryForm.GotoTreePage();
        //        DataEntryForm.TreeViewMoveLast();
        //    }

        //}

        //void OnSample(TallyAction action, CountTreeDO count)
        //{
        //    TreeVM tree;
        //    this.Controller.ViewController.SignalMeasureTree();

        //    tree = Controller.CreateNewTreeEntry(count);
        //    action.TreeRecord = tree;
        //    tree.CountOrMeasure = "M";
        //    this.Controller.TrySaveTree(tree);
            

        //    if (MessageBox.Show("Would You Like To Enter Tree Data?", "Sample", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
        //        == DialogResult.Yes)
        //    {
        //        DataEntryForm.GotoTreePage();
        //        DataEntryForm.TreeViewMoveLast();
        //    }
        //}

        //private void _BS_tallyHistory_ListChanged(object sender, ListChangedEventArgs e)
        //{
        //    if (e.ListChangedType == ListChangedType.ItemAdded)
        //    {
        //        _BS_tallyHistory.MoveLast();
        //    }
        //}

        
       
    }
}
