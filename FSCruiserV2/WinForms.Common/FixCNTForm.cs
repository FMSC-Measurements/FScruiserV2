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
        IFixCNTTallyPopulationProvider _populationProvider;

        FixCNTForm()
        {            
            InitializeComponent();
        }

        public FixCNTForm(IFixCNTTallyPopulationProvider populationProvider)
            : this()
        {                        
            _tallyControl.PopulationProvider = populationProvider;
        }

        public DialogResult ShowDialog(FixCNTPlot plot)
        {
            _tallyControl.TallyCountProvider = plot;
            this.Text = string.Format("Stratum:{0} Plot:{1}", plot.Stratum.Code, plot.PlotNumber);
            return this.ShowDialog();
        }
    }
}
