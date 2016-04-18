using System;

using System.Collections.Generic;
using System.Text;

namespace FSCruiser.Core.Models
{
    public class FixCNTTallyBucket
    {
        public FixCNTTallyPopulation TallyPopulation { get; set; }

        public PlotVM Plot { get; set; }

        public double InvervalValue { get; set; }

        public int TreeCount 
        {
            get { return Trees.Count; }
        }

        public IList<TreeVM> Trees { get; set; }

        public void Tally()
        {
            throw new NotImplementedException();
        }
    }
}
