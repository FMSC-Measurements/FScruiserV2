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
        IFixCNTTallyCountProvider _tallyCountProvider; 
        public IFixCNTTallyCountProvider TallyCountProvider 
        {
            get { return _tallyCountProvider; }
            set
            {
                if (_tallyCountProvider == value) { return; }
                UnWireTallyCountProvider();
                _tallyCountProvider = value;
                WireTallyCountProvider();
            }
        }

        

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

        void WireTallyCountProvider()
        {
            var tallyCountProvider = TallyCountProvider;
            if (tallyCountProvider != null)
            {
                tallyCountProvider.TallyCountChanged += new EventHandler<TallyCountChangedEventArgs>(tallyCountProvider_TallyCountChanged);
            }
        }

        void UnWireTallyCountProvider()
        {
            var tallyCountProvider = TallyCountProvider;
            if (tallyCountProvider != null)
            {
                tallyCountProvider.TallyCountChanged -= tallyCountProvider_TallyCountChanged;
            }
        }

        void tallyCountProvider_TallyCountChanged(object sender, TallyCountChangedEventArgs e)
        {
            if (e == null) { throw new ArgumentNullException("e"); }

            foreach (var c in Controls)
            {
                var tallyRow = c as FixCntTallyRow;
                if (tallyRow != null)
                { tallyRow.HandleTreeCountChanged(e); }
            }
        }

        public void NotifyTallyClicked(IFixCNTTallyBucket tallyBucket)
        {
            var ea = new FixCNTTallyEventArgs() { TallyBucket = tallyBucket };
            OnTallyClicked(ea);
        }

        public void OnTallyClicked(FixCNTTallyEventArgs e)
        {
            TallyCountProvider.Tally(e.TallyBucket);
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
