using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms.Common
{
    public partial class FixCNTForm : Form
    {
        FixCntTallyControl _tallyControl;

        FixCNTForm()
        {
            InitializeComponent();
        }

        public FixCNTForm(IFixCNTTallyPopulationProvider populationProvider) : this()
        {


            _tallyControl = new FixCntTallyControl(populationProvider);
            this.SuspendLayout();

            _tallyControl.Dock = System.Windows.Forms.DockStyle.Fill;
            _tallyControl.Location = new System.Drawing.Point(0, 0);
            _tallyControl.Name = "tallyControl";
            _tallyControl.TabIndex = 0;

            this.ResumeLayout(false);
        }



        public DialogResult ShowDialog(FixCNTPlot plot)
        {
            _tallyControl.TallyCountProvider = plot;

            return this.ShowDialog();
        }
    }
}
