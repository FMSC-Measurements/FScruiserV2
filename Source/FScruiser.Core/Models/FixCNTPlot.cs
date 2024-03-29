﻿using System;

using CruiseDAL.DataObjects;
using FMSC.ORM.EntityModel.Attributes;
using FScruiser.Core.Services;
using FMSC.ORM.Core;
using CruiseDAL;

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

        void Tally(IPlotDataService dataService, IFixCNTTallyBucket tallyBucket);
    }

    public class FixCNTPlot : Plot, IFixCNTTallyCountProvider
    {
        public FixCNTPlot()
        { }

        public FixCNTPlot(DAL ds)
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
            if(tallyBucket == null) { throw new ArgumentNullException("tallyBucket"); }

            var population = tallyBucket.TallyPopulation;
            if(population == null) { throw new ArgumentNullException("population"); }

            var tallyClass = population.TallyClass;
            if(tallyClass == null) { throw new ArgumentNullException("tallyClass"); }

            int count = 0;
            foreach (var tree in Trees)
            {
                if (tree.SampleGroup_CN == population.SampleGroup_CN
                    && tree.TreeDefaultValue_CN == population.TreeDefaultValue_CN
                    && tallyBucket.MidpointValue == tallyClass.GetTreeFieldValue(tree))
                {
                    count += (int)tree.TreeCount;
                }
            }

            return count;
        }

        //public override Tree CreateNewTreeEntry(SampleGroup sg, TreeDefaultValueDO tdv, bool isMeasure)
        //{
        //    return base.CreateNewTreeEntry(sg, tdv, false);
        //}

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

        public void Tally(IPlotDataService dataService, IFixCNTTallyBucket tallyBucket)
        {
            var tree = dataService.CreateNewTreeEntry(this, tallyBucket.TallyPopulation.SampleGroup,
                tallyBucket.TallyPopulation.TreeDefaultValue, false);

            tallyBucket.TallyPopulation.TallyClass.SetTreeFieldValue(tree, tallyBucket);
            if (tree.TrySave())
            {
                Trees.Add(tree);
            }

            NotifyTallyCountChanged(tallyBucket);
        }
    }
}