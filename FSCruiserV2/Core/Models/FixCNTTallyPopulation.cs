using System;

using System.Collections.Generic;
using System.Text;
using CruiseDAL.DataObjects;

namespace FSCruiser.Core.Models
{
    public class FixCNTTallyPopulation
    {
        public TreeDefaultValueDO Species { get; set; }

        public double IntervalSize { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }

        public IList<FixCNTTallyBucket> Buckets { get; set; }
    }
}
