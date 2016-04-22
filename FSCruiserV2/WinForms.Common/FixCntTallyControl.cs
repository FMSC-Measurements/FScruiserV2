using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms.Common
{

    public class FixCNTTallyEventArgs : EventArgs
    {
        public IFixCNTTallyBucket TallyBucket {get; set; }
    }


    public partial class FixCntTallyControl : UserControl
    {
        public IFixCNTTallyProvider TallyCountProvider { get; set; }

        public FixCntTallyControl(IFixCNTTallyPopulationProvider provider)
        {
            InitializeComponent();

            var tallyPopulations = provider.GetFixCNTTallyPopulations();

            foreach (var pop in tallyPopulations)
            {
                var tallyRow = new FixCntTallyRow(pop, this) { Dock = DockStyle.Top };
                this.Controls.Add(tallyRow);
            }

        }

        public void NotifyTallyClicked(IFixCNTTallyBucket tallyBucket)
        {
            var ea = new FixCNTTallyEventArgs() { TallyBucket = tallyBucket };
            OnTallyClicked(ea);
        }

        public void OnTallyClicked(FixCNTTallyEventArgs e)
        {

        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            InternalAjustRowHeights();
        }

        protected void InternalAjustRowHeights()
        {
            var layoutHeight = this.Height;
            var numTallyRows = Controls.Count;

            var rowHeight = layoutHeight / numTallyRows;

            foreach (Control row in Controls)
            {
                row.Height = rowHeight;
            }
        }

    }
}
