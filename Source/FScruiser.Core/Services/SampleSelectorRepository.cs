﻿using FMSC.Sampling;
using FScruiser.Data;
using FScruiser.Sampling;
using FScruiser.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FScruiser.Services
{
    public class SampleSelectorRepository : ISampleSelectorRepository
    {

        private Dictionary<string, ISampleSelector> _sampleSelectors = new Dictionary<string, ISampleSelector>();

        public SampleSelectorRepository(ISamplerInfoDataservice dataservice)
        {
            if(dataservice == null) { throw new ArgumentNullException("dataservice"); }
            Dataservice = dataservice;
        }

        public ISamplerInfoDataservice Dataservice { get; set; }

        public ISampleSelector GetSamplerBySampleGroupCode(string stratumCode, string sgCode)
        {
            if (string.IsNullOrEmpty(stratumCode)) { throw new ArgumentException("'stratumCode' cannot be null or empty", "stratumCode"); }
            if (string.IsNullOrEmpty(sgCode)) { throw new ArgumentException("'sgCode' cannot be null or empty", "sgCode"); }

            var key = stratumCode + "/" + sgCode;

            if (_sampleSelectors.ContainsKey(key) == false)
            {
                var samplerInfo = Dataservice.GetSamplerInfo(stratumCode, sgCode);

                var sampler = MakeSampleSelecter(samplerInfo);
                sampler.StratumCode = stratumCode;
                sampler.SampleGroupCode = sgCode;

                _sampleSelectors.Add(key, sampler);
            }

            return _sampleSelectors[key];
        }

        public ISampleSelector MakeSampleSelecter(SamplerInfo samplerInfo)
        {
            if(samplerInfo == null) { throw new ArgumentNullException("samplerInfo"); }
            var method = samplerInfo.Method;

            if (samplerInfo.UseExternalSampler)
            {
                return new ExternalSampleSelectorPlaceholder()
                {
                    Frequency = samplerInfo.SamplingFrequency,
                    SampleGroupCode = samplerInfo.SampleGroupCode,
                    StratumCode = samplerInfo.StratumCode,
                };
            }

            switch (method)
            {
                case "100":
                case "FIX":
                case "PNT":
                case "FIXCNT":
                    {
                        return new HundredPCTSelector();
                    }

                case "STR":
                    {
                        var freq = samplerInfo.SamplingFrequency;
                        if (freq == 0) { return new ZeroFrequencySelecter(); }

                        var state = Dataservice.GetSamplerState(samplerInfo.StratumCode, samplerInfo.SampleGroupCode);

                        var sampleSelectorType = (state != null && state.SampleSelectorType != null && state.Counter > 0) ? 
                            state.SampleSelectorType 
                            : samplerInfo.SampleSelectorType;
                        // default sample selector for STR is blocked
                        if (sampleSelectorType == CruiseDAL.Schema.CruiseMethods.SYSTEMATIC_SAMPLER_TYPE)
                        { return MakeSystematicSampleSelector(state, samplerInfo); }
                        else
                        { return MakeBlockSampleSelector(state, samplerInfo); }
                    }
                case "S3P":
                    {
                        return MakeS3PSampleSelector(samplerInfo);
                    }
                case "3P":
                case "P3P":
                case "F3P":
                    {
                        return MakeThreePSampleSelector(samplerInfo);
                    }
                case "FCM":
                case "PCM":
                    {
                        var freq = samplerInfo.SamplingFrequency;
                        if (freq == 0) { return new ZeroFrequencySelecter(); }

                        return MakeSystematicSampleSelector(samplerInfo);
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        public ISampleSelector MakeS3PSampleSelector(SamplerInfo samplerState)
        {
            var state = Dataservice.GetSamplerState(samplerState.StratumCode, samplerState.SampleGroupCode);

            if (state != null)
            {
                return new S3PSelector(samplerState.SamplingFrequency, samplerState.KZ, state.Counter, state.BlockState);
            }
            else
            {
                return new S3PSelector(samplerState.SamplingFrequency, samplerState.KZ);
            }

        }

        public ISampleSelector MakeThreePSampleSelector(SamplerInfo samplerInfo)
        {
            var state = Dataservice.GetSamplerState(samplerInfo.StratumCode, samplerInfo.SampleGroupCode);

            if (state != null)
            {
                return new ThreePSelecter(samplerInfo.KZ,
                    samplerInfo.InsuranceFrequency,
                    state.Counter,
                    state.InsuranceIndex,
                    state.InsuranceCounter);
            }
            else
            {
                return new ThreePSelecter(samplerInfo.KZ, samplerInfo.InsuranceFrequency);
            }


        }

        public ISampleSelector MakeSystematicSampleSelector(SamplerInfo samplerInfo)
        {
            var state = Dataservice.GetSamplerState(samplerInfo.StratumCode, samplerInfo.SampleGroupCode);
            return MakeSystematicSampleSelector(state, samplerInfo);
        }

        public ISampleSelector MakeSystematicSampleSelector(SamplerState state, SamplerInfo samplerInfo)
        {
            if (samplerInfo == null) { throw new ArgumentNullException("samplerInfo"); }

            var freq = samplerInfo.SamplingFrequency;
            if (state != null)
            {
                return new SystematicSelecter(freq,
                    samplerInfo.InsuranceFrequency,
                    state.Counter,
                    state.InsuranceIndex,
                    state.InsuranceCounter,
                    state.SystematicIndex);
            }
            else
            {
                return new SystematicSelecter(freq, samplerInfo.InsuranceFrequency, true);
            }
        }

        public ISampleSelector MakeBlockSampleSelector(SamplerInfo samplerInfo)
        {
            var state = Dataservice.GetSamplerState(samplerInfo.StratumCode, samplerInfo.SampleGroupCode);
            return MakeBlockSampleSelector(state, samplerInfo);
        }

        public ISampleSelector MakeBlockSampleSelector(SamplerState state, SamplerInfo samplerInfo)
        {
            if (samplerInfo == null) {  throw new ArgumentNullException("samplerInfo"); }

            var freq = samplerInfo.SamplingFrequency;

            if (state == null)
            {
                return new BlockSelecter(freq, samplerInfo.InsuranceFrequency);
            }
            else
            {
                return new BlockSelecter(freq,
                    samplerInfo.InsuranceFrequency,
                    state.BlockState,
                    state.Counter,
                    state.InsuranceIndex,
                    state.InsuranceCounter);
            }
        }

        public void SaveSamplerStates()
        {
            foreach (var sampler in _sampleSelectors.Values.Select(x => x))
            {
                //if(sampler is ZeroFrequencySelecter)    { continue; }
                //if(sampler is HundredPCTSelector)       { continue; }

                SaveSampler(sampler);
            }
        }

        public void SaveSampler(ISampleSelector sampler)
        {
            var state = new SamplerState(sampler);
            Dataservice.UpsertSamplerState(state);
        }
    }
}