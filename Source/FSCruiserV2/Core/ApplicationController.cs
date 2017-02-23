﻿using System;
using System.ComponentModel;
using System.Reflection;
using CruiseDAL;
using CruiseDAL.DataObjects;
using FSCruiser.Core.Models;
using FSCruiser.Core.Workers;
using FScruiser.Core.Services;

namespace FSCruiser.Core
{
    public class ApplicationController : IApplicationController
    {
        FileLoadWorker _fileLoadWorker;

        public ApplicationSettings Settings
        {
            get { return ApplicationSettings.Instance; }
        }

        public IExceptionHandler ExceptionHandler { get; set; }

        public DAL DataStore { get; protected set; }

        public IViewController ViewController { get; protected set; }

        public ApplicationController(IViewController viewController)
        {
            viewController.ApplicationController = this;
            viewController.ApplicationClosing += new CancelEventHandler(OnApplicationClosing);
            this.ViewController = viewController;

            ExceptionHandler = new FSCruiser.WinForms.ExceptionHandler();

            try
            {
                ApplicationSettings.Initialize();
            }
            catch
            {
                DialogService.Instance.ShowMessage("Unable to load applications settings");
                ApplicationSettings.Instance = new ApplicationSettings();
            }
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
            DialogService.ShowMessage(optMessage, "Non-Critical Error");
            //MessageBox.Show(optMessage, "Non-Critical Error");
        }

        public void HandleException(Exception ex, string optMessage, bool isCritical, bool createErrorReport)
        {
            if (createErrorReport)
            {
#if NetCF
                FMSC.Utility.ErrorHandling.ErrorReport report = new FMSC.Utility.ErrorHandling.ErrorReport(ex, Assembly.GetCallingAssembly());
                report.MakeErrorReport();
#endif
            }

            Logger.Log.E(ex);
            DialogService.ShowMessage(optMessage ?? ex.Message,
                (isCritical) ? "Error" : "Non-Critical Error");
        }

        #endregion exception handleing

        #region File

        public void OpenFile()
        {
            string filePath;
            if (this.ViewController.ShowOpenCruiseFileDialog(out filePath))
            {
                OpenFile(filePath);
            }
        }

        public void OpenFile(string path)
        {
            if (this._fileLoadWorker != null)
            {
                _fileLoadWorker.Dispose();
            }

            var worker = new FileLoadWorker(path, this);
            worker.ExceptionThrown += this.HandleFileLoadError;
            worker.Ended += this.HandleFileLoadEnd;
            worker.Starting += this.HandleFileLoadStart;
            worker.Start();

            this._fileLoadWorker = worker;
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

            if (this._fileLoadWorker.IsDone)
            {
                var dataStore = _fileLoadWorker.DataStore;
                DataStore = dataStore;
                _fileLoadWorker.Dispose();
                _fileLoadWorker = null;

                ViewController.EnableLogGrading = DataStore.ExecuteScalar<bool>("SELECT LogGradingEnabled FROM Sale Limit 1;");

                var filePath = dataStore.Path;
                var fileName = System.IO.Path.GetFileName(dataStore.Path);

                ApplicationSettings.Instance.AddRecentProject(new RecentProject(fileName, filePath));
                try
                {
                    ApplicationSettings.Save();
                }
                catch { /* do nothing */ } //TODO Nbug
            }
            ViewController.HandleFileStateChanged();
        }

        #endregion File

        #region backup

        private string GetBackupFileName(string backupDir, bool addTimeStamp)
        {
            string originalFileName = System.IO.Path.GetFileName(this.DataStore.Path);

            //regex disected
            //prefix (optional): "BACK_"
            //core: one or more characters that extends until the time or postfix is found
            //time (optional): time stamp
            //compID(optional): component number or master file indicator
            //ext: file extention and optional component indicator

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

            if (ApplicationSettings.Instance.BackUpToCurrentDir
                || String.IsNullOrEmpty(ApplicationSettings.Instance.BackupDir))
            {
                backupDir = System.IO.Path.GetDirectoryName(this.DataStore.Path);
            }
            else
            {
                backupDir = ApplicationSettings.Instance.BackupDir;
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
                this.DataStore.CopyTo(path, true);
            }
            catch (Exception e)
            {
                this.HandleException(e, "Could not perform back up.\r\nMake sure you have permission to write to the directory selected", false, true);
            }
            finally
            {
                this.ViewController.HideWait();
            }
        }

        #endregion backup

        public void LogTreeCountEdit(CountTreeDO countTree, long oldValue, long newValue)
        {
            this.DataStore.LogMessage(String.Format("Tree Count Edit: CT_CN={0}; PrevVal={1}; NewVal={2}", countTree.CountTree_CN, oldValue, newValue), "I");
        }

        public void LogSumKPIEdit(CountTreeDO countTree, long oldValue, long newValue)
        {
            this.DataStore.LogMessage(String.Format("SumKPI Edit: CT_CN={0}; PrevVal={1}; NewVal={2}", countTree.CountTree_CN, oldValue, newValue), "I");
        }

        private void OnApplicationClosing(object sender, CancelEventArgs e)
        {
            try
            {
                ApplicationSettings.Save();
            }
            catch { /* do nothing */ }
            if (this.DataStore != null)
            {
                this.DataStore.Dispose();
            }
        }

        public void OnLeavingCurrentUnit(System.ComponentModel.CancelEventArgs e)
        {
            if (!e.Cancel && ApplicationSettings.Instance.BackUpMethod == BackUpMethod.LeaveUnit)
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
                if (_fileLoadWorker != null)
                {
                    _fileLoadWorker.Dispose();
                    _fileLoadWorker = null;
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