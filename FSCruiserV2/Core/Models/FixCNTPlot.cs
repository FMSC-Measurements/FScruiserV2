using System;

using System.Collections.Generic;
using System.Text;
using CruiseDAL.DataObjects;

namespace FSCruiser.Core.Models
{

    public class TallyCountChangedEventArgs : EventArgs
    {
        FixCNTTallyBucket TallyBucket;
    }

    public interface IFixCNTTallyProvider
    {
        event EventHandler TallyCountChanged;

        int GetTallyCount(FixCNTTallyBucket tallyBucket);

        void Tally(FixCNTTallyBucket tallyBucket);
        

    }

    public class FixCNTPlot : PlotVM 
    {
        public Dictionary<IFixCNTTallyPopulation
            , Dictionary<int, FixCNTTallyBucket>> TallyBuckets { get; protected set; }



    }
}
