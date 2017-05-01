using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class FormLimitingDistance
    {
        #region Calculator

        LimitingDistanceCalculator _calculator;

        protected LimitingDistanceCalculator Calculator
        {
            get { return _calculator; }
            set
            {
                OnCalculatorChanging();
                _calculator = value;
                OnCalculatorChangeed();
            }
        }

        private void OnCalculatorChanging()
        {
            var calc = Calculator;
            if (calc != null)
            {
                calc.PropertyChanged -= _calculator_PropertyChanged;
            }
        }

        private void OnCalculatorChangeed()
        {
            var calc = Calculator;
            if (calc != null)
            {
                calc.PropertyChanged += _calculator_PropertyChanged;

                _BS_calculator.DataSource = calc;
            }
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

        #endregion Calculator

        #region Plot

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

        #endregion Plot

        public FormLimitingDistance(LimitingDistanceCalculator calculator) : this()
        {
            Calculator = calculator;
        }

        public FormLimitingDistance(float bafOrFPS, bool isVariableRadius) : this(new LimitingDistanceCalculator())
        {
            Calculator.IsVariableRadius = isVariableRadius;
            Calculator.BAForFPSize = bafOrFPS;
        }

        public string Report
        {
            get
            {
                return _calculator.GenerateReport();
            }
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

        private void _cancelMI_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}