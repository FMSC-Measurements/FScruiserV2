using System;
using System.Windows.Forms;
using FSCruiser.Core;
using FSCruiser.Core.Models;
using System.ComponentModel;

namespace FSCruiser.WinForms
{
    public partial class FormManageCruisers : Form
    {
        private void _initialsTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                this.AddCruiser();
                e.Handled = true;
            }
        }
    }
}