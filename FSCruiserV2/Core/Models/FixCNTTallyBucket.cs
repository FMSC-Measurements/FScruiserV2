using System;

using System.Collections.Generic;
using System.Text;

namespace FSCruiser.Core.Models
{
    public class FixCNTTallyBucket
    {
        public IFixCNTTallyPopulation TallyPopulation { get; set; }

        public double InvervalValue { get; set; }

        public int TreeCount { get; set; }

        public event EventHandler TreeCountChanged;

        protected void NotifyTreeCountChanged()
        {
            this.OnTreeCountChanged(new EventArgs());
        }

        protected void OnTreeCountChanged(EventArgs e)
        {
            if (TreeCountChanged != null)
            {
                this.TreeCountChanged(this, e);
            }
        }

    }
}
