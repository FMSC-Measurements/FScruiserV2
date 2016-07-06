using System;
using FSCruiser.Core.Workers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FScruiser.Core.Workers.Test
{
    public class TestWorker : Worker
    {
        TestContext _testContext;

        public TestWorker(string name, TestContext testContext)
            : base()
        {
            Name = name;
            _testContext = testContext;
        }

        Action DoWork { get; set; }

        protected override void OnEnded(WorkerProgressChangedEventArgs e)
        {
            _testContext.WriteLine("OnEnded");
            base.OnEnded(e);
        }

        protected override void OnExceptionThrown(WorkerExceptionThrownEventArgs e)
        {
            _testContext.WriteLine("OnExceptionThrown");
            base.OnExceptionThrown(e);
        }

        protected override void OnProgressChanged(WorkerProgressChangedEventArgs e)
        {
            _testContext.WriteLine("OnProgressChanged");
            base.OnProgressChanged(e);
        }

        protected override void OnStarting(WorkerProgressChangedEventArgs e)
        {
            _testContext.WriteLine("OnStarting");
            base.OnStarting(e);
        }

        protected override void WorkerMain()
        {
            if (DoWork != null)
            {
                DoWork();
            }
        }
    }

    /// <summary>
    ///This is a test class for WorkerTest and is intended
    ///to contain all WorkerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class WorkerTest
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
        ///A test for BeginWork
        ///</summary>
        [TestMethod()]
        public void BeginWorkTest()
        {
            var worker = new TestWorker("BeginWorkTest", testContextInstance);

            int callCount = 0;
            worker.Starting += ((x, y) => callCount = 1);

            worker.Start();

            Assert.AreEqual(1, callCount);
        }

        ///// <summary>
        /////A test for CalcPercentDone
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("FScruiserPC.exe")]
        //public void CalcPercentDoneTest()
        //{
        //    int workExpected = 0; // TODO: Initialize to an appropriate value
        //    int workDone = 0; // TODO: Initialize to an appropriate value
        //    int expected = 0; // TODO: Initialize to an appropriate value
        //    int actual;
        //    actual = Worker_Accessor.CalcPercentDone(workExpected, workDone);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for Cancel
        /////</summary>
        //[TestMethod()]
        //public void CancelTest()
        //{
        //    Worker target = CreateWorker(); // TODO: Initialize to an appropriate value
        //    target.Cancel();
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for CheckCanceled
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("FScruiserPC.exe")]
        //public void CheckCanceledTest()
        //{
        //    PrivateObject param0 = null; // TODO: Initialize to an appropriate value
        //    Worker_Accessor target = new Worker_Accessor(param0); // TODO: Initialize to an appropriate value
        //    target.CheckCanceled();
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for DoWork
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("FScruiserPC.exe")]
        //public void DoWorkTest()
        //{
        //    PrivateObject param0 = null; // TODO: Initialize to an appropriate value
        //    Worker_Accessor target = new Worker_Accessor(param0); // TODO: Initialize to an appropriate value
        //    target.DoWork();
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for Kill
        /////</summary>
        //[TestMethod()]
        //public void KillTest()
        //{
        //    Worker target = CreateWorker(); // TODO: Initialize to an appropriate value
        //    target.Kill();
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for NotifyExceptionThrown
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("FScruiserPC.exe")]
        //public void NotifyExceptionThrownTest()
        //{
        //    PrivateObject param0 = null; // TODO: Initialize to an appropriate value
        //    Worker_Accessor target = new Worker_Accessor(param0); // TODO: Initialize to an appropriate value
        //    Exception ex = null; // TODO: Initialize to an appropriate value
        //    bool expected = false; // TODO: Initialize to an appropriate value
        //    bool actual;
        //    actual = target.NotifyExceptionThrown(ex);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for NotifyProgressChanged
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("FScruiserPC.exe")]
        //public void NotifyProgressChangedTest()
        //{
        //    PrivateObject param0 = null; // TODO: Initialize to an appropriate value
        //    Worker_Accessor target = new Worker_Accessor(param0); // TODO: Initialize to an appropriate value
        //    string message = string.Empty; // TODO: Initialize to an appropriate value
        //    target.NotifyProgressChanged(message);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for NotifyWorkEnded
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("FScruiserPC.exe")]
        //public void NotifyWorkEndedTest()
        //{
        //    PrivateObject param0 = null; // TODO: Initialize to an appropriate value
        //    Worker_Accessor target = new Worker_Accessor(param0); // TODO: Initialize to an appropriate value
        //    string message = string.Empty; // TODO: Initialize to an appropriate value
        //    target.NotifyWorkEnded(message);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for NotifyWorkStarting
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("FScruiserPC.exe")]
        //public void NotifyWorkStartingTest()
        //{
        //    PrivateObject param0 = null; // TODO: Initialize to an appropriate value
        //    Worker_Accessor target = new Worker_Accessor(param0); // TODO: Initialize to an appropriate value
        //    target.NotifyWorkStarting();
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for OnEnded
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("FScruiserPC.exe")]
        //public void OnEndedTest()
        //{
        //    PrivateObject param0 = null; // TODO: Initialize to an appropriate value
        //    Worker_Accessor target = new Worker_Accessor(param0); // TODO: Initialize to an appropriate value
        //    WorkerProgressChangedEventArgs e = null; // TODO: Initialize to an appropriate value
        //    target.OnEnded(e);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for OnExceptionThrown
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("FScruiserPC.exe")]
        //public void OnExceptionThrownTest()
        //{
        //    PrivateObject param0 = null; // TODO: Initialize to an appropriate value
        //    Worker_Accessor target = new Worker_Accessor(param0); // TODO: Initialize to an appropriate value
        //    WorkerExceptionThrownEventArgs e = null; // TODO: Initialize to an appropriate value
        //    target.OnExceptionThrown(e);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for OnProgressChanged
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("FScruiserPC.exe")]
        //public void OnProgressChangedTest()
        //{
        //    PrivateObject param0 = null; // TODO: Initialize to an appropriate value
        //    Worker_Accessor target = new Worker_Accessor(param0); // TODO: Initialize to an appropriate value
        //    WorkerProgressChangedEventArgs e = null; // TODO: Initialize to an appropriate value
        //    target.OnProgressChanged(e);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for OnStarting
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("FScruiserPC.exe")]
        //public void OnStartingTest()
        //{
        //    PrivateObject param0 = null; // TODO: Initialize to an appropriate value
        //    Worker_Accessor target = new Worker_Accessor(param0); // TODO: Initialize to an appropriate value
        //    WorkerProgressChangedEventArgs e = null; // TODO: Initialize to an appropriate value
        //    target.OnStarting(e);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for Wait
        /////</summary>
        //[TestMethod()]
        //public void WaitTest1()
        //{
        //    Worker target = CreateWorker(); // TODO: Initialize to an appropriate value
        //    bool expected = false; // TODO: Initialize to an appropriate value
        //    bool actual;
        //    actual = target.Wait();
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for Wait
        /////</summary>
        //[TestMethod()]
        //public void WaitTest()
        //{
        //    Worker target = CreateWorker(); // TODO: Initialize to an appropriate value
        //    int millSecTimeout = 0; // TODO: Initialize to an appropriate value
        //    bool expected = false; // TODO: Initialize to an appropriate value
        //    bool actual;
        //    actual = target.Wait(millSecTimeout);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for WorkerMain
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("FScruiserPC.exe")]
        //public void WorkerMainTest()
        //{
        //    // Private Accessor for WorkerMain is not found. Please rebuild the containing project or run the Publicize.exe manually.
        //    Assert.Inconclusive("Private Accessor for WorkerMain is not found. Please rebuild the containing proje" +
        //            "ct or run the Publicize.exe manually.");
        //}

        ///// <summary>
        /////A test for IsCanceled
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("FScruiserPC.exe")]
        //public void IsCanceledTest()
        //{
        //    PrivateObject param0 = null; // TODO: Initialize to an appropriate value
        //    Worker_Accessor target = new Worker_Accessor(param0); // TODO: Initialize to an appropriate value
        //    bool expected = false; // TODO: Initialize to an appropriate value
        //    bool actual;
        //    target.IsCanceled = expected;
        //    actual = target.IsCanceled;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for IsDone
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("FScruiserPC.exe")]
        //public void IsDoneTest()
        //{
        //    PrivateObject param0 = null; // TODO: Initialize to an appropriate value
        //    Worker_Accessor target = new Worker_Accessor(param0); // TODO: Initialize to an appropriate value
        //    bool expected = false; // TODO: Initialize to an appropriate value
        //    bool actual;
        //    target.IsDone = expected;
        //    actual = target.IsDone;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for IsWorking
        /////</summary>
        //[TestMethod()]
        //public void IsWorkingTest()
        //{
        //    Worker target = CreateWorker(); // TODO: Initialize to an appropriate value
        //    bool actual;
        //    actual = target.IsWorking;
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for Name
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("FScruiserPC.exe")]
        //public void NameTest()
        //{
        //    PrivateObject param0 = null; // TODO: Initialize to an appropriate value
        //    Worker_Accessor target = new Worker_Accessor(param0); // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    target.Name = expected;
        //    actual = target.Name;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        //internal virtual Worker CreateWorker()
        //{
        //    // TODO: Instantiate an appropriate concrete class.
        //    Worker target = null;
        //    return target;
        //}

        ///// <summary>
        /////A test for ThreadLock
        /////</summary>
        //[TestMethod()]
        //public void ThreadLockTest()
        //{
        //    Worker target = CreateWorker(); // TODO: Initialize to an appropriate value
        //    object actual;
        //    actual = target.ThreadLock;
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for UnitsOfWorkCompleated
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("FScruiserPC.exe")]
        //public void UnitsOfWorkCompleatedTest()
        //{
        //    PrivateObject param0 = null; // TODO: Initialize to an appropriate value
        //    Worker_Accessor target = new Worker_Accessor(param0); // TODO: Initialize to an appropriate value
        //    int expected = 0; // TODO: Initialize to an appropriate value
        //    int actual;
        //    target.UnitsOfWorkCompleated = expected;
        //    actual = target.UnitsOfWorkCompleated;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        //internal virtual Worker_Accessor CreateWorker_Accessor()
        //{
        //    // TODO: Instantiate an appropriate concrete class.
        //    Worker_Accessor target = null;
        //    return target;
        //}

        ///// <summary>
        /////A test for UnitsOfWorkExpected
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("FScruiserPC.exe")]
        //public void UnitsOfWorkExpectedTest()
        //{
        //    PrivateObject param0 = null; // TODO: Initialize to an appropriate value
        //    Worker_Accessor target = new Worker_Accessor(param0); // TODO: Initialize to an appropriate value
        //    int expected = 0; // TODO: Initialize to an appropriate value
        //    int actual;
        //    target.UnitsOfWorkExpected = expected;
        //    actual = target.UnitsOfWorkExpected;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}
    }
}