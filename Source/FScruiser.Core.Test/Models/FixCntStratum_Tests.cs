using CruiseDAL;
using FluentAssertions;
using FSCruiser.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace FScruiser.Core.Test.Models
{
    public class FixCntStratum_Tests : TestBase
    {
        public FixCntStratum_Tests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void TallyClass()
        {
            var filePath = GetTestFile("test_FixCNT.cruise");

            using(var db = new DAL(filePath))
            {
                var stuff = db.QueryGeneric("SELECT * FROM FixCNTTallyClass;").ToArray();

                var fixSt = db.From<FixCNTStratum>().Query().Single();
                fixSt.Should().NotBeNull();

                var tallyClass = fixSt.TallyClass;
                tallyClass.Should().NotBeNull();

                tallyClass.Field.Should().BeOneOf(FixCNTTallyFields.DBH,
                                                  FixCNTTallyFields.TOTALHEIGHT,
                                                  FixCNTTallyFields.DRC);
            }
        }
    }
}
