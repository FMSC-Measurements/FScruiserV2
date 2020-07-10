using System;
using System.ComponentModel;
using System.Reflection;
using CruiseDAL;
using CruiseDAL.DataObjects;
using FSCruiser.Core.Models;
using FSCruiser.Core.Workers;
using FScruiser.Core.Services;
using System.Diagnostics;
#if !NetCF
using Microsoft.AppCenter.Crashes;
#endif

namespace FSCruiser.Core
{
    public class ApplicationController : IApplicationController
    {
        FileLoadWorker _fileLoadWorker;

        public IExceptionHandler ExceptionHandler { get; set; }

        public DAL DataStore { get; protected set; }

        public IViewController ViewController { get; protected set; }

        public ApplicationSettings Settings { get; set; }

        public event Action FileStateChanged;

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
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                DialogService.Instance.ShowMessage("Unable to load applications settings");
                ApplicationSettings.Instance = new ApplicationSettings();
            }
            Settings = ApplicationSettings.Instance;
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
#if !NetCF
                Crashes.TrackError(ex);
#endif
                Trace.WriteLine("Error::" + ex.Message + "::" + optMessage);
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

#if !NetCF
            Crashes.TrackError(ex);
#endif
            Trace.WriteLine("Error::" + ex.Message + "::" + optMessage);

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

        protected void OnFileStateChanged()
        {
            var fileStateChanged = FileStateChanged;
            if (fileStateChanged != null)
            {
                fileStateChanged();
            }
        }

        void HandleFileLoadError(object sender, WorkerExceptionThrownEventArgs e)
        {
            var ex = e.Exception;
            if (ex is FMSC.ORM.SchemaUpdateException)//file could not be updated. reason why is in innerException
            {
                ex = ex.InnerException;
            }

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
            else

                OnFileStateChanged();
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

                var filePath = dataStore.Path;
                var fileName = System.IO.Path.GetFileName(dataStore.Path);

                var appSettings = ApplicationSettings.Instance;

                appSettings.AddRecentProject(new RecentProject(fileName, filePath));
                try
                {
                    Settings.Save();
                }
                catch { /* do nothing */ } //TODO Nbug
            }
            OnFileStateChanged();
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

            if (Settings.BackUpToCurrentDir
                || String.IsNullOrEmpty(Settings.BackupDir))
            {
                backupDir = System.IO.Path.GetDirectoryName(this.DataStore.Path);
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

        private void OnApplicationClosing(object sender, CancelEventArgs e)
        {
            try
            {
                Settings.Save();
            }
            catch { /* do nothing */ }
            if (this.DataStore != null)
            {
                this.DataStore.Dispose();
            }
        }

        public void OnLeavingCurrentUnit()
        {
            if (Settings.BackUpMethod == BackUpMethod.LeaveUnit)
            {
                this.PerformBackup(true);
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