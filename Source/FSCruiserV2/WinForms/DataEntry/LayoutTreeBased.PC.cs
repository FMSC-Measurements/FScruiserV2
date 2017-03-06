using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FSCruiser.Core;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;
using FScruiser.Core.Services;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class LayoutTreeBased : TreeBasedTallyView_Base, ITallyView
    {
        //private Panel _visableTallyPanel;
        //private bool _viewLoading = true;

        //public IApplicationController Controller { get; protected set; }
        //public Dictionary<char, StratumVM> StrataHotKeyLookup { get; protected set; }
        //public IList<StratumVM> Strata { get; protected set; }
        //public StratumVM SelectedStratum { get; protected set; }
        //public FormDataEntryLogic DataEntryController { get; protected set; }

        //public bool ViewLoading { get { return _viewLoading; } }

        //public IDataEntryView DataEntryForm
        //{
        //    get
        //    {
        //        return (FormDataEntry)this.TopLevelControl;
        //    }
        //}

        LayoutTreeBased()
            : base()
        {
            InitializeComponent();

            ((System.ComponentModel.ISupportInitialize)(this._BS_tallyHistory)).BeginInit();
            this._tallyHistoryLB.DataSource = this._BS_tallyHistory;
            ((System.ComponentModel.ISupportInitialize)(this._BS_tallyHistory)).EndInit();

            this._untallyBTN.Click += new System.EventHandler(this.OnUntallyButtonClicked);
        }

        public LayoutTreeBased(IApplicationController controller
            , IDataEntryDataService dataService
            , FormDataEntryLogic dataEntryController)
            : this()
        {
            base.Initialize(controller, dataService, dataEntryController, _leftContentPanel);
        }

        //private void PopulateStrata(CuttingUnitVM unit)
        //{
        //    //unit.Strata.Populate();
        //    Strata = unit.GetTreeBasedStrata();
        //    foreach (StratumVM stratum in this.Strata)
        //    {
        //        //if ((Controller.GetStrataDataEntryMode(stratum) & DataEntryMode.Plot)
        //        //    == DataEntryMode.Plot) { continue; }

        //        Button strataButton = new Button();
        //        Panel tallyContainer = new Panel();
        //        //StratumInfo stratumInfo = new StratumInfo(stratum);
        //        stratum.TallyContainer = tallyContainer;
        //        //Strata.Add(stratumInfo);
        //        //tallyContainer.SuspendLayout();
        //        //strataButton.SuspendLayout();

        //        tallyContainer.Dock = DockStyle.Top;
        //        tallyContainer.Visible = false;
        //        tallyContainer.Parent = _leftContentPanel;
        //        tallyContainer.Tag = stratum;

        //        strataButton.Height = 25;
        //        strataButton.BackColor = System.Drawing.Color.FromArgb(0x2F, 0x4F, 0x4F); //Color.DarkSlateGray;// DarkGray;// Green;System.Drawing.Color.FromArgb(0x2F, 0x4F, 0x4F);
        //        strataButton.ForeColor = Color.White;
        //        strataButton.Text = stratum.GetDescriptionShort();
        //        if (stratum.Hotkey != null && stratum.Hotkey.Length > 0)
        //        {
        //            strataButton.Text += "[" + stratum.Hotkey.Substring(0, 1) + "]";
        //        }

        //        strataButton.Click += new EventHandler(this.HandleStratumButtonClicked);
        //        strataButton.Dock = DockStyle.Top;
        //        //FMSC.Controls.DpiHelper.AdjustControl(strataButton);
        //        strataButton.Parent = _leftContentPanel;
        //        strataButton.Tag = stratum;

        //        DataEntryMode mode = stratum.GetDataEntryMode();
        //        this.DataEntryController.PopulateTallies(stratum, mode, unit, tallyContainer, this);
        //        //AdjustPanelHeight(tallyContainer);

        //        if (string.IsNullOrEmpty(stratum.Hotkey) == false)
        //        {
        //            StrataHotKeyLookup.Add(char.ToUpper(stratum.Hotkey[0]), stratum);
        //        }
        //    }

        //}

        //private void DisplayTallyPanel(StratumVM stratumInfo)
        //{
        //    this.SelectedStratum = stratumInfo;
        //    Panel tallyContainer = (Panel)stratumInfo.TallyContainer;

        //    if (_visableTallyPanel != null && _visableTallyPanel == tallyContainer && tallyContainer.Visible == true)
        //    {
        //        tallyContainer.Visible = false;
        //        _visableTallyPanel = null;
        //        return;
        //    }
        //    else if (_visableTallyPanel != null && _visableTallyPanel != tallyContainer)
        //    {
        //        _visableTallyPanel.Visible = false;

        //    }

        //    if (tallyContainer == _visableTallyPanel) { return; }
        //    _visableTallyPanel = tallyContainer;

        //    tallyContainer.Visible = true;
        //}

        //private void HandleStratumButtonClicked(object sender, EventArgs e)
        //{
        //    Button strataButton = (Button)sender;
        //    StratumVM stratumInfo = (StratumVM)strataButton.Tag;

        //    DisplayTallyPanel(stratumInfo);
        //}

        //private void HandleSettingsButtonClick(object sender, EventArgs e)
        //{
        //    Button settingsbutton = (Button)sender;
        //    TallyRow row = (TallyRow)settingsbutton.Parent.Parent;
        //    CountTreeVM count = (CountTreeVM)row.Tag;
        //    Controller.ViewController.ShowTallySettings(count);
        //    row.DiscriptionLabel.Text = count.Tally.Description;
        //}

        //private void HandleTallyButtonClick(object sender, EventArgs e)
        //{
        //    Control button = (Control)sender;
        //    TallyRow row = (TallyRow)button.Parent.Parent;
        //    CountTreeVM count = (CountTreeVM)row.Tag;
        //    OnTally(count);

        //}

        #region ITallyView Members

        //public Dictionary<char, CountTreeVM> HotKeyLookup
        //{
        //    get
        //    {
        //        if (SelectedStratum != null)
        //        {
        //            return SelectedStratum.HotKeyLookup;
        //        }
        //        return null;
        //    }
        //}

        //public bool HandleEscKey()
        //{
        //    this.DataEntryController.View.GotoTreePage();
        //    return true;
        //}

        //public bool HandleHotKeyFirst(char key)
        //{
        //    key = char.ToUpper(key);
        //    if (StrataHotKeyLookup.ContainsKey(key))
        //    {
        //        DisplayTallyPanel(StrataHotKeyLookup[key]);
        //        return true;
        //    }
        //    return false;
        //}

        //public bool HotKeyEnabled
        //{
        //    get { return true; }
        //}

        //public bool HandleKeyDown(char key)
        //{
        //    return false;
        //}

        //public bool HandleKeyUp(char key)
        //{
        //    return false;
        //}

        //public void HandleLoad()
        //{
        //    this.PopulateStrata(this.DataEntryController.Unit);

        //    //if there is only one strata in the unit
        //    //display the counts for that stratum
        //    if (this.Strata.Count == 1)
        //    {
        //        this.DisplayTallyPanel(this.Strata[0]);
        //    }

        //    this._BS_tallyHistory.DataSource = this.DataEntryController.Unit.TallyHistoryBuffer;
        //    this._viewLoading = false;
        //}

        public override void MakeSGList(IEnumerable<SampleGroup> sampleGroups, Panel container)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (NotImplementedException)
            { }
        }

        //public override Control MakeTallyRow(Control container, CountTreeVM count)
        //{
        //    TallyRow row = new TallyRow();
        //    row.SuspendLayout();
        //    row.DiscriptionLabel.Text = count.Tally.Description;

        //    row.TallyButtonClicked += new EventHandler(this.OnTallyButtonClicked);
        //    row.SettingsButtonClicked += new EventHandler(this.OnTallySettingsClicked);

        //    if (count.Tally.Hotkey != null && count.Tally.Hotkey.Length > 0)
        //    {
        //        row.HotKeyLabel.Text = count.Tally.Hotkey.Substring(0, 1);
        //    }

        //    row.TallyButton.DataBindings.Add(new Binding("Text", count, "TreeCount"));

        //    row.Count = count;
        //    row.Parent = container;

        //    row.Dock = DockStyle.Top;
        //    row.ResumeLayout(false);
        //    return row;
        //}

        public override Control MakeTallyRow(Control container, SubPop subPop)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (NotImplementedException)
            { }
            return null;
        }

        //void OnSample(TallyAction action, CountTreeVM count, int kpi)
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

        //void OnSample(TallyAction action, CountTreeVM count)
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

        //public void OnTally(CountTreeVM count)
        //{
        //    if (_viewLoading) { return; }
        //    this.DataEntryController.OnTally(count);
        //    this._BS_tallyHistory.MoveLast();

        //    //if (_viewLoading) { return; }
        //    //SampleSelecter sampler = (SampleSelecter)count.SampleGroup.Tag;
        //    //TallyAction action = new TallyAction(count);

        //    //DataEntryMode mode = Controller.GetStrataDataEntryMode(count.SampleGroup.Stratum);
        //    //if ((mode & DataEntryMode.ThreeP) == DataEntryMode.ThreeP)
        //    //{
        //    //    int kpi = 0;
        //    //    int? value = Controller.GetKPI((int)count.SampleGroup.MinKPI, (int)count.SampleGroup.MaxKPI);
        //    //    if (value == null)
        //    //    {
        //    //        MessageBox.Show("No Value Entered");
        //    //        return;
        //    //    }
        //    //    else
        //    //    {
        //    //        kpi = value.Value;
        //    //    }
        //    //    if (kpi == -1)
        //    //    {
        //    //        TreeVM tree;
        //    //        tree = Controller.CreateNewTreeEntry(count);
        //    //        tree.STM = "Y";
        //    //        Controller.TrySaveTree(tree);
        //    //        action.TreeRecord = tree;
        //    //    }
        //    //    else
        //    //    {
        //    //        action.KPI = kpi;
        //    //        count.SumKPI += kpi;
        //    //        ThreePItem item = (ThreePItem)((ThreePSelecter)sampler).NextItem();
        //    //        if (item != null && kpi > item.KPI)
        //    //        {
        //    //            if (sampler.IsSelectingITrees)
        //    //            {
        //    //                item.IsInsuranceItem = sampler.InsuranceCounter.Next();
        //    //            }
        //    //            if (item.IsInsuranceItem)
        //    //            {
        //    //                Controller.ViewController.SignalInsuranceTree();

        //    //                TreeVM tree;
        //    //                tree = Controller.CreateNewTreeEntry(count);
        //    //                tree.KPI = kpi;
        //    //                tree.CountOrMeasure = "I";
        //    //                Controller.TrySaveTree(tree);
        //    //                action.TreeRecord = tree;
        //    //            }
        //    //            else
        //    //            {
        //    //                OnSample(action, count, kpi);
        //    //            }
        //    //        }
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    boolItem item = (boolItem)sampler.NextItem();
        //    //    if (item != null && item.IsInsuranceItem)
        //    //    {
        //    //        Controller.ViewController.SignalInsuranceTree();
        //    //        TreeVM tree;
        //    //        tree = Controller.CreateNewTreeEntry(count);
        //    //        tree.CountOrMeasure = "I";
        //    //        Controller.TrySaveTree(tree);
        //    //        action.TreeRecord = tree;
        //    //    }
        //    //    else if (item != null)
        //    //    {
        //    //        OnSample(action, count);
        //    //    }
        //    //}

        //    //count.TreeCount++;
        //    //Controller.AddTallyAction(action);
        //    //this._BS_tallyHistory.MoveLast();
        //    //Controller.OnTally();
        //}

        //public void HandleStratumLoaded(Control container)
        //{
        //    if (container.InvokeRequired)
        //    {
        //        Action<Panel> a = new Action<Panel>(AdjustPanelHeight);
        //        container.Invoke(a, container);
        //    }
        //    else
        //    {
        //        this.AdjustPanelHeight(container);
        //    }
        //}

        //private void AdjustPanelHeight(Control panel)
        //{
        //    int totalChildHeight = 0;
        //    foreach (Control c in panel.Controls)
        //    {
        //        totalChildHeight += c.Height;
        //    }
        //    panel.Height = totalChildHeight;
        //}

        #endregion ITallyView Members
    }
}