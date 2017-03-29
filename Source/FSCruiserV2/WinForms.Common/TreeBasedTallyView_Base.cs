using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FSCruiser.Core;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;
using FSCruiser.WinForms.DataEntry;
using FScruiser.Core.Services;

namespace FSCruiser.WinForms
{
    public partial class TreeBasedTallyView_Base : UserControl, ITallyView
    {
        #region Fields

        Panel _visableTallyPanel;

        #endregion Fields

        #region properties

        public IApplicationController Controller { get; protected set; }

        protected IDataEntryDataService DataService { get; set; }

        public IDataEntryView DataEntryForm
        {
            get
            {
                return (FSCruiser.WinForms.DataEntry.FormDataEntry)this.TopLevelControl;
            }
        }

        public Dictionary<char, Stratum> StrataHotKeyLookup { get; protected set; }

        public Dictionary<Stratum, Panel> StrataViews { get; protected set; }

        public IEnumerable<Stratum> Strata { get; protected set; }

        public Stratum SelectedStratum { get; protected set; }

        protected Panel StrataViewContainer { get; set; }

        #endregion properties

        protected TreeBasedTallyView_Base()
        {
            this.StrataHotKeyLookup = new Dictionary<char, Stratum>();
            this.StrataViews = new Dictionary<Stratum, Panel>();

            InitializeComponent();
        }

        protected void Initialize(IApplicationController controller
            , IDataEntryDataService dataService
            , FormDataEntryLogic dataEntryController
            , Panel strataViewContainer)
        {
            StrataViewContainer = strataViewContainer;
            DataEntryController = dataEntryController;
            Controller = controller;
            DataService = dataService;
            Strata = DataService.TreeStrata;
        }

        protected void InitializeStrataViews()
        {
            this.SuspendLayout();

            this.PopulateStrata();

            //if there is only one strata in the unit
            //display the counts for that stratum
            if (Strata.Count() == 1)
            {
                var singleStratum = Strata.First();
                this.DisplayTallyPanel(singleStratum);
            }

            this.ResumeLayout(false);
        }

        private void PopulateStrata()
        {
            foreach (Stratum stratum in this.Strata)
            {
                if (stratum.Method == CruiseDAL.Schema.CruiseMethods.H_PCT) { continue; }
                //if ((Controller.GetStrataDataEntryMode(stratum) & DataEntryMode.Plot)
                //    == DataEntryMode.Plot) { continue; }

                Button strataButton = new Button();
                Panel tallyContainer = new Panel();
                //StratumInfo stratumInfo = new StratumInfo(stratum);
                //stratum.TallyContainer = tallyContainer;
                //Strata.Add(stratumInfo);
                //tallyContainer.SuspendLayout();
                //strataButton.SuspendLayout();

                tallyContainer.Dock = DockStyle.Top;
                tallyContainer.Visible = false;
                tallyContainer.Parent = this.StrataViewContainer;
                tallyContainer.Tag = stratum;

                strataButton.Height = 25;
                strataButton.BackColor = System.Drawing.Color.FromArgb(0x2F, 0x4F, 0x4F); //Color.DarkSlateGray;// DarkGray;// Green;System.Drawing.Color.FromArgb(0x2F, 0x4F, 0x4F);
                strataButton.ForeColor = Color.White;
                strataButton.Text = stratum.GetDescriptionShort();
                if (stratum.Hotkey != null && stratum.Hotkey.Length > 0)
                {
                    strataButton.Text += "[" + stratum.Hotkey.Substring(0, 1) + "]";
                }

                strataButton.Click += new EventHandler(OnStrataButtonClicked);
                strataButton.Dock = DockStyle.Top;
#if NetCF
                FMSC.Controls.DpiHelper.AdjustControl(strataButton);
#endif
                strataButton.Parent = this.StrataViewContainer;
                strataButton.Tag = stratum;

                StrataViews.Add(stratum, tallyContainer);

                foreach (CountTree count in stratum.Counts)
                {
                    MakeTallyRow(tallyContainer, count);
                    AdjustPanelHeight(tallyContainer);
                }

                if (string.IsNullOrEmpty(stratum.Hotkey) == false)
                {
                    StrataHotKeyLookup.Add(char.ToUpper(stratum.Hotkey[0]), stratum);
                }
            }
        }

        #region overrid methods

        //protected override void OnKeyUp(KeyEventArgs e)
        //{
        //    base.OnKeyUp(e);
        //    char key = (char)e.KeyValue;
        //    e.Handled = this.DataEntryController.ProcessHotKey(key, this);
        //}

        #endregion overrid methods

        protected void OnSgButtonClick(object sender, EventArgs e)
        {
            Button sgButton = (Button)sender;
            Panel spContainer = (Panel)sgButton.Tag;
            spContainer.Visible = !spContainer.Visible;
        }

        protected void OnSpeciesButtonClick(object sender, EventArgs e)
        {
            if (_viewLoading) { return; }
            Button button = (Button)sender;
            SubPop subPop = (SubPop)button.Tag;

            var tree = DataService.CreateNewTreeEntry(subPop.SG.Stratum, subPop.SG, subPop.TDV, true);
            tree.TreeCount = 1;

            DialogService.AskCruiser(tree);

            DataService.AddNonPlotTree(tree);
            DataEntryForm.GotoTreePage();
        }

        protected void OnTallySettingsClicked(object sender, EventArgs e)
        {
            var row = (ITallyButton)sender;
            CountTree count = row.Count;
            Controller.ViewController.ShowTallySettings(count);
            //row.DiscriptionLabel.Text = count.Tally.Description;
        }

