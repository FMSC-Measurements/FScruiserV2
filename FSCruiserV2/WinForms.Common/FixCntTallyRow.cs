using System;
using System.Windows.Forms;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms.Common
{
    public partial class FixCntTallyRow : UserControl
    {
        public FixCntTallyRow()
        {
            InitializeComponent();
        }

        public FixCntTallyRow(IFixCNTTallyPopulation population, FixCNTTallyControl tallyLayout)
        {
            InitializeComponent();

            TallyLayout = tallyLayout;

            _speciesName_LBL.Text = population.TreeDefaultValue.Species;
            this.SuspendLayout();
            foreach (var tally in population.Buckets)
            {
                var tallyButton = new FixCNTTallyButton(tally, tallyLayout);
                tallyButton.Dock = DockStyle.Right;
                _tallyContainer_PNL.Controls.Add(tallyButton);
            }
            ResumeLayout(false);
        }

        public FixCNTTallyControl TallyLayout { get; set; }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.Visible == false) { return; }

            InternalPerformLayout();
        }

        protected void InternalPerformLayout()
        {
            var tallyContainerWidth = _tallyContainer_PNL.Width;
            var tallyContainerChildCount = _tallyContainer_PNL.Controls.Count;

            var childWidth = (tallyContainerChildCount > 0) ? tallyContainerWidth / tallyContainerChildCount
                : 0;

            foreach (Control child in _tallyContainer_PNL.Controls)
            {
                child.Width = childWidth;
            }
        }

        public void HandleTreeCountChanged(TallyCountChangedEventArgs ea)
        {
            foreach (Control c in _tallyContainer_PNL.Controls)
            {
                var tallyButton = c as FixCNTTallyButton;
                if (tallyButton != null)
                { tallyButton.HandleTreeCountChanged(ea); }
            }
        }
    }
}