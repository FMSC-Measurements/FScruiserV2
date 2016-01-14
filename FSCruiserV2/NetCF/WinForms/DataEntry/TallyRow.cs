using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FMSC.Controls;
using FMSC.Controls.Mobile;
using CruiseDAL.DataObjects;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class TallyRow : UserControl
    {
        public event EventHandler TallyButtonClicked;
        public event EventHandler SettingsButtonClicked; 

        public ButtonPanel TallyButton { get { return this._tallyButton; } }
        public Label DiscriptionLabel { get { return this._discriptionLabel; } }
        public Button SettingsButton { get { return this._settingsButton; } }
        public Label HotKeyLabel { get { return this._hotKeyLabel; } }
        
        public CountTreeVM Count { get; set; }

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
            }
        }

        public TallyRow()
        {
            InitializeComponent();
            this._tallyButton.Text = "____";

        }


    }


}
