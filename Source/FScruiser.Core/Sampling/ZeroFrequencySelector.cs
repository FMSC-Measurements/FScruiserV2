using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using FMSC.Sampling;

namespace FScruiser.Sampling
{
    public class ZeroFrequencySelecter : IFrequencyBasedSelecter
    {
        public int Frequency
        {
            get { return 0; }
        }

        public int Count { get; set; }

        public int ITreeFrequency { get { return 0; } }

        public bool IsSelectingITrees { get { return false; } }

        public int InsuranceCounter { get { return 0; } }

        public int InsuranceIndex { get { return 0; } }

        public string StratumCode { get; set; }
        public string SampleGroupCode { get; set; }

        public SampleResult Sample()
        {
            Count++;
            return SampleResult.C;
        }
    }
}
