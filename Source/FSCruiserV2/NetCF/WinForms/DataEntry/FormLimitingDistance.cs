using System;
using System.Windows.Forms;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class FormLimitingDistance : Form
    {
        LimitingDistanceCalculator _calculator;

        public FormLimitingDistance(float bafOrFPS, bool isVariableRadius)
        {
            _calculator = new LimitingDistanceCalculator(isVariableRadius)
                {
                    BAForFPSize = bafOrFPS
                };

            InitializeComponent();

            //initailize form state
            this._calculateMI.Enabled = false;
            this._bafOrfpsLBL.Text = (_calculator.IsVariableRadius) ? "BAF" : "FPS";

            _calculator.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(_calculator_PropertyChanged);

            _BS_calculator.DataSource = _calculator;

            foreach (var i in LimitingDistanceCalculator.MEASURE_TO_OPTIONS)
            { _measureToCB.Items.Add(i); }

#if NetCF
            if (ViewController.PlatformType == FMSC.Controls.PlatformType.WinCE)
            {
                this.WindowState = FormWindowState.Maximized;
                this._ceControlPanel.Visible = true;
                this.Menu = null;
                this.mainMenu1.Dispose();
                this.mainMenu1 = null;
            }
            else if (ViewController.PlatformType == FMSC.Controls.PlatformType.WM)
            {
                this.components = this.components ?? new System.ComponentModel.Container();
                this._sip = new Microsoft.WindowsCE.Forms.InputPanel();
                this.components.Add(_sip);
                this._sip.EnabledChanged += new EventHandler(_sip_EnabledChanged);
            }
#endif
        }

        public bool IsVariableRadius { get; set; }

        public string Report
        {
            get
            {
                return _calculator.GenerateReport();
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            _BS_calculator.EndEdit();
            base.OnClosing(e);
        }

        private void _calculateBTN_Click(object sender, EventArgs e)
        {
            _BS_calculator.EndEdit();
            _BS_calculator.ResetBindings(false);
            //_calculator.Recalculate();
        }

        void _calculator_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "LimitingDistance")
            {
                UpdateLimitingDistance();
            }
        }

        private void _cancelMI_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
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

        void UpdateLimitingDistance()
        {
            var limitingDistance = _calculator.LimitingDistance;
            if (limitingDistance <= 0
                || double.IsNaN(limitingDistance))
            {
                _calculateBTN.Enabled = _calculateMI.Enabled = false;

                this.LimitingDistance.Text = string.Empty;
            }
            else
            {
                _calculateBTN.Enabled = _calculateMI.Enabled = true;

                this.LimitingDistance.Text = string.Format(
                    "{0:F}' to {1} of tree"
                    , limitingDistance
                    , _calculator.MeasureTo);
            }
        }
    }
}