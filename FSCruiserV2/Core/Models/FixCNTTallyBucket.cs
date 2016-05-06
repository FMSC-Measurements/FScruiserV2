using System;

using System.Collections.Generic;
using System.Text;

namespace FSCruiser.Core.Models
{
    public interface IFixCNTTallyBucket
    {
        IFixCNTTallyPopulation TallyPopulation { get; set; }

        double IntervalValue { get; set; }

        FixCNTTallyField Field { get; set; }
    }


    public class FixCNTTallyBucket : IFixCNTTallyBucket
    {
        public IFixCNTTallyPopulation TallyPopulation { get; set; }

        public double IntervalValue { get; set; }

        public FixCNTTallyField Field { get; set; }

        //public int TreeCount { get; set; }

        //public event EventHandler TreeCountChanged;

        //protected void NotifyTreeCountChanged()
        //{
        //    this.OnTreeCountChanged(new EventArgs());
        //}

        //protected void OnTreeCountChanged(EventArgs e)
        //{
        //    if (TreeCountChanged != null)
        //    {
        //        this.TreeCountChanged(this, e);
        //    }
        //}

    }
}
