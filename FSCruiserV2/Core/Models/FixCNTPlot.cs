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
        event EventHandler<TallyCountChangedEventArgs> TallyCountChanged;

        int GetTallyCount(FixCNTTallyBucket tallyBucket);

        void Tally(FixCNTTallyBucket tallyBucket);
        

    }

    public class FixCNTPlot : PlotVM, IFixCNTTallyProvider
    {
        public event EventHandler<TallyCountChangedEventArgs> TallyCountChanged;

        public Dictionary<FixCNTTallyBucket, int> TallyCounts { get; protected set; }

        public int GetTallyCount(FixCNTTallyBucket tallyBucket)
        {
            return TallyCounts[tallyBucket];

            throw new NotImplementedException();
        }

        public void Tally(FixCNTTallyBucket tallyBucket)
        {

            var tree = base.CreateNewTreeEntry(tallyBucket.TallyPopulation.SampleGroup,
                tallyBucket.TallyPopulation.TreeDefaultValue, true);
            tallyBucket.TallyPopulation.TallyClass.SetTreeFieldValue(tree, tallyBucket);


            

            throw new NotImplementedException();
        }






    }
}
