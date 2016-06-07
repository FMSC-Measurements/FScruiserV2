using System;
using System.Drawing;
using System.Windows.Forms;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms.Common
{
    public partial class FixCNTTallyControl : UserControl
    {
        IFixCNTTallyPopulationProvider _populationProvider;

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

        public IFixCNTTallyPopulationProvider PopulationProvider
        {
            get { return _populationProvider; }
            set
            {
                OnPopulationProviderChanging();
                _populationProvider = value;
                OnPopulationProviderChanged();
            }
        }

        public FixCNTTallyControl()
        {
            InitializeComponent();
        }

        void OnPopulationProviderChanging()
        {
        }

        void OnPopulationProviderChanged()
        {
            if (_populationProvider != null)
            {
                SuspendLayout();
                var tallyPopulations = _populationProvider.GetFixCNTTallyPopulations();

                int rowCounter = 0;
                foreach (var pop in tallyPopulations)
                {
                    var tallyRow = new FixCntTallyRow(pop, this);
                    tallyRow.Dock = DockStyle.Top;
                    if (rowCounter++ % 2 == 0)
                    {
                        tallyRow.BackColor = SystemColors.ControlDark;
                    }
                    this.Controls.Add(tallyRow);
                }
                ResumeLayout(false);
            }
        }

        void WireTallyCountProvider()
        {
            var tallyCountProvider = TallyCountProvider;
            if (tallyCountProvider != null)
            {
                tallyCountProvider.TallyCountChanged += new EventHandler<TallyCountChangedEventArgs>(tallyCountProvider_TallyCountChanged);

                var updateEventArgs =
                    new TallyCountChangedEventArgs() { CountProvider = tallyCountProvider };

                UpdateTallyCount(updateEventArgs);
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
            UpdateTallyCount(e);
        }

        void UpdateTallyCount(TallyCountChangedEventArgs e)
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

            if (numTallyRows == 0) { return; }

            var rowHeight = layoutHeight / numTallyRows;

            foreach (Control row in Controls)
            {
                row.Height = rowHeight;
            }
        }
    }

    public class FixCNTTallyEventArgs : EventArgs
    {
        public IFixCNTTallyBucket TallyBucket { get; set; }
    }
}