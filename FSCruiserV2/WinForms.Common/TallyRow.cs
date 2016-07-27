using System;
using System.ComponentModel;
using System.Windows.Forms;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class TallyRow : UserControl, ITallyButton
    {
        CountTreeVM _count;

        public TallyRow(CountTreeVM count)
        {
            InitializeComponent();

            _tallyBTN.Click += new EventHandler(this.OnTallyButtonClicked);
            _settingsBTN.Click += new EventHandler(this.OnSettingsButtonClicked);
            this.Count = count;
            UpdateTallyButton();
        }

        public event EventHandler SettingsButtonClicked;

        public event EventHandler TallyButtonClicked;

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

        protected void OnSettingsButtonClicked(object sender, EventArgs e)
        {
            if (SettingsButtonClicked != null)
            {
                this.SettingsButtonClicked(this, e);
                UpdateTallyButton();
            }
        }

        protected void OnTallyButtonClicked(object sender, EventArgs e)
        {
            if (TallyButtonClicked != null)
            {
                this.TallyButtonClicked(this, e);
            }
        }

        void Count_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TreeCount")
            {
                UpdateTallyButton();
            }
        }

        void UnWireCount()
        {
            if (Count != null)
            {
                Count.PropertyChanged -= Count_PropertyChanged;
            }
        }

        void UpdateTallyButton()
        {
            System.Diagnostics.Debug.Assert(Count != null);
            var hotkey = (!String.IsNullOrEmpty(Count.Tally.Hotkey)) ?
                "[" + Count.Tally.Hotkey.Substring(0, 1) + "] "
                : String.Empty;

            this._tallyBTN.Text = string.Format("{0}{1}|{2}"
                    , hotkey
                    , Count.Tally.Description
                    , Count.TreeCount);
        }

        void WireUpCount()
        {
            if (Count != null)
            {
                this.Count.PropertyChanged += new PropertyChangedEventHandler(Count_PropertyChanged);
            }
        }

        protected class TallyRowButton
#if NetCF
 : FMSC.Controls.ButtonPanel
#else
 : Button
#endif
        {
            public TallyRowButton() : base() { }
        }
    }
}