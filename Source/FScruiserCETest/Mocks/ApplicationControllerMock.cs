﻿using System;
using System.Collections.Generic;
using CruiseDAL.DataObjects;
using FSCruiser.Core;
using FSCruiser.Core.Models;

namespace FSCruiserV2.Test.Mocks
{
    public class ApplicationControllerMock : IApplicationController
    {
        public int ISampleCount { get; set; }

        public int SampleCount { get; set; }


        public ApplicationControllerMock()
        {
            ViewController = new ViewControllerMock() { ApplicationController = this };
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


        #region IApplicationController Members

        public event Action FileStateChanged;

        public IViewController ViewController
        {
            get;
            set;
        }

        public CruiseDAL.DAL DataStore
        {
            get { throw new NotImplementedException(); }
        }

        public void OpenFile()
        {
            throw new NotImplementedException();
        }

        public void OpenFile(string path)
        {
            throw new NotImplementedException();
        }

        public void HandleNonCriticalException(Exception ex, string optMessage)
        {
            throw new NotImplementedException();
        }

        public void OnLeavingCurrentUnit()
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

        #endregion
    }
}