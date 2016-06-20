using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;
using CruiseDAL;
using CruiseDAL.DataObjects;
using FSCruiser.Core.Models;
using FSCruiser.Core.Workers;

namespace FSCruiser.Core
{
    public enum BackUpMethod { None = 0, LeaveUnit = 1, TimeInterval = 2 }

    public class ApplicationController : IApplicationController
    {
        //private int _talliesSinceLastSave = 0;

        //private List<CruiserVM> _cruisers;
        //private bool _allowBackup;

        FileLoadWorker FileLoadWorker { get; set; }

        public ApplicationSettings Settings
        {
            get { return ApplicationSettings.Instance; }
        }

        public IExceptionHandler ExceptionHandler { get; set; }

        //public List<CruiserVM> Cruisers { get { return _cruisers; } }
        public CruiseDAL.DAL _cDal
        {
            get
            {
                if (FileLoadWorker == null) { return null; }
                else { return FileLoadWorker.DataStore; }
            }
        }

        public IList<CuttingUnitVM> CuttingUnits
        {
            get
            {
                if (FileLoadWorker == null) { return null; }
                else { return FileLoadWorker.CuttingUnits; }
            }
        }

        CuttingUnitVM _currentUnit;

        public CuttingUnitVM CurrentUnit
        {
            get { return _currentUnit; }
            set
            {
                if (_currentUnit == value) { return; }
                _currentUnit = value;
                OnCurrentUnitChanged();
            }
        }

        //public long UnitTreeNumIndex = 0;
        //public BackUpMethod BackUpMethod { get; set; }

        public IViewController ViewController { get; protected set; }

        public ApplicationController(IViewController viewController)
        {
            viewController.ApplicationController = this;
            viewController.ApplicationClosing += new CancelEventHandler(OnApplicationClosing);
            this.ViewController = viewController;

            ExceptionHandler = new FSCruiser.WinForms.ExceptionHandler();
            ApplicationSettings.ExceptionHandler = ExceptionHandler;

            ApplicationSettings.Initialize();
        }

        #region exception handleing

        public void HandleNonCriticalException(Exception ex, string optMessage)
        {
            if (optMessage == null)
            {
                optMessage = ex.Message;
            }
            if (ex != null)
            {
                Logger.Log.E(optMessage, ex);
            }
            ViewController.ShowMessage(optMessage, "Non-Critical Error", MessageBoxIcon.Asterisk);
            //MessageBox.Show(optMessage, "Non-Critical Error");
        }

        public void HandleException(Exception ex, string optMessage, bool isCritical, bool createErrorReport)
        {
            if (createErrorReport)
            {
                FMSC.Utility.ErrorHandling.ErrorReport report = new FMSC.Utility.ErrorHandling.ErrorReport(ex, Assembly.GetCallingAssembly());
                report.MakeErrorReport();
            }

            Logger.Log.E(ex);
            ViewController.ShowMessage(optMessage ?? ex.Message,
                (isCritical) ? "Error" : "Non-Critical Error",
                MessageBoxIcon.Asterisk);
        }

        #endregion exception handleing

        #region File

        public void OpenFile()
        {
            string filePath;
            if (this.ViewController.ShowOpenCruiseFileDialog(out filePath) == DialogResult.OK)
            {
                OpenFile(filePath);
            }
        }

        public void OpenFile(string path)
        {
            if (this.FileLoadWorker != null)
            {
                FileLoadWorker.Dispose();
            }

            var worker = new FileLoadWorker(path, this);
            worker.ExceptionThrown += this.HandleFileLoadError;
            worker.Ended += this.HandleFileLoadEnd;
            worker.Starting += this.HandleFileLoadStart;
            worker.BeginWork();

            this.FileLoadWorker = worker;
        }

        void HandleFileLoadError(object sender, WorkerExceptionThrownEventArgs e)
        {
            var ex = e.Exception;
            if (ex is FMSC.ORM.ReadOnlyException)
            {
                HandleException(ex, "Unable to open file because it is read only", false, true);
                e.Handled = true;
            }
            else if (ex is FMSC.ORM.SQLException)
            {
                HandleException(ex, "File Read Error : " + ex.GetType().Name, false, true);
                e.Handled = true;
            }
            else if (ex is System.IO.IOException)
            {
                HandleException(ex, "Unable to open file : " + ex.GetType().Name, false, true);
                e.Handled = true;
            }

            ViewController.HandleFileStateChanged();
        }

        void HandleFileLoadStart(object sender
            , FSCruiser.Core.Workers.WorkerProgressChangedEventArgs e)
        {
            ViewController.ShowWait();
        }

        void HandleFileLoadEnd(object sender
            , FSCruiser.Core.Workers.WorkerProgressChangedEventArgs e)
        {
            //var worker = sender as FSCruiser.Core.Workers.FileLoadWorker;
            //if (worker == null) { return; }

            ViewController.HideWait();
            ViewController.HandleFileStateChanged();
            if (this.FileLoadWorker.IsDone)
            {
                var filePath = _cDal.Path;
                var fileName = System.IO.Path.GetFileName(this._cDal.Path);

                ViewController.EnableLogGrading = _cDal.ExecuteScalar<bool>("SELECT LogGradingEnabled FROM Sale Limit 1;");

                Settings.AddRecentProject(new RecentProject(fileName, filePath));
                ApplicationSettings.Save();
            }
        }

