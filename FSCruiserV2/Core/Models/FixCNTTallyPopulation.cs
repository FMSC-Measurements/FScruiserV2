using System.Collections.Generic;
using System.Linq;
using CruiseDAL.DataObjects;
using FMSC.ORM.EntityModel.Attributes;

namespace FSCruiser.Core.Models
{
    public interface IFixCNTTallyPopulation
    {
        //string SpeciesName { get; set; }

        long? TreeDefaultValue_CN { get; }

        TreeDefaultValueDO TreeDefaultValue { get; }

        long? SampleGroup_CN { get; }

        SampleGroupVM SampleGroup { get; }

        long? FixCNTTallyClass_CN { get; set; }

        IFixCNTTallyClass TallyClass { get; set; }

        double IntervalSize { get; set; }

        double Min { get; set; }

        double Max { get; set; }

        ICollection<FixCNTTallyBucket> Buckets { get; }
    }

    [EntitySource(SourceName = "FixCNTTallyPopulation")]
    public class FixCNTTallyPopulation : IFixCNTTallyPopulation
    {
        #region IFixCNTTallyPopulation Members

        [Field(Name = "SampleGroup_CN")]
        public long? SampleGroup_CN { get; set; }

        SampleGroupVM _sampleGroup;

        public SampleGroupVM SampleGroup { get; set; }

        [Field(Name = "TreeDefaultValue_CN")]
        public long? TreeDefaultValue_CN { get; set; }

        public TreeDefaultValueDO TreeDefaultValue { get; set; }

        [Field(Name = "FixCNTTallyClass_CN")]
        public long? FixCNTTallyClass_CN { get; set; }

        public IFixCNTTallyClass TallyClass { get; set; }

        [Field(Name = "IntervalSize")]
        public double IntervalSize { get; set; }

        [Field(Name = "Min")]
        public double Min { get; set; }

        [Field(Name = "Max")]
        public double Max { get; set; }

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

        #endregion IFixCNTTallyPopulation Members

        IEnumerable<FixCNTTallyBucket> MakeTallyBuckets()
        {
            var interval = Min + IntervalSize / 2;
            do
            {
                var bucket = new FixCNTTallyBucket()
                {
                    MidpointValue = interval,
                    TallyPopulation = this,
                    Field = TallyClass.Field
                };

                interval += IntervalSize;

                yield return bucket;
            } while (interval <= Max);
        }
    }
}