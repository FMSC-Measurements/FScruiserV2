using System;

using System.Windows.Forms;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms.Common
{
    public partial class FixCNTTallyButton : UserControl
    {
        public FixCNTTallyButton()
        {
            InitializeComponent();
        }

        public FixCNTTallyButton(IFixCNTTallyBucket bucket, FixCNTTallyControl tallyLayout)
        {
            InitializeComponent();
            Click += new EventHandler(FixCNTTallyButton_Click);
            foreach (Control c in Controls)
            {
                c.Click += FixCNTTallyButton_Click;
            }

            TallyLayout = tallyLayout;
            Bucket = bucket;
        }

        void FixCNTTallyButton_Click(object sender, EventArgs e)
        {
            this.TallyLayout.NotifyTallyClicked(this.Bucket);
        }

        public FixCNTTallyControl TallyLayout { get; set; }

        IFixCNTTallyBucket _bucket;

        public IFixCNTTallyBucket Bucket
        {
            get { return _bucket; }
            set
            {
                OnBucketChanging();
                _bucket = value;
                OnBucketChanged();
            }
        }

        private void OnBucketChanged()
        {
            var intVal = (_bucket != null) ? _bucket.MidpointValue : 0;
            _bucketValue_LBL.Text = String.Format("{0}{1}", intVal,
                Bucket.Field == FixCNTTallyField.DBH ? "\"" : "'");
        }

        private void OnBucketChanging()
        {
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
        }

        public void HandleTreeCountChanged(TallyCountChangedEventArgs ea)
        {
            if (ea.TallyBucket != null
                && object.ReferenceEquals(ea.TallyBucket, this))
            { return; }

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