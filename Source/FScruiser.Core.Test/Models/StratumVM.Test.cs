using System.Collections.Generic;
using CruiseDAL.DataObjects;
using FSCruiser.Core.Models;
using Xunit;
using FluentAssertions;

namespace FScruiser.Core.Test
{
    public class StratumVMTest
    {
        [Fact]
        public void PopulateHotKeyLookupTest()
        {
            var stratum = new Stratum();

            var counts = new CountTree[]
                { new CountTree() { Tally = new TallyDO() { Hotkey = "A" } },
                    new CountTree() {Tally = new TallyDO() { Hotkey = "Banana" } },
                new CountTree() {Tally = new TallyDO() { Hotkey = "cat" } }};

            var samplegroups = new SampleGroup[] {
                new SampleGroup() { Counts = counts }
            };

            stratum.SampleGroups = new List<SampleGroup>(samplegroups);

            stratum.HotKeyLookup.Should().NotBeNull();

            stratum.PopulateHotKeyLookup();
            stratum.HotKeyLookup.Should().NotBeNull();
            stratum.HotKeyLookup.ContainsKey('A').Should().BeTrue();
            stratum.HotKeyLookup.ContainsKey('B').Should().BeTrue();
            stratum.HotKeyLookup.ContainsKey('C').Should().BeTrue();
            stratum.HotKeyLookup.ContainsKey('D').Should().BeFalse();
        }

        [Fact]
        public void GetCountByHotKeyTest()
        {
            var stratum = new Stratum();

            var counts = new CountTree[]
                { new CountTree() { Tally = new TallyDO() { Hotkey = "A" } },
                    new CountTree() {Tally = new TallyDO() { Hotkey = "Banana" } },
                new CountTree() {Tally = new TallyDO() { Hotkey = "cat" } }};

            var samplegroups = new SampleGroup[] {
                new SampleGroup() { Counts = counts }
            };

            stratum.SampleGroups = new List<SampleGroup>(samplegroups);

            stratum.GetCountByHotKey('A').ShouldBeEquivalentTo(counts[0]);
            stratum.GetCountByHotKey('B').ShouldBeEquivalentTo(counts[1]);
            stratum.GetCountByHotKey('C').ShouldBeEquivalentTo(counts[2]);
            stratum.GetCountByHotKey('0').Should().BeNull();
        }
    }
}