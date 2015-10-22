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
        public FormNumPad()
        {
            InitializeComponent();
        }

        #region INumPad Members

        public DialogResult ShowDialog(int? initialValue, bool canReturnNull)
        {
            throw new NotImplementedException();
        }

        public DialogResult ShowDialog(int? min, int? max, int? initialValue, bool canReturnNull)
        {
            throw new NotImplementedException();
        }

        public int? UserEnteredValue
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
