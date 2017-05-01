using System;
using System.Windows.Forms;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class FormLimitingDistance : Form
    {
        public FormLimitingDistance()
        {
            InitializeComponent();

            foreach (var i in LimitingDistanceCalculator.MEASURE_TO_OPTIONS)
            { _measureToCB.Items.Add(i); }

            //initialize form state
            this._calculateBTN.Enabled = false;

#if NetCF
            if (ViewController.PlatformType == FMSC.Controls.PlatformType.WM)
            {
                this.components = this.components ?? new System.ComponentModel.Container();
                this._sip = new Microsoft.WindowsCE.Forms.InputPanel();
                this.components.Add(_sip);
                this._sip.EnabledChanged += new EventHandler(_sip_EnabledChanged);
                _ceControlPanel.Visible = false;

                var mainMenu1 = new System.Windows.Forms.MainMenu();
                var _cancel_MI = new System.Windows.Forms.MenuItem();
                var _calculate_MI = new System.Windows.Forms.MenuItem();

                // 
                // mainMenu1
                // 
                mainMenu1.MenuItems.Add(_cancel_MI);
                mainMenu1.MenuItems.Add(_calculate_MI);
                // 
                // _cancel_MI
                // 
                _cancel_MI.Text = "Cancel";
                _cancel_MI.Click += new System.EventHandler(this._cancelMI_Click);
                // 
                // _calculate_MI
                // 
                _calculate_MI.Text = "Calculate";
                _calculate_MI.Click += new System.EventHandler(this._calculateBTN_Click);

                Menu = mainMenu1;
            }
            else
            {
                this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            }
#endif
        }

        void _sip_EnabledChanged(object sender, EventArgs e)
        {
            this._sipPlaceholder.Height = (this._sip.Enabled) ? this._sip.Bounds.Height : 0;
        }

        void _TB_GotFocus(object sender, EventArgs e)
        {
            var tb = sender as TextBox;
            if (tb == null) { return; }
            tb.BeginInvoke(new Action(tb.SelectAll));
        }
    }
}