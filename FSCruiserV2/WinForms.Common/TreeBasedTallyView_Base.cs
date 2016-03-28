using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FSCruiser.Core.ViewInterfaces;
using FSCruiser.Core.Models;
using FSCruiser.Core;
using FSCruiser.Core.DataEntry;
using FSCruiser.WinForms.DataEntry;

namespace FSCruiser.WinForms
{
    public partial class TreeBasedTallyView_Base : UserControl, ITallyView
    {
        #region Fields
        Panel _visableTallyPanel;
        
        #endregion

        #region properties
        public IApplicationController Controller { get; protected set; }

        public IDataEntryView DataEntryForm
        {
            get
            {
                return (FSCruiser.WinForms.DataEntry.FormDataEntry)this.TopLevelControl;
            }
        }

        public Dictionary<char, StratumVM> StrataHotKeyLookup { get; protected set; }
        public Dictionary<StratumVM, Panel> StrataViews { get; protected set; }

        public IList<StratumVM> Strata { get; protected set; }
        public StratumVM SelectedStratum { get; protected set; }

        protected Panel StrataViewContainer { get; set; }
        #endregion

        protected TreeBasedTallyView_Base()
        {
            this.StrataHotKeyLookup = new Dictionary<char, StratumVM>();
            this.StrataViews = new Dictionary<StratumVM, Panel>();

            InitializeComponent();
        }

        


        protected void Initialize(IApplicationController controller
            , FormDataEntryLogic dataEntryController, Panel strataViewContainer)
        {
            this.StrataViewContainer = strataViewContainer;
            this.DataEntryController = dataEntryController;
            this.Controller = controller;
            Strata = dataEntryController.Unit.TreeStrata;
        }

        protected void InitializeStrataViews()
        {
            this.SuspendLayout();

            this.PopulateStrata();

            //if there is only one strata in the unit 
            //display the counts for that stratum
            if (this.Strata.Count == 1)
            {
                this.DisplayTallyPanel(this.Strata[0]);
            }

            this.ResumeLayout(false);
        }

        private void PopulateStrata()
        {
            foreach (StratumVM stratum in this.Strata)
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

                DataEntryMode mode = stratum.GetDataEntryMode();
                this.DataEntryController.PopulateTallies(stratum
                    , mode
                    , DataEntryController.Unit
                    , tallyContainer
                    , this);

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

        #region overrid methods
        //protected override void OnKeyUp(KeyEventArgs e)
        //{
        //    base.OnKeyUp(e);
        //    char key = (char)e.KeyValue;
        //    e.Handled = this.DataEntryController.ProcessHotKey(key, this);
        //}

        #endregion

        //protected virtual Control MakeTallyRow(Control container, SubPop subPop)
        //{ }

        //protected virtual Control MakeTallyRow(Control container, CountTreeVM count)
        //{ }

        //protected virtual void MakeSGList(List<SampleGroupVM> list, Panel container)
        //{ }

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

            var tree = DataEntryController.Unit.CreateNewTreeEntry(subPop.SG.Stratum, subPop.SG, subPop.TDV, true);
            tree.TreeCount = 1;

            this.Controller.ViewController.ShowCruiserSelection(tree);

            DataEntryController.Unit.AddNonPlotTree(tree);
            DataEntryForm.GotoTreePage();
        }

        protected void OnTallySettingsClicked(object sender, EventArgs e)
        {
            TallyRow row = (TallyRow)sender;
            CountTreeVM count = row.Count;
            Controller.ViewController.ShowTallySettings(count);
            //row.DiscriptionLabel.Text = count.Tally.Description;
        }

        protected void OnTallyButtonClicked(object sender, EventArgs e)
        {            
            TallyRow row = (TallyRow)sender;
            CountTreeVM count = row.Count;
            OnTally(count);
        }

        protected void OnStrataButtonClicked(object sender, EventArgs e)
        {
            Button strataButton = (Button)sender;
            StratumVM stratumInfo = (StratumVM)strataButton.Tag;

            DisplayTallyPanel(stratumInfo);

        }

        protected void OnUntallyButtonClicked(object sender, EventArgs e)
        {
            if (_viewLoading) { return; }

            TallyAction selectedAction = _BS_tallyHistory.Current as TallyAction;

            if (selectedAction == null) { return; }
            if (MessageBox.Show("Are you sure you want to untally the selected record?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
                == DialogResult.No) { return; }

            DataEntryController.Unit.TallyHistoryBuffer.Remove(selectedAction);
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

        protected void DisplayTallyPanel(StratumVM stratumInfo)
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

        public virtual void MakeSGList(List<FSCruiser.Core.Models.SampleGroupVM> list, Panel container)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (NotImplementedException)
            { }
        }

        public Control MakeTallyRow(Control container, CountTreeVM count)
        {
            TallyRow row = new TallyRow(count);
            row.SuspendLayout();

            row.TallyButtonClicked += new EventHandler(this.OnTallyButtonClicked);
            row.SettingsButtonClicked += new EventHandler(this.OnTallySettingsClicked);

            row.Height = 56;
            row.Parent = container;

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

        public void OnTally(FSCruiser.Core.Models.CountTreeVM count)
        {
            if (_viewLoading) { return; }
            this.DataEntryController.OnTally(count);
            this._BS_tallyHistory.MoveLast();
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

        public void SaveCounts()
        {
            foreach (StratumVM stratum in Strata)
            {
                stratum.SaveCounts();
            }
        }

        public bool TrySaveCounts()
        {
            bool success = true;
            foreach (StratumVM stratum in Strata)
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

        #endregion

        #region IDataEntryPage Members
        bool _viewLoading = true;

        public bool ViewLoading { get { return _viewLoading; } }

        public void HandleLoad()
        {
            this.InitializeStrataViews();

            _BS_tallyHistory.DataSource = DataEntryController.Unit.TallyHistoryBuffer;
            this._viewLoading = false;
        }

        public bool PreviewKeypress(string key)
        {
            if (key == "Escape")//esc
            {
                this.DataEntryController.View.GotoTreePage();
                return true;
            }

            if (key.Length == 1)
            {
                var keyChar = char.ToUpper(key[0]);
                if (StrataHotKeyLookup.ContainsKey(keyChar))
                {
                    DisplayTallyPanel(StrataHotKeyLookup[keyChar]);
                    return true;
                }
            }
            return false;
        }

        //public bool HandleEscKey()
        //{
        //    this.DataEntryController.View.GotoTreePage();
        //    return true;
        //}

        #endregion
    }
}
