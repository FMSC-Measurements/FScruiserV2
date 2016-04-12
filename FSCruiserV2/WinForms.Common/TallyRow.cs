using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class TallyRow : UserControl
    {
        protected class TallyRowButton
#if NetCF        
            : FMSC.Controls.Mobile.ButtonPanel
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

        public TallyRow(CountTreeVM count)
        {
            InitializeComponent();

            _tallyBTN.Click += new EventHandler(this.OnTallyButtonClicked);
            _settingsBTN.Click += new EventHandler(this.OnSettingsButtonClicked);
            this.Count = count;
            UpdateTallyButton();
        }

        void UpdateTallyButton()
        {
            System.Diagnostics.Debug.Assert(Count != null);
            var buttonText = string.Empty;
            if (Count.Tally.Hotkey != null && Count.Tally.Hotkey.Length > 0)
            {
                buttonText = "[" + Count.Tally.Hotkey.Substring(0, 1) +"] ";
            }
            buttonText += string.Format("{0}\r\n Count:{1}", Count.Tally.Description, Count.TreeCount);

            this._tallyBTN.Text = buttonText;
        }

        public void AdjustHeight()
        {
            
            var g = base.CreateGraphics();
            var fHeight = g.MeasureString("|", _tallyBTN.Font).Height;
            this.Height = (int)Math.Ceiling(2.2 * fHeight);
        }

        protected void OnTallyButtonClicked(object sender, EventArgs e)
        {
            if(TallyButtonClicked != null)
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

        void  Count_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TreeCount")
            {
                UpdateTallyButton();
            }
        }


        

        
    }
}
