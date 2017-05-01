using System;
using System.Windows.Forms;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class FormLimitingDistance : Form
    {
        LimitingDistanceCalculator _calculator;

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
                _cancel_MI.Click += new System.EventHandler(this._cancel_MI_Click);
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

        public FormLimitingDistance(float bafOrFPS, bool isVariableRadius)
            : this()
        {
            _calculator.IsVariableRadius = isVariableRadius;
            _calculator.BAForFPSize = bafOrFPS;
        }

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

        public string Report
        {
            get
            {
                return _calculator.GenerateReport();
            }
        }

        public DialogResult ShowDialog(Plot plot)
        {
            var stratum = plot.Stratum;

            _calculator.IsVariableRadius = Array.IndexOf(CruiseDAL.Schema.CruiseMethods.VARIABLE_RADIUS_METHODS, stratum.Method) > -1;
            _calculator.BAForFPSize = (_calculator.IsVariableRadius) ? stratum.BasalAreaFactor : stratum.FixedPlotSize;

            return ShowDialog();
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

        private void _cancel_MI_Click(object sender, EventArgs e)
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
            var isLimitingDistanceValid = limitingDistance > 0
                && !double.IsNaN(limitingDistance);

            _limitingDistanceLBL.Text = String.Format("Limiting Distance: {0}"
                , (isLimitingDistanceValid) ? string.Format( "{0:F3}' to {1} of tree" , limitingDistance , _calculator.MeasureTo)
                : string.Empty);

            _calculateBTN.Enabled = _calculateBTN.Enabled = isLimitingDistanceValid;
        }
    }
}