using CruiseDAL.DataObjects;
using CruiseDAL.Schema;
using FluentAssertions;
using FScruiser.Core.Services;
using FSCruiser.Core;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FScruiser.Core.Test.ViewModels
{
    public class FormDataEntryLogicTest
    {
        [Theory]
        [InlineData(1, 0, "M", false, false)]
        //TODO it would be nice to have a better way to guarntee a insurance sample
        [InlineData(2, 1, "I", false, false)]//if freq is 1 sampler wont do insurance
        [InlineData(1, 0, "M", true, true)]
        //[InlineData(0,0, "C", false, false)]//frequency of 0 is not allowed and breaks the OnTally
        public void OnTallyTest_STR(int frequency, int insuranceFreq, string resultCountMeasure, bool enableCruiserPopup, bool enterMeasureTreeData)
        {

            using (var ds = CreateDatastore(CruiseDAL.Schema.CruiseMethods.STR, frequency, insuranceFreq))
            {
                var dataService = new IDataEntryDataService("01", ds);

                var count = ds.From<CountTree>().Read().Single();

                var tallyHistory = new List<TallyAction>();

                var appSettingsMock = new Mock<IApplicationSettings>();
                appSettingsMock.Setup(x => x.EnableCruiserPopup).Returns(enableCruiserPopup);
                appSettingsMock.Setup(x => x.EnableAskEnterTreeData).Returns(enterMeasureTreeData);

                var dataEntryViewMock = new Mock<IDataEntryView>();
                var dialogServiceMock = new Mock<IDialogService>();
                dialogServiceMock.Setup(x => x.AskYesNo(It.Is<string>(s => s == "Would you like to enter tree data now?"), It.IsAny<string>(), It.IsAny<bool>()))
                    .Returns(enterMeasureTreeData);

                var soundServiceMock = new Mock<ISoundService>();


                FormDataEntryLogic.OnTally(count, dataService, tallyHistory, 
                    appSettingsMock.Object, 
                    dataEntryViewMock.Object, 
                    dialogServiceMock.Object, 
                    soundServiceMock.Object);

                

                tallyHistory.Should().HaveCount(1);
                var tallyAction = tallyHistory.Single();

                var treeCount = ds.ExecuteScalar<int>("SELECT Sum(TreeCount) FROM CountTree;");
                treeCount.ShouldBeEquivalentTo(1);

                soundServiceMock.Verify(x => x.SignalTally(It.IsAny<bool>()));

                if (resultCountMeasure == "M" || resultCountMeasure == "I")
                {
                    tallyAction.TreeRecord.Should().NotBeNull();

                    dataService.NonPlotTrees.Should().HaveCount(1);

                    var tree = ds.From<Tree>().Read().Single();
                    tree.Should().NotBeNull();
                    tree.CountOrMeasure.ShouldBeEquivalentTo(resultCountMeasure);

                    if(resultCountMeasure == "M")
                    {
                        soundServiceMock.Verify(x => x.SignalMeasureTree());
                    }
                    else
                    {
                        soundServiceMock.Verify(x => x.SignalInsuranceTree());
                    }

                    if (enterMeasureTreeData)
                    {
                        dataEntryViewMock.Verify(v => v.GotoTreePage());//verify GoToTreePage was called
                    }

                    if (enableCruiserPopup)
                    {
                        dialogServiceMock.Verify(x => x.AskCruiser(It.IsNotNull<Tree>()));
                    }
                    else
                    {
                        dialogServiceMock.Verify(x => x.ShowMessage(It.Is<string>(s => s.Contains("Tree #")), It.IsAny<string>()));
                    }
                }
            }
        }

        private CruiseDAL.DAL CreateDatastore(string cruiseMethod, int freqORkz, int insuranceFreq)
        {
            var ds = new CruiseDAL.DAL();
            try
            {
                var sale = new SaleDO()
                {
                    DAL = ds,
                    SaleNumber = "12345",
                    Region = "1",
                    Forest = "1",
                    District = "1",
                    Purpose = "something",
                    LogGradingEnabled = true
                };
                sale.Save();

                var stratum = new StratumDO()
                {
                    DAL = ds,
                    Code = "01",
                    Method = cruiseMethod
                };
                stratum.Save();

                var cuttingUnit = new CuttingUnitDO()
                {
                    DAL = ds,
                    Code = "01"
                };
                cuttingUnit.Save();

                var cust = new CuttingUnitStratumDO()
                {
                    DAL = ds,
                    CuttingUnit = cuttingUnit,
                    Stratum = stratum
                };
                cust.Save();

                var sampleGroup = new SampleGroupDO() {
                    DAL = ds,
                    Stratum = stratum,
                    Code = "01",
                    PrimaryProduct = "01",                    
                    UOM = "something",
                    CutLeave = "something",
                    InsuranceFrequency = insuranceFreq
                };

                if(CruiseMethods.THREE_P_METHODS.Contains(cruiseMethod))
                {
                    sampleGroup.KZ = freqORkz;
                }
                else
                {
                    sampleGroup.SamplingFrequency = freqORkz;
                }

                sampleGroup.Save();

                var tally = new TallyDO()
                {
                    DAL = ds,
                    Hotkey = "A",
                    Description = "something"
                };
                tally.Save();

                var count = new CountTreeDO()
                {
                    DAL = ds,
                    CuttingUnit = cuttingUnit,
                    SampleGroup = sampleGroup,
                    Tally = tally
                };
                count.Save();

                return ds;
            }
            catch
            {
                ds.Dispose();
                throw;
            }
        }

        [Fact]
        public void TallyStandardTest()
        {
            var count = new CountTree() { TreeCount = 0 };
            FMSC.Sampling.SampleSelecter sampleSelector = new FMSC.Sampling.SystematicSelecter(1);//100%

            var expectedTree = new Tree();
            var dataServiceMock = new Moq.Mock<ITreeDataService>();

            dataServiceMock.Setup(ds => ds.CreateNewTreeEntry(It.IsAny<CountTree>())).Returns(expectedTree);


            var result = FormDataEntryLogic.TallyStandard(count, sampleSelector, dataServiceMock.Object);

            result.Should().NotBeNull();
            result.TreeRecord.Should().BeSameAs(expectedTree);
            result.Count.Should().BeSameAs(count);
            count.TreeCount.ShouldBeEquivalentTo(1);
            expectedTree.CountOrMeasure.ShouldAllBeEquivalentTo("M");

            sampleSelector = new ZeroPCTSelector();//0%

            sampleSelector.Next().ShouldBeEquivalentTo(false);

            result = FormDataEntryLogic.TallyStandard(count, sampleSelector, dataServiceMock.Object);
            result.TreeRecord.Should().BeNull();

            count.TreeCount.ShouldBeEquivalentTo(2);

        }

        [Fact]
        public void TallyThreePTest()
        {
            int expectedKPI = 101;
            int minKPI = 101;
            int maxKPI = 102;

            var dialogServiceMock = new Mock<IDialogService>();
            dialogServiceMock.Setup(ds => ds.AskKPI(It.Is<int>(x => x == minKPI), It.Is<int>(x => x == maxKPI)))
                .Returns(expectedKPI);

            var sg = new SampleGroup() { MinKPI = minKPI, MaxKPI = maxKPI };

            var count = new CountTree() { TreeCount = 0, SumKPI = 0 };
            FMSC.Sampling.SampleSelecter sampleSelector = new FMSC.Sampling.ThreePSelecter(1, 1, 0);



            var expectedTree = new Tree();
            var dataServiceMock = new Moq.Mock<ITreeDataService>();

            dataServiceMock.Setup(ds => ds.CreateNewTreeEntry(It.IsAny<CountTree>())).Returns(expectedTree);

            var result = FormDataEntryLogic.TallyThreeP(count, sampleSelector, sg, dataServiceMock.Object, dialogServiceMock.Object);

            result.Should().NotBeNull();
            result.TreeRecord.Should().BeSameAs(expectedTree);
            result.Count.Should().BeSameAs(count);
            result.KPI.ShouldBeEquivalentTo(expectedKPI);

            count.TreeCount.ShouldBeEquivalentTo(1);
            count.SumKPI.ShouldBeEquivalentTo(expectedKPI);

            expectedTree.CountOrMeasure.ShouldBeEquivalentTo("M");
        }

        [Fact]
        public void TallyThreePTest_UserDontEnterKPI()
        {
            int? expectedKPI = null;
            int minKPI = 101;
            int maxKPI = 102;

            var dialogServiceMock = new Mock<IDialogService>();
            dialogServiceMock.Setup(ds => ds.AskKPI(It.Is<int>(x => x == minKPI), It.Is<int>(x => x == maxKPI)))
                .Returns(expectedKPI);

            var sg = new SampleGroup() { MinKPI = minKPI, MaxKPI = maxKPI };

            var count = new CountTree() { TreeCount = 0, SumKPI = 0 };
            FMSC.Sampling.SampleSelecter sampleSelector = new FMSC.Sampling.ThreePSelecter(1, 1, 0);



            var expectedTree = new Tree();
            var dataServiceMock = new Moq.Mock<ITreeDataService>();

            dataServiceMock.Setup(ds => ds.CreateNewTreeEntry(It.IsAny<CountTree>())).Returns(expectedTree);

            var result = FormDataEntryLogic.TallyThreeP(count, sampleSelector, sg, dataServiceMock.Object, dialogServiceMock.Object);

            result.Should().BeNull();

            count.TreeCount.ShouldBeEquivalentTo(0);
            count.SumKPI.ShouldBeEquivalentTo(0);
        }

        [Fact]
        public void TallyThreePTest_STM()
        {
            int expectedKPI = -1;//when kpi is -1, kpi entered was STM
            int minKPI = 101;
            int maxKPI = 102;

            var dialogServiceMock = new Mock<IDialogService>();
            dialogServiceMock.Setup(ds => ds.AskKPI(It.Is<int>(x => x == minKPI), It.Is<int>(x => x == maxKPI)))
                .Returns(expectedKPI);

            var sg = new SampleGroup() { MinKPI = minKPI, MaxKPI = maxKPI };

            var count = new CountTree() { TreeCount = 0, SumKPI = 0 };
            FMSC.Sampling.SampleSelecter sampleSelector = new FMSC.Sampling.ThreePSelecter(1, 1, 0);



            var expectedTree = new Tree();
            var dataServiceMock = new Moq.Mock<ITreeDataService>();

            dataServiceMock.Setup(ds => ds.CreateNewTreeEntry(It.IsAny<CountTree>())).Returns(expectedTree);

            var result = FormDataEntryLogic.TallyThreeP(count, sampleSelector, sg, dataServiceMock.Object, dialogServiceMock.Object);

            result.Should().NotBeNull();
            result.TreeRecord.Should().BeSameAs(expectedTree);
            result.Count.Should().BeSameAs(count);
            result.KPI.ShouldBeEquivalentTo(0);

            count.TreeCount.ShouldBeEquivalentTo(1);
            count.SumKPI.ShouldBeEquivalentTo(0);

            expectedTree.CountOrMeasure.Should().BeNull();
            expectedTree.STM = "Y";
        }

        public class ZeroPCTSelector : FMSC.Sampling.SampleSelecter
        {
            public override FMSC.Sampling.SampleItem NextItem()
            {
                return null;
            }

            public override bool Ready(bool throwException)
            {
                return true;
            }
        }
    }
}
