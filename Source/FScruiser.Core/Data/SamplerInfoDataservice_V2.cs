using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CruiseDAL;
using CruiseDAL.Schema;
using FScruiser.Models;

namespace FScruiser.Data
{
    public class SamplerInfoDataservice_V2 : Dataservice_Base_V2, ISamplerInfoDataservice
    {
        public SamplerInfoDataservice_V2(DAL database) : base(database)
        {
        }

        public SamplerInfo GetSamplerInfo(string stratumCode, string sampleGroupCode)
        {
            return Database.Query<SamplerInfo>(
                "SELECT st.Code AS StratumCode, " +
                "sg.Code AS SampleGroupCode, " +
                "st.Method, " +
                String.Format("(CASE sg.SampleSelectorType WHEN '{0}' THEN 1 ELSE 0 END) AS UseExternalSampler,", CruiseMethods.CLICKER_SAMPLER_TYPE) +
                "sg.SamplingFrequency, " +
                "sg.InsuranceFrequency, " +
                "sg.KZ " +
                "FROM SampleGroup AS sg " +
                "JOIN Stratum AS st USING (Stratum_CN) " +
                "WHERE st.Code = @p1 AND sg.Code = @p2;",
                stratumCode, sampleGroupCode)
                .FirstOrDefault();
        }

        public SamplerState GetSamplerState(string stratumCode, string sampleGroupCode)
        {
            return Database.Query<SamplerState>(
                "SELECT st.Code AS StratumCode, " +
                "sg.Code AS SampleGroupCode, " +
                "ss.SampleSelectorType, " +
                "ss.BlockState, " +
                "ss.SystematicIndex, " +
                "ss.Counter, " +
                "ss.InsuranceIndex, " +
                "ss.InsuranceCounter " +
                "FROM SamplerState AS ss " +
                "JOIN SampleGroup AS sg USING (SampleGroup_CN) " +
                "JOIN Stratum AS st USING (Stratum_CN) " +
                "WHERE st.Code = @p1 AND sg.Code = @p2;",
                stratumCode, sampleGroupCode)
                .FirstOrDefault();
        }

        public void UpsertSamplerState(SamplerState samplerState)
        {
            Database.Execute2(
@"INSERT INTO SamplerState (
    SampleGroup_CN,
    SampleSelectorType,
    BlockState,
    SystematicIndex,
    Counter,
    InsuranceIndex,
    InsuranceCounter
) VALUES (
    (SELECT SampleGroup_CN 
        FROM SampleGroup AS sg 
        JOIN Stratum AS st USING (Stratum_CN) 
        WHERE st.Code = @StratumCode AND sg.Code = @SampleGroupCode) ,
    @SampleSelectorType,
    @BlockState,
    @SystematicIndex,
    @Counter,
    @InsuranceIndex,
    @InsuranceCounter
)
ON CONFLICT (SampleGroup_CN) DO
UPDATE SET
        BlockState = @BlockState,
        SystematicIndex = @SystematicIndex,
        Counter = @Counter,
        InsuranceIndex = @InsuranceIndex,
        InsuranceCounter = @InsuranceCounter
    WHERE SampleGroup_CN = (
        SELECT SampleGroup_CN 
            FROM SampleGroup AS sg 
            JOIN Stratum AS st USING (Stratum_CN)
            WHERE st.Code = @StratumCode AND sg.Code = @SampleGroupCode);",
                new
                {
                    samplerState.BlockState,
                    samplerState.Counter,
                    samplerState.InsuranceCounter,
                    samplerState.InsuranceIndex,
                    samplerState.SystematicIndex,
                    samplerState.SampleSelectorType,
                    samplerState.SampleGroupCode,
                    samplerState.StratumCode,
                }
            );
        }
    }
}
