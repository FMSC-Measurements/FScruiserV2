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


namespace FSCruiser.Core
{
     

    public enum BackUpMethod { None = 0, LeaveUnit = 1, TimeInterval = 2 }

    public class ApplicationController : IApplicationController
    {
        

        //private int _talliesSinceLastSave = 0;
        
        //private List<CruiserVM> _cruisers;
        //private bool _allowBackup; 
        

        public ApplicationSettings Settings { get; set; }
        //public List<CruiserVM> Cruisers { get { return _cruisers; } }
        public CruiseDAL.DAL _cDal { get; set; }
        public List<CuttingUnitVM> CuttingUnits { get; protected set; }


        

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

        public void Run()
        {
            ViewController.BeginShowSplash();
            Application.Run(ViewController.MainView);
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

        public bool OpenFile()
        {
            string fileName;
            if (this.ViewController.ShowOpenCruiseFileDialog(out fileName) == DialogResult.OK)
            {
                return OpenFile(fileName);
            }
            return false;
        }

        protected bool OpenFile(string path)
        {
            try
            {
                this.LoadDatabase(path);

                //var fileName = Path.GetFileName(path);
                //if (fileName.StartsWith("BACK_"))
                //{
                //    _allowBackup = false;
                //    this.ViewController.ShowMessage("The file you have opened is marked as a backup\r\n" +
                //        "Its recomended you don't modify your backup files", "Warning", MessageBoxIcon.Hand);
                //}
                //else
                //{
                //    _allowBackup = true;
                //}

                return true;
            }
            catch (FMSC.ORM.ReadOnlyException ex)
            {
                this.HandleException(ex, "Unable to open file becaus it is read only", false, true);
                return false;
            }
            catch (FMSC.ORM.SQLException ex)
            {
                this.HandleException(ex, "Unable to open file : " + ex.GetType().Name, false, true);
                return false;
            }
            catch (System.IO.IOException ex)
            {
                this.HandleException(ex, "Unable to open file : " + ex.GetType().Name, false, true);
                return false;
            }
        }

        protected void LoadDatabase(String path)
        {
            Debug.Assert(!string.IsNullOrEmpty(path));

            if (_cDal != null)
            {
                _cDal.Dispose();
            }
            try
            {
                this.ViewController.ShowWait();
                
                _cDal = new CruiseDAL.DAL(path);
                _cDal.LogMessage(string.Format("Opened By FSCruiser ({0})", Constants.FSCRUISER_VERSION), "I");
                //read all cutting units
                var units = this._cDal.From<CuttingUnitVM>().Read().ToList();
                //HACK insert dummy unit for unit dropdown, this needs to be done by main form 
                units.Insert(0, new CuttingUnitVM());
                this.CuttingUnits = units;

                //read the sale, to see if log grading is enabled
                this.ViewController.EnableLogGrading =
                    this._cDal.ExecuteScalar<bool>(
                    "Select LogGradingEnabled FROM Sale LIMIT 1;");

                var fileName = System.IO.Path.GetFileName(path);
                this.ViewController.MainView.Text = fileName;
            }
            catch
            {
                this.ViewController.MainView.Text = FSCruiser.Core.Constants.APP_TITLE;
                throw;
            }
            finally
            {
                this.ViewController.HideWait();
            }
        }

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
                var path = GetExecutionDirectory() + Constants.APP_SETTINGS_PATH;
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
            if (File.Exists(GetExecutionDirectory() + Constants.APP_SETTINGS_PATH) == true)
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ApplicationSettings));
                    using (StreamReader reader = new StreamReader(GetExecutionDirectory() + Constants.APP_SETTINGS_PATH))
                    {
                        this.Settings = (ApplicationSettings)serializer.Deserialize(reader);
                    }
                    

