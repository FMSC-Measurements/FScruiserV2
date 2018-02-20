using FluentAssertions;
using FSCruiser.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FScruiser.Core.Test.Models
{
    public class TallyHistoryCollectionTest
    {
        [Fact]
        public void SerializeDeserialize_Test()
        {
            var tallyHistoryCollection = new TallyHistoryCollection(10);

            tallyHistoryCollection.Add(new TallyAction()
            {
                CountCN = 1,
                Time = "123",
                TreeCN = 2,
                TreeEstimateCN = 3
            });

            var xmlText = tallyHistoryCollection.Serialize();

            var deserializeResult = TallyHistoryCollection.Deserialize(xmlText);

            deserializeResult.Should().HaveSameCount(tallyHistoryCollection);

            foreach (var item in deserializeResult.Zip(tallyHistoryCollection, (x, y) => new { Left = x, Right = y }))
            {
                item.Left.TreeCN.ShouldBeEquivalentTo(item.Right.TreeCN);
                item.Left.TreeEstimateCN.ShouldBeEquivalentTo(item.Right.TreeEstimateCN);
                item.Left.CountCN.ShouldBeEquivalentTo(item.Right.CountCN);
                item.Left.KPI.ShouldBeEquivalentTo(item.Right.KPI);
                item.Left.Time.ShouldBeEquivalentTo(item.Right.Time);
            }
        }

        [Fact]
        public void SerializeDeserialize_Empty_Test()
        {
            var tallyHistoryCollection = new TallyHistoryCollection(10);

            var xmlText = tallyHistoryCollection.Serialize();

            var deserializeResult = TallyHistoryCollection.Deserialize(xmlText);

            deserializeResult.Should().HaveSameCount(tallyHistoryCollection);

            foreach (var item in deserializeResult.Zip(tallyHistoryCollection, (x, y) => new { Left = x, Right = y }))
            {
                item.Left.TreeCN.ShouldBeEquivalentTo(item.Right.TreeCN);
                item.Left.TreeEstimateCN.ShouldBeEquivalentTo(item.Right.TreeEstimateCN);
                item.Left.CountCN.ShouldBeEquivalentTo(item.Right.CountCN);
                item.Left.KPI.ShouldBeEquivalentTo(item.Right.KPI);
                item.Left.Time.ShouldBeEquivalentTo(item.Right.Time);
            }
        }

        [Fact]
        public void InflateTest()
        {
            using (var dataStore = new CruiseDAL.DAL())
            {
                dataStore.Insert(new CuttingUnit() { Code = "01", CuttingUnit_CN = 1 });
                dataStore.Insert(new Stratum() { Code = "01", Method = "", Stratum_CN = 1 });
                dataStore.Insert(new SampleGroup() { Code = "01", CutLeave = "", UOM = "", Stratum_CN = 1, SampleGroup_CN = 1, PrimaryProduct = "01" });
                dataStore.Insert(new CruiseDAL.DataObjects.TallyDO() { Tally_CN = 1, Hotkey = "", Description = "" });

                dataStore.Insert(new Tree() {TreeNumber = 1, Tree_CN = 1, CuttingUnit_CN = 1, Stratum_CN = 1 });
                dataStore.Insert(new CountTree() { CountTree_CN = 1, CuttingUnit_CN = 1, SampleGroup_CN = 1 });
                dataStore.Insert(new CruiseDAL.DataObjects.TreeEstimateDO() { CountTree_CN = 1 });

                var tallyAction = new TallyAction() { TreeCN = 1, CountCN = 1, TreeEstimateCN = 1 };

                TallyHistoryCollection.Inflate(dataStore, tallyAction);

                tallyAction.TreeRecord.Should().NotBeNull();
                tallyAction.Count.Should().NotBeNull();
                tallyAction.TreeEstimate.Should().NotBeNull();
            }
        }

        [Fact]
        public void Add_Test_WithLimitedSize()
        {
            var tallHistoryCollection = new TallyHistoryCollection(1);

            var ta1 = new TallyAction();

            tallHistoryCollection.Add(ta1);

            tallHistoryCollection.Should().HaveCount(1);

            var ta2 = new TallyAction();
            tallHistoryCollection.Add(ta2);
            tallHistoryCollection.Should().HaveCount(1);
            tallHistoryCollection.Should().Contain(ta2);
            tallHistoryCollection.Should().NotContain(ta1);

        }

        [Fact]
        public void Remove_Test()
        {
            var tallyHistoryCollection = new TallyHistoryCollection(1);

            var ta1 = new TallyAction();

            tallyHistoryCollection.Add(ta1);

            tallyHistoryCollection.MonitorEvents();
            tallyHistoryCollection.Remove(ta1);
            tallyHistoryCollection.ShouldRaise(nameof(TallyHistoryCollection.ItemRemoving));
        }

        [Fact]
        public void RemoveAt_Test()
        {
            var tallyHistoryCollection = new TallyHistoryCollection(1);

            var ta1 = new TallyAction();

            tallyHistoryCollection.Add(ta1);

            tallyHistoryCollection.MonitorEvents();
            tallyHistoryCollection.RemoveAt(0);
            tallyHistoryCollection.ShouldRaise(nameof(TallyHistoryCollection.ItemRemoving));
        }
    }
}
