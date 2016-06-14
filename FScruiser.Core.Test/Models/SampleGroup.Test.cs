using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FSCruiser.Core.Models
{
    /// <summary>
    /// Summary description for SampleGroup
    /// </summary>
    [TestClass]
    public class SampleGroupTest
    {
        public SampleGroupTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion Additional test attributes

        [TestMethod]
        public void MakeSampleSelecterTest_100PCT()
        {
            var st = new StratumVM() { Method = "100PCT" };

            var sg = new SampleGroupVM() { Stratum = st };

            sg.SampleSelectorState = null;

            Assert.IsNull(sg.MakeSampleSelecter());
            Assert.IsNull(sg.Sampler);
        }

        [TestMethod]
        public void MakeSampleSelecterTest_STR()
        {
            var st = new StratumVM() { Method = "STR" };

            //test: if sampling freq is 0
            //then Sampler is null
            var sg = new SampleGroupVM()
            {
                Stratum = st,
                SamplingFrequency = 0
            };

            Assert.IsNull(sg.MakeSampleSelecter());
            Assert.IsNull(sg.Sampler);

            //test: if sampling freq is > 0
            //AND SampleSelectorType is not defined
            //THEN Sampler is not null
            //AND is of type Blocked
            sg = new SampleGroupVM()
            {
                Stratum = st,
                SamplingFrequency = 1
            };

            Assert.IsNotNull(sg.MakeSampleSelecter());
            Assert.IsNotNull(sg.Sampler);
            Assert.IsInstanceOfType(sg.Sampler, typeof(FMSC.Sampling.BlockSelecter));

            var blockSampler = sg.Sampler as FMSC.Sampling.BlockSelecter;
            Assert.AreEqual(1, blockSampler.Frequency);

            //test: if sampling freq is > 0
            //AND SampleSelectorType is Systematic
            //THEN Sampler is not null
            //AND is of type Systematic
            sg = new SampleGroupVM()
            {
                Stratum = st,
                SamplingFrequency = 1,
                SampleSelectorType = "SystematicSelecter"
            };

            Assert.IsNotNull(sg.MakeSampleSelecter());
            Assert.IsNotNull(sg.Sampler);

            Assert.IsInstanceOfType(sg.Sampler, typeof(FMSC.Sampling.SystematicSelecter));

            var systmaticSampler = sg.Sampler as FMSC.Sampling.SystematicSelecter;
            Assert.AreEqual(1, systmaticSampler.Frequency);
        }

        [TestMethod]
        public void MakeSampleSelecterTest_3P()
        {
            var st = new StratumVM() { Method = "3P" };

            var sg = new SampleGroupVM()
            {
                Stratum = st,
                SampleSelectorState = null
            };

            sg.MakeSampleSelecter();

            Assert.IsNull(sg.Sampler);
        }
    }
}