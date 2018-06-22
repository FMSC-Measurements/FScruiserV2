using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMSC.Sampling;
using FluentAssertions;
using Xunit;

namespace FSCruiser.Core.Models
{
    public class SampleGroupTest
    {
        [Fact]
        public void MakeSampleSelecterTest_100PCT()
        {
            var st = new Stratum() { Method = "100PCT" };

            var sg = new SampleGroup()
            {
                Stratum = st,
                SampleSelectorState = "something",
                SampleSelectorType = "something"
            };

            sg.SampleSelectorState = null;

            sg.MakeSampleSelecter().Should().BeNull();
            sg.Sampler.Should().BeNull();
        }

        [Fact]
        public void MakeSampleSelecterTest_STR()
        {
            var st = new Stratum() { Method = "STR" };

            //test: if sampling freq is 0
            //then Sampler is null
            var sg = new SampleGroup()
            {
                Stratum = st,
                SamplingFrequency = 0,
                SampleSelectorState = "something"
            };

            sg.MakeSampleSelecter().Should().BeNull();
            sg.Sampler.Should().BeNull();

            //test: if sampling freq is > 0
            //AND SampleSelectorType is not defined
            //THEN Sampler is not null
            //AND is of type Blocked
            sg = new SampleGroup()
            {
                Stratum = st,
                SamplingFrequency = 1,
                SampleSelectorState = "something"
            };

            sg.MakeSampleSelecter().Should().NotBeNull();
            sg.Sampler.Should().NotBeNull();
            sg.Sampler.Should().BeOfType<BlockSelecter>();

            var blockSampler = sg.Sampler as FMSC.Sampling.BlockSelecter;
            blockSampler.Frequency.Should().Be(1);

            //test: if sampling freq is > 0
            //AND SampleSelectorType is Systematic
            //THEN Sampler is not null
            //AND is of type Systematic
            sg = new SampleGroup()
            {
                Stratum = st,
                SamplingFrequency = 1,
                SampleSelectorType = "SystematicSelecter",
                SampleSelectorState = "something"
            };

            sg.MakeSampleSelecter().Should().NotBeNull();
            sg.Sampler.Should().NotBeNull();

            sg.Sampler.Should().BeOfType<SystematicSelecter>();

            var systmaticSampler = sg.Sampler as FMSC.Sampling.SystematicSelecter;
            systmaticSampler.Frequency.Should().Be(1);
        }

        [Fact]
        public void MakeSampleSelecterTest_STR_Clicker()
        {
            var st = new Stratum() { Method = "STR" };

            //test: if sampling freq is 0
            //then Sampler is null
            var sg = new SampleGroup()
            {
                Stratum = st,
                SamplingFrequency = 20,
                SampleSelectorType = CruiseDAL.Schema.CruiseMethods.CLICKER_SAMPLER_TYPE,
                SampleSelectorState = "something"
            };

            sg.MakeSampleSelecter().Should().BeOfType<ClickerSelecter>();
            sg.Sampler.Should().NotBeNull();

            sg.Sampler.Next().Should().BeTrue(); //clicker selector should always return true
            sg.Sampler.NextItem().IsSelected.Should().BeTrue();
        }

        [Fact]
        public void MakeSampleSelecterTest_FCM_PCM()
        {
            MakeSampleSelecterTest_FCM_PCM_helper("FCM");
            MakeSampleSelecterTest_FCM_PCM_helper("PCM");
        }

        void MakeSampleSelecterTest_FCM_PCM_helper(string method)
        {
            var st = new Stratum() { Method = method };

            //test: if sampling freq is 0
            //then Sampler is null
            var sg = new SampleGroup()
            {
                Stratum = st,
                SamplingFrequency = 0
            };

            sg.MakeSampleSelecter().Should().BeNull();
            sg.Sampler.Should().BeNull();

            //test: if sampling freq is > 0
            //AND SampleSelectorType is not defined
            //THEN Sampler is not null
            //AND is of type Systematic
            sg = new SampleGroup()
            {
                Stratum = st,
                SamplingFrequency = 1,
                InsuranceFrequency = 1
            };

            sg.MakeSampleSelecter().Should().NotBeNull();
            sg.Sampler.Should().NotBeNull();
            sg.Sampler.Should().BeOfType<SystematicSelecter>();

            var sampler = sg.Sampler as FMSC.Sampling.IFrequencyBasedSelecter;
            sampler.Frequency.Should().Be(1);
            sampler.ITreeFrequency.Should().Be(1);
        }

        [Fact]
        public void MakeSampleSelecterTest_3P()
        {
            var st = new Stratum()
            {
                Method = "3P"
            };

            var sg = new SampleGroup()
            {
                Stratum = st,
                SampleSelectorState = null,
                KZ = 100
            };

            sg.MakeSampleSelecter().Should().NotBeNull();
            sg.Sampler.Should().NotBeNull();
            sg.Sampler.Should().BeOfType<ThreePSelecter>();
            ((ThreePSelecter)sg.Sampler).KZ.Should().Be(100);
        }

        [Fact]
        public void MakeSampleSelecterTest_F3P()
        {
            var st = new Stratum()
            {
                Method = "F3P"
            };

            var sg = new SampleGroup()
            {
                Stratum = st,
                SampleSelectorState = null,
                KZ = 100
            };

            sg.MakeSampleSelecter().Should().NotBeNull();
            sg.Sampler.Should().NotBeNull();
            sg.Sampler.Should().BeOfType<ThreePSelecter>();
            ((ThreePSelecter)sg.Sampler).KZ.Should().Be(100);
        }

        [Fact]
        public void MakeSampleSelecterTest_P3P()
        {
            var st = new Stratum()
            {
                Method = "P3P"
            };

            var sg = new SampleGroup()
            {
                Stratum = st,
                SampleSelectorState = null,
                KZ = 100
            };

            sg.MakeSampleSelecter().Should().NotBeNull();
            sg.Sampler.Should().NotBeNull();
            sg.Sampler.Should().BeOfType<ThreePSelecter>();
            ((ThreePSelecter)sg.Sampler).KZ.Should().Be(100);
        }
    }
}