                    //this._cruisers = settings.Cruisers;
                    //this._backupDir = settings.BackupDir;
                    //this.BackUpMethod = settings.BackUpMethod;
                    //this.ViewController.EnableCruiserSelectionPopup = (this._cruisers != null && this._cruisers.Count > 0);
                }
                catch (Exception e)
                {
                    this.HandleNonCriticalException(e, "Fail to load application settings");
                }
            }

            if (this.Settings == null)
            {
                this.Settings = new ApplicationSettings();
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
            //clean up path, in FF name is a URI
            if (name.StartsWith(@"file:///"))
            {
                name = name.Replace("file:///", string.Empty);
            }
            string dir = System.IO.Path.GetDirectoryName(name);
            return dir;
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
                if (this._cDal != null)
                {
                    this._cDal.Dispose();
                }
                if (this.ViewController != null)
                {
                    this.ViewController.Dispose();
                    this.ViewController = null;
                }
                //if (this._loadCuttingUnitDataThread != null)
                //{
                //    this._loadCuttingUnitDataThread.Abort();
                //    this._loadCuttingUnitDataThread = null;
                //}
                //if (this._saveTreesWorkerThread != null)
                //{
                //    this._saveTreesWorkerThread.Abort();
                //    this._saveTreesWorkerThread = null;
                //}
                //if (this._validateTreesWorkerThread != null)
                //{
                //    this._validateTreesWorkerThread.Abort();
                //    this._validateTreesWorkerThread = null;
                //}
            }
        }

        #endregion


        // Update the FScruiser version here.
        //All contants moved to Constants.cs
        //public const string FSCRUISER_VERSION = Constants.FSCRUISER_VERSION;//Depreciated
        //public readonly int MAX_TALLY_HISTORY_SIZE = 10;
        //public readonly int SAVE_INTERVAL = 10;
        //public const string CRUISERS_FILENAME = "\\Cruisers.xml";


        //private readonly TreeDefaultValueDO _newPopPlaceHolder = new TreeDefaultValueDO()
        //{
        //    Species = "<new>"
        //};

        //private BindingList<TreeVM> _nonPlotTreeList;

        //public List<TreeVM> CurrentUnitTreeList { get; protected set; }
        //public BindingList<TreeVM> CurrentUnitNonPlotTreeList
        //{
        //    get
        //    {
        //        return _nonPlotTreeList;
        //    }
        //}
        //public BindingList<TallyAction> TallyHistory { get; protected set; }
        //public List<SampleGroupVM> SampleGroups { get; protected set; }
        //public StratumVM DefaultStratum { get; protected set; }
        //public bool EnableCruiserSelectionPopup { get; set; }

        //private Thread _validateTreesWorkerThread;
        //private Thread _saveTreesWorkerThread;
        //private Thread _loadCuttingUnitDataThread;

        //public bool ShowLimitingDistanceDialog(StratumVM stratum, PlotVM plot, TreeVM optTree)
        //{
        //    string logMessage = String.Empty;
        //    bool isVariableRadius = Array.IndexOf(CruiseDAL.Schema.Constants.CruiseMethods.VARIABLE_RADIUS_METHODS, stratum.Method) > -1;
        //    float bafOrFixedPlotSize = (isVariableRadius) ? stratum.BasalAreaFactor : stratum.FixedPlotSize;
        //    DialogResult dResult = ViewController.ShowLimitingDistanceDialog(bafOrFixedPlotSize, isVariableRadius, optTree, out logMessage);
        //    if (dResult == DialogResult.OK)
        //    {
        //        plot.Remarks += logMessage;
        //        return true;
        //    }
        //    return false;

        //}

        //public void ShowLogs(TreeVM tree)
        //{
        //    if (tree.TrySave())
        //    {
        //        this.ViewController.ShowLogsView(tree.Stratum, tree);
        //    }
        //    else
        //    {
        //        ViewController.ShowMessage("Unable to save tree. Ensure Tree Number, Sample Group and Stratum are valid"
        //            ,null, MessageBoxIcon.Hand);
        //    }
        //}

        //public void ShowMain()
        //{
        //    ViewController.MainView.Show();
        //}



        //public int ShowNumericValueInput(int? min, int? max, int? initialValue)
        //{
        //    return (int)(ShowNumericValueInput(min, max, initialValue, false) ?? -1);
        //}

        //public int? ShowNumericValueInput(int? min, int? max, int? initialValue, bool acceptNullInput)
        //{
        //    ViewController.NumPadDialog.ShowDialog(min, max, initialValue, acceptNullInput);
        //    return ViewController.NumPadDialog.UserEnteredValue;
        //}

        //public void ShowDataEntry(CuttingUnitDO unit)
        //{
        //    Form view = null;
        //    DataEntryMode mode = GetUnitDataEntryMode(unit);
        //    if ((mode & DataEntryMode.Unknown) == DataEntryMode.Unknown)
        //    {
        //        throw new InvalidOperationException();
        //    }
        //    //if ((mode & DataEntryMode.Plot) == DataEntryMode.Plot)
        //    //{
        //    //    view = ViewController.GetPlotView(unit);
        //    //}
        //    //else if ((mode & DataEntryMode.Tree) == DataEntryMode.Tree)
        //    //{
        //    //    view = ViewController.GetTreeBasedView(unit);
        //    //}
        //    view = new FormDataEntry(this, unit);
        //    //throw exception if view null?
        //    view.ShowDialog();

        //}

        //public void ShowTallySettings(CountTreeDO count)
        //{
        //    ViewController.TallySettingsView.ShowDialog(count);
        //}


        //public DialogResult ShowPlotInfo(PlotInfo plotInfo, bool allowEdit)
        //{
        //    if (plotInfo == null) { return DialogResult.None; }
        //    IPlotInfoDialog view = null;

        //    if (plotInfo.Plot.Stratum.Method == "3PPNT" && allowEdit)
        //    {
        //        view = ViewController.PlotInfo3PPNTView;
        //    }
        //    else
        //    {
        //        view = ViewController.PlotInfoView;
        //    }

        //    if (view != null)
        //    {
        //        return view.ShowDialog(plotInfo, allowEdit);
        //    }
        //    else
        //    {
        //        return DialogResult.Cancel;
        //    }
        //}

        //        [Obsolete]
        //        public void InitializeSampleGroups(CuttingUnitDO unit)
        //        {
        //            //create a list of all samplegroups in the unit
        //            this.SampleGroups = _cDal.Read<SampleGroupVM>("SampleGroup", @"JOIN Stratum ON SampleGroup.Stratum_CN = Stratum.Stratum_CN 
        //                JOIN CuttingUnitStratum ON CuttingUnitStratum.Stratum_CN = Stratum.Stratum_CN
        //                WHERE CuttingUnitStratum.CuttingUnit_CN = ?", unit.CuttingUnit_CN);

        //            //initialize sample selectors for all sampleGroups
        //            foreach (SampleGroupVM sg in this.SampleGroups)
        //            {
        //                //DataEntryMode mode = GetStrataDataEntryMode(sg.Stratum);
        //                sg.Sampler = this.MakeSampleSelecter(sg);
        //            }
        //        }

        //        [Obsolete]
        //        public void InitializeUnitTreeList()
        //        {
        //            //create a list of all trees in the unit
        //            this.CurrentUnitTreeList = _cDal.Read<TreeVM>("Tree", "WHERE CuttingUnit_CN = ?", this.CurrentUnit.CuttingUnit_CN);
        //            //this.InternalValiateTrees((ICollection<TreeVM>)this.CurrentUnitTreeList);
        //            this.ValidateTreesAsync(this.CurrentUnitTreeList);
        //        }

        //        [Obsolete]
        //        public void LoadCuttingUnitData()
        //        {

        //            InitializeSampleGroups(this.CurrentUnit);
        //            //InitializeUnitTreeNumIndex();
        //            InitializeTallyHistory(this.CurrentUnit);

        //            InitializeUnitTreeList();
        //            //create a list of just trees in tree based strata
        //            List<TreeVM> nonPlotTrees = _cDal.Read<TreeVM>("Tree", @"JOIN Stratum ON Tree.Stratum_CN = Stratum.Stratum_CN WHERE Tree.CuttingUnit_CN = ? AND
        //                        (Stratum.Method = '100' OR Stratum.Method = 'STR' OR Stratum.Method = '3P' OR Stratum.Method = 'S3P') ORDER BY TreeNumber", this.CurrentUnit.CuttingUnit_CN);
        //            this._nonPlotTreeList = new BindingList<TreeVM>(nonPlotTrees);

        //            if (this._cDal.GetRowCount("CuttingUnitStratum", "WHERE CuttingUnit_CN = ?", this.CurrentUnit.CuttingUnit_CN) == 1)
        //            {
        //                this.DefaultStratum = this._cDal.ReadSingleRow<StratumVM>("Stratum", "JOIN CuttingUnitStratum USING (Stratum_CN) WHERE CuttingUnit_CN = ?",
        //                    this.CurrentUnit.CuttingUnit_CN);
        //            }
        //            else
        //            {
        //                this.DefaultStratum = this._cDal.ReadSingleRow<StratumVM>("Stratum",
        //                        "JOIN CuttingUnitStratum USING (Stratum_CN) WHERE CuttingUnit_CN = ? AND Method = ?",
        //                        CruiseDAL.Schema.Constants.CruiseMethods.H_PCT,
        //                        this.CurrentUnit.CuttingUnit_CN);
        //            }



        //            this.ViewController.HandleCuttingUnitDataLoaded();

        //        }

        //        [Obsolete]
        //        public void AsyncLoadCuttingUnitData()
        //        {
        //            if (this._loadCuttingUnitDataThread != null)
        //            {
        //                this._loadCuttingUnitDataThread.Abort();
        //            }
        //            this._loadCuttingUnitDataThread = new Thread(this.LoadCuttingUnitData);
        //            this._loadCuttingUnitDataThread.IsBackground = true;
        //            this._loadCuttingUnitDataThread.Priority = Constants.LOAD_CUTTINGUNITDATA_PRIORITY;
        //            this._loadCuttingUnitDataThread.Start();

        //            // ThreadPool.QueueUserWorkItem((t) => { LoadCuttingUnitData(); });
        //        }

        //public TreeVM UserAddTree(TreeVM templateTree, StratumVM knownStratum, PlotVM knownPlot)
        //{
        //    TreeVM newTree;
        //    SampleGroupVM assumedSG = null;
        //    TreeDefaultValueDO assumedTDV = null;


        //    if (knownPlot == null)//if we have plot then we already know stratum
        //    {
        //        //extrapolate stratum 
        //        if (knownStratum == null && this.DefaultStratum != null)//default stratum is going to be our first choice
        //        {
        //            knownStratum = this.DefaultStratum;
        //        }
        //        else if (knownStratum == null && templateTree != null)//if no default stratum try to use stratum/samplegroup from previous tree in view
        //        {
        //            assumedSG = templateTree.SampleGroup;
        //            assumedTDV = templateTree.TreeDefaultValue;
        //            if (assumedSG != null)
        //            {
        //                knownStratum = assumedSG.Stratum;
        //            }
        //            else
        //            {
        //                knownStratum = templateTree.Stratum;
        //            }
        //        }
        //    }

        //    //extrapolate sample group
        //    if(knownStratum != null && assumedSG == null)//if we have a stratum but no sample group, pick the first one
        //    {
        //        List<SampleGroupVM> samplegroups = _cDal.Read<SampleGroupVM>("SampleGroup", "WHERE Stratum_CN = ?", knownStratum.Stratum_CN);
        //        if (samplegroups.Count == 1)
        //        {
        //            assumedSG = samplegroups[0];
        //        }
        //    }

        //    newTree = CurrentUnit.CreateNewTreeEntry(knownStratum, assumedSG, assumedTDV, knownPlot, true);

        //    this.ViewController.ShowCruiserSelection(newTree);

        //    //if a 3P plot method set Count Measure to empty. 
        //    if(knownPlot != null 
        //        && Array.IndexOf(CruiseMethods.THREE_P_METHODS, knownStratum.Method) >= 0)
        //    {
        //        newTree.CountOrMeasure = string.Empty;
        //    }

        //    newTree.TreeCount = 1; //user added trees need a tree count of one because they aren't being tallied 
        //    this.TrySaveTree(newTree);

        //    this.OnTally();

        //    return newTree;
        //}

        ///// <summary>
        ///// Creates a new plot using the plot info view and adds it to the given stratum's plot collection
        ///// </summary>
        ///// <param name="stratum">stratum to create plot in</param>
        ///// <returns>reference to newly created plot</returns>
        //public PlotVM AddPlot(StratumVM stratum)
        //{
        //    PlotVM newPlot = new PlotVM(this._cDal);
        //    newPlot.CuttingUnit = this.CurrentUnit;
        //    newPlot.Stratum = stratum;
        //    newPlot.PlotNumber = this.GetNextPlotNumber(this.CurrentUnit, stratum);

        //    //PlotInfo plotInfo = new PlotInfo(newPlot, stratum);
        //    //newPlot.NextPlotTreeNum = 1;
        //    if (this.ViewController.ShowPlotInfo(newPlot, true) == DialogResult.OK)
        //    {
        //        foreach (PlotVM pi in stratum.Plots)
        //        {
        //            if (pi.PlotNumber == newPlot.PlotNumber)
        //            {
        //                MessageBox.Show(String.Format("Plot Number {0} Already Exists", newPlot.PlotNumber));
        //                return this.AddPlot(stratum);
        //            }
        //        }


        //        newPlot.Save();
        //        stratum.Plots.Add(newPlot);
        //        newPlot.CheckDataState();

        //        if (!String.IsNullOrEmpty(newPlot.IsEmpty) && String.Compare(newPlot.IsEmpty.Trim(), "True", true) == 0)
        //        {
        //            return this.AddPlot(stratum) ?? newPlot;//add plot may return null, in that case return most recently created plot
        //        }
        //        else if (newPlot.Stratum.Method == "3PPNT" && newPlot.Trees.Count == 0)
        //        {
        //            return this.AddPlot(stratum) ?? newPlot;//add plot may return null, in that case return most recently created plot
        //        }
        //        return newPlot;
        //    }
        //    return null;
        //}

        //moved to FormDataEntryLogic
        //public bool ProcessHotKey(char key, ITallyView view)
        //{
        //    if (view.HotKeyLookup == null) { return false; }
        //    if (view.HotKeyEnabled && view.HotKeyLookup.ContainsKey(key))
        //    {
        //        CountTreeVM count = view.HotKeyLookup[key];
        //        view.OnTally(count);
        //        return true;
        //    }
        //    else if (view.HotKeyEnabled)
        //    {
        //        this.ViewController.SignalInvalidAction();
        //        return false;
        //    }
        //    return false;
        //}

        //[Obsolete]
        //public void DeleteTree(TreeVM tree)
        //{
        //    //ReleaseUnitTreeNumber((int)tree.TreeNumber);
        //    tree.Delete();
        //    //TreeDO.RecursiveDeleteTree(tree);
        //    CurrentUnitTreeList.Remove(tree);
        //    this.CurrentUnitNonPlotTreeList.Remove(tree);
        //}


        //public void DeleteTree(TreeVM tree, PlotVM plot)
        //{
        //    //if (tree.TreeNumber == plot.NextPlotTreeNum - 1)
        //    //{
        //    //    plot.NextPlotTreeNum--;
        //    //}
        //    tree.Delete();
        //    //TreeDO.RecursiveDeleteTree(tree);
        //    CurrentUnitTreeList.Remove(tree);
        //    plot.Trees.Remove(tree);
        //}

        //public void DeletePlot(PlotVM plot)
        //{
        //    try
        //    {
        //        _cDal.BeginTransaction();

        //        foreach (TreeVM tree in plot.Trees)
        //        {
        //            tree.Delete();
        //            CurrentUnitTreeList.Remove(tree);

        //            //TreeDO.RecursiveDeleteTree(tree);
        //        }
        //        plot.Delete();
        //        _cDal.CommitTransaction();
        //    }
        //    catch (Exception e)
        //    {
        //        _cDal.RollbackTransaction();
        //        throw e;
        //    }
        //}

        //public RowValidator GetTreeValidator(TreeDefaultValueDO tdv)
        //{
        //    if (_treeValidatorLookup.ContainsKey(tdv) == false)
        //    {
        //        RowValidator validator = new RowValidator();
        //        List<TreeAuditValueDO> tavList = _cDal.Read<TreeAuditValueDO>("TreeAuditValue", "JOIN TreeAudit WHERE TreeAuditValue.TreeAudit_CN = TreeAudit.TreeAudit_CN AND TreeAudit.TreeDefaultValue_CN", tdv.TreeDefaultValue_CN);
        //        foreach (TreeAuditValueDO tav in tavList)
        //        {
        //            validator.Add(tav);
        //        }
        //        _treeValidatorLookup.Add(tdv, validator);
        //    }

        //    return _treeValidatorLookup[tdv];
        //}



        //public DataEntryMode GetStrataDataEntryMode(StratumDO stratum)
        //{
        //    switch (stratum.Method)
        //    {
        //        case "100":
        //            {
        //                return DataEntryMode.Tree | DataEntryMode.HundredPct;
        //            }
        //        case "STR":
        //            {
        //                return DataEntryMode.Tree | DataEntryMode.TallyTree;
        //            }
        //        case "3P":
        //            {
        //                return DataEntryMode.Tree | DataEntryMode.ThreeP | DataEntryMode.TallyTree;
        //            }
        //        case "PNT":
        //        case "FIX":
        //            {
        //                return DataEntryMode.Plot| DataEntryMode.OneStagePlot;
        //            }
        //        case "FCM":
        //        case "PCM":
        //            {
        //                return DataEntryMode.Plot;
        //            }

        //        case "F3P":
        //        case "P3P":
        //        case "3PPNT":
        //            {
        //                return DataEntryMode.Plot | DataEntryMode.ThreeP;
        //            }
        //        case "S3P":
        //        default:
        //            {
        //                return DataEntryMode.HundredPct;//fall back on 100pct
        //            }
        //    }

        //}

        //public CountTreeVM GetCountRecord(TreeDO tree)
        //{
        //    return this._cDal.ReadSingleRow<CountTreeVM>(CruiseDAL.Schema.COUNTTREE._NAME,
        //        "WHERE SampleGroup_CN = ? AND CuttingUnit_CN = ? AND (TreeDefaultValue_CN = ? or ifnull(TreeDefaultValue_CN, 0) = 0)",
        //        tree.SampleGroup_CN,
        //        tree.CuttingUnit_CN,
        //        tree.TreeDefaultValue_CN);
        //}

        //        public IList<StratumVM> GetUnitPlotStrata()
        //        {
        //            IList<StratumVM> list = _cDal.Read<StratumVM>("Stratum", 
        //@"JOIN CuttingUnitStratum USING (Stratum_CN) 
        //WHERE CuttingUnitStratum.CuttingUnit_CN = ? 
        //AND Stratum.Method IN ( 'FIX', 'FCM', 'F3P', 'PNT', 'PCM', 'P3P', '3PPNT')", CurrentUnit.CuttingUnit_CN);
        //            foreach(StratumVM s in list)
        //            {
        //                //if(s.Plots == null)
        //                //{
        //                //    s.Plots = this._cDal.Read<PlotInfo>("Plot", "WHERE Stratum_CN = ? AND CuttingUnit_CN = ? ORDER BY PlotNumber", s.Stratum_CN, CurrentUnit.CuttingUnit_CN);
        //                //}
        //                if (s.Method == "3PPNT")
        //                {
        //                    if (s.KZ3PPNT <= 0)
        //                    {
        //                        MessageBox.Show("error 3PPNT missing KZ value, please return to Cruise System Manger and fix");
        //                        return null;
        //                    }
        //                    s.SampleSelecter = new ThreePSelecter((int)s.KZ3PPNT, 1000000, 0);
        //                }
        //                s.LoadTreeFieldNames();
        //            }
        //            return list;
        //        }

        //        public IList<StratumVM> GetUnitTreeBasedStrata()
        //        {
        //            IList<StratumVM> list = _cDal.Read<StratumVM>("Stratum", 
        //@"JOIN CuttingUnitStratum USING (Stratum_CN) 
        //WHERE CuttingUnitStratum.CuttingUnit_CN = ? 
        //AND Method IN ( '100', 'STR', '3P', 'S3P')", CurrentUnit.CuttingUnit_CN);

        //            foreach (StratumVM s in list)
        //            {
        //                s.LoadTreeFieldNames();
        //            }
        //            return list;
        //            //List<StratumDO> list = new List<StratumDO>();
        //            //foreach(StratumDO st in CurrentUnit.Strata)
        //            //{
        //            //    if((GetStrataDataEntryMode(st) & DataEntryMode.Tree) == DataEntryMode.Tree)
        //            //    {
        //            //        list.Add(st);
        //            //    }
        //            //}
        //            //return list;
        //        }

        //public IList<StratumVM> GetUnitStrata()
        //{
        //    return _cDal.Read<StratumVM>(CruiseDAL.Schema.STRATUM._NAME, "JOIN CuttingUnitStratum USING (Stratum_CN) WHERE CuttingUnitStratum.CuttingUnit_CN = ?", this.CurrentUnit.CuttingUnit_CN);
        //}

        //public DataEntryMode GetUnitDataEntryMode(CuttingUnitDO unit)
        //{
        //    //"SELECT goupe_concat(Method, ' ') FROM Stratum JOIN CuttingUnitStratum USING (Stratum_CN) WHERE CuttingUnit_CN = {0} GROUP BY Stratum.Method;";
        //    List<StratumVM> strata = unit.ReadStrata<StratumVM>();
        //    DataEntryMode mode = DataEntryMode.Unknown;
        //    foreach (StratumVM stratum in strata)
        //    {
        //        mode = mode | GetStrataDataEntryMode(stratum);
        //    }

        //    return mode;
        //}

        //public object GetTreeSGList(TreeVM tree)
        //{
        //    if (tree.Stratum == null)
        //    {
        //        return Constants.EMPTY_SG_LIST;
        //    }

        //    return _cDal.Read<SampleGroupVM>("SampleGroup", "WHERE Stratum_CN = ?", tree.Stratum_CN);
        //}

        //public ICollection<TreeDefaultValueDO> GetTreeTDVList(TreeVM tree)
        //{
        //    if (tree == null) { return Constants.EMPTY_SPECIES_LIST; }
        //    if (tree.Stratum == null)
        //    {
        //        //if (this.CurrentUnit.Strata.Count == 1)
        //        //{
        //        //    tree.Stratum = this.CurrentUnit.Strata[0];
        //        //}
        //        //else
        //        //{
        //        //    return Constants.EMPTY_SPECIES_LIST;
        //        //}
        //        return Constants.EMPTY_SPECIES_LIST;
        //    }

        //    if (tree.SampleGroup == null)
        //    {
        //        if (_cDal.GetRowCount("SampleGroup", "WHERE Stratum_CN = ?", tree.Stratum_CN) == 1)
        //        {
        //            tree.SampleGroup = _cDal.ReadSingleRow<SampleGroupVM>("SampleGroup", "WHERE Stratum_CN = ?", tree.Stratum_CN);
        //        }
        //        if (tree.SampleGroup == null)
        //        {
        //            return Constants.EMPTY_SPECIES_LIST;
        //        }
        //    }



        //    //if (tree.SampleGroup.TreeDefaultValues.IsPopulated == false)
        //    //{
        //    //    tree.SampleGroup.TreeDefaultValues.Populate();
        //    //}
        //    List<TreeDefaultValueDO> tdvs = this._cDal.Read<TreeDefaultValueDO>("TreeDefaultValue", "JOIN SampleGroupTreeDefaultValue USING (TreeDefaultValue_CN) WHERE SampleGroup_CN = ?", tree.SampleGroup_CN);

        //    if (Constants.NEW_SPECIES_OPTION)
        //    {
        //        tdvs.Add(_newPopPlaceHolder); 
        //        return tdvs; 
        //        //int cnt = tdvs.Count + 1;
        //        //TreeDefaultValueDO[] array = new TreeDefaultValueDO[cnt];
        //        //tree.SampleGroup.TreeDefaultValues.CopyTo(array, 0);
        //        //array[array.Length - 1] = _newPopPlaceHolder;
        //        //return array;
        //    }
        //    else
        //    {
        //        return tdvs; 
        //        //return tree.SampleGroup.TreeDefaultValues;
        //    }


        //}

        //[Obsolete]
        //public TreeVM CreateNewTreeEntry(CountTreeVM count)
        //{
        //    return CreateNewTreeEntry(count, null, true);
        //}

        //[Obsolete]
        //public TreeVM CreateNewTreeEntry(CountTreeVM count, PlotVM plot, bool isMeasure)
        //{
        //    //count.Save();
        //    return CreateNewTreeEntry(count.CuttingUnit, count.SampleGroup.Stratum, count.SampleGroup, count.TreeDefaultValue, plot, isMeasure);
        //}


        //public TreeVM CreateNewTreeEntry(CuttingUnitDO unit, StratumVM stratum, SampleGroupVM sg, TreeDefaultValueDO tdv, PlotVM plot, bool isMeasure)
        //{


        //    TreeVM newTree = new TreeVM(this._cDal);
        //    newTree.TreeCount = 0;
        //    newTree.CountOrMeasure = (isMeasure) ? "M" : "C";
        //    if (unit != null) { newTree.CuttingUnit = unit; }
        //    else { newTree.CuttingUnit = this.CurrentUnit; }
        //    if (sg != null)
        //    {
        //        newTree.SampleGroup = sg;
        //        if (tdv == null)
        //        {
        //            if (sg.TreeDefaultValues.IsPopulated == false) { sg.TreeDefaultValues.Populate(); }
        //            if (sg.TreeDefaultValues.Count == 1)
        //            {
        //                tdv = sg.TreeDefaultValues[0];
        //            }
        //        }
        //    }
        //    if (stratum != null) { newTree.Stratum = stratum; }
        //    if (tdv != null)
        //    {
        //        this.SetTreeTDV(newTree, tdv);
        //    }

        //    if (plot != null)
        //    {
        //        newTree.Plot = plot;
        //        newTree.TreeNumber = plot.NextPlotTreeNum + 1;
        //        newTree.TreeCount = 1;
        //    }
        //    else
        //    {
        //        newTree.TreeNumber = GetNextUnitTreeNumber();
        //        this.CurrentUnitNonPlotTreeList.Add(newTree);
        //    }

        //    lock (((ICollection)this.CurrentUnitTreeList).SyncRoot)
        //    {
        //        this.CurrentUnitTreeList.Add(newTree);
        //    }

        //    newTree.Validate();
        //    //newTree.Save();

        //    if (this.EnableCruiserSelectionPopup && isMeasure)
        //    {
        //        this.ViewController.ShowCruiserSelection(newTree);
        //    }

        //    return newTree;
        //}

        //[Obsolete]
        //public void SetTreeTDV(TreeVM tree, TreeDefaultValueDO tdv)
        //{
        //    if (tdv == _newPopPlaceHolder && tree.SampleGroup != null)
        //    {
        //        tdv = ViewController.ShowAddPopulation(tree.SampleGroup);
        //    }

        //    tree.TreeDefaultValue = tdv;
        //    if (tdv != null)
        //    {
        //        tree.Species = tdv.Species;

        //        tree.LiveDead = tdv.LiveDead;
        //        tree.Grade = tdv.TreeGrade;
        //        tree.FormClass = tdv.FormClass;
        //        tree.RecoverablePrimary = tdv.Recoverable;
        //        //tree.HiddenPrimary = tdv.HiddenPrimary;//#367
        //    }
        //    else
        //    {
        //        tree.Species = string.Empty;
        //        tree.LiveDead = string.Empty;
        //        tree.Grade = string.Empty;
        //        tree.FormClass = 0;
        //        tree.RecoverablePrimary = 0;
        //        tree.HiddenPrimary = 0;
        //    }
        //}

        //public void SignalMeasureTree()
        //{
        //    Win32.MessageBeep(Win32.MB_ICONQUESTION);//TODO externalize to view Controller
        //    MessageBox.Show("Measure Tree");
        //}

        //public void SignalInsuranceTree()
        //{
        //    Win32.MessageBeep(Win32.MB_ICONASTERISK);//TODO externalize to view Controller
        //    MessageBox.Show("Insurance Tree");
        //}


        //private void InitializeUnitTreeNumIndex()
        //{
        //long? value = (long?)_cDal.ExecuteScalar("Select Max(TreeNumber) FROM Tree WHERE CuttingUnit_CN = " + this.CurrentUnit.CuttingUnit_CN.ToString());
        //if (value.HasValue)
        //{
        //    this.UnitTreeNumIndex = (int)value.Value;
        //}
        //else
        //{
        //    this.UnitTreeNumIndex = 0;
        //}


        //if (CurrentUnitTreeList.Count > 0)
        //{
        //    String query = String.Format("Select Max(TreeNumber) as CurTreeNum FROM Tree WHERE CuttingUnit_CN = {0};", this.CurrentUnit.CuttingUnit_CN);
        //    List<System.Collections.IList> result = _cDal.Query(query, "CurTreeNum");
        //    Int64? value = (Int64?)(result[0][0]);
        //    if (value.HasValue)
        //    {
        //        this.UnitTreeNumIndex = (int)value.Value;
        //    }
        //    else
        //    {
        //        this.UnitTreeNumIndex = 0;
        //    }
        //}
        //else
        //{
        //    this.UnitTreeNumIndex = 0;
        //}
        //}

        //public int GetLogNumerIndexStart(TreeVM tree)
        //{
        //    long? value = (long?)this._cDal.ExecuteScalar(String.Format("SELECT MAX(CAST(LogNumber AS NUMERIC)) FROM Log WHERE Tree_CN = {0};", tree.Tree_CN));
        //    if (value.HasValue)
        //    {
        //        return (int)value.Value;
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}


        //[Obsolete]
        //private long GetNextUnitTreeNumber()
        //{
        //    if(this.CurrentUnitNonPlotTreeList == null || this.CurrentUnitNonPlotTreeList.Count == 0)
        //    { return 1; }
        //    TreeVM lastTree = this.CurrentUnitNonPlotTreeList[this.CurrentUnitNonPlotTreeList.Count - 1];
        //    long lastTreeNum = lastTree.TreeNumber;
        //    return lastTreeNum + 1;
        //    //return ++UnitTreeNumIndex;
        //}

        //[Obsolete]
        //public bool EnsureTreeNumberAvalible(long start)
        //{
        //    //long highestTreeNum = 0;
        //    foreach (TreeVM tree in this.CurrentUnitNonPlotTreeList)
        //    {
        //        if (tree.TreeNumber == start)
        //        {
        //            return false;
        //        }
        //        //else if (tree.TreeNumber > highestTreeNum)
        //        //{
        //        //    highestTreeNum = tree.TreeNumber;
        //        //}
        //    }
        //    //if (start >= highestTreeNum)
        //    //{
        //    //    UnitTreeNumIndex = start;
        //    //}
        //    return true;
        //}

        //[Obsolete]
        //public bool EnsureTreeNumberAvalible(long start, PlotVM plot)
        //{
        //    long highestTreeNum = 0;
        //    foreach (TreeVM tree in plot.Trees)
        //    {
        //        if (tree.TreeNumber == start)
        //        {
        //            return false;
        //        }
        //        else if (tree.TreeNumber > highestTreeNum)
        //        {
        //            highestTreeNum = tree.TreeNumber;
        //        }
        //    }
        //    //if (start >= highestTreeNum)
        //    //{
        //    //    plot.NextPlotTreeNum = start;
        //    //}
        //    return true;
        //}

        /// <summary>
        /// Creates a StratumInfo object and populates plots and plot tree lists 
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="stratum"></param>
        /// <returns></returns>
        //public StratumInfo CreateStratumInfo(CuttingUnitDO unit, StratumDO stratum)
        //{
        //    StratumInfo stInfo = new StratumInfo(stratum);
        //    List<PlotInfo> plots = this._cDal.Read<PlotInfo>("Plot", "WHERE Stratum_CN = ? AND CuttingUnit_CN = ? ORDER BY PlotNumber", stratum.Stratum_CN, unit.CuttingUnit_CN);
        //    stInfo.Plots = plots;
        //    //foreach (PlotDO p in plots)
        //    //{
        //    //    List<TreeVM> trees = this._cDal.Read<TreeVM>("Tree", "WHERE Stratum_CN = ? AND CuttingUnit_CN = ? AND Plot_CN = ? ORDER BY TreeNumber", stratum.Stratum_CN, unit.CuttingUnit_CN, p.Plot_CN);
        //    //    PlotInfo newPlotInfo = new PlotInfo(p, stInfo, trees);
        //    //    long? value = this._cDal.ExecuteScalar(String.Format("Select MAX(TreeNumber) FROM Tree WHERE Plot_CN = {0}", p.Plot_CN)) as long?;
        //    //    newPlotInfo.NextPlotTreeNum = (value.HasValue)? (int)value.Value : 1;
        //    //    stInfo.Plots.Add(newPlotInfo);
        //    //}

        //    if (stratum.Method == "3PPNT")
        //    {
        //        if (stratum.KZ3PPNT <= 0)
        //        {
        //            MessageBox.Show("error 3PPNT missing KZ value, please return to Cruise System Manger and fix");
        //            return null;
        //        }
        //        stInfo.SampleSelecter = new ThreePSelecter((int)stratum.KZ3PPNT, 1000000, 0);
        //    }

        //    return stInfo;
        //}

        ////public int GetNextPlotNumber(CuttingUnitDO unit, StratumDO stratum)
        ////{
        ////    try
        ////    {
        ////        int highestInUnit = 0;
        ////        int highestInStratum = 0;

        ////        {
        ////            string query = string.Format("Select Max(PlotNumber) FROM Plot WHERE CuttingUnit_CN = {0}", unit.CuttingUnit_CN);
        ////            long? result = _cDal.ExecuteScalar(query) as long?;
        ////            highestInUnit = (result != null) ? (int)result.Value : 0;
        ////        }
        ////        //List<IList> qResult = _cDal.Query(query, "highest");
        ////        //if (qResult.Count != 0 && qResult[0] != null && qResult[0].Count != 0)
        ////        //{
        ////        //    long? result = (long?)_cDal.Query(query, "highest")[0][0];
        ////        //    highestInUnit = (result != null) ? (int)result.Value : 0;
        ////        //}
        ////        {
        ////            string query = string.Format("Select Max(PlotNumber) FROM Plot WHERE CuttingUnit_CN = {0} AND Stratum_CN = {1}", unit.CuttingUnit_CN, stratum.Stratum_CN);
        ////            long? result = _cDal.ExecuteScalar(query) as long?;
        ////            highestInStratum = (result != null) ? (int)result.Value : 0;
        ////        }
        ////        //qResult = _cDal.Query(query, "highest");
        ////        //if (qResult.Count != 0 && qResult[0] != null && qResult[0].Count != 0)
        ////        //{
        ////        //    long? result = (long?)_cDal.Query(query, "highest")[0][0];
        ////        //    highestInStratum = (result != null) ? (int)result.Value : 0;
        ////        //}

        ////        if (highestInUnit - highestInStratum > 0)
        ////        {
        ////            return highestInUnit;
        ////        }
        ////        return highestInUnit + 1;
        ////    }
        ////    catch (Exception e)
        ////    {
        ////        Logger.Log.E("Unable to establish next plot number", e);
        ////        return 0;
        ////    }
        ////}

        //protected void SaveSampleGroups()
        //{
        //    foreach (SampleGroupVM sg in this.SampleGroups)
        //    {
        //        SerializeSamplerState(sg);
        //        sg.Save();
        //    }
        //}

        //public void SaveCounts()
        //{
        //    foreach (CountTreeVM count in Counts)
        //    {
        //        //ApplicationController.SerializeCountSampleState(count);
        //        count.Save();
        //    }
        //}

        //internal static void SerializeCountSampleState(CountTreeDO count)
        //{
        //    SampleSelecter selector = count.Tag as SampleSelecter;
        //    if (selector != null && (selector is BlockSelecter || selector is SystematicSelecter))
        //    {
        //        XmlSerializer serializer = new XmlSerializer(selector.GetType());
        //        StringWriter writer = new StringWriter();
        //        selector.Count = (int)count.TreeCount;
        //        serializer.Serialize(writer, selector);
        //        count.SampleSelectorState = writer.ToString();
        //        count.SampleSelectorType = selector.GetType().Name;
        //        //count.TreeCount = selector.Count;
        //    }
        //}

        //internal static SampleSelecter DeserializeCountSampleState(CountTreeDO count)
        //{
        //    XmlSerializer serializer = null;

        //    switch (count.SampleSelectorType)
        //    {
        //        case "BlockSelecter":
        //            {
        //                serializer = new XmlSerializer(typeof(BlockSelecter));
        //                break;
        //            }
        //        case "SRSSelecter":
        //            {
        //                return new SRSSelecter((int)count.SampleGroup.SamplingFrequency, (int)count.SampleGroup.InsuranceFrequency);
        //            }
        //        case "SystematicSelecter":
        //            {
        //                serializer = new XmlSerializer(typeof(SystematicSelecter));
        //                break;
        //            }
        //        case "ThreePSelecter":
        //            {
        //                return new ThreePSelecter((int)count.SampleGroup.KZ, 10000, (int)count.SampleGroup.InsuranceFrequency);
        //            }
        //    }

        //    StringReader reader = new StringReader(count.SampleSelectorState);
        //    return (SampleSelecter)serializer.Deserialize(reader);
        //}

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