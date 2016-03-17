using System;

using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FSCruiser.WinForms;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;
using FSCruiser.Core;
using FSCruiser.WinForms.DataEntry;

namespace FSCruiser.WinForms.Common
{
    public abstract class WinFormsViewControllerBase : IViewController
    {
        private static Thread _splashThread;

        private Dictionary<StratumDO, FormLogs> _logViews = new Dictionary<StratumDO, FormLogs>();

        protected object _dataEntrySyncLock = new object();
        private FormMain _main;
        private FormNumPad _numPadDialog;
        private Form3PNumPad _threePNumPad;
        private FormCruiserSelection _cruiserSelectionView;
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

        public FormCruiserSelection CruiserSelectionView
        {
            get
            {
                if (_cruiserSelectionView == null)
                {
                    _cruiserSelectionView = new FormCruiserSelection(this.ApplicationController);
                }
                return _cruiserSelectionView;
            }
        }

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

        public FormLogs GetLogsView(StratumDO stratum)
        {
            if (_logViews.ContainsKey(stratum))
            {
                return _logViews[stratum];
            }
            FormLogs logView = new FormLogs(this.ApplicationController, stratum.Stratum_CN.Value);
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


        public void ShowCruiserSelection(TreeVM tree)
        {
            if (this.ApplicationController.Settings.EnableCruiserPopup)
            {
                this.CruiserSelectionView.ShowDialog(tree);
            }
        }

        public abstract System.Windows.Forms.DialogResult ShowEditSampleGroup(CruiseDAL.DataObjects.SampleGroupDO sg, bool allowEdit);


        public abstract System.Windows.Forms.DialogResult ShowEditTreeDefault(CruiseDAL.DataObjects.TreeDefaultValueDO tdv);


        public abstract System.Windows.Forms.DialogResult ShowLimitingDistanceDialog(float baf, bool isVariableRadius, TreeVM optTree, out string logMessage);


        public void ShowLogsView(StratumDO stratum, TreeVM tree)
        {
            if (stratum == null)
            {
                MessageBox.Show("Invalid Action. Stratum not set.");
            }
            this.GetLogsView(stratum).ShowDialog(tree);
        }


        public abstract void ShowManageCruisers();


        public abstract System.Windows.Forms.DialogResult ShowOpenCruiseFileDialog(out string fileName);


        public abstract void ShowDataEntry(CuttingUnitVM unit);

        //public int? ShowNumericValueInput(int? min, int? max, int? initialValue, bool acceptNullInput)
        //{
        //    this.NumPadDialog.ShowDialog(min, max, initialValue, acceptNullInput);
        //    return this.NumPadDialog.UserEnteredValue;
        //}

        public DialogResult ShowPlotInfo(PlotVM plot, PlotStratum stratum, bool isNewPlot)
        {
            System.Diagnostics.Debug.Assert(plot != null);
            System.Diagnostics.Debug.Assert(stratum != null);

            if (stratum.Is3PPNT && isNewPlot)
            {
                using (var view = new Form3PPNTPlotInfo(this))
                {
                    return view.ShowDialog(plot, stratum, isNewPlot);
                }
            }
            else
            {
                using (var view = new FormPlotInfo())
                {
                    return view.ShowDialog(plot, stratum, isNewPlot);
                }
            }
        }


        public void ShowTallySettings(CountTreeVM count)
        {
            using(FormTallySettings view = new FormTallySettings(this.ApplicationController))
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
            ThreePNumPad.ShowDialog(min, max, null, true);
            return ThreePNumPad.UserEnteredValue;
        }
        
 

        public abstract void SignalMeasureTree(bool showMessage);


        public abstract void SignalInsuranceTree();

        


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

        #endregion

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
                if (this._cruiserSelectionView != null)
                {
                    this._cruiserSelectionView.Dispose();
                    this._cruiserSelectionView = null; 
                }

            }

        }

        #endregion

        protected void OnApplicationClosing(object sender, CancelEventArgs e)
        {
            if (ApplicationClosing != null)
            {
                this.ApplicationClosing(sender, e);
            }
        }
    }
}
