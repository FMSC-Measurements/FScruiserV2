using System;

using CruiseDAL.DataObjects;
using FMSC.ORM.EntityModel.Attributes;

namespace FSCruiser.Core.Models
{
    public class TallyCountChangedEventArgs : EventArgs
    {
        public IFixCNTTallyBucket TallyBucket { get; set; }

        public IFixCNTTallyCountProvider CountProvider { get; set; }
    }

    public interface IFixCNTTallyCountProvider
    {
        event EventHandler<TallyCountChangedEventArgs> TallyCountChanged;

        int GetTallyCount(IFixCNTTallyBucket tallyBucket);

        void Tally(IFixCNTTallyBucket tallyBucket);
    }

    public class FixCNTPlot : PlotVM, IFixCNTTallyCountProvider
    {
        public FixCNTPlot()
        { }

        public FixCNTPlot(CruiseDAL.DAL ds)
            : base(ds)
        { }

        public event EventHandler<TallyCountChangedEventArgs> TallyCountChanged;

        [IgnoreField]
        public new FixCNTStratum Stratum
        {
            get
            {
                return (FixCNTStratum)base.Stratum;
            }
            set
            {
                base.Stratum = value;
            }
        }

        public override StratumDO GetStratum()
        {
            if (DAL == null) { return null; }
            return DAL.ReadSingleRow<FixCNTStratum>(this.Stratum_CN);
        }

        public int GetTallyCount(IFixCNTTallyBucket tallyBucket)
        {
            int count = 0;
            var population = tallyBucket.TallyPopulation;
            var tallyClass = population.TallyClass;

            foreach (var tree in Trees)
            {
                if (tree.SampleGroup_CN == population.SampleGroup_CN
                    && tree.TreeDefaultValue_CN == population.TreeDefaultValue_CN
                    && tallyBucket.MidpointValue == tallyClass.GetTreeFieldValue(tree))
                {
                    count++;
                }
            }

            return count;
        }

        public override void PopulateTrees()
        {
            base.PopulateTrees();
        }

        protected void NotifyTallyCountChanged(IFixCNTTallyBucket tallyBucket)
        {
            var args = new TallyCountChangedEventArgs()
            {
                CountProvider = this
                ,
                TallyBucket = tallyBucket
            };
            OnTallyCountChanged(args);
        }

        protected void OnTallyCountChanged(TallyCountChangedEventArgs ea)
        {
            if (TallyCountChanged != null)
            {
                TallyCountChanged(this, ea);
            }
        }

        public void Tally(IFixCNTTallyBucket tallyBucket)
        {
            var tree = base.CreateNewTreeEntry(tallyBucket.TallyPopulation.SampleGroup,
                tallyBucket.TallyPopulation.TreeDefaultValue, true);

            tallyBucket.TallyPopulation.TallyClass.SetTreeFieldValue(tree, tallyBucket);
            if (tree.TrySave())
            {
                Trees.Add(tree);
            }

            NotifyTallyCountChanged(tallyBucket);
        }
    }
}