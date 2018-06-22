using CruiseDAL;
using CruiseDAL.DataObjects;
using FluentAssertions;
using FScruiser.Core.Services;
using FSCruiser.Core.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FScruiser.Core.Test.Services
{
    public class LogDataServiceTest
    {
        public LogDataServiceTest()
        {
        }

        public DAL CreateDataStore(int treesToCreate, int numLogToCreate)
        {
            var ds = new DAL();
            try
            {
                var stratum = new StratumDO()
                {
                    DAL = ds,
                    Code = "01",
                    Method = "something"
                };

                stratum.Save();


                //set up the log fields
                int counter = 1;
                var logFieldSetups = CruiseDAL.Schema.LOG._ALL
                    .Select(x => new LogFieldSetupDO(){ DAL = ds,Field = x, FieldOrder = counter++, Heading = x, Stratum = stratum })
                    .ToList();

                foreach (var lfs in logFieldSetups)
                {
                    lfs.Save();
                }

                var cuttingUnit = new CuttingUnitDO()
                {
                    DAL = ds,
                    Code = "01"
                };

                cuttingUnit.Save();

                for (int j = 0; j < treesToCreate; j++)
                {
                    var tree = new TreeDO()
                    {
                        DAL = ds,
                        CuttingUnit = cuttingUnit,
                        Stratum = stratum,
                        TreeNumber = j + 1
                    };

                    tree.Save();

                    for (int i = 0; i < numLogToCreate; i++)
                    {
                        var log = new LogDO()
                        {
                            DAL = ds,
                            LogNumber = (i + 1).ToString(),
                            Tree = tree,
                        };

                        log.Save();
                    }
                }

                return ds;
            }
            catch
            {
                ds.Dispose();
                throw;
            }
        }

        [Fact]
        public void CtorTest()
        {
            using (var ds = CreateDataStore(1, 5))
            {
                var tree = ds.From<Tree>().Read().FirstOrDefault();

                tree.Should().NotBeNull();

                var stratum = tree.Stratum;

                stratum.Should().NotBeNull();

                var cuttingUnit = tree.CuttingUnit;

                cuttingUnit.Should().NotBeNull();

                var logDs = new ILogDataService(tree, null, ds);

                logDs.Logs.Should().NotBeNullOrEmpty();
            }
        }

        [Fact]
        public void CtorTestwithLogRules()
        {
            var numLogExpected = 10;

            var regionLogRuleMock = new Mock<RegionLogInfo>();
            regionLogRuleMock.Setup(rlr => rlr.GetLogRule(It.IsAny<string>()).GetDefaultLogCount(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<long>()))
            .Returns(numLogExpected);

            using (var ds = CreateDataStore(1, 0))
            {
                var tree = ds.From<Tree>().Read().FirstOrDefault();

                tree.Should().NotBeNull();

                var stratum = tree.Stratum;

                stratum.Should().NotBeNull();

                var cuttingUnit = tree.CuttingUnit;

                cuttingUnit.Should().NotBeNull();

                var tdv = new TreeDefaultValueDO()
                {
                    MerchHeightLogLength = 16
                };

                tree.TreeDefaultValue = tdv;

                var logDs = new ILogDataService(tree, regionLogRuleMock.Object, ds);

                logDs.Logs.Should().HaveCount(10);

                foreach (var log in logDs.Logs)
                {
                    ValidateLog(log);
                }
            }
        }

        [Fact]
        public void AddLogTest()
        {
            using (var ds = CreateDataStore(1, 0))
            {
                var tree = ds.From<Tree>().Read().FirstOrDefault();
                tree.Stratum.Should().NotBeNull();
                tree.CuttingUnit.Should().NotBeNull();

                var logDs = new ILogDataService(tree, null, ds);
                logDs.Logs.Should().BeEmpty();

                //add log
                var log = logDs.AddLogRec();
                ValidateLog(log);

                logDs.Logs.Should().Contain(log);

                var log2 = logDs.AddLogRec();

                log2.LogNumber.Should().BeGreaterThan(log.LogNumber);

                logDs.Logs.Should().Contain(log2);

                logDs.Invoking(lds => lds.Save()).Should().NotThrow();
            }
        }

        [Fact]
        public void DeleteLogTest()
        {
            using (var ds = CreateDataStore(1, 1))
            {
                var tree = ds.From<Tree>().Read().FirstOrDefault();
                tree.Stratum.Should().NotBeNull();
                tree.CuttingUnit.Should().NotBeNull();

                var logDs = new ILogDataService(tree, null, ds);

                logDs.Logs.Should().HaveCount(1);

                logDs.AddLogRec();

                foreach (var log in logDs.Logs.ToArray())
                {
                    logDs.DeleteLog(log);
                    tree.LogCountDirty.Should().BeTrue();
                }

                logDs.Logs.Should().BeEmpty();

                logDs.Invoking(lds => lds.Save()).Should().NotThrow();

                ds.GetRowCount("Log", $"WHERE Tree_CN = {tree.Tree_CN}");
            }
        }

        void ValidateLog(Log log)
        {
            log.DAL.Should().NotBeNull();
            log.LogNumber.Should().BeGreaterThan(0);
            log.Tree_CN.Should().BeGreaterThan(0);
        }

        [Fact]
        public void ValidateLogGradeTest()
        {
            var log = new Log();

            IEnumerable<LogGradeAuditRule> audits = null;

            //Action callingValidateWithNullAuditCollection = () => ILogDataService.ValidateLogGrade(log, audits);
            //callingValidateWithNullAuditCollection.ShouldNotThrow();
            ILogDataService.ValidateLogGrade(log, audits).Should().BeTrue();

            audits = new LogGradeAuditRule[] { };
            ILogDataService.ValidateLogGrade(log, audits).Should().BeTrue();

            audits = new LogGradeAuditRule[] { new LogGradeAuditRule() };
            ILogDataService.ValidateLogGrade(log, audits).Should().BeTrue();

            audits = new LogGradeAuditRule[] { new LogGradeAuditRule() { ValidGrades = "" } };
            ILogDataService.ValidateLogGrade(log, audits).Should().BeTrue();

            log.Grade = "0 ";
            audits = new LogGradeAuditRule[] { new LogGradeAuditRule() { ValidGrades = "0" } };
            ILogDataService.ValidateLogGrade(log, audits).Should().BeTrue();

            log.Grade = "0 ";
            audits = new LogGradeAuditRule[] { new LogGradeAuditRule() { ValidGrades = "1" } };
            ILogDataService.ValidateLogGrade(log, audits).Should().BeFalse();
            log[nameof(log.Grade)].Should().NotBeNullOrEmpty();
        }
    }
}