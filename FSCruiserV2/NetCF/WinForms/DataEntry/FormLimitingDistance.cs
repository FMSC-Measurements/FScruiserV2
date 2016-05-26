using System;
using System.Windows.Forms;
using FSCruiser.Core;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class FormLimitingDistance : Form
    {


        public FormLimitingDistance()
        {
            InitializeComponent();

            _calculator.LimitingDistanceChanged += new EventHandler(_calculator_LimitingDistanceChanged);

            _BS_calculator.DataSource = _calculator;

            _measureToCB.Items.Add("Face");
            _measureToCB.Items.Add("Center");

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

        LimitingDistanceCalculator _calculator = new LimitingDistanceCalculator();

        private double _limitingDistance;

        public bool IsVariableRadius { get; set; }

        public string LogMessage { get; set; }

        public DialogResult ShowDialog(float bafOrFPS, bool isVariableRadius, TreeVM optTree, out string logMessage)
        {
            _calculator.Reset();
            _calculator.BAForFPSize = bafOrFPS;
            _calculator.IsVariableRadius = isVariableRadius;

            _BS_calculator.ResetBindings(false);

            //initailize form state
            this._calculateMI.Enabled = false;
            this._bafOrfpsLBL.Text = (isVariableRadius) ? "BAF" : "FPS";

            var dialogResult = this.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                logMessage = _calculator.GenerateReport();
            }
            else
            {
                logMessage = string.Empty;
            }

            return dialogResult;
        }

        void UpdateTreeInOrOut()
        {
            if (_calculator.SlopeDistance <= 0
                || _calculator.LimitingDistance <= 0.0)
            {
                _treeIsLBL.Text = string.Empty;
            }

            // FSH 2409.12 35.22a
            var isTreeIn = _calculator.SlopeDistance <= _calculator.LimitingDistance;
            if (isTreeIn)
            {
                this._treeIsLBL.Text = "IN";
            }
            else
            {
                this._treeIsLBL.Text = "OUT";
            }
        }

        void UpdateLimitingDistance()
        {
            var limitingDistance = _calculator.LimitingDistance;
            if (limitingDistance <= 0
                || double.IsNaN(limitingDistance))
            {
                _calculateBTN.Enabled = _calculateMI.Enabled = false;

                this._limitingDistanceLBL.Text = string.Empty;
            }
            else
            {
                _calculateBTN.Enabled = _calculateMI.Enabled = true;

                this._limitingDistanceLBL.Text = string.Format(
                    "{0:F}' to {1} of tree"
                    , limitingDistance
                    , _calculator.MeasureToStr);
            }
        }

        void _calculator_LimitingDistanceChanged(object sender, EventArgs e)
        {
            UpdateLimitingDistance();
        }

        private void _calculateBTN_Click(object sender, EventArgs e)
        {
            UpdateTreeInOrOut();
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

        void _sip_EnabledChanged(object sender, EventArgs e)
        {
            this._sipPlaceholder.Height = (this._sip.Enabled) ? this._sip.Bounds.Height : 0;
        }
    }
}