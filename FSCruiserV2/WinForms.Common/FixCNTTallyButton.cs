using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms.Common
{
    public partial class FixCNTTallyButton : UserControl
    {
        public FixCNTTallyButton(IFixCNTTallyBucket bucket, FixCntTallyControl tallyLayout)
        {
            InitializeComponent();

            TallyLayout = tallyLayout;

            Bucket = bucket;

            _bucketValue_LBL.Text = bucket.IntervalValue.ToString() + "\"";

        }

        public FixCntTallyControl TallyLayout { get; set; }

        //FixCNTTallyBucket _bucket;
        public IFixCNTTallyBucket Bucket { get; set; }


        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            this.TallyLayout.NotifyTallyClicked(this.Bucket);
        }

        public void HandleTreeCountChanged(TallyCountChangedEventArgs ea)
        {
            if (ea.TallyBucket != null && ea.TallyBucket != this) { return; }

            var tallyCountProvider = ea.CountProvider;
            if (tallyCountProvider != null)
            {

                var tallyCount = tallyCountProvider.GetTallyCount(this.Bucket);
                _tallyCount_LBL.Text = tallyCount.ToString();
            }
            else
            {
                _tallyCount_LBL.Text = "-";
            }
        }
    }
}
