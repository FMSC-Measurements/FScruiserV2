using System;
using System.Windows.Forms;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class FormLimitingDistance : Form
    {
        LimitingDistanceCalculator _calculator;

        Plot _plot;

        public Plot Plot
        {
            get { return _plot; }
            set
            {
                _plot = value;
                OnPlotChanged();
            }
        }

        private void OnPlotChanged()
        {
            if (_plot != null)
            {
                var stratum = _plot.Stratum;

                _calculator.IsVariableRadius = Array.IndexOf(CruiseDAL.Schema.CruiseMethods.VARIABLE_RADIUS_METHODS, stratum.Method) > -1;
                _calculator.BAForFPSize = (_calculator.IsVariableRadius) ? stratum.BasalAreaFactor : stratum.FixedPlotSize;
            }
        }

        public FormLimitingDistance()
        {
            InitializeComponent();

            foreach (var i in LimitingDistanceCalculator.MEASURE_TO_OPTIONS)
            { _measureToCB.Items.Add(i); }

            //initialize form state
            this._calculateBTN.Enabled = false;

            _calculator = new LimitingDistanceCalculator();
            _calculator.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(_calculator_PropertyChanged);

            _BS_calculator.DataSource = _calculator;

#if NetCF
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            if (ViewController.PlatformType == FMSC.Controls.PlatformType.WM)
            {
                this.components = this.components ?? new System.ComponentModel.Container();
                this._sip = new Microsoft.WindowsCE.Forms.InputPanel();
                this.components.Add(_sip);
                this._sip.EnabledChanged += new EventHandler(_sip_EnabledChanged);
            }
#endif
        }

        public FormLimitingDistance(float bafOrFPS, bool isVariableRadius) : this()
        {
            _calculator.IsVariableRadius = isVariableRadius;
            _calculator.BAForFPSize = bafOrFPS;
        }

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
            else if (e.PropertyName == "IsVariableRadius")
            {
                _bafOrfpsLBL.Text = (_calculator.IsVariableRadius) ? "BAF" : "FPS";
            }
        }

        private void _cancelMI_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
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
            var isLimitingDistanceValid = limitingDistance > 0
                && !double.IsNaN(limitingDistance);

            _limitingDistanceLBL.Text = String.Format("Limiting Distance: {0}"
                , (isLimitingDistanceValid) ? string.Format("{0:F3}' to {1} of tree", limitingDistance, _calculator.MeasureTo)
                : string.Empty);

            _calculateBTN.Enabled = _calculateBTN.Enabled = isLimitingDistanceValid;
        }
    }
}