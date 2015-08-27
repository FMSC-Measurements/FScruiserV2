using System;

using System.Collections.Generic;
using System.Text;
using FSCruiserV2.Forms;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using CruiseDAL.DataObjects;

namespace FSCruiserV2.Logic
{
    public abstract class WinFormsViewControllerBase : IViewController
    {
        private static Thread _splashThread; 

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



        public void BeginShowSplash()
        {
            _splashThread = new Thread(ViewController.ShowSplash);//TODO ensure thread gets killed when not needed
            _splashThread.Name = "Splash";
            _splashThread.Start();
        }

        private static void ShowSplash()
        {
            using (FormAbout a = new FormAbout())
            {
                Application.Run(a);
            }
        }





        public void HandleCuttingUnitDataLoaded()
        {
            lock (_dataEntrySyncLock)
            {
                if (_dataEntryView != null)
                {
                    _dataEntryView.HandleCuttingUnitDataLoaded();
                }
            }
        }

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
            this.CruiserSelectionView.ShowDialog(tree);
        }

        public abstract System.Windows.Forms.DialogResult ShowEditSampleGroup(CruiseDAL.DataObjects.SampleGroupDO sg, bool allowEdit);


        public abstract System.Windows.Forms.DialogResult ShowEditTreeDefault(CruiseDAL.DataObjects.TreeDefaultValueDO tdv);


        public abstract System.Windows.Forms.DialogResult ShowLimitingDistanceDialog(float baf, bool isVariableRadius, TreeVM optTree, out string logMessage);


        public abstract void ShowLogsView(CruiseDAL.DataObjects.StratumDO stratum, TreeVM tree);


        public abstract void ShowManageCruisers();


        public abstract System.Windows.Forms.DialogResult ShowOpenCruiseFileDialog(out string fileName);


        public abstract void ShowDataEntry(CuttingUnitDO unit);
       

        public DialogResult ShowPlotInfo(PlotVM plotInfo, bool allowEdit)
        {
            if (plotInfo == null) { return DialogResult.None; }
            IPlotInfoDialog view = null;
            try
            {
                if (plotInfo.Stratum.Method == "3PPNT" && allowEdit)
                {
                    view = new Form3PPNTPlotInfo(this.ApplicationController);
                }
                else
                {
                    view = new FormPlotInfo(this.ApplicationController);
                }

                return view.ShowDialog(plotInfo, allowEdit);
            }
            finally
            {
                ((Form)view).Dispose();
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
