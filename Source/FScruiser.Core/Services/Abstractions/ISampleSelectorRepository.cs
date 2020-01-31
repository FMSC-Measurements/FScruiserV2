using FMSC.Sampling;

namespace FScruiser.Services
{
    public interface ISampleSelectorRepository
    {
        ISampleSelector GetSamplerBySampleGroupCode(string stratumCode, string sgCode);

        void SaveSamplerStates();

        void SaveSampler(ISampleSelector sampler);
    }
}
