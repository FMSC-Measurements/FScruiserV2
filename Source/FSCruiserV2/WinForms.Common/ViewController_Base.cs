using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FSCruiser.Core;
using FSCruiser.Core.Models;
using FSCruiser.WinForms.DataEntry;
using FScruiser.Core.Services;
using CruiseDAL;
using System.Diagnostics;
using FScruiser.Services;
using FScruiser.Data;

#if !NetCF
using Microsoft.AppCenter.Crashes;
#endif

namespace FSCruiser.WinForms.Common
{
    public abstract class WinFormsViewControllerBase : IViewController
    {
        //private Dictionary<StratumDO, FormLogs> _logViews = new Dictionary<StratumDO, FormLogs>();

        protected object _dataEntrySyncLock = new object();
        private FormMain _main;

        private int _showWait = 0;

        public IApplicationController ApplicationController { get; set; }

        #region IViewController Members

        public event CancelEventHandler ApplicationClosing;

        //public bool EnableCruiserSelectionPopup { get; set; }

        public FormMain MainView
        {
            get
            {
                if (_main == null)
                {
                    _main = new FormMain(this.ApplicationController);
                    _main.Closing += new CancelEventHandler(OnApplicationClosing);
#if !NetCF
                    _main.StartPosition = FormStartPosition.CenterScreen;
#endif
                }
                return _main;
            }
            protected set
            {
                _main = value;
            }
        }

        public void Run()
        {
            BeginShowSplash();
            Application.Run(MainView);
        }

        public abstract void BeginShowSplash();

        public void ShowMain()
        {
            this.MainView.Show();
        }

        public void ShowAbout()
        {
            using (FormAbout view = new FormAbout())
            {
                view.ShowDialog();
            }
        }

        public abstract void ShowBackupUtil();

        public abstract bool ShowOpenCruiseFileDialog(out string fileName);

        protected void ShowDataEntry(IDataEntryDataService dataService, ISampleSelectorRepository sampleSelectorRepository)
        {
            try
            {
                using (var dataEntryView = new FormDataEntry(this.ApplicationController
                    , ApplicationSettings.Instance
                    , dataService, sampleSelectorRepository))
                {
#if !NetCF
                    dataEntryView.ShowDialog(MainView);
#else
                    dataEntryView.ShowDialog();
#endif
                }
            }
            catch (UserFacingException e)
            {
                var exType = e.GetType();

                MessageBox.Show(e.Message, exType.Name);
            }
            finally
            {
                SaveData(dataService, sampleSelectorRepository);
            }
        }

        void SaveData(IDataEntryDataService dataService, ISampleSelectorRepository sampleSelectorRepository)
        {
            try
            {
                sampleSelectorRepository.SaveSamplerStates();

                Exception ex;

                ex = dataService.SaveNonPlotData();
                ex = dataService.SavePlotData() ?? ex;
                if (ex != null)
                {
                    throw ex;
                }

                ApplicationController.OnLeavingCurrentUnit();
            }
            catch (FMSC.ORM.ReadOnlyException ex)
            {
                MessageBox.Show("File Is Read Only \r\n" + dataService.DataStore.Path);
            }
            catch (FMSC.ORM.ConstraintException ex)
            {
                MessageBox.Show("Data Constraint Failed\r\n" + ex.Message, "Error");
                if (DialogService.AskYesNo("Would you like to go back to data entry?", string.Empty))
                {
                    ShowDataEntry(dataService, sampleSelectorRepository);
                }
            }
            catch (Exception ex)
            {
                ReportException(ex);
                if (DialogService.AskYesNo("Would you like to go back to data entry?", string.Empty))
                {
                    ShowDataEntry(dataService, sampleSelectorRepository);
                }
            }
        }

        void ReportException(Exception e)
        {
#if NetCF
            try
            {
                var report = new FMSC.Utility.ErrorHandling.ErrorReport(e, System.Reflection.Assembly.GetExecutingAssembly());
                report.MakeErrorReport();
                MessageBox.Show(e.GetType().Name, "Error");
            }
            catch(Exception ex)
            {
                Debug.Fail(ex.Message);
            }
#else
            //NBug.Exceptions.Report(e);
            Crashes.TrackError(e);
#endif
        }

        public void ShowDataEntry(CuttingUnit unit)
        {
            lock (_dataEntrySyncLock)
            {
                IDataEntryDataService dataService;
                ISampleSelectorRepository sampleSelectorReop;
                try
                {
                    dataService = new IDataEntryDataService(unit.Code, ApplicationController.DataStore);
                    sampleSelectorReop = new SampleSelectorRepository(new SamplerInfoDataservice_V2(ApplicationController.DataStore));
                }
                catch (CruiseConfigurationException e)
                {
                    MessageBox.Show(e.Message, e.GetType().Name);
                    return;
                }
                catch (UserFacingException e)
                {
                    var exType = e.GetType();
                    MessageBox.Show(e.Message, exType.Name);
                    return;
                }

                try
                {
                    ShowDataEntry(dataService, sampleSelectorReop);
                }
                catch (Exception ex)
                {
                    ReportException(ex);
                    //var timeStamp = DateTime.Now.ToString("HH_mm");

                    //var dumFilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "FScruiserDump" + timeStamp + ".xml");
                    //MessageBox.Show("FScruiser encountered a unexpected problem\r\n"
                    //    + "Dumping tree data to " + dumFilePath);
                    //dataService.Dump(dumFilePath);
                }
            }
        }

        public void ShowWait()
        {
            System.Threading.Interlocked.Increment(ref this._showWait);
            Cursor.Current = Cursors.WaitCursor;
        }

        public void HideWait()
        {
            if (System.Threading.Interlocked.Decrement(ref this._showWait) == 0)
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion IViewController Members

        #region IDisposable Members

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._main != null)
                {
                    this._main.Dispose();
                    this._main = null;
                }
            }
        }

        #endregion IDisposable Members

        protected void OnApplicationClosing(object sender, CancelEventArgs e)
        {
            if (ApplicationClosing != null)
            {
                this.ApplicationClosing(sender, e);
            }
        }
    }
}