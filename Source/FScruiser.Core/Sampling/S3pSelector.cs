using FMSC.Sampling;
using System;

namespace FScruiser.Sampling
{
    public class S3PSelector : ISampleSelector
    {
        BlockSelecter _blockSelecter;
        ThreePSelecter _threePSelecter;

        protected BlockSelecter BlockSelecter { get { return _blockSelecter; } }

        protected ThreePSelecter ThreePSelecter { get { return _threePSelecter; } }

        public string BlockState { get { return BlockSelecter.BlockState; } }

        public int Count { get { return BlockSelecter.Count; } }

        public int ITreeFrequency { get { return 0; } }

        public bool IsSelectingITrees { get { return false; } }

        public int InsuranceCounter { get { return BlockSelecter.InsuranceCounter; } }

        public int InsuranceIndex { get { return BlockSelecter.InsuranceIndex; } }

        public string StratumCode 
        { 
            get { throw new NotImplementedException(); } 
            set { throw new NotImplementedException(); }
        }
        public string SampleGroupCode 
        { 
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public S3PSelector(int freq, int kz)
        {
            _blockSelecter = new BlockSelecter(freq, 0);
            _threePSelecter = new ThreePSelecter(kz, 0);
        }

        public S3PSelector(int freq, int kz, int count, string blockState)
        {
            _blockSelecter = new BlockSelecter(freq, 0, blockState, count, 0, 0);
            _threePSelecter = new ThreePSelecter(kz, 0);
        }

        public SampleResult Sample()
        {
            return BlockSelecter.Sample();
        }

        public SampleResult Sample(int kpi, out int rand)
        {
            return ThreePSelecter.Sample(kpi, out rand);
        }
    }
}
