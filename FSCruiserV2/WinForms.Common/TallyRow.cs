using System;
using System.ComponentModel;
using System.Windows.Forms;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class TallyRow : UserControl
    {
        protected class TallyRowButton
#if NetCF
 : FMSC.Controls.ButtonPanel
#else
 : Button
#endif
        {
            public TallyRowButton() : base() { }
        }

        public event EventHandler TallyButtonClicked;

        public event EventHandler SettingsButtonClicked;

        CountTreeVM _count;

        public CountTreeVM Count
        {
            get { return _count; }
            set
            {
                UnWireCount();
                _count = value;
                WireUpCount();
            }
        }

        public bool ShowTallyCount { get; set; }

        public TallyRow(CountTreeVM count, bool showTallyCount)
        {
            InitializeComponent();
            ShowTallyCount = showTallyCount;

            _tallyBTN.Click += new EventHandler(this.OnTallyButtonClicked);
            _settingsBTN.Click += new EventHandler(this.OnSettingsButtonClicked);
            this.Count = count;
            UpdateTallyButton();
        }

        void UpdateTallyButton()
        {
            System.Diagnostics.Debug.Assert(Count != null);
            var hotkey = (!String.IsNullOrEmpty(Count.Tally.Hotkey)) ?
                "[" + Count.Tally.Hotkey.Substring(0, 1) + "] "
                : String.Empty;

            if (ShowTallyCount)
            {
                this._tallyBTN.Text = string.Format("{0}{1}|{2}"
                    , hotkey
                    , Count.Tally.Description
                    , Count.TreeCount);
            }
            else
            {
                this._tallyBTN.Text = string.Format("{0}{1}"
                    , hotkey
                    , Count.Tally.Description);
            }
        }

        public void AdjustHeight()
        {
            var g = base.CreateGraphics();
            var fHeight = g.MeasureString("|", _tallyBTN.Font).Height;
            this.Height = (int)Math.Ceiling(fHeight) + 5;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AdjustHeight();
        }

        protected void OnTallyButtonClicked(object sender, EventArgs e)
        {
            if (TallyButtonClicked != null)
            {
                this.TallyButtonClicked(this, e);
            }
        }

        protected void OnSettingsButtonClicked(object sender, EventArgs e)
        {
            if (SettingsButtonClicked != null)
            {
                this.SettingsButtonClicked(this, e);
                UpdateTallyButton();
            }
        }

        void WireUpCount()
        {
            if (Count != null)
            {
                this.Count.PropertyChanged += new PropertyChangedEventHandler(Count_PropertyChanged);
            }
        }

        void UnWireCount()
        {
            if (Count != null)
            {
                Count.PropertyChanged -= Count_PropertyChanged;
            }
        }

        void Count_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TreeCount")
            {
                UpdateTallyButton();
            }
        }
    }
}