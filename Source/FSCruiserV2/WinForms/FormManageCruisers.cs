using System;
using System.ComponentModel;
using System.Windows.Forms;
using FSCruiser.Core;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms
{
    public partial class FormManageCruisers : Form
    {
        private void _initialsTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.AddCruiser();
            }
        }
    }
}