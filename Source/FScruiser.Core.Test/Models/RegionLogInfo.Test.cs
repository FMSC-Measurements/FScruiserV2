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
        public void CreateTestWithBlankSpecies()
        {
            var inst = new RegionLogInfo();

            var logRule = new LogRule();
            logRule.Add(new LogHeightClass(0, 0, 0));

            inst.AddRule(logRule);
            inst.GetLogRule(null).Should().NotBeNull();
            inst.GetLogRule(string.Empty).Should().NotBeNull();

            inst.GetLogRule("something").Should().BeNull();
            //-------------------------------------------------------
            inst = new RegionLogInfo();

            logRule = new LogRule(string.Empty);
            logRule.Add(new LogHeightClass(0, 0, 0));

            inst.AddRule(logRule);
            inst.GetLogRule(null).Should().NotBeNull();
            inst.GetLogRule(string.Empty).Should().NotBeNull();

            inst.GetLogRule("something").Should().BeNull();
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
            output.Length.Should().BeGreaterThan(2, "because otherwise its just an empty json obj");

            var result = JsonConvert.DeserializeObject<RegionLogInfo>(output);

            result.Rules.Should().HaveSameCount(logRule.Rules);
        }
    }
}