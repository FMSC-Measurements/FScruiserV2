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

        public Button TallyButton { get { return this._tallyBTN; } }
        public Label DiscriptionLabel { get { return this._descriptionLBL; } }
        public Button SettingsButton { get { return this._settingsBTN; } }
        public Label HotKeyLabel { get { return this._hotKeyLBL; } }

        public CountTreeVM Count { get; set; }

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
            }
        }
        

        public TallyRow()
        {
            InitializeComponent();
        }
    }
}
