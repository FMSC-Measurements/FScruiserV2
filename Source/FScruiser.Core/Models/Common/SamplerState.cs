using FMSC.Sampling;
using FScruiser.Sampling;

namespace FScruiser.Models
{
    public class SamplerState
    {
        public SamplerState()
        {
        }

        public SamplerState(ISampleSelector sampler)
        {
            SampleSelectorType = sampler.GetType().Name;
            StratumCode = sampler.StratumCode;
            SampleGroupCode = sampler.SampleGroupCode;
            Counter = sampler.Count;
            InsuranceCounter = sampler.InsuranceCounter;
            InsuranceIndex = sampler.InsuranceIndex;

            var samplerName = sampler.GetType().Name;
            switch (samplerName)
            {
                case "SystematicSelecter":
                    {
                        SystematicIndex = ((SystematicSelecter)sampler).HitIndex;
                        break;
                    }
                case "BlockSelecter":
                    {
                        BlockState = ((BlockSelecter)sampler).BlockState;
                        break;
                    }
                case "ThreePSelecter":
                    {
                        break;
                    }
                case "S3PSelector":
                    {
                        BlockState = ((S3PSelector)sampler).BlockState;
                        break;
                    }
            }
        }


        public string StratumCode { get; set; }
        public string SampleGroupCode { get; set; }
        public string SampleSelectorType { get; set; }
        public string BlockState { get; set; }
        public int SystematicIndex { get; set; }
        public int Counter { get; set; }
        public int InsuranceIndex { get; set; }
        public int InsuranceCounter { get; set; }
    }
}
