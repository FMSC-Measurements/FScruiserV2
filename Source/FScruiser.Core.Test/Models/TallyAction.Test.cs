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
    public class TallyActionTest
    {
        [Fact]
        public void ToStringTest()
        {
            var tallyAction = new TallyAction();
            tallyAction.ToString().Should().NotBeNullOrWhiteSpace();
            tallyAction.ToString().ShouldBeEquivalentTo("-- ----");

            tallyAction.Count = new CountTree();

            tallyAction.ToString().Should().NotBeNullOrWhiteSpace();
            tallyAction.ToString().ShouldBeEquivalentTo("-- ----");

            tallyAction.Count.SampleGroup = new SampleGroup() { Code = "1111111", Stratum = new Stratum() { Code = "222" } };

            tallyAction.ToString().Should().NotBeNullOrWhiteSpace();
            tallyAction.ToString().ShouldBeEquivalentTo("22 1111");

            tallyAction.Count.SampleGroup.Code = "1";
            tallyAction.Count.SampleGroup.Stratum.Code = "2";

            tallyAction.ToString().Should().NotBeNullOrWhiteSpace();
            tallyAction.ToString().ShouldBeEquivalentTo("2 1");
        }
    }
}