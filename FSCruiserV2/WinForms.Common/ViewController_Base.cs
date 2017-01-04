using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FSCruiser.Core;
using FSCruiser.Core.Models;
using FSCruiser.WinForms.DataEntry;

namespace FSCruiser.WinForms.Common
{
    public abstract class WinFormsViewControllerBase : IViewController
    {
        private Dictionary<StratumDO, FormLogs> _logViews = new Dictionary<StratumDO, FormLogs>();

        protected object _dataEntrySyncLock = new object();
        private FormMain _main;
        private FormNumPad _numPadDialog;
        private Form3PNumPad _threePNumPad;
        protected FormDataEntry _dataEntryView;

        private int _showWait = 0;
        private bool _enableLogGrading = true;

        public IApplicationController ApplicationController { get; set; }

        #region IViewController Members

        public event CancelEventHandler ApplicationClosing;

        public bool EnableLogGrading
        {
            get { return _enableLogGrading; }
            set
            {
                lock (_dataEntrySyncLock)
                {
                    if (value == _enableLogGrading) { return; }
                    _enableLogGrading = value;
                    if (_dataEntryView != null)
                    {
                        _dataEntryView.HandleEnableLogGradingChanged();
                    }
                }
            }
        }

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

        public FormNumPad NumPadDialog
        {
            get
            {
                if (_numPadDialog == null)
                {
                    _numPadDialog = new FormNumPad();
                }
                return _numPadDialog;
            }
            protected set
            {
                _numPadDialog = value;
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

        public FormLogs GetLogsView(Stratum stratum)
        {
            if (_logViews.ContainsKey(stratum))
            {
                return _logViews[stratum];
            }
            FormLogs logView = new FormLogs(this.ApplicationController, stratum);
            _logViews.Add(stratum, logView);

            return logView;
        }

        //public void HandleCuttingUnitDataLoaded()
        //{
        //    lock (_dataEntrySyncLock)
        //    {
        //        if (_dataEntryView != null)
        //        {
        //            _dataEntryView.HandleCuttingUnitDataLoaded();
        //        }
        //    }
        //}

        public void HandleCruisersChanged()
        {
            lock (_dataEntrySyncLock)
            {
                if (_dataEntryView != null)
                {
                    _dataEntryView.HandleCruisersChanged();
                }
            }
        }

        public void HandleFileStateChanged()
        {
            if (this.MainView != null)
            {
                this.MainView.HandleFileStateChanged();
            }
        }

        public abstract void SignalInvalidAction();

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

        public abstract CruiseDAL.DataObjects.TreeDefaultValueDO ShowAddPopulation();

        public abstract CruiseDAL.DataObjects.TreeDefaultValueDO ShowAddPopulation(CruiseDAL.DataObjects.SampleGroupDO sg);

        public abstract void ShowBackupUtil();

        public virtual void ShowCruiserSelection(Tree tree)
        { }

        public abstract System.Windows.Forms.DialogResult ShowEditSampleGroup(CruiseDAL.DataObjects.SampleGroupDO sg, bool allowEdit);

        public abstract System.Windows.Forms.DialogResult ShowEditTreeDefault(CruiseDAL.DataObjects.TreeDefaultValueDO tdv);

        public abstract System.Windows.Forms.DialogResult ShowLimitingDistanceDialog(float baf, bool isVariableRadius, out string logMessage);

        public void ShowLogsView(Stratum stratum, Tree tree)
        {
            if (stratum == null)
            {
                MessageBox.Show("Invalid Action. Stratum not set.");
            }
            this.GetLogsView(stratum).ShowDialog(tree);
        }

        public abstract void ShowManageCruisers();

        public abstract System.Windows.Forms.DialogResult ShowOpenCruiseFileDialog(out string fileName);

        public void ShowDataEntry(CuttingUnit unit)
        {
            lock (_dataEntrySyncLock)
            {
                try
                {
                    using (_dataEntryView = new FormDataEntry(this.ApplicationController, unit))
                    {
#if !NetCF
                        _dataEntryView.Owner = MainView;
#endif
                        _dataEntryView.ShowDialog();
                    }
                }
                catch (UserFacingException e)
                {
                    var exType = e.GetType();

                    MessageBox.Show(e.Message, exType.Name);
                }
                finally
                {
                    _dataEntryView = null;
                }
            }
        }

        //public int? ShowNumericValueInput(int? min, int? max, int? initialValue, bool acceptNullInput)
        //{
        //    this.NumPadDialog.ShowDialog(min, max, initialValue, acceptNullInput);
        //    return this.NumPadDialog.UserEnteredValue;
        //}

        public DialogResult ShowPlotInfo(Plot plot, PlotStratum stratum, bool isNewPlot)
        {
            System.Diagnostics.Debug.Assert(plot != null);
            System.Diagnostics.Debug.Assert(stratum != null);

            if (stratum.Is3PPNT && isNewPlot)
            {
                using (var view = new Form3PPNTPlotInfo(this))
                {
#if !NetCF
                    view.Owner = this._dataEntryView;
                    view.StartPosition = FormStartPosition.CenterParent;
#endif
                    return view.ShowDialog(plot, stratum, isNewPlot);
                }
            }
            else
            {
                using (var view = new FormPlotInfo())
                {
#if !NetCF
                    view.Owner = this._dataEntryView;
                    view.StartPosition = FormStartPosition.CenterParent;
#endif
                    return view.ShowDialog(plot, stratum, isNewPlot);
                }
            }
        }

        public void ShowTallySettings(CountTree count)
        {
            try
            {
                count.Save();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return;
            }

            using (FormTallySettings view = new FormTallySettings(this.ApplicationController))
            {
                view.ShowDialog(count);
            }
        }

        public void ShowMessage(String message, String caption, MessageBoxIcon icon)
        {
            MessageBox.Show(message, caption, MessageBoxButtons.OK, icon, MessageBoxDefaultButton.Button1);
        }

        public bool AskYesNo(String message, String caption, MessageBoxIcon icon)
        {
            return DialogResult.Yes == MessageBox.Show(message, caption, MessageBoxButtons.YesNo, icon, MessageBoxDefaultButton.Button2);
        }

        public bool AskYesNo(String message, String caption, MessageBoxIcon icon, bool defaultNo)
        {
            return DialogResult.Yes == MessageBox.Show(message,
                caption,
                MessageBoxButtons.YesNo,
                icon,
                (defaultNo) ? MessageBoxDefaultButton.Button2 : MessageBoxDefaultButton.Button1);
        }

        public bool AskCancel(String message, String caption, MessageBoxIcon icon, bool defaultCancel)
        {
            return MessageBox.Show(message,
                caption,
                MessageBoxButtons.OKCancel,
                icon,
                (defaultCancel) ? MessageBoxDefaultButton.Button2 : MessageBoxDefaultButton.Button1) == DialogResult.Cancel;
        }

        /// <summary>
        /// </summary>
        /// <returns>KPI, value is -1 if STM</returns>
        public int? AskKPI(int min, int max)
        {
            ThreePNumPad.ShowDialog(min, max, null, false);
            return ThreePNumPad.UserEnteredValue;
        }

        public abstract void SignalMeasureTree(bool showMessage);

        public abstract void SignalInsuranceTree();

        public abstract void SignalTally();

        public abstract void SignalPageChanged();

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
                if (this._numPadDialog != null)
                {
                    this._numPadDialog.Dispose();
                    this._numPadDialog = null;
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