using FScruiser.Models;

namespace FScruiser.Data
{
    public interface ISamplerInfoDataservice
    {
        SamplerInfo GetSamplerInfo(string stratumCode, string sampleGroupCode);
        SamplerState GetSamplerState(string stratumCode, string sampleGroupCode);
        void UpsertSamplerState(SamplerState samplerState);
    }
}
