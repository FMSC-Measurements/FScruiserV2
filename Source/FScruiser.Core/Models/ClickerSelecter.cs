using FMSC.Sampling;
using System;

namespace FSCruiser.Core.Models
{
    public class ClickerSelecter : IFrequencyBasedSelecter
    {
        public int Count { get; protected set; }

        public int ITreeFrequency => 0;

        public bool IsSelectingITrees => false;

        public int InsuranceCounter => default(int);

        public int InsuranceIndex => default(int);

        public string StratumCode { get; set; }
        public string SampleGroupCode { get; set; }

        public int Frequency => 1;

        public SampleResult Sample()
        {
            return SampleResult.M;
        }
    }
}