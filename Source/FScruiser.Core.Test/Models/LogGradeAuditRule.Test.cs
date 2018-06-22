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
    public class LogGradeAuditRuleTest
    {
        [Fact]
        public void GradesTest()
        {
            var lga = new LogGradeAuditRule()
            {
                ValidGrades = "0,1, 2 , 3"
            };

            lga.Grades.Should().Contain(new string[] { "0", "1", "2", "3" });
        }

        //[Theory]
        //[InlineData("0", .5f, "0, 1", 0.0f, true)]
        //[InlineData("2", .5f, "0, 1", 0.0f, false)]
        //[InlineData("0", .5f, "0, 1", .5f, true)]
        //[InlineData("2", .5f, "0, 1", .5f, false)]
        //[InlineData("0", .51f, "0, 1", .5f, false)]
        //[InlineData("2", .51f, "0, 1", .5f, false)]
        //public void ValidateLogTest(string grade, float defect, string auditGrades, float maxDefect, bool failPass)
        //{
        //    var log = new Log()
        //    {
        //        Grade = grade,
        //        SeenDefect = defect
        //    };

        //    var lga = new LogGradeAuditRule()
        //    {
        //        ValidGrades = auditGrades,
        //        DefectMax = maxDefect
        //    };

        //    lga.ValidateLog(log).Should().Be(failPass, maxDefect.ToString());
        //}
    }
}