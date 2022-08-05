using CruiseDAL;
using CruiseDAL.V2.Models;
using FluentAssertions;
using FMSC.Sampling;
using FScruiser.Core.Test.Data;
using FScruiser.Data;
using FScruiser.Models;
using FScruiser.Sampling;
using FScruiser.Services;
using Moq;
using System;
using System.Linq;
using Xunit;
using CruiseMethods = CruiseDAL.Schema.CruiseMethods;

namespace FScruiser.Core.Test.Services
{
    public class SampleSelectorRepository_Tests : DataTestBase
    {
        public DAL CreateDataStore(string stCode, string sgCode,  string method, string sampleSelectorType = null)
        {
            sampleSelectorType ??= CruiseDAL.Schema.CruiseMethods.BLOCK_SAMPLER_TYPE;

            var ds = new DAL();

            var st = new Stratum
            {
                Code = stCode,
                Method = method,

            };
            ds.Insert(st);

            var sg = new SampleGroup
            {
                Code = sgCode,
                Stratum_CN = st.Stratum_CN.Value,
                CutLeave = "C",
                UOM = "something",
                PrimaryProduct = "something",
                SampleSelectorType = sampleSelectorType,
            };
            ds.Insert(sg);

            return ds;
        }


        [Fact]
        public void GetSamplerBySampleGroupCode_100pct()
        {
            var stCode = "00";
            var sgCode = "01";

            using (var db = CreateDataStore(stCode, sgCode, "100"))
            {
                var sids = new SamplerInfoDataservice_V2(db);
                var ssRepo = new SampleSelectorRepository(sids);

                var sampler = ssRepo.GetSamplerBySampleGroupCode(stCode, sgCode);
                sampler.Should().NotBeNull();

                sampler.StratumCode.Should().Be(stCode);
                sampler.SampleGroupCode.Should().Be(sgCode);

                sampler.Should().BeAssignableTo<IFrequencyBasedSelecter>();
                ((IFrequencyBasedSelecter)sampler).Sample().Should().Be(SampleResult.M);

                var samplerAgain = ssRepo.GetSamplerBySampleGroupCode(stCode, sgCode);
                samplerAgain.Should().BeSameAs(sampler);

                ssRepo.SaveSamplerStates();
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public void GetSamplerBySampleGroupCode_STR(int freq)
        {
            var stCode = "00";
            var sgCode = "01";
            var iFreq = 2;

            using (var db = CreateDataStore(stCode, sgCode, "STR"))
            {
                db.Execute($"UPDATE SampleGroup SET SamplingFrequency = @p2, InsuranceFrequency = @p3 WHERE Code = @p1;",
                    sgCode, freq, iFreq);

                var sids = new SamplerInfoDataservice_V2(db);
                var ssRepo = new SampleSelectorRepository(sids);

                var sampler = ssRepo.GetSamplerBySampleGroupCode(stCode, sgCode);
                sampler.Should().NotBeNull();

                sampler.StratumCode.Should().Be(stCode);
                sampler.SampleGroupCode.Should().Be(sgCode);

                sampler.Should().BeAssignableTo<IFrequencyBasedSelecter>();
                if (freq > 0)
                { sampler.Should().BeOfType<BlockSelecter>(); }
                else
                { sampler.Should().BeOfType<ZeroFrequencySelecter>(); }
                ((IFrequencyBasedSelecter)sampler).Sample().Should().NotBeNull();

                ((IFrequencyBasedSelecter)sampler).Sample().Should().NotBeNull();

                var samplerAgain = ssRepo.GetSamplerBySampleGroupCode(stCode, sgCode);
                samplerAgain.Should().BeSameAs(sampler);

                ssRepo.SaveSamplerStates();
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public void GetSamplerBySampleGroupCode_STR_Systematic(int freq)
        {
            var stCode = "00";
            var sgCode = "01";
            var iFreq = 2;

            using (var db = CreateDataStore(stCode, sgCode, "STR"))
            {
                db.Execute($"UPDATE SampleGroup SET SamplingFrequency = @p2, InsuranceFrequency = @p3, SampleSelectorType = @p4 WHERE Code = @p1;",
                    sgCode, freq, iFreq, CruiseDAL.Schema.CruiseMethods.SYSTEMATIC_SAMPLER_TYPE);

                var sids = new SamplerInfoDataservice_V2(db);
                var ssRepo = new SampleSelectorRepository(sids);

                var sampler = ssRepo.GetSamplerBySampleGroupCode(stCode, sgCode);
                sampler.Should().NotBeNull();

                sampler.StratumCode.Should().Be(stCode);
                sampler.SampleGroupCode.Should().Be(sgCode);

                sampler.Should().BeAssignableTo<IFrequencyBasedSelecter>();
                if (freq > 0)
                { sampler.Should().BeOfType<SystematicSelecter>(); }
                else
                { sampler.Should().BeOfType<ZeroFrequencySelecter>(); }
                ((IFrequencyBasedSelecter)sampler).Sample().Should().NotBeNull();

                var samplerAgain = ssRepo.GetSamplerBySampleGroupCode(stCode, sgCode);
                samplerAgain.Should().BeSameAs(sampler);

                ssRepo.SaveSamplerStates();
            }
        }

        [Fact]
        public void GetSamplerBySampleGroupCode_STR_externalSampler()
        {
            var stCode = "00";
            var sgCode = "01";
            var freq = 5;

            using (var db = CreateDataStore(stCode, sgCode, "STR"))
            {
                db.Execute($"UPDATE SampleGroup SET SamplingFrequency = @p2, SampleSelectorType = '{CruiseMethods.CLICKER_SAMPLER_TYPE}' WHERE Code = @p1;",
                    sgCode, freq);

                var sids = new SamplerInfoDataservice_V2(db);
                var ssRepo = new SampleSelectorRepository(sids);

                var sampler = ssRepo.GetSamplerBySampleGroupCode(stCode, sgCode);
                sampler.Should().NotBeNull();

                sampler.StratumCode.Should().Be(stCode);
                sampler.SampleGroupCode.Should().Be(sgCode);

                sampler.Should().BeAssignableTo<IFrequencyBasedSelecter>();
                ((IFrequencyBasedSelecter)sampler).Sample().Should().NotBeNull();

                var samplerAgain = ssRepo.GetSamplerBySampleGroupCode(stCode, sgCode);
                samplerAgain.Should().BeSameAs(sampler);

                ssRepo.SaveSamplerStates();
            }
        }

        [Fact]
        public void GetSamplerBySampleGroupCode_3P()
        {
            var stCode = "00";
            var sgCode = "01";
            var kz = 101;
            var kpi = 50;

            using (var db = CreateDataStore(stCode, sgCode, "3P"))
            {
                db.Execute($"UPDATE SampleGroup SET KZ = @p2 WHERE Code = @p1;",
                    sgCode, kz);

                var sids = new SamplerInfoDataservice_V2(db);
                var ssRepo = new SampleSelectorRepository(sids);

                var sampler = ssRepo.GetSamplerBySampleGroupCode(stCode, sgCode);
                sampler.Should().NotBeNull();

                sampler.StratumCode.Should().Be(stCode);
                sampler.SampleGroupCode.Should().Be(sgCode);

                sampler.Should().BeAssignableTo<IThreePSelector>();
                ((IThreePSelector)sampler).Sample(kpi).Should().NotBeNull();

                var samplerAgain = ssRepo.GetSamplerBySampleGroupCode(stCode, sgCode);
                samplerAgain.Should().BeSameAs(sampler);

                ssRepo.SaveSamplerStates();
            }
        }

        [Theory]
        [InlineData("F3P")]
        [InlineData("P3P")]
        public void GetSamplerBySampleGroupCode_3P_MockedDS(string method)
        {
            var stCode = "00";
            var sgCode = "01";
            var kz = 101;
            var kpi = 50;
            var iFreq = 2;

            var sInfo = new SamplerInfo()
            {
                KZ = kz,
                InsuranceFrequency = iFreq,
                SampleGroupCode = sgCode,
                StratumCode = stCode,
                Method = method,
            };

            var sState = new FScruiser.Models.SamplerState()
            {
                SampleGroupCode = sgCode,
                StratumCode = stCode,
            };

            var mockSids = new Mock<ISamplerInfoDataservice>();
            mockSids.Setup(x => x.GetSamplerInfo(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(sInfo);
            mockSids.Setup(x => x.GetSamplerState(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(sState);

            var ssRepo = new SampleSelectorRepository(mockSids.Object);

            var sampler = ssRepo.GetSamplerBySampleGroupCode(stCode, sgCode);
            sampler.Should().NotBeNull();

            sampler.StratumCode.Should().Be(stCode);
            sampler.SampleGroupCode.Should().Be(sgCode);

            sampler.Should().BeAssignableTo<IThreePSelector>();

            var threePSampler = sampler as IThreePSelector;
            threePSampler.Sample(kpi).Should().NotBeNull();

            var samplerAgain = ssRepo.GetSamplerBySampleGroupCode(stCode, sgCode);
            samplerAgain.Should().BeSameAs(sampler);
        }

        [Theory]
        [InlineData("STR")]
        public void GetSamplerBySampleGroupCode_STR_MockedDS(string method)
        {
            var stCode = "00";
            var sgCode = "01";
            var freq = 5;
            var iFreq = 2;
            var blockState = new String(BlockSelecter.GenerateBlock(freq).Select(x => x ? '-' : 'x').ToArray());

            var sInfo = new SamplerInfo()
            {
                SamplingFrequency = freq,
                InsuranceFrequency = iFreq,
                SampleGroupCode = sgCode,
                StratumCode = stCode,
                Method = method,
            };

            var sState = new FScruiser.Models.SamplerState()
            {
                SampleGroupCode = sgCode,
                StratumCode = stCode,
                SampleSelectorType = nameof(BlockSelecter),
                BlockState = blockState,
            };

            var mockSids = new Mock<ISamplerInfoDataservice>();
            mockSids.Setup(x => x.GetSamplerInfo(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(sInfo);
            mockSids.Setup(x => x.GetSamplerState(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(sState);

            var ssRepo = new SampleSelectorRepository(mockSids.Object);

            var sampler = ssRepo.GetSamplerBySampleGroupCode(stCode, sgCode);
            sampler.Should().NotBeNull();

            sampler.StratumCode.Should().Be(stCode);
            sampler.SampleGroupCode.Should().Be(sgCode);

            sampler.Should().BeAssignableTo<IFrequencyBasedSelecter>();

            var freqSampler = sampler as IFrequencyBasedSelecter;
            freqSampler.Frequency.Should().Be(freq);
            freqSampler.Sample().Should().NotBeNull();

            var samplerAgain = ssRepo.GetSamplerBySampleGroupCode(stCode, sgCode);
            samplerAgain.Should().BeSameAs(sampler);
        }

        [Theory]
        [InlineData("FCM")]
        [InlineData("PCM")]
        public void GetSamplerBySampleGroupCode_PlotSingleState_MockedDS(string method)
        {
            var stCode = "00";
            var sgCode = "01";
            var freq = 5;
            var iFreq = 2;

            var sInfo = new SamplerInfo()
            {
                SamplingFrequency = freq,
                InsuranceFrequency = iFreq,
                SampleGroupCode = sgCode,
                StratumCode = stCode,
                Method = method,
            };

            var sState = new FScruiser.Models.SamplerState()
            {
                SampleGroupCode = sgCode,
                StratumCode = stCode,
                SampleSelectorType = nameof(BlockSelecter),
            };

            var mockSids = new Mock<ISamplerInfoDataservice>();
            mockSids.Setup(x => x.GetSamplerInfo(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(sInfo);
            mockSids.Setup(x => x.GetSamplerState(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(sState);

            var ssRepo = new SampleSelectorRepository(mockSids.Object);

            var sampler = ssRepo.GetSamplerBySampleGroupCode(stCode, sgCode);
            sampler.Should().NotBeNull();

            sampler.StratumCode.Should().Be(stCode);
            sampler.SampleGroupCode.Should().Be(sgCode);

            sampler.Should().BeAssignableTo<IFrequencyBasedSelecter>();
            sampler.Should().BeOfType<SystematicSelecter>();

            var freqSampler = sampler as IFrequencyBasedSelecter;
            freqSampler.Frequency.Should().Be(freq);
            freqSampler.Sample().Should().NotBeNull();

            var samplerAgain = ssRepo.GetSamplerBySampleGroupCode(stCode, sgCode);
            samplerAgain.Should().BeSameAs(sampler);
        }

        [Theory]
        [InlineData("FCM")]
        [InlineData("PCM")]
        public void GetSamplerBySampleGroupCode_FCM_PCM(string method)
        {
            var stCode = "00";
            var sgCode = "01";

            using (var db = CreateDataStore(stCode, sgCode, method))
            {
                var sids = new SamplerInfoDataservice_V2(db);
                var ssRepo = new SampleSelectorRepository(sids);

                var sampler = ssRepo.GetSamplerBySampleGroupCode(stCode, sgCode);
                sampler.Should().NotBeNull();

                sampler.StratumCode.Should().Be(stCode);
                sampler.SampleGroupCode.Should().Be(sgCode);

                sampler.Should().BeAssignableTo<IFrequencyBasedSelecter>();
                ((IFrequencyBasedSelecter)sampler).Sample().Should().NotBeNull();

                var samplerAgain = ssRepo.GetSamplerBySampleGroupCode(stCode, sgCode);
                samplerAgain.Should().BeSameAs(sampler);
            }
        }

        [Theory]
        [InlineData("F3P")]
        [InlineData("P3P")]
        public void GetSamplerBySampleGroupCode_F3P_P3P(string method)
        {
            var stCode = "00";
            var sgCode = "01";
            var kz = 101;

            using (var db = CreateDataStore(stCode, sgCode, method))
            {
                db.Execute($"UPDATE SampleGroup SET KZ = @p2 WHERE Code = @p1;",
                    sgCode, kz);

                var sids = new SamplerInfoDataservice_V2(db);
                var ssRepo = new SampleSelectorRepository(sids);

                var sampler = ssRepo.GetSamplerBySampleGroupCode(stCode, sgCode);
                sampler.Should().NotBeNull();

                sampler.StratumCode.Should().Be(stCode);
                sampler.SampleGroupCode.Should().Be(sgCode);

                sampler.Should().BeAssignableTo<IThreePSelector>();
                ((IThreePSelector)sampler).Sample(50).Should().NotBeNull();

                var samplerAgain = ssRepo.GetSamplerBySampleGroupCode(stCode, sgCode);
                samplerAgain.Should().BeSameAs(sampler);
            }
        }

        [Theory]
        [InlineData("100")]
        [InlineData("FIX")]
        [InlineData("PNT")]
        [InlineData("FIXCNT")]
        [InlineData("STR")]
        [InlineData("S3P")]
        [InlineData("3P")]
        [InlineData("P3P")]
        [InlineData("F3P")]
        [InlineData("FCM")]
        [InlineData("PCM")]
        public void SaveSamplerStates(string method)
        {
            var freq = 5;
            var stCode = "00";
            var sgCode = "01";
            var iFreq = 2;

            using (var db = CreateDataStore(stCode, sgCode, method))
            {
                db.Execute($"UPDATE SampleGroup SET SamplingFrequency = @p2, InsuranceFrequency = @p3 WHERE Code = @p1;",
                    sgCode, freq, iFreq);

                var sids = new SamplerInfoDataservice_V2(db);
                var ssRepo = new SampleSelectorRepository(sids);

                var sampler = ssRepo.GetSamplerBySampleGroupCode(stCode, sgCode);
                sampler.Should().NotBeNull();

                // effect the sampler state
                if (sampler is IFrequencyBasedSelecter fbs)
                { fbs.Sample(); } 
                else if (sampler is IThreePSelector tps)
                { tps.Sample(10); }
                else { throw new Exception("unexpected sampler type"); }

                // verify that repo cache mechinism works and returns same instance of sampler
                var samplerAgain = ssRepo.GetSamplerBySampleGroupCode(stCode, sgCode);
                samplerAgain.Should().BeSameAs(sampler);

                // save sampler states and repopulate them
                ssRepo.SaveSamplerStates();

                // repopulate samplers using a new repo
                var ssRepo2 = new SampleSelectorRepository(sids);
                var samplerAgian2 = ssRepo2.GetSamplerBySampleGroupCode(stCode, sgCode);
                samplerAgian2.Should().BeEquivalentTo(sampler);

                // make sure it works
                if (samplerAgian2 is IFrequencyBasedSelecter fbs2)
                { fbs2.Sample(); }
                else if (sampler is IThreePSelector tps)
                { tps.Sample(10); }
                else { throw new Exception("unexpected sampler type"); }
            }
        }

        [Theory]
        [InlineData("100", null)]
        [InlineData("FIX", null)]
        [InlineData("PNT", null)]
        [InlineData("FIXCNT", null)]
        [InlineData("STR", null)]
        [InlineData("STR", "SystematicSelecter")]
        [InlineData("S3P", null)]
        [InlineData("3P", null)]
        [InlineData("P3P", null)]
        [InlineData("F3P", null)]
        [InlineData("FCM", null)]
        [InlineData("PCM", null)]
        public void SaveSamplerStates_WithoutTallying(string method, string sampleSelectorType)
        {
            var freq = 5;
            var stCode = "00";
            var sgCode = "01";
            var iFreq = 1;

            using (var db = CreateDataStore(stCode, sgCode, method))
            {
                if (sampleSelectorType != null)
                {
                    db.Execute($"UPDATE SampleGroup SET SamplingFrequency = @p2, InsuranceFrequency = @p3, SampleSelectorType = @p4 WHERE Code = @p1;",
                    sgCode, freq, iFreq, sampleSelectorType);
                }
                else
                {
                    db.Execute($"UPDATE SampleGroup SET SamplingFrequency = @p2, InsuranceFrequency = @p3 WHERE Code = @p1;",
                        sgCode, freq, iFreq);
                }

                var sids = new SamplerInfoDataservice_V2(db);
                var ssRepo = new SampleSelectorRepository(sids);

                var sampler = ssRepo.GetSamplerBySampleGroupCode(stCode, sgCode);
                sampler.Should().NotBeNull();

                var samplerAgain = ssRepo.GetSamplerBySampleGroupCode(stCode, sgCode);
                samplerAgain.Should().BeSameAs(sampler);

                // save sampler states and repopulate them
                ssRepo.SaveSamplerStates();

                // repopulate samplers using a new repo
                var ssRepo2 = new SampleSelectorRepository(sids);
                var samplerAgian2 = ssRepo2.GetSamplerBySampleGroupCode(stCode, sgCode);
                samplerAgian2.Should().BeEquivalentTo(sampler);

                // make sure it works
                if (samplerAgian2 is IFrequencyBasedSelecter fbs2)
                { fbs2.Sample(); }
                else if (sampler is IThreePSelector tps)
                { tps.Sample(10); }
                else { throw new Exception("unexpected sampler type"); }
            }
        }
    }
}
