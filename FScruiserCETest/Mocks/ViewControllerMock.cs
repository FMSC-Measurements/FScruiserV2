using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using FSCruiserV2.Logic;

namespace FSCruiserV2.Test.Mocks
{
    public class ViewControllerMock : IViewController 
    {

        #region IViewController Members

        public event System.ComponentModel.CancelEventHandler ApplicationClosing;

        private ApplicationControllerMock _controller; 

        public IApplicationController ApplicationController
        {
            get
            {
                return _controller;
            }
            set
            {
                _controller = (ApplicationControllerMock)value;
            }
        }

        public bool EnableLogGrading
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public FSCruiserV2.Forms.FormMain MainView
        {
            get { throw new NotImplementedException(); }
        }

        public FSCruiserV2.Forms.FormNumPad NumPadDialog
        {
            get { throw new NotImplementedException(); }
        }

        public FSCruiserV2.Forms.Form3PNumPad ThreePNumPad
        {
            get { throw new NotImplementedException(); }
        }

        public FSCruiserV2.Forms.FormPlotInfo PlotInfoView
        {
            get { throw new NotImplementedException(); }
        }

        public void BeginShowSplash()
        {
            throw new NotImplementedException();
        }

        public void HandleCuttingUnitDataLoaded()
        {
            throw new NotImplementedException();
        }

        public void HandleCruisersChanged()
        {
            throw new NotImplementedException();
        }

        public void SignalInvalidAction()
        {
            throw new NotImplementedException();
        }

        public void ShowMain()
        {
            throw new NotImplementedException();
        }

        public void ShowAbout()
        {
            throw new NotImplementedException();
        }

        public CruiseDAL.DataObjects.TreeDefaultValueDO ShowAddPopulation()
        {
            throw new NotImplementedException();
        }

        public CruiseDAL.DataObjects.TreeDefaultValueDO ShowAddPopulation(CruiseDAL.DataObjects.SampleGroupDO sg)
        {
            throw new NotImplementedException();
        }



        public void ShowMessage(string message, string caption, System.Windows.Forms.MessageBoxIcon icon)
        {
            System.Diagnostics.Trace.WriteLine(String.Format("Message Box: caption = {0}; message = {1};", caption, message)); 
        }

        public bool AskYesNo(string message, string caption, System.Windows.Forms.MessageBoxIcon icon)
        {
            return AskYesNo(message, caption, icon, true);
        }

        public bool AskYesNo(string message, string caption, System.Windows.Forms.MessageBoxIcon icon, bool defaultNo)
        {
            System.Diagnostics.Trace.WriteLine(String.Format("Question Box: caption = {0}; message = {1};", caption, message));
            if (message == "Would You Like To Enter Tree Data?")
            {
                return false;
            }
            return true; 
        }

        public void SignalMeasureTree()
        {
            this._controller.SampleCount++;
            System.Diagnostics.Trace.WriteLine("Measure Tree");
        }

        public void SignalInsuranceTree()
        {
            this._controller.ISampleCount++; 
            System.Diagnostics.Trace.WriteLine("Measure Tree");
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void ShowBackupUtil()
        {
            throw new NotImplementedException();
        }

        public void ShowCruiserSelection(TreeVM tree)
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

        public System.Windows.Forms.DialogResult ShowLimitingDistanceDialog(float baf, bool isVariableRadius, TreeVM optTree, out string logMessage)
        {
            throw new NotImplementedException();
        }

        public void ShowLogsView(CruiseDAL.DataObjects.StratumDO stratum, TreeVM tree)
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

        public void ShowDataEntry(CruiseDAL.DataObjects.CuttingUnitDO unit)
        {
            throw new NotImplementedException();
        }

        public System.Windows.Forms.DialogResult ShowPlotInfo(PlotVM plotInfo, bool allowEdit)
        {
            throw new NotImplementedException();
        }

        public void ShowTallySettings(CountTreeVM count)
        {
            throw new NotImplementedException();
        }

        public bool AskCancel(string message, string caption, System.Windows.Forms.MessageBoxIcon icon, bool defaultCancel)
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

        #endregion
    }
}
