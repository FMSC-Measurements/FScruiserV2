using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class TallyRow : UserControl
    {
        public Button TallyButton { get { return this._tallyBTN; } }
        public Label DiscriptionLabel { get { return this._descriptionLBL; } }
        public Button SettingsButton { get { return this._settingsBTN; } }
        public Label HotKeyLabel { get { return this._hotKeyLBL; } }

        public TallyRow()
        {
            InitializeComponent();
        }
    }
}
