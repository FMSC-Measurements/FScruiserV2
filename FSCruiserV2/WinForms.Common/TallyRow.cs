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
        public event EventHandler TallyButtonClicked;
        public event EventHandler SettingsButtonClicked; 

        public Button SettingsButton { get { return this._settingsBTN; } }

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

        TallyRow()
        {
            InitializeComponent();
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
            buttonText += string.Format("{0}\r\n Tree Count:{1}", Count.Tally.Description, Count.TreeCount);

            this._tallyBTN.Text = buttonText;
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
