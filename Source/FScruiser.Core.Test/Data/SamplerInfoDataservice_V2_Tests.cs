﻿using CruiseDAL;
using CruiseDAL.DataObjects;
using CruiseDAL.V2.Models;
using FluentAssertions;
using FScruiser.Data;
using FScruiser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FScruiser.Core.Test.Data
{
    public class SamplerInfoDataservice_V2_Tests : DataTestBase
    {
        [Fact]
        public void GetSamplerInfo()
        {
            var stratumCode = "00";
            var sgCode = "01";
            using var db = new DAL();

            var unit = new CuttingUnit
            {
                Code = "u1"
            };
            db.Insert(unit);

            var stratum = new Stratum
            {
                Code = stratumCode,
                Method = "STR",
            };
            db.Insert(stratum);

            var sg = new SampleGroup
            {
                Code = sgCode,
                Stratum_CN = stratum.Stratum_CN.Value,
                CutLeave = "C",
                UOM = "something",
                PrimaryProduct = "something",
                SamplingFrequency = 10,
            };
            db.Insert(sg);

            var countTree = new CountTree
            {
                CuttingUnit_CN = 1,
                SampleGroup_CN = sg.SampleGroup_CN.Value,
                TreeCount = 101,
            };
            db.Insert(countTree);

            //var strata = db.Query<StratumDO>("select * from Stratum;").ToArray();
            //var sgs = db.Query<SampleGroupDO>("Select * from samplegroup;").ToArray();

            var ds = new SamplerInfoDataservice_V2(db);
            var si = ds.GetSamplerInfo(stratumCode, sgCode);
            si.Should().NotBeNull();
            si.SamplingFrequency.Should().Be(sg.SamplingFrequency);
            si.TreeCountCountTree.Should().Be(countTree.TreeCount);
            
        }

        [Fact]
        public void GetSamplerState_returnsNullWhenNoState()
        {
            using (var db = CreateDataStore(methods: new[] { "STR" }))
            {
                var ds = new SamplerInfoDataservice_V2(db);
                var ss = ds.GetSamplerState("00", "01");
                ss.Should().BeNull();
            }
        }

        [Fact]
        public void GetSamplerState()
        {
            using (var db = CreateDataStore(methods: new[] { "STR" }))
            {
                var sg = db.Query<SampleGroupDO>("Select * from SampleGroup;").First();
                db.Execute2("INSERT INTO SamplerState (SampleGroup_CN) values (@SampleGroup_CN)", sg);

                var ds = new SamplerInfoDataservice_V2(db);
                var ss = ds.GetSamplerState("00", "01");
                ss.Should().NotBeNull();
            }
        }

        [Fact]
        public void UpsertSamplerState_insert()
        {
            var stCode = "00";
            var sgCode = "01";

            using (var db = CreateDataStore(methods: new[] { "STR" }))
            {
                var random = new Random();

                var sg = db.Query<SampleGroupDO>("Select * from SampleGroup;").First();

                var ds = new SamplerInfoDataservice_V2(db);

                var ss = new FScruiser.Models.SamplerState()
                {
                    StratumCode = stCode,
                    SampleGroupCode = sgCode,
                    Counter = random.Next(1000),
                    BlockState = "blockStateTest1",
                    InsuranceCounter = random.Next(1000),
                    InsuranceIndex = random.Next(1000),
                    SampleSelectorType = "ssTypeTest1",
                    SystematicIndex = random.Next(1000),
                };
                ds.UpsertSamplerState(ss);

                var ssAgain = ds.GetSamplerState(stCode, sgCode);
                ssAgain.Should().NotBeNull();
                ssAgain.Should().BeEquivalentTo(ss);
            }
        }

        [Fact]
        public void UpsertSamplerState_insert_throws()
        {
            var stCode = "00";
            var sgCode = "01";

            using (var db = CreateDataStore(methods: new[] { "STR" }))
            {
                var random = new Random();

                var sg = db.Query<SampleGroupDO>("Select * from SampleGroup;").First();

                var ds = new SamplerInfoDataservice_V2(db);

                var ss = new FScruiser.Models.SamplerState()
                {
                    StratumCode = stCode,
                    SampleGroupCode = "something",
                    Counter = random.Next(1000),
                    BlockState = "blockStateTest1",
                    InsuranceCounter = random.Next(1000),
                    InsuranceIndex = random.Next(1000),
                    SampleSelectorType = "ssTypeTest1",
                    SystematicIndex = random.Next(1000),
                };
                ds.Invoking(x => x.UpsertSamplerState(ss)).Should().Throw<FMSC.ORM.ConstraintException>();

                var ssAgain = ds.GetSamplerState(stCode, sgCode);
                ssAgain.Should().BeNull();
                //ssAgain.Should().BeEquivalentTo(ss);
            }
        }

        [Fact]
        public void UpsertSamplerState_update()
        {
            var random = new Random();
            var stCode = "00";
            var sgCode = "01";            

            using (var db = CreateDataStore(methods: new[] { "STR" }))
            {
                var sg = db.Query<SampleGroupDO>("Select * from SampleGroup;").First();
                //db.Execute2("INSERT INTO SamplerState (SampleGroup_CN) values (@SampleGroup_CN)", sg);

                var ds = new SamplerInfoDataservice_V2(db);

                var ss = new FScruiser.Models.SamplerState()
                {
                    StratumCode = stCode,
                    SampleGroupCode = sgCode,
                    Counter = random.Next(1000),
                    BlockState = "blockStateTest1",
                    InsuranceCounter = random.Next(1000),
                    InsuranceIndex = random.Next(1000),
                    SampleSelectorType = "ssTypeTest1", 
                    SystematicIndex = random.Next(1000),
                };
                ds.UpsertSamplerState(ss);

                ss.Counter = random.Next(100);
                ss.BlockState = "blockStateTest2";
                ss.InsuranceCounter = random.Next(1000);
                ss.InsuranceIndex = random.Next(1000);
                //ss.SampleSelectorType = "ssTypeTest2";
                ss.SystematicIndex = random.Next(1000);
                ds.UpsertSamplerState(ss);

                var ssAgain = ds.GetSamplerState(stCode, sgCode);
                ssAgain.Should().NotBeNull();
                ssAgain.Should().BeEquivalentTo(ss);
            }
        }
    }
}