        #endregion File

        private void OnCurrentUnitChanged()
        {
        }

        #region tally setup

        public TreeDefaultValueDO CreateNewTreeDefaultValue(String pProd)
        {
            TreeDefaultValueDO newTDV = new TreeDefaultValueDO();
            newTDV.DAL = this._cDal;
            newTDV.PrimaryProduct = pProd;
            newTDV.LiveDead = "L";

            if (this.ViewController.ShowEditTreeDefault(newTDV) == DialogResult.OK)
            {
                try
                {
                    newTDV.Save();
                    return newTDV;
                }
                catch (Exception e)
                {
                    ExceptionHandler.HandelEx(new UserFacingException("oops Tree Default save error", e));
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public SampleGroupDO CreateNewSampleGroup(StratumDO stratum)
        {
            SampleGroupDO newSG = new SampleGroupDO();
            newSG.DAL = this._cDal;
            newSG.Stratum = stratum;
            newSG.UOM = this._cDal.ExecuteScalar("Select DefaultUOM FROM Sale;") as String;
            //newSG.UOM = this._cDal.ReadSingleRow<SaleDO>("Sale", false, null).DefaultUOM;

            if (this.ViewController.ShowEditSampleGroup(newSG, true) == DialogResult.OK)
            {
                return newSG;
            }
            else
            {
                return null;
            }
        }

        #endregion tally setup

        #region backup

        private string GetBackupFileName(string backupDir, bool addTimeStamp)
        {
            string originalFileName = System.IO.Path.GetFileName(this._cDal.Path);

            //regex disected
            //prefix (optional): "BACK_"
            //core: one or more characters that extends until the time or postfix is found
            //time (optional): time stamp
            //postfix: file extention and optional component indicator

            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(
                @"(?<prefix>BACK_)*"
                + @"(?<core>.+?)"
                //+ @"(?<time>\(\d{4}_\d{2}_\d{2}__\d{2}_\d{2}\))?"
                + @"(?<compID>[.](?:[m]|\d+))?"
                + @"(?<ext>[\.](?:cruise))"
                , System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            if (!regex.IsMatch(originalFileName))
            {
                return null;
            }

            string backupFileName = regex.Replace(originalFileName,
                (m) =>
                {
                    var result = Constants.BACKUP_PREFIX + m.Groups["core"].Value;
                    if (addTimeStamp)
                    {
                        result += DateTime.Now.ToString(Constants.BACKUP_TIME_FORMAT);
                    }
                    result += (m.Groups["compID"] != null) ? m.Groups["compID"].Value : String.Empty;
                    result += ".back-cruise";
                    return result;
                }
            );

            return backupDir + "\\" + backupFileName;
        }

        public void PerformBackup(bool useTS)
        {
            string backupDir;
            if (Settings.BackUpToCurrentDir || String.IsNullOrEmpty(Settings.BackupDir))
            {
                backupDir = System.IO.Path.GetDirectoryName(this._cDal.Path);
            }
            else
            {
                backupDir = Settings.BackupDir;
            }

            this.PerformBackup(this.GetBackupFileName(backupDir, useTS));
        }

        public void PerformBackup(string path)
        {
            //if (!_allowBackup)
            //{
            //    ViewController.ShowMessage("Back up not allowed", "Warning", MessageBoxIcon.Asterisk);
            //    return;
            //}

            try
            {
                if (path == null)
                {
                    throw new ArgumentException("Invalid Backup file path", "path");
                }

                this.ViewController.ShowWait();
                this._cDal.CopyTo(path, true);
            }
            catch (Exception e)
            {
                this.HandleException(e, "Could not perform back up.\r\nMake sure you have permission to write to the directory selected", false, true);
                //this.HandleNonCriticalException(e, "oops. Something went wrong creating a backup.\r\n Try exiting FSCruiser and manualy creating a copy of the file");
            }
            finally
            {
                this.ViewController.HideWait();
            }
        }

        #endregion backup

        public void LogTreeCountEdit(CountTreeDO countTree, long oldValue, long newValue)
        {
            this._cDal.LogMessage(String.Format("Tree Count Edit: CT_CN={0}; PrevVal={1}; NewVal={2}", countTree.CountTree_CN, oldValue, newValue), "I");
        }

        public void LogSumKPIEdit(CountTreeDO countTree, long oldValue, long newValue)
        {
            this._cDal.LogMessage(String.Format("SumKPI Edit: CT_CN={0}; PrevVal={1}; NewVal={2}", countTree.CountTree_CN, oldValue, newValue), "I");
        }

        private void OnApplicationClosing(object sender, CancelEventArgs e)
        {
            ApplicationSettings.Save();
            if (this._cDal != null)
            {
                this._cDal.Dispose();
            }
        }

        public void OnLeavingCurrentUnit(System.ComponentModel.CancelEventArgs e)
        {
            if (!e.Cancel && this.Settings.BackUpMethod == BackUpMethod.LeaveUnit)
            {
                this.PerformBackup(false);
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (FileLoadWorker != null)
                {
                    FileLoadWorker.Dispose();
                    FileLoadWorker = null;
                }
                if (this.ViewController != null)
                {
                    this.ViewController.Dispose();
                    this.ViewController = null;
                }
            }
        }

        #endregion IDisposable Members
    }
}