using System;

using System.Collections.Generic;
using System.Text;

namespace FSCruiser.Core.Models
{
    public class FixCNTPlot : PlotVM 
    {
        public Dictionary<FixCNTTallyPopulation
            , Dictionary<int, FixCNTTallyBucket>> TallyBuckets { get; protected set; }



    }
}
