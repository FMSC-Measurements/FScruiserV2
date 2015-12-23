using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FSCruiser.Core.ViewInterfaces;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class FormNumPad : Form, INumPad
    {
        private int? _minValue;
        private int? _maxValue;
        private bool _canReturnNull = false;

        public FormNumPad()
        {
            InitializeComponent();
        }

        #region INumPad Members

        public DialogResult ShowDialog(int? initialValue, bool canReturnNull)
        {
            return this.ShowDialog(null, null, initialValue, canReturnNull);
        }

        public DialogResult ShowDialog(int? min, int? max, int? initialValue, bool canReturnNull)
        {
            if (min != null && min > 0 && min < max)
            {
                this._minValue = min;
            }
            else
            {
                min = null;
            }
            if (max != null && max > 0 && max > min)
            {
                this._maxValue = max;
            }
            else
            {
                this._maxValue = null;
            }
            this.UserEnteredValue = initialValue;
            this._canReturnNull = canReturnNull;
            return this.ShowDialog();
        }

        public virtual int? UserEnteredValue
        {
            get
            {
                if (String.IsNullOrEmpty(_outputView.Text))
                {
                    return null;
                }

                return Convert.ToInt32(_outputView.Text);
            }
            set
            {
                _outputView.Text = (value != null) ? value.Value.ToString() : String.Empty;
            }
        }

        #endregion
    }
}
