using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using CruiseDAL.DataObjects;

namespace FSCruiser.Core.Models
{
    public interface IFixCNTTallyPopulation
    {
        //string SpeciesName { get; set; }

        IFixCNTTallyClass TallyClass { get; set; }

        long? ID { get; }

        long? SampleGroup_CN { get; }
        long? TreeDefaultValue_CN { get; }

        TreeDefaultValueDO TreeDefaultValue { get; }
        SampleGroupVM SampleGroup { get; }

        double IntervalSize { get; set; }
        double Min { get; set; }
        double Max { get; set; }

        ICollection<FixCNTTallyBucket> Buckets { get; }

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

        public SampleGroupVM SampleGroup
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

        ICollection<FixCNTTallyBucket> _buckets;
        public ICollection<FixCNTTallyBucket> Buckets
        {
            get
            {
                if (_buckets == null)
                {
                    _buckets = MakeTallyBuckets().ToArray();
                }
                return _buckets;
            }
        }

        #endregion

        IEnumerable<FixCNTTallyBucket> MakeTallyBuckets()
        {
            var interval = Min;
            do
            {
                var bucket = new FixCNTTallyBucket()
                {
                    IntervalValue = interval
                    ,TallyPopulation = this
                };

                interval += IntervalSize;

                yield return bucket;
            } while (interval <= Max);
        }
    }
}
