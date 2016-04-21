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
        public FixCNTTallyButton(FixCNTTallyBucket bucket)
        {
            InitializeComponent();

            Bucket = bucket;

            _bucketValue_LBL.Text = bucket.InvervalValue.ToString() + "\"";

        }

        FixCNTTallyBucket _bucket;
        public FixCNTTallyBucket Bucket 
        {
            get { return _bucket; }
            set
            {
                if (_bucket != null)
                {
                    UnWireTallyBucket(_bucket);
                }
                _bucket = value;
                if (_bucket != null)
                {
                    WireUpTallyBucket(_bucket);
                }
            }
        }

        protected void WireUpTallyBucket(FixCNTTallyBucket value)
        {
            _bucketValue_LBL.Text = value.InvervalValue.ToString() + "\"";
            value.TreeCountChanged += new EventHandler(Bucket_TreeCountChanged);
        }

        protected void UnWireTallyBucket(FixCNTTallyBucket value)
        {
            _bucketValue_LBL.Text = string.Empty;
            value.TreeCountChanged -= Bucket_TreeCountChanged;
        }

        void Bucket_TreeCountChanged(object sender, EventArgs e)
        {
            UpdateTreeCount();
        }

        private void UpdateTreeCount()
        {
            _tallyCount_LBL.Text = this.Bucket.TreeCount.ToString();

        }
    }
}
