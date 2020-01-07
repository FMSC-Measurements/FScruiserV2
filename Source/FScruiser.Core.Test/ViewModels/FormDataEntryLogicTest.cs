using CruiseDAL.DataObjects;
using CruiseDAL.Schema;
using FluentAssertions;
using FMSC.Sampling;
using FScruiser.Core.Services;
using FScruiser.Services;
using FSCruiser.Core;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FScruiser.Core.Test.ViewModels
{
    public class FormDataEntryLogicTest
    {
        [Theory]
        [InlineData(1, 0, "M", false, false)]
        //TODO it would be nice to have a better way to guarntee a insurance sample
        //[InlineData(2, 1, "I", false, false)]//if freq is 1 sampler wont do insurance
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

                var samplerRepo = new Mock<ISampleSelectorRepository>();

                FormDataEntryLogic.OnTally(count, dataService, tallyHistory,
                    appSettingsMock.Object,
                    dataEntryViewMock.Object,
                    dialogServiceMock.Object,
                    soundServiceMock.Object,
                    samplerRepo.Object);

                tallyHistory.Should().HaveCount(1);
                var tallyAction = tallyHistory.Single();

                var treeCount = ds.ExecuteScalar<int>("SELECT Sum(TreeCount) FROM CountTree;");
                treeCount.Should().Be(1);

                soundServiceMock.Verify(x => x.SignalTally(It.IsAny<bool>()));

                if (resultCountMeasure == "M" || resultCountMeasure == "I")
                {
                    tallyAction.TreeRecord.Should().NotBeNull();

                    dataService.NonPlotTrees.Should().HaveCount(1);

                    var tree = ds.From<Tree>().Read().Single();
                    tree.Should().NotBeNull();
                    tree.CountOrMeasure.Should().Be(resultCountMeasure);

                    if (resultCountMeasure == "M")
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

                var sampleGroup = new SampleGroupDO()
                {
                    DAL = ds,
                    Stratum = stratum,
                    Code = "01",
                    PrimaryProduct = "01",
                    UOM = "something",
                    CutLeave = "something",
                    InsuranceFrequency = insuranceFreq
                };

                if (CruiseMethods.THREE_P_METHODS.Contains(cruiseMethod))
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
            FMSC.Sampling.IFrequencyBasedSelecter sampleSelector = new FMSC.Sampling.SystematicSelecter(1, 0, true);//100%

            var expectedTree = new Tree();

            var dataServiceMock = new Moq.Mock<ITreeDataService>();
            dataServiceMock.Setup(ds => ds.CreateNewTreeEntry(It.IsAny<CountTree>())).Returns(expectedTree);

            var dialogServiceMock = new Mock<IDialogService>();
            //dialogServiceMock.Setup()

            var result = FormDataEntryLogic.TallyStandard(count, sampleSelector, dataServiceMock.Object, dialogServiceMock.Object);

            result.Should().NotBeNull();
            result.TreeRecord.Should().BeSameAs(expectedTree);
            result.Count.Should().BeSameAs(count);
            result.TreeCount.Should().Be(1);
            result.KPI.Should().Be(0);

            expectedTree.CountOrMeasure.Should().Be("M");

            sampleSelector = new ZeroPCTSelector();//0%

            sampleSelector.Sample().Should().Be(SampleResult.C);

            result = FormDataEntryLogic.TallyStandard(count, sampleSelector, dataServiceMock.Object, dialogServiceMock.Object);
            result.TreeRecord.Should().BeNull();
            result.TreeCount.Should().Be(1);
            result.KPI.Should().Be(0);
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
            var sampleSelector = new FMSC.Sampling.ThreePSelecter(1, 0);

            var expectedTree = new Tree();
            var dataServiceMock = new Moq.Mock<ITreeDataService>();

            dataServiceMock.Setup(ds => ds.CreateNewTreeEntry(It.IsAny<CountTree>())).Returns(expectedTree);

            var result = FormDataEntryLogic.TallyThreeP(count, sampleSelector, sg, dataServiceMock.Object, dialogServiceMock.Object);

            result.Should().NotBeNull();
            result.TreeRecord.Should().BeSameAs(expectedTree);
            result.Count.Should().BeSameAs(count);
            result.KPI.Should().Be(expectedKPI);
            result.TreeCount.Should().Be(1);

            expectedTree.CountOrMeasure.Should().Be("M");
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
            var sampleSelector = new FMSC.Sampling.ThreePSelecter(1,  0);

            var expectedTree = new Tree();
            var dataServiceMock = new Moq.Mock<ITreeDataService>();

            dataServiceMock.Setup(ds => ds.CreateNewTreeEntry(It.IsAny<CountTree>())).Returns(expectedTree);

            var result = FormDataEntryLogic.TallyThreeP(count, sampleSelector, sg, dataServiceMock.Object, dialogServiceMock.Object);

            result.Should().BeNull();
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
            var sampleSelector = new FMSC.Sampling.ThreePSelecter(1, 0);

            var expectedTree = new Tree();
            var dataServiceMock = new Moq.Mock<ITreeDataService>();

            dataServiceMock.Setup(ds => ds.CreateNewTreeEntry(It.IsAny<CountTree>())).Returns(expectedTree);

            var result = FormDataEntryLogic.TallyThreeP(count, sampleSelector, sg, dataServiceMock.Object, dialogServiceMock.Object);

            result.Should().NotBeNull();
            result.TreeRecord.Should().BeSameAs(expectedTree);
            result.Count.Should().BeSameAs(count);
            result.KPI.Should().Be(0);
            result.TreeCount.Should().Be(1);

            expectedTree.CountOrMeasure.Should().BeNull();
            expectedTree.STM = "Y";
        }

        public class ZeroPCTSelector : FMSC.Sampling.IFrequencyBasedSelecter
        {
            public int Frequency => 0;

            public int Count => 0;

            public int ITreeFrequency => 0;

            public bool IsSelectingITrees => false;

            public int InsuranceCounter => 0;

            public int InsuranceIndex => 0;

            public string StratumCode { get; set; }
            public string SampleGroupCode { get; set; }

            public SampleResult Sample()
            {
                return SampleResult.C;
            }
        }
    }
}