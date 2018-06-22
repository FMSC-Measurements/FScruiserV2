using FluentAssertions;
using FSCruiser.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FScruiser.Core.Test.Models
{
    public class RegionLogInfoTest
    {
        [Fact]
        public void GetLogRule_with_species_not_set()
        {
            var inst = new RegionLogInfo();

            var logRule = new LogRule();
            logRule.Add(new LogHeightClass(0, 0, 0));

            inst.AddRule(logRule);
            inst.GetLogRule(null).Should().NotBeNull();
            inst.GetLogRule(string.Empty).Should().NotBeNull();

            inst.GetLogRule("something").Should().BeNull();

        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void GetLogRule_with_blank_speckes(string species)
        {
            var inst = new RegionLogInfo();

            var logRule = new LogRule(species);
            logRule.Add(new LogHeightClass(0, 0, 0));

            inst.AddRule(logRule);
            inst.GetLogRule(null).Should().NotBeNull("null value");
            inst.GetLogRule("").Should().NotBeNull("empty value");
            inst.GetLogRule(" ").Should().NotBeNull("white space");

            inst.GetLogRule("something").Should().BeNull();
        }

        [Theory]
        [InlineData("1", true, "01")]
        [InlineData("1", true, " 01 ")]
        [InlineData("1", true, "1")]
        [InlineData("01", true, "1")]
        [InlineData("1", true, "001")]
        [InlineData("1", true, " 001")]
        [InlineData("1", false, " 10")]
        public void GetLogRule(string speciesIn, bool shouldReturnValue, params string[] speciesOut)
        {
            var inst = new RegionLogInfo();

            var logRule = new LogRule(speciesIn);
            logRule.Add(new LogHeightClass(0, 0, 0));

            inst.AddRule(logRule);

            foreach (var sp in speciesOut)
            {
                var value = inst.GetLogRule(sp);
                if (shouldReturnValue)
                {
                    value.Should().NotBeNull();
                }
                else
                {
                    value.Should().BeNull();
                }
            }
        }

        [Fact]
        public void CreateTest()
        {
            var inst = new RegionLogInfo();

            var logRule = new LogRule("some thing");
            logRule.Add(new LogHeightClass(0, 0, 0));

            inst.AddRule(logRule);

            inst.GetLogRule("something").Should().BeNull();
            inst.GetLogRule("thing").Should().NotBeNull();
            inst.GetLogRule("some").Should().NotBeNull();

            inst.GetLogRule(null).Should().BeNull();
            inst.GetLogRule(string.Empty).Should().BeNull();

            var logRule2 = new LogRule();
            logRule2.Add(new LogHeightClass(0, 0, 0));

            inst.AddRule(logRule2);

            inst.GetLogRule(null).Should().NotBeNull();
            inst.GetLogRule(string.Empty).Should().NotBeNull();
        }

        [Fact]
        public void SerializeTest()
        {
            var logRule = Core.Services.RegionalLogRuleProvider.GetRegionLoginfo(5); 

            var output = JsonConvert.SerializeObject(logRule);

            output.Should().NotBeNullOrEmpty();
            output.Length.Should().BeGreaterThan(2, "otherwise its just an empty json obj");

            var result = JsonConvert.DeserializeObject<RegionLogInfo>(output);

            result.Rules.Should().HaveSameCount(logRule.Rules);
        }
    }
}