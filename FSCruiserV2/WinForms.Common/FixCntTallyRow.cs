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
    public partial class FixCntTallyRow : UserControl
    {
        public FixCntTallyRow(IFixCNTTallyPopulation obj, FixCntTallyControl tallyLayout)
        {
            InitializeComponent();

            TallyLayout = tallyLayout;

            this.SuspendLayout();
            foreach (var tally in obj.Buckets)
            {
                var tallyButton = new FixCNTTallyButton(tally, tallyLayout);
                _tallyContainer_PNL.Controls.Add(tallyButton);
            }
            ResumeLayout(false);

        }


        public FixCntTallyControl TallyLayout { get; set; }


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
            foreach (Control c in Controls)
            {
                var tallyButton = c as FixCNTTallyButton;
                if (tallyButton != null)
                { tallyButton.HandleTreeCountChanged(ea); }
            }
        }

    }
}
