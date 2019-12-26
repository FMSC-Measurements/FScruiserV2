using FMSC.Sampling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScruiser.Sampling
{
    public class ExternalSampleSelectorPlaceholder : IFrequencyBasedSelecter
    {
        public int Frequency { get; set; }

        public int Count { get; protected set; }

        public int ITreeFrequency { get { return 0; } }

        public bool IsSelectingITrees { get { return false; } }

        public int InsuranceCounter { get { return 0; } }

        public int InsuranceIndex { get { return 0; } }

        public string StratumCode { get; set; }
        public string SampleGroupCode { get; set; }

        public SampleResult Sample()
        {
            Count++;
            return SampleResult.M;
        }
    }
}
