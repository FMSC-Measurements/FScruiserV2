using System;

using System.Collections.Generic;
using System.Text;
using CruiseDAL.DataObjects;

namespace FSCruiser.Core.Models
{
    public interface IFixCNTTallyPopulation
    {
        //string SpeciesName { get; set; }

        IFixCNTTallyClass TallyClass { get; set; }

        TreeDefaultValueDO TreeDefaultValue { get; set; }
        SampleGroupDO SampleGroup { get; set; }

        double IntervalSize { get; set; }
        double Min { get; set; }
        double Max { get; set; }

        IList<FixCNTTallyBucket> Buckets { get; set; }

    }

    public class FixCNTTallyPopulation : IFixCNTTallyPopulation
    {

        #region IFixCNTTallyPopulation Members

        public TreeDefaultValueDO TreeDefaultValue
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public SampleGroupDO SampleGroup
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double IntervalSize
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double Min
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double Max
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IList<FixCNTTallyBucket> Buckets
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
