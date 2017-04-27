using System;
using System.ComponentModel;
using System.Windows.Forms;
using FSCruiser.Core.ViewInterfaces;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class Form3PNumPad : Form, INumPad
    {
        private int? _minValue;
        private int? _maxValue;
        private bool _canReturnNull = false;

        public Form3PNumPad()
        {
            InitializeComponent();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            // HACK we need to handle dialog keys manualy
            // otherwise the parent form recieves the key press too
            if (e.KeyData == Keys.Enter)
            {
                e.Handled = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else if (e.KeyData == Keys.Escape)
            {
                e.Handled = true;
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            else
            {
                base.OnKeyUp(e);
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            _outputView.Focus();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (this.DialogResult == DialogResult.Cancel)
            {
                this.UserEnteredValue = null;
                return;
            }

            if (!_canReturnNull && this.UserEnteredValue == null)
            {
                MessageBox.Show(this, "No Value Entered");
                e.Cancel = true;
            }

            if (this._outputView.Text == "STM")
            {
                return;
            }

            if (this._minValue != null && this.UserEnteredValue < this._minValue.Value)
            {
                MessageBox.Show("Must be Greater or Equal to " + this._minValue.ToString());
                e.Cancel = true;
            }
            else if (this._maxValue != null && this.UserEnteredValue > this._maxValue.Value)
            {
                MessageBox.Show("Must be Less Than or Equal to " + this._maxValue.ToString());
                e.Cancel = true;
            }
        }

        public DialogResult ShowDialog(int? initialValue, bool canReturnNull)
        {
            return this.ShowDialog(null, null, initialValue, canReturnNull);
        }

        public DialogResult ShowDialog(int? min, int? max, int? initialValue, bool canReturnNull)
        {
            _minValue = (min != null && min > 0 && min < max) ? min : null;

            _maxValue = (max != null && max > 0 && max > min) ? max : null;

            this.UserEnteredValue = initialValue;
            this._canReturnNull = canReturnNull;
            return this.ShowDialog();
        }

        public int? UserEnteredValue
        {
            get
            {
                if (String.IsNullOrEmpty(_outputView.Text))
                {
                    return null;
                }
                if (_outputView.Text == "STM")
                {
                    return -1;
                }
                return Convert.ToInt32(_outputView.Text);
            }
            set
            {
                _outputView.Text = (value != null) ? value.Value.ToString() : String.Empty;
            }
        }

        private void _stmBTN_Click(object sender, EventArgs e)
        {
            _outputView.Text = "STM";
        }

        private void _clearBTN_Click(object sender, EventArgs e)
        {
            _outputView.Text = string.Empty;
        }
    }
}