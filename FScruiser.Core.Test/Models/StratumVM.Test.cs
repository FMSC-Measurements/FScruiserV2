using System.Collections.Generic;
using CruiseDAL.DataObjects;
using FSCruiser.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FScruiser.Core.Test
{
    /// <summary>
    ///This is a test class for StratumVMTest and is intended
    ///to contain all StratumVMTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StratumVMTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes

        //
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //

        #endregion Additional test attributes

        /// <summary>
        ///A test for PopulateHotKeyLookup
        ///</summary>
        [TestMethod()]
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

            Assert.IsNotNull(stratum.HotKeyLookup);

            stratum.PopulateHotKeyLookup();
            Assert.IsNotNull(stratum.HotKeyLookup);
            Assert.IsTrue(stratum.HotKeyLookup.ContainsKey('A'));
            Assert.IsTrue(stratum.HotKeyLookup.ContainsKey('B'));
            Assert.IsTrue(stratum.HotKeyLookup.ContainsKey('C'));
        }

        [TestMethod()]
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

            Assert.IsTrue(stratum.GetCountByHotKey('A') == counts[0]);
            Assert.IsTrue(stratum.GetCountByHotKey('B') == counts[1]);
            Assert.IsTrue(stratum.GetCountByHotKey('C') == counts[2]);
            Assert.IsNull(stratum.GetCountByHotKey('0'));
        }
    }
}