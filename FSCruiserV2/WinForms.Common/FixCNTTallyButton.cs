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
        public FixCNTTallyButton(FixCNTTallyBucket bucket, FixCntTallyControl tallyLayout)
        {
            InitializeComponent();

            TallyLayout = tallyLayout;

            Bucket = bucket;

            _bucketValue_LBL.Text = bucket.IntervalValue.ToString() + "\"";

        }

        public FixCntTallyControl TallyLayout { get; set; }

        //FixCNTTallyBucket _bucket;
        public FixCNTTallyBucket Bucket { get; set; }
        //{
        //    get { return _bucket; }
        //    set
        //    {
        //        if (_bucket != null)
        //        {
        //            UnWireTallyBucket(_bucket);
        //        }
        //        _bucket = value;
        //        if (_bucket != null)
        //        {
        //            WireUpTallyBucket(_bucket);
        //        }
        //    }
        //}

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            this.TallyLayout.NotifyTallyClicked(this.Bucket);
        }

        //protected void WireUpTallyBucket(FixCNTTallyBucket value)
        //{
        //    _bucketValue_LBL.Text = value.IntervalValue.ToString() + "\"";
        //    value.TreeCountChanged += new EventHandler(Bucket_TreeCountChanged);
        //}

        //protected void UnWireTallyBucket(FixCNTTallyBucket value)
        //{
        //    _bucketValue_LBL.Text = string.Empty;
        //    value.TreeCountChanged -= Bucket_TreeCountChanged;
        //}

        //void Bucket_TreeCountChanged(object sender, EventArgs e)
        //{
        //    UpdateTreeCount();
        //}

        public void UpdateTreeCount()
        {
            var tallyCountProvider = TallyLayout.TallyCountProvider;

            var tallyCount = tallyCountProvider.GetTallyCount(this.Bucket).ToString();
                _tallyCount_LBL.Text = (tallyCountProvider != null) ?
                    tallyCountProvider.GetTallyCount(this.Bucket).ToString() :
                    "no plot";
        }
    }
}
