using System.Collections.Generic;
using CruiseDAL.DataObjects;
using FSCruiser.Core.Models;
using Xunit;
using FluentAssertions;
using CruiseDAL.Schema;
using System.Linq;

namespace FScruiser.Core.Test
{
    public class StratumTest
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

            stratum.GetCountByHotKey('A').Should().Be(counts[0]);
            stratum.GetCountByHotKey('B').Should().Be(counts[1]);
            stratum.GetCountByHotKey('C').Should().Be(counts[2]);
            stratum.GetCountByHotKey('0').Should().BeNull();
        }

        [Fact]
        public void TestIs3P()
        {
            foreach (var st in MakeThreePMethods())
            {
                st.Is3P.Should().BeTrue();
            }

            foreach (var st in MakeNonThreePMethods())
            {
                st.Is3P.Should().BeFalse();
            }
        }

        IEnumerable<Stratum> MakeThreePMethods()
        {
            foreach (var method in CruiseMethods.THREE_P_METHODS)
            {
                yield return new Stratum()
                { Method = method };
            }
        }

        IEnumerable<Stratum> MakeNonThreePMethods()
        {
            foreach (var method in CruiseMethods.SUPPORTED_METHODS.Except(CruiseMethods.THREE_P_METHODS))
            {
                yield return new Stratum()
                { Method = method };
            }
        }
    }
}