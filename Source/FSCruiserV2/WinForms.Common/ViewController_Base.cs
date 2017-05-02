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

namespace FSCruiser.WinForms.Common
{
    public abstract class WinFormsViewControllerBase : IViewController
    {
        //private Dictionary<StratumDO, FormLogs> _logViews = new Dictionary<StratumDO, FormLogs>();

        protected object _dataEntrySyncLock = new object();
        private FormMain _main;
        private Form3PNumPad _threePNumPad;

        private int _showWait = 0;
        private bool _enableLogGrading = true;

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

        public Form3PNumPad ThreePNumPad
        {
            get
            {
                if (_threePNumPad == null)
                {
                    _threePNumPad = new Form3PNumPad();
                }
                return _threePNumPad;
            }
            protected set
            {
                _threePNumPad = value;
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

        protected void ShowDataEntry(IDataEntryDataService dataService)
        {
            try
            {
                using (var dataEntryView = new FormDataEntry(this.ApplicationController
                    , ApplicationSettings.Instance
                    , dataService))
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
                SaveData(dataService);
            }
        }

        void SaveData(IDataEntryDataService dataService)
        {
            try
            {
                Exception ex;

                ex = dataService.SaveNonPlotData();
                ex = dataService.SavePlotData() ?? ex;
                if (ex != null)
                {
                    throw ex;
                }

                ApplicationController.OnLeavingCurrentUnit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().Name + " " + ex.Message, "");
                if (DialogService.AskYesNo("Relaunch data entry?", string.Empty))
                {
                    ShowDataEntry(dataService);
                }
            }
        }

        public void ShowDataEntry(CuttingUnit unit)
        {
            lock (_dataEntrySyncLock)
            {
                IDataEntryDataService dataService;
                try
                {
                    dataService = new IDataEntryDataService(unit.Code, ApplicationController.DataStore);
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
                    ShowDataEntry(dataService);
                }
                catch
                {
                    //var timeStamp = DateTime.Now.ToString("HH_mm");

                    //var dumFilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "FScruiserDump" + timeStamp + ".xml");
                    //MessageBox.Show("FScruiser encountered a unexpected problem\r\n"
                    //    + "Dumping tree data to " + dumFilePath);
                    //dataService.Dump(dumFilePath);
                }
            }
        }

        //public int? ShowNumericValueInput(int? min, int? max, int? initialValue, bool acceptNullInput)
        //{
        //    this.NumPadDialog.ShowDialog(min, max, initialValue, acceptNullInput);
        //    return this.NumPadDialog.UserEnteredValue;
        //}

        /// <summary>
        /// </summary>
        /// <returns>KPI, value is -1 if STM</returns>
        public int? AskKPI(int min, int max)
        {
            ThreePNumPad.ShowDialog(min, max, null, false);
            return ThreePNumPad.UserEnteredValue;
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