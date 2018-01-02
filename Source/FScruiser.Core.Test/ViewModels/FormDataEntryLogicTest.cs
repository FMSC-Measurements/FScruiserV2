using FluentAssertions;
using FScruiser.Core.Services;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;
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
        [Fact]
        public void OnTallyTest()
        {
            


        }

        [Fact]
        public void TallyStandardTest()
        {
            var count = new CountTree() { TreeCount = 0 };
            var sampleSelector = new FMSC.Sampling.SystematicSelecter(1);//100%

            var expectedTree = new Tree();
            var dataServiceMock = new Moq.Mock<ITreeDataService>();

            dataServiceMock.Setup(ds => ds.CreateNewTreeEntry(It.IsAny<CountTree>())).Returns(expectedTree);


            var result = FormDataEntryLogic.TallyStandard(count, sampleSelector, dataServiceMock.Object);

            result.Should().NotBeNull();
            result.TreeRecord.Should().BeSameAs(expectedTree);
            result.Count.Should().BeSameAs(count);
            count.TreeCount.ShouldBeEquivalentTo(1);
            expectedTree.CountOrMeasure.ShouldAllBeEquivalentTo("M");

            //sampleSelector = new FMSC.Sampling.SystematicSelecter(0);//0%
            //result = FormDataEntryLogic.TallyStandard(count, sampleSelector, dataServiceMock.Object);
            //count.TreeCount.ShouldBeEquivalentTo(2);
            //expectedTree.CountOrMeasure.ShouldAllBeEquivalentTo("M");
        }
    }
}
