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
    public class LogHeightClassTest
    {
        [Fact]
        public void IsInRangeTest()
        {
            var from = 0.0f;
            var to = 10.0f;

            var lhc = new LogHeightClass(from, to, 10);

            lhc.IsInRange(from).Should().BeTrue("range is inclusive");
            lhc.IsInRange(to).Should().BeTrue("range is inclusive");

            lhc.IsInRange(from - .01f).Should().BeFalse();
            lhc.IsInRange(to + .01f).Should().BeFalse();
        }

        [Fact]
        public void GetDefaultLogCountTest()
        {
            var dbh = 5;
            var numLogsBase = 10;
            var numLogsExpected = numLogsBase;

            var lhc = new LogHeightClass(0, 0, numLogsBase);

            lhc.GetDefaultLogCount(dbh).ShouldBeEquivalentTo(numLogsExpected);
        }

        [Fact]
        public void GetDefaultLogCountTestWithBreaks()
        {
            var numLogsBase = 10;
            var breaks = new uint[] { 5, 10 };

            var lhc = new LogHeightClass(0, 0, numLogsBase).WithBreaks(breaks);

            ValidateLogCountWithBreaks(lhc, numLogsBase, breaks);

            lhc = new LogHeightClass(0, 0, numLogsBase).WithBreaks(breaks.Reverse().ToArray());

            ValidateLogCountWithBreaks(lhc, numLogsBase, breaks);
        }

        [Fact]
        public void SerializeTest()
        {
            var from = 0.0f;
            var to = 10.0f;
            var numLogsBase = 10;
            var breaks = new uint[] { 5, 10 };

            var lhc = new LogHeightClass(from, to, numLogsBase).WithBreaks(breaks);

            DoSerialize(from, to, numLogsBase, breaks, lhc);

            breaks = null;
            lhc = new LogHeightClass(from, to, numLogsBase);

            DoSerialize(from, to, numLogsBase, breaks, lhc);
        }

        private static void DoSerialize(float from, float to, int numLogsBase, uint[] breaks, LogHeightClass lhc)
        {
            var output = JsonConvert.SerializeObject(lhc);

            output.Should().NotBeEmpty();

            var result = JsonConvert.DeserializeObject<LogHeightClass>(output);

            result.Num16FtLogs.ShouldBeEquivalentTo(numLogsBase);
            result.From.ShouldBeEquivalentTo(from);
            result.To.ShouldBeEquivalentTo(to);
            if (breaks != null && breaks.Count() > 0)
            {
                result.Breaks.Should().NotBeNullOrEmpty();
                result.Breaks.Should().HaveSameCount(breaks);
            }
            else
            {
                result.Breaks.Should().BeNull();
            }
        }

        void ValidateLogCountWithBreaks(LogHeightClass lhc, int numLogsBase, IEnumerable<uint> breaks)
        {
            lhc.GetDefaultLogCount(0).ShouldBeEquivalentTo(numLogsBase);
            lhc.GetDefaultLogCount(breaks.First() - .01f).ShouldBeEquivalentTo(numLogsBase);

            lhc.GetDefaultLogCount(breaks.First()).ShouldBeEquivalentTo(numLogsBase + 1);
            lhc.GetDefaultLogCount(breaks.ElementAt(1)).ShouldBeEquivalentTo(numLogsBase + 2);
        }
    }
}