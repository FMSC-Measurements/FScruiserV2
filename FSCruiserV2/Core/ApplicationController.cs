using System;
using System.Linq;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using CruiseDAL;
using FSCruiser.Core.Models;
using System.Diagnostics;
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

        public ApplicationSettings Settings { get; set; }
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


        

        public CuttingUnitVM CurrentUnit { get; set; }


        //public long UnitTreeNumIndex = 0;
        //public BackUpMethod BackUpMethod { get; set; }
        
        


        public IViewController ViewController { get; protected set; }





        

        

        public ApplicationController(IViewController viewController)
        {
            viewController.ApplicationController = this;
            viewController.ApplicationClosing +=new CancelEventHandler(OnApplicationClosing);
            this.ViewController = viewController;

            //TallyHistory = new BindingList<TallyAction>();
            this.LoadAppSettings();
            
        }





        


        #region exception handleing 
        public void HandleNonCriticalException(Exception ex, string optMessage)
        {
            if(optMessage == null)
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
            if(createErrorReport)
            {
                FMSC.Utility.ErrorHandling.ErrorReport report = new FMSC.Utility.ErrorHandling.ErrorReport(ex, Assembly.GetCallingAssembly());
                report.MakeErrorReport();
            }

            Logger.Log.E(ex);
            ViewController.ShowMessage(optMessage ?? ex.Message,
                (isCritical) ? "Error" : "Non-Critical Error",
                MessageBoxIcon.Asterisk);
        }
        #endregion

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

            //Debug.Assert(!string.IsNullOrEmpty(path));

            //try
            //{
            //    this.LoadDatabase(path);

            //    //var fileName = Path.GetFileName(path);
            //    //if (fileName.StartsWith("BACK_"))
            //    //{
            //    //    _allowBackup = false;
            //    //    this.ViewController.ShowMessage("The file you have opened is marked as a backup\r\n" +
            //    //        "Its recomended you don't modify your backup files", "Warning", MessageBoxIcon.Hand);
            //    //}
            //    //else
            //    //{
            //    //    _allowBackup = true;
            //    //}

            //    Settings.AddRecentProject(new RecentProject(Path.GetFileName(path), path));
            //    SaveAppSettings();

            //    return true;
            //}
            //catch (FMSC.ORM.ReadOnlyException ex)
            //{
            //    this.HandleException(ex, "Unable to open file becaus it is read only", false, true);
            //    return false;
            //}
            //catch (FMSC.ORM.SQLException ex)
            //{
            //    this.HandleException(ex, "Unable to open file : " + ex.GetType().Name, false, true);
            //    return false;
            //}
            //catch (System.IO.IOException ex)
            //{
            //    this.HandleException(ex, "Unable to open file : " + ex.GetType().Name, false, true);
            //    return false;
            //}
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
                SaveAppSettings();
            }
            

        }

        //protected void LoadDatabase(String path)
        //{
        //    Debug.Assert(!string.IsNullOrEmpty(path));

        //    if (_cDal != null)
        //    {
        //        _cDal.Dispose();
        //    }
        //    try
        //    {
        //        this.ViewController.ShowWait();
                
        //        _cDal = new CruiseDAL.DAL(path);
        //        _cDal.LogMessage(string.Format("Opened By FSCruiser ({0})", Constants.FSCRUISER_VERSION), "I");
        //        //read all cutting units
        //        var units = this._cDal.From<CuttingUnitVM>().Read().ToList();
        //        //HACK insert dummy unit for unit dropdown, this needs to be done by main form 
        //        units.Insert(0, new CuttingUnitVM());
        //        this.CuttingUnits = units;

        //        //read the sale, to see if log grading is enabled
        //        this.ViewController.EnableLogGrading =
        //            this._cDal.ExecuteScalar<bool>(
        //            "Select LogGradingEnabled FROM Sale LIMIT 1;");

        //        var fileName = System.IO.Path.GetFileName(path);
        //        this.ViewController.MainView.Text = fileName;
        //    }
        //    catch
        //    {
        //        this.ViewController.MainView.Text = FSCruiser.Core.Constants.APP_TITLE;
        //        throw;
        //    }
        //    finally
        //    {
        //        this.ViewController.HideWait();
        //    }
        //}

        #region load CuttingUnit



        public void LoadCuttingUnit(CuttingUnitVM unit)
        {
            if (unit == this.CurrentUnit) { return; }//should be equivilant to referenceEquals

            //_talliesSinceLastSave = 0;
            //this.TallyHistory.Clear();
            this.CurrentUnit = unit;

            

            //this.Counts = new List<CountTreeVM>();

            

            //this.LoadCuttingUnitData();
        }
        #endregion

        

        //#region Cruisers
        //public CruiserVM[] GetCruiserList()
        //{
        //    if(this._cruisers == null)
        //    {
        //        return new CruiserVM[0];
        //    }
        //    else
        //    {
        //        return this._cruisers.ToArray();
        //    }

        //}

        //public void AddCruiser(string initials)
        //{
        //    if (this._cruisers == null)
        //    {
        //        this._cruisers = new List<CruiserVM>();
        //    }
        //    this._cruisers.Add(new CruiserVM(initials));
        //}

        //public void RemoveCruiser(CruiserVM cruiser)
        //{
        //    this._cruisers.Remove(cruiser);
        //}
        //#endregion

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
                    this.HandleNonCriticalException(e, "oops Tree Default save error");
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
        #endregion

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
                (m) => {
                    var result = Constants.BACKUP_PREFIX + m.Groups["core"].Value;
                    if(addTimeStamp)
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
        #endregion

        //public void OnTally()
        //{
        //    _talliesSinceLastSave++;

        //    if (_talliesSinceLastSave >= Constants.SAVE_INTERVAL)
        //    {
        //        _talliesSinceLastSave = 0;
        //        this.CurrentUnit.TrySaveTreesAsync();
        //        //this.SaveTreesAsync();
        //    }
        //}


        public void LogTreeCountEdit(CountTreeDO countTree, long oldValue, long newValue)
        {
            this._cDal.LogMessage(String.Format("Tree Count Edit: CT_CN={0}; PrevVal={1}; NewVal={2}", countTree.CountTree_CN, oldValue, newValue), "I");
        }

        public void LogSumKPIEdit(CountTreeDO countTree, long oldValue, long newValue)
        {
            this._cDal.LogMessage(String.Format("SumKPI Edit: CT_CN={0}; PrevVal={1}; NewVal={2}", countTree.CountTree_CN, oldValue, newValue), "I");
        }

        #region App Settings
        protected void SaveAppSettings()
        {
            if (this.Settings == null)
            {
                return;
            }
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ApplicationSettings));
                var dir = ApplicationController.ApplicationSettingDirectory;

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                var path = ApplicationController.ApplicationSettingFilePath;

                using (StreamWriter writer = new StreamWriter(path))
                {
                    serializer.Serialize(writer, this.Settings);
                }
            }
            catch (Exception e)
            {
                this.HandleNonCriticalException(e, "Unabel to save user settings");
            }
        }

        protected void LoadAppSettings()
        {
            if (File.Exists(ApplicationController.ApplicationSettingFilePath))
            {
                var appSettingsPath = ApplicationController.ApplicationSettingFilePath;
                try
                {
                    this.Settings = ApplicationSettings.Deserialize(appSettingsPath);
                }
                catch (Exception e)
                {
                    this.HandleNonCriticalException(e, "Fail to load application settings");
                }
            }
            else 
            {
                var oldSettingsPath = System.IO.Path.Combine(ApplicationController.GetExecutionDirectory()
                    , Constants.APP_SETTINGS_PATH);

                if (File.Exists(oldSettingsPath))
                {
                    try
                    {
                        this.Settings = ApplicationSettings.Deserialize(oldSettingsPath);
                    }
                    catch (Exception e)
                    {
                        this.HandleNonCriticalException(e, "Fail to load application settings");
                    }
                    try
                    {
                        System.IO.File.Delete(oldSettingsPath);
                    }
                    catch { }

                }
            }

            if (this.Settings == null)
            {
                this.Settings = new ApplicationSettings();
            }

        }

        ApplicationSettings Deserialize(string path)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ApplicationSettings));
                using (StreamReader reader = new StreamReader(path))
                {
                    return (ApplicationSettings)serializer.Deserialize(reader);
                }
            }
            catch (Exception e)
            {
                this.HandleNonCriticalException(e, "Fail to load application settings");
                return null;
            }
        }
        #endregion

        private void OnApplicationClosing(object sender, CancelEventArgs e)
        {
            this.SaveAppSettings();
            if (this._cDal != null)
            {
                this._cDal.Dispose();
            }
            
        }

        public void OnLeavingCurrentUnit(System.ComponentModel.CancelEventArgs e)
        {
            if (!this.Save())
            {
                this.HandleNonCriticalException(null, "Something went wrong saving the data for this unit, check trees for errors and try again");
                //MessageBox.Show("Something went wrong saving the data for this unit, check trees for errors and try again");
                e.Cancel = true;
            }
            if (!e.Cancel && this.Settings.BackUpMethod == BackUpMethod.LeaveUnit)
            {
                this.PerformBackup(false);
            }

        }

        public bool Save()
        {
            this.ViewController.ShowWait();

            try
            {
                ////this._cDal.BeginTransaction();//not doing transactions right now, need to do http://fmsc-projects.herokuapp.com/issues/526 first
                //this.SaveTallyHistory();
                //this.SaveTrees();
                ////this.SaveCounts();
                //this.SaveSampleGroups();

                return this.CurrentUnit.SaveFieldData();
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                //this._cDal.EndTransaction();
                this.ViewController.HideWait();
            }
        }

        #region static methods

        public static string GetExecutionDirectory()
        {
            string name = Assembly.GetCallingAssembly().GetName().CodeBase;
            //Assembly.GetCallingAssembly().GetName().CodeBase;
            
            //clean up path, in FF name is a URI
            if (name.StartsWith(@"file:///"))
            {
                name = name.Replace("file:///", string.Empty);
            }
            string dir = System.IO.Path.GetDirectoryName(name);
            return dir;
        }

        public static string ApplicationSettingDirectory
        {
            get
            {
#if NetCF 
                return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FScruiser"); ; 
             
#else
                return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FScruiser");
#endif
            }
        }

        public static string ApplicationSettingFilePath
        {
            get
            {
                var dir = ApplicationSettingDirectory;
                return System.IO.Path.Combine(dir, Constants.APP_SETTINGS_PATH);
            }
        }

        #endregion


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

        #endregion

        #region tally history
        //public void AddTallyAction(TallyAction action)
        //{
        //    if (action.KPI != 0)
        //    {
        //        TreeEstimateDO te = new TreeEstimateDO(this._cDal);
        //        te.KPI = action.KPI;
        //        te.CountTree = action.Count;
        //        action.TreeEstimate = te;
        //        te.Save();
        //    }


        //    if (TallyHistory.Count >= Constants.MAX_TALLY_HISTORY_SIZE)
        //    {
        //        //TallyAction a = TallyHistory[0];
        //        TallyHistory.RemoveAt(0);
        //    }
        //    action.Time = DateTime.Now.ToString("hh:mm");
        //    TallyHistory.Add(action);
        //}

        //public void Untally(TallyAction action)
        //{
        //    if (action != null)
        //    {
        //        if (TallyHistory.Remove(action))
        //        {
        //            action.Count.TreeCount--;
        //            //action.Sampler.Count -= 1;
        //            if (action.KPI > 0)
        //            {
        //                action.Count.SumKPI -= action.KPI;
        //                action.TreeEstimate.Delete();
        //            }

        //            if (action.TreeRecord != null)
        //            {
        //                DeleteTree(action.TreeRecord);
        //            }
        //        }

        //    }
        //}

        //protected void SaveTallyHistory()
        //{
        //    try
        //    {
        //        this.CurrentUnit.TallyHistory = this.SerializeTallyHistory(this.TallyHistory);
        //        if (!String.IsNullOrEmpty(this.CurrentUnit.TallyHistory))
        //        {
        //            this.CurrentUnit.Save();
        //        }
        //    }
        //    catch(Exception e)
        //    {
        //        this.HandleException(e, "Unable to save tally history", false, true);
        //        //this.HandleNonCriticalException(e, "Unable to save tally history");
        //    }
        //}

        //public void InitializeTallyHistory(CuttingUnitDO unit)
        //{
        //    if (!String.IsNullOrEmpty(this.CurrentUnit.TallyHistory))
        //    {
        //        try
        //        {
        //            foreach (TallyAction action in this.DeserializeTallyHistory(this.CurrentUnit.TallyHistory))
        //            {
        //                TallyHistory.Add(action);
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            this.HandleNonCriticalException(e, "Unable to load tally history");
        //        }
        //    }
        //}

        //protected string SerializeTallyHistory(IList<TallyAction> tallyHistory)
        //{
        //    using(StringWriter writer = new StringWriter())
        //    {
        //        XmlSerializer serializer = new XmlSerializer(typeof(TallyAction[]));
        //        TallyAction[] array = new TallyAction[tallyHistory.Count];
        //        tallyHistory.CopyTo(array, 0);
        //        serializer.Serialize(writer, array);
        //        return writer.ToString();                
        //    }            
        //}

        //protected TallyAction[] DeserializeTallyHistory(string stuff)
        //{
        //    TallyAction[] tallyHistory = null;
        //    try
        //    {
        //        XmlSerializer serializer = new XmlSerializer(typeof(TallyAction[]));
        //        using (StringReader reader = new StringReader(stuff))
        //        {
        //            tallyHistory = (TallyAction[])serializer.Deserialize(reader);
        //        }
        //        int len = tallyHistory.Length;
        //        if (len > Constants.MAX_TALLY_HISTORY_SIZE)
        //        {
        //            TallyAction[] a = new TallyAction[Constants.MAX_TALLY_HISTORY_SIZE];
        //            Array.Copy(tallyHistory, len - Constants.MAX_TALLY_HISTORY_SIZE, a, 0, Constants.MAX_TALLY_HISTORY_SIZE);
        //            //tallyHistory.CopyTo(a, (len - this.MAX_TALLY_HISTORY_SIZE) - 2);
        //            tallyHistory = a;
        //        }
        //        foreach (TallyAction action in tallyHistory)
        //        {
        //            action.PopulateData(_cDal);
        //        }
        //    }
        //    catch
        //    {
        //        //do nothing
        //    }

        //    return tallyHistory;
        //}
        #endregion

        #region save trees
        //private void SaveTreesAsync()
        //{
        //    if (this._saveTreesWorkerThread != null)
        //    {
        //        this._saveTreesWorkerThread.Abort();
        //    }
        //    try
        //    {
        //        this._saveTreesWorkerThread = new Thread(this.SaveTrees);
        //        this._saveTreesWorkerThread.IsBackground = true;
        //        this._saveTreesWorkerThread.Priority = Constants.SAVE_TREES_THREAD_PRIORISTY;
        //        this._saveTreesWorkerThread.Start();
        //    }
        //    catch
        //    {
        //        this._saveTreesWorkerThread = null;
        //    }


        //}

        //public bool TrySaveTree(TreeVM tree)
        //{
        //    try
        //    {
        //        tree.Save();
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        this.HandleNonCriticalException(e, "Unable to save tree. Ensure Tree Number, Sample Group and Stratum are valid");
        //        return false;
        //    }
        //}

        //protected void SaveTrees()
        //{
        //    this.SaveTrees((ICollection<TreeVM>)CurrentUnitTreeList);
        //}

        //public void SaveTrees(ICollection<TreeVM> list)
        //{
        //    bool success = true; 
        //    if (list != null)
        //    {
        //        TreeVM[] a = new TreeVM[list.Count];
        //        list.CopyTo(a, 0);

        //        this._cDal.BeginTransaction();
        //        foreach (TreeVM t in a)
        //        {
        //            try
        //            {
        //                t.Save(OnConflictOption.Fail);//fail on conflict, but donot cancel current transaction 
        //            }
        //            catch (FMSC.ORM.SQLException)
        //            {
        //                //TODO rethrow with meaningful exception, not this success crap 
        //                success = false;
        //            }

        //        }
        //        this._cDal.CommitTransaction();

        //        //saving any tree data from previouly loaded unit

        //        //this._cDal.Save(a);
        //    }
        //    if (!success)
        //    {
        //        throw new FMSC.ORM.SQLException("not all trees were able to be saved", null);
        //    }

        //}
        #endregion

        #region validate trees
        //public bool ValidateTrees()
        //{
        //    return ValidateTrees((ICollection<TreeVM>)CurrentUnitTreeList);
        //}

        //public bool ValidateTrees(ICollection<TreeVM> list)
        //{
        //    this.ViewController.ShowWait();
        //    try
        //    {
        //        //this._cDal.BeginTransaction();//TODO encapsulate in trasaction once trasaction tracking is done
        //        bool valid = InternalValidateTrees(list);
        //        return valid;
        //    }
        //    finally
        //    {
        //        //this._cDal.EndTransaction();
        //        this.ViewController.HideWait();
        //    }

        //}

        //private void InternalValidateTrees()
        //{
        //    this.InternalValidateTrees(this.CurrentUnitTreeList);
        //}

        //private bool InternalValidateTrees(ICollection<TreeVM> list)
        //{
        //    bool valid = true;
        //    TreeVM[] a = new TreeVM[list.Count];
        //    list.CopyTo(a, 0);
        //    foreach (TreeVM tree in a)
        //    {
        //        if (tree.Stratum != null && tree.Stratum.TreeFieldNames != null)
        //        {
        //            valid = tree.Validate(tree.Stratum.TreeFieldNames) && valid;
        //        }
        //        else
        //        {
        //            valid = tree.Validate() && valid;
        //        }
        //        try
        //        {
        //            tree.SaveErrors();
        //        }
        //        catch
        //        {
        //            valid = false;
        //            //TODO should we do something if tree error unable to save
        //        }
        //    }
        //    return valid;
        //}

        //private bool InternalValidateTrees(ICollection<TreeVM> trees, ICollection<string> fields)
        //{
        //    bool valid = true;
        //    TreeVM[] a = new TreeVM[trees.Count];
        //    trees.CopyTo(a, 0);
        //    foreach (TreeDO tree in a)
        //    {
        //        valid = tree.Validate(fields) && valid;
        //        try
        //        {
        //            tree.SaveErrors();
        //        }
        //        catch
        //        {
        //            valid = false;
        //            //TODO should we do something if tree error unable to save
        //        }
        //    }
        //    return valid;
        //}

        //private void ValidateTreesAsync(ICollection<TreeVM> list)
        //{
        //    if (this._validateTreesWorkerThread != null)
        //    {
        //        this._validateTreesWorkerThread.Abort();
        //    }
        //    this._validateTreesWorkerThread = new Thread(this.InternalValidateTrees);
        //    this._validateTreesWorkerThread.IsBackground = true;
        //    this._validateTreesWorkerThread.Priority = ThreadPriority.BelowNormal;
        //    this._validateTreesWorkerThread.Start();
        //}
        #endregion

        #region Sample Selecter Methods
        //public void SerializeSamplerState(SampleGroupVM sg)
        //{
        //    SampleSelecter selector = sg.Sampler;
        //    if (selector != null && (selector is BlockSelecter || selector is SystematicSelecter))
        //    {
        //        XmlSerializer serializer = new XmlSerializer(selector.GetType());
        //        StringWriter writer = new StringWriter();
        //        //selector.Count = (int)count.TreeCount;
        //        serializer.Serialize(writer, selector);
        //        sg.SampleSelectorState = writer.ToString();
        //        sg.SampleSelectorType = selector.GetType().Name;
        //        //count.TreeCount = selector.Count;
        //    }
        //}

        //public SampleSelecter LoadSamplerState(SampleGroupDO sg)
        //{
        //    XmlSerializer serializer = null;

        //    switch (sg.SampleSelectorType)
        //    {
        //        case "Block":
        //        case "BlockSelecter":
        //            {
        //                serializer = new XmlSerializer(typeof(BlockSelecter));
        //                break;
        //            }
        //        case "SRS":
        //        case "SRSSelecter":
        //            {
        //                return new SRSSelecter((int)sg.SamplingFrequency, (int)sg.InsuranceFrequency);
        //            }
        //        case "Systematic":
        //        case "SystematicSelecter":
        //            {
        //                serializer = new XmlSerializer(typeof(SystematicSelecter));
        //                break;
        //            }
        //        case "ThreeP":
        //        case "ThreePSelecter":
        //            {
        //                return new ThreePSelecter((int)sg.KZ, 10000, (int)sg.InsuranceFrequency);
        //            }
        //    }
        //    if (serializer != null)
        //    {
        //        using (StringReader reader = new StringReader(sg.SampleSelectorState))
        //        {
        //            SampleSelecter sampler = (SampleSelecter)serializer.Deserialize(reader);
        //            return sampler;
        //        }
        //    }
            
        //    return null;
        //}

        //public SampleSelecter MakeSampleSelecter(SampleGroupVM sg)
        //{
        //    SampleSelecter selecter = null;
        //    //if ((sg.TallyMethod & CruiseDAL.Enums.TallyMode.Manual) == CruiseDAL.Enums.TallyMode.Manual)
        //    //{
        //    //    return null;
        //    //}

        //    if(string.IsNullOrEmpty(sg.SampleSelectorState))
        //    {
        //        switch (sg.Stratum.Method)
        //        {
        //            case "100":
        //                {
        //                    selecter = null;
        //                    break;
        //                }
        //            case "STR":
        //                {
        //                    if (sg.SampleSelectorType == "SystematicSelecter" && Constants.ALLOW_STR_SYSTEMATIC)
        //                    {
        //                        selecter = MakeSystematicSampleSelector(sg);
        //                    }
        //                    else
        //                    {
        //                        selecter = MakeBlockSampleSelector(sg);
        //                    }
        //                    break;
        //                }
        //            case "3P":
        //            case "F3P":
        //            case "P3P":
        //                {
        //                    selecter = MakeThreePSampleSelector(sg);
        //                    break;
        //                }
        //            case "FIX":
        //            case "PNT":
        //                {
        //                    selecter = null;
        //                    break;
        //                }
        //            case "FCM":
        //            case "PCM":
        //                {
        //                    selecter = MakeSystematicSampleSelector(sg);
        //                    break;
        //                }
        //        }
        //    }
        //    else
        //    {
        //        selecter = LoadSamplerState(sg);

        //        //ensure sampler frequency matches sample group freqency 
        //        if (selecter != null && selecter is FMSC.Sampling.IFrequencyBasedSelecter
        //            && ( ((FMSC.Sampling.IFrequencyBasedSelecter)selecter).Frequency != sg.SamplingFrequency
        //            || ((FMSC.Sampling.IFrequencyBasedSelecter)selecter).ITreeFrequency != sg.InsuranceFrequency) )
        //        {
        //            //HACK older versions of FMSC.Sampling would use -1 instead of 0 if InsuranceFrequency was 0
        //            if (((FMSC.Sampling.IFrequencyBasedSelecter)selecter).ITreeFrequency == -1
        //                && sg.InsuranceFrequency == 0)
        //            {
        //                return selecter;
        //            }

        //            if (sg.CanEditSampleGroup())
        //            {
        //                this._cDal.LogMessage(string.Format("Frequency missmatch on SG:{0} Sf={1} If={2}; SelectorState: Sf={3} If={4}; SelectorState reset",
        //                    sg.Code,
        //                    sg.SamplingFrequency,
        //                    sg.InsuranceFrequency,
        //                    ((FMSC.Sampling.IFrequencyBasedSelecter)selecter).Frequency,
        //                    ((FMSC.Sampling.IFrequencyBasedSelecter)selecter).ITreeFrequency),"I");

        //                sg.SampleSelectorState = string.Empty;
        //                selecter = MakeSampleSelecter(sg);
        //            }
        //            else
        //            {
        //                this._cDal.LogMessage(string.Format("Frequency missmatch on SG:{0} Sf={1} If={2}; SelectorState: Sf={3} If={4}; changes reverted",
        //                    sg.Code,
        //                    sg.SamplingFrequency,
        //                    sg.InsuranceFrequency,
        //                    ((FMSC.Sampling.IFrequencyBasedSelecter)selecter).Frequency,
        //                    ((FMSC.Sampling.IFrequencyBasedSelecter)selecter).ITreeFrequency),"I");

                        
        //                this.ViewController.ShowMessage("Oops! Sample Frequency on sample group " +
        //                    sg.Code + " has been modified.\r\n If you are trying to change the sample freqency during a cruise, you should create a new sample group.",
        //                    "Error", MessageBoxIcon.Exclamation); 
        //                sg.SamplingFrequency = ((FMSC.Sampling.IFrequencyBasedSelecter)selecter).Frequency;
        //                sg.InsuranceFrequency = ((FMSC.Sampling.IFrequencyBasedSelecter)selecter).ITreeFrequency;
        //            }

        //        }
        //    }

        //    return selecter;
        //}

        //private SampleSelecter MakeThreePSampleSelector(SampleGroupDO sg)
        //{
        //    SampleSelecter selecter = null;
        //    int iFrequency = (int)sg.InsuranceFrequency;
        //    int KZ = (int)sg.KZ;
        //    int maxKPI = 100000;            
        //    selecter = new FMSC.Sampling.ThreePSelecter(KZ, maxKPI, iFrequency);
        //    return selecter;
        //}

        //private SampleSelecter MakeSystematicSampleSelector(SampleGroupDO sg)
        //{
        //    SampleSelecter selecter = null;
        //    int iFrequency = (int)sg.InsuranceFrequency;
        //    int frequency = (int)sg.SamplingFrequency;
        //    if (frequency == 0) { selecter = null; }
        //    else
        //    {
        //        selecter = new FMSC.Sampling.SystematicSelecter(frequency, iFrequency, true);
        //    }
        //    return selecter;
        //}

        //private SampleSelecter MakeBlockSampleSelector(SampleGroupDO sg)
        //{
        //    SampleSelecter selecter = null;
        //    int iFrequency = (int)sg.InsuranceFrequency;
        //    int frequency = (int)sg.SamplingFrequency;
        //    if (frequency == 0) { selecter = null; }
        //    else
        //    {
        //        selecter = new FMSC.Sampling.BlockSelecter(frequency, iFrequency);
        //    }
        //    return selecter;
        //}
        #endregion

        
    }
}