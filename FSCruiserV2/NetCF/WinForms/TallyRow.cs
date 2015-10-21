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

namespace FSCruiserV2
{
    public partial class TallyRow : UserControl
    {
        public ButtonPanel TallyButton { get { return this._tallyButton; } }
        public Label DiscriptionLabel { get { return this._discriptionLabel; } }
        public Button SettingsButton { get { return this._settingsButton; } }
        public Label HotKeyLabel { get { return this._hotKeyLabel; } }
        
        public FSCruiserV2.Logic.CountTreeVM Count { get; set; }

        public TallyRow()
        {
            InitializeComponent();
            this._tallyButton.Text = "____";

        }


    }


}