        protected void OnTallyButtonClicked(object sender, EventArgs e)
        {
            var row = (ITallyButton)sender;
            CountTree count = row.Count;
            OnTally(count);
        }

        protected void OnStrataButtonClicked(object sender, EventArgs e)
        {
            Button strataButton = (Button)sender;
            Stratum stratumInfo = (Stratum)strataButton.Tag;

            DisplayTallyPanel(stratumInfo);
        }

        protected void OnUntallyButtonClicked(object sender, EventArgs e)
        {
            if (_viewLoading) { return; }

            TallyAction selectedAction = _BS_tallyHistory.Current as TallyAction;

            if (selectedAction == null) { return; }
            if (MessageBox.Show("Are you sure you want to untally the selected record?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
                == DialogResult.No) { return; }

            DataService.CuttingUnit.TallyHistoryBuffer.Remove(selectedAction);
        }

        protected void AdjustPanelHeight(Panel panel)
        {
            int totalChildHeight = 0;
            foreach (Control c in panel.Controls)
            {
                totalChildHeight += c.Height;
            }
            panel.Height = totalChildHeight;
        }

        protected void DisplayTallyPanel(Stratum stratumInfo)
        {
            System.Diagnostics.Debug.Assert(StrataViews.ContainsKey(stratumInfo));
            if (!StrataViews.ContainsKey(stratumInfo)) { return; }

            Panel tallyContainer = StrataViews[stratumInfo];

            // if strata is already displayed
            if (_visableTallyPanel != null
                && _visableTallyPanel == tallyContainer
                && tallyContainer.Visible == true)
            {
                // toggle off visability
                _visableTallyPanel.Visible = false;
                this.SelectedStratum = null;
                _visableTallyPanel = null;
                return;
            }
            else if (_visableTallyPanel != null
                && _visableTallyPanel != tallyContainer)
            {
                // hide current stratum
                _visableTallyPanel.Visible = false;
            }

            this.SelectedStratum = stratumInfo;
            _visableTallyPanel = tallyContainer;
            if (_visableTallyPanel != null)
            {
                _visableTallyPanel.Visible = true;
            }
        }

        #region ITallyView members

        public FormDataEntryLogic DataEntryController { get; protected set; }

        public Dictionary<char, CountTree> HotKeyLookup
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

        public bool HotKeyEnabled
        {
            get { return true; }
        }

        //public bool HandleHotKeyFirst(char key)
        //{
        //    key = char.ToUpper(key);
        //    if (StrataHotKeyLookup.ContainsKey(key))
        //    {
        //        DisplayTallyPanel(StrataHotKeyLookup[key]);
        //        return true;
        //    }
        //    return false;

        //    //return Controller.ProcessHotKey(key, this);
        //}

        public virtual void MakeSGList(IEnumerable<SampleGroup> sampleGroups, Panel container)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (NotImplementedException)
            { }
        }

        public Control MakeTallyRow(Control container, CountTree count)
        {
            var row = new TallyRow(count);
            row.SuspendLayout();

            row.TallyButtonClicked += new EventHandler(this.OnTallyButtonClicked);
            row.SettingsButtonClicked += new EventHandler(this.OnTallySettingsClicked);

            row.Parent = container;
            row.AdjustHeight();

            row.Dock = DockStyle.Top;
            row.ResumeLayout(false);
            return row;
        }

        public virtual Control MakeTallyRow(Control container, FSCruiser.Core.Models.SubPop subPop)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (NotImplementedException)
            { }
            return null;
        }

        public void OnTally(FSCruiser.Core.Models.CountTree count)
        {
            if (_viewLoading) { return; }
            this.DataEntryController.OnTally(count);
            this._BS_tallyHistory.MoveLast();
        }

        public void SaveCounts()
        {
            foreach (Stratum stratum in Strata)
            {
                stratum.SaveCounts();
            }
        }

        public bool TrySaveCounts()
        {
            bool success = true;
            foreach (Stratum stratum in Strata)
            {
                if (!stratum.TrySaveCounts())
                {
                    System.Diagnostics.Debug.Fail("unable to save St:"
                        + stratum.Code + " counts");
                    success = false;
                }
            }
            return success;
        }

        #endregion ITallyView members

        #region IDataEntryPage Members

        bool _viewLoading = true;

        public bool ViewLoading { get { return _viewLoading; } }

        public void HandleLoad()
        {
            this.InitializeStrataViews();

            _BS_tallyHistory.DataSource = DataService.CuttingUnit.TallyHistoryBuffer;
            this._viewLoading = false;
        }

        public bool PreviewKeypress(string keyStr)
        {
            if (string.IsNullOrEmpty(keyStr)) { return false; }

            if (keyStr == ApplicationSettings.Instance.JumpTreeTallyKeyStr)
            {
                this.DataEntryController.View.GotoTreePage();
                return true;
            }
            else if (keyStr == ApplicationSettings.Instance.UntallyKeyStr)
            {
                OnUntallyButtonClicked(null, null);
                return true;
            }
            else if (keyStr.Length == 1)
            {
                var keyChar = keyStr.First();
                if (StrataHotKeyLookup.ContainsKey(keyChar))
                {
                    DisplayTallyPanel(StrataHotKeyLookup[keyChar]);
                    return true;
                }
            }
            return false;
        }

        public void NotifyEnter()
        { /*do nothing */}

        #endregion IDataEntryPage Members
    }
}