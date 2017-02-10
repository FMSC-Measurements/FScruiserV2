using System;
using System.Collections.Generic;
using CruiseDAL.DataObjects;
using FSCruiser.Core;
using FSCruiser.Core.Models;

namespace FSCruiserV2.Test.Mocks
{
    public class ApplicationControllerMock : IApplicationController
    {
        public ApplicationControllerMock()
        {
            ViewController = new ViewControllerMock() { ApplicationController = this };
        }

        public CruiseDAL.DAL _cDal
        {
            get;
            set;
        }

        public CuttingUnit CurrentUnit
        {
            get;
            set;
        }

        public IList<CuttingUnit> CuttingUnits
        {
            get;
            set;
        }

        public int ISampleCount { get; set; }

        public int SampleCount { get; set; }

        public ApplicationSettings Settings
        {
            get { throw new NotImplementedException(); }
        }

        public IViewController ViewController { get; set; }

        public CruiseDAL.DataObjects.SampleGroupDO CreateNewSampleGroup(CruiseDAL.DataObjects.StratumDO stratum)
        {
            throw new NotImplementedException();
        }

        public CruiseDAL.DataObjects.TreeDefaultValueDO CreateNewTreeDefaultValue(string pProd)
        {
            throw new NotImplementedException();
        }

        public void HandleNonCriticalException(Exception ex, string optMessage)
        {
            throw new NotImplementedException();
        }

        public void LogSumKPIEdit(CountTreeDO countTree, long oldValue, long newValue)
        {
            throw new NotImplementedException();
        }

        public void LogTreeCountEdit(CountTreeDO countTree, long oldValue, long newValue)
        {
            throw new NotImplementedException();
        }

        public void OnLeavingCurrentUnit(System.ComponentModel.CancelEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OpenFile()
        {
            throw new NotImplementedException();
        }

        public void OpenFile(string path)
        {
            throw new NotImplementedException();
        }

        public void PerformBackup(string path)
        {
            throw new NotImplementedException();
        }

        public void PerformBackup(bool incluedTimeStamp)
        {
            throw new NotImplementedException();
        }

        public void ResetSampleCounts()
        {
            SampleCount = 0;
            ISampleCount = 0;
        }

        #region IDisposable Members

        public void Dispose()
        {
            return;
        }

        #endregion IDisposable Members
    }
}