using System;
using CruiseDAL.DataObjects;
using FSCruiser.Core;
using FSCruiser.Core.Models;
using FScruiser.Core.Services;

namespace FSCruiserV2.Test.Mocks
{
    public class ViewControllerMock : IViewController
    {
        #region IViewController Members

        private ApplicationControllerMock _controller;

        public event System.ComponentModel.CancelEventHandler ApplicationClosing;

        public IApplicationController ApplicationController { get; set; }

        public bool EnableLogGrading { get; set; }

        public bool AskYesNo(string message, string caption, System.Windows.Forms.MessageBoxIcon icon, bool defaultNo)
        {
            System.Diagnostics.Debug.WriteLine(String.Format("Question Box: caption = {0}; message = {1};", caption, message));
            if (message == "Would You Like To Enter Tree Data?")
            {
                return false;
            }
            return true;
        }

        public void ShowMessage(string message, string caption, System.Windows.Forms.MessageBoxIcon icon)
        {
            System.Diagnostics.Debug.WriteLine(String.Format("Message Box: caption = {0}; message = {1};", caption, message));
        }

        public void SignalInsuranceTree()
        {
            this._controller.ISampleCount++;
            System.Diagnostics.Debug.WriteLine("Measure Tree");
        }

        public void SignalMeasureTree()
        {
            this._controller.SampleCount++;
            System.Diagnostics.Debug.WriteLine("Measure Tree");
        }

        public void HandleFileStateChanged()
        {
            throw new NotImplementedException();
        }

        public void ShowAbout()
        {
            throw new NotImplementedException();
        }

        public TreeDefaultValueDO ShowAddPopulation()
        {
            throw new NotImplementedException();
        }

        public TreeDefaultValueDO ShowAddPopulation(SampleGroupDO sg)
        {
            throw new NotImplementedException();
        }

        public void ShowBackupUtil()
        {
            throw new NotImplementedException();
        }


        public bool ShowLimitingDistanceDialog(float baf, bool isVariableRadius, out string logMessage)
        {
            throw new NotImplementedException();
        }

        public void ShowLogsView(Stratum stratum, Tree tree)
        {
            throw new NotImplementedException();
        }

        public void ShowManageCruisers()
        {
            throw new NotImplementedException();
        }

        public bool ShowOpenCruiseFileDialog(out string fileName)
        {
            throw new NotImplementedException();
        }

        public void ShowDataEntry(CuttingUnit unit)
        {
            throw new NotImplementedException();
        }

        public bool ShowPlotInfo(IDataEntryDataService dataService, Plot plotInfo, PlotStratum stratum, bool allowEdit)
        {
            throw new NotImplementedException();
        }

        public void ShowTallySettings(CountTree count)
        {
            throw new NotImplementedException();
        }

        public int? AskKPI(int min, int max)
        {
            throw new NotImplementedException();
        }

        public void ShowWait()
        {
            throw new NotImplementedException();
        }

        public void HideWait()
        {
            throw new NotImplementedException();
        }

        #endregion IViewController Members

        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion IDisposable Members
    }
}