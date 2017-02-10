using System;
using System.Collections.Generic;
using CruiseDAL.DataObjects;
using FSCruiser.Core.Models;

namespace FSCruiser.Core
{
    public interface IApplicationController : IDisposable
    {
        #region Properties

        IViewController ViewController { get; }

        //ApplicationSettings Settings { get; }

        CruiseDAL.DAL DataStore { get; }

        #endregion Properties

        #region Methods

        //SampleGroupDO CreateNewSampleGroup(StratumDO stratum);

        //TreeDefaultValueDO CreateNewTreeDefaultValue(String pProd);

        void OpenFile();

        void OpenFile(String path);

        void HandleNonCriticalException(Exception ex, string optMessage);

        void LogTreeCountEdit(CountTreeDO countTree, long oldValue, long newValue);

        void LogSumKPIEdit(CountTreeDO countTree, long oldValue, long newValue);

        void OnLeavingCurrentUnit(System.ComponentModel.CancelEventArgs e);

        void PerformBackup(string path);

        void PerformBackup(bool incluedTimeStamp);

        #endregion Methods
    }
}