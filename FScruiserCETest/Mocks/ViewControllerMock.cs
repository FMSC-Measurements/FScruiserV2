using System;
using CruiseDAL.DataObjects;
using FSCruiser.Core;
using FSCruiser.Core.Models;
using FSCruiser.WinForms;
using FSCruiser.WinForms.DataEntry;

namespace FSCruiserV2.Test.Mocks
{
    public class ViewControllerMock : IViewController
    {
        #region IViewController Members

        private ApplicationControllerMock _controller;

        public event System.ComponentModel.CancelEventHandler ApplicationClosing;

        public IApplicationController ApplicationController { get; set; }

        public bool EnableLogGrading { get; set; }

        public FormMain MainView
        {
            get { throw new NotImplementedException(); }
        }

        public FormNumPad NumPadDialog
        {
            get { throw new NotImplementedException(); }
        }

        public FormPlotInfo PlotInfoView
        {
            get { throw new NotImplementedException(); }
        }

        public Form3PNumPad ThreePNumPad
        {
            get { throw new NotImplementedException(); }
        }

        public bool AskCancel(string message, string caption, System.Windows.Forms.MessageBoxIcon icon, bool defaultCancel)
        {
            throw new NotImplementedException();
        }

        public bool AskYesNo(string message, string caption, System.Windows.Forms.MessageBoxIcon icon)
        {
            return AskYesNo(message, caption, icon, true);
        }

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

        public void BeginShowSplash()
        {
            throw new NotImplementedException();
        }

        public void HandleCruisersChanged()
        {
            throw new NotImplementedException();
        }

        public void HandleCuttingUnitDataLoaded()
        {
            throw new NotImplementedException();
        }

        public void HideWait()
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

        public TreeDefaultValueDO ShowAddPopulation(CruiseDAL.DataObjects.SampleGroupDO sg)
        {
            throw new NotImplementedException();
        }

        public void ShowBackupUtil()
        {
            throw new NotImplementedException();
        }

        public void ShowCruiserSelection(Tree tree)
        {
            throw new NotImplementedException();
        }

        public void ShowDataEntry(CruiseDAL.DataObjects.CuttingUnitDO unit)
        {
            throw new NotImplementedException();
        }

        public System.Windows.Forms.DialogResult ShowEditSampleGroup(CruiseDAL.DataObjects.SampleGroupDO sg, bool allowEdit)
        {
            throw new NotImplementedException();
        }

        public System.Windows.Forms.DialogResult ShowEditTreeDefault(CruiseDAL.DataObjects.TreeDefaultValueDO tdv)
        {
            throw new NotImplementedException();
        }

        public System.Windows.Forms.DialogResult ShowLimitingDistanceDialog(float baf, bool isVariableRadius, Tree optTree, out string logMessage)
        {
            throw new NotImplementedException();
        }

        public void ShowLogsView(CruiseDAL.DataObjects.StratumDO stratum, Tree tree)
        {
            throw new NotImplementedException();
        }

        public void ShowMain()
        {
            throw new NotImplementedException();
        }

        public void ShowManageCruisers()
        {
            throw new NotImplementedException();
        }

        public System.Windows.Forms.DialogResult ShowOpenCruiseFileDialog(out string fileName)
        {
            throw new NotImplementedException();
        }

        public System.Windows.Forms.DialogResult ShowPlotInfo(Plot plotInfo, bool allowEdit)
        {
            throw new NotImplementedException();
        }

        public void ShowTallySettings(CountTree count)
        {
            throw new NotImplementedException();
        }

        public void ShowWait()
        {
            throw new NotImplementedException();
        }

        public void SignalInvalidAction()
        {
            throw new NotImplementedException();
        }

        public void HandleFileStateChanged()
        {
            throw new NotImplementedException();
        }

        public System.Windows.Forms.DialogResult ShowLimitingDistanceDialog(float baf, bool isVariableRadius, out string logMessage)
        {
            throw new NotImplementedException();
        }

        public void ShowLogsView(Stratum stratum, Tree tree)
        {
            throw new NotImplementedException();
        }

        public void ShowDataEntry(CuttingUnit unit)
        {
            throw new NotImplementedException();
        }

        public System.Windows.Forms.DialogResult ShowPlotInfo(Plot plotInfo, PlotStratum stratum, bool allowEdit)
        {
            throw new NotImplementedException();
        }

        public int? AskKPI(int min, int max)
        {
            throw new NotImplementedException();
        }

        public void SignalMeasureTree(bool showMessage)
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