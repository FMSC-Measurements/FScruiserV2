using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using FSCruiserV2.Logic;
using CruiseDAL.DataObjects;
using FMSC.Sampling;
using System.IO;
using System.Xml.Serialization;

namespace FSCruiserV2.Test.Mocks
{
    public class ApplicationControllerMock : IApplicationController
    {

        public int SampleCount { get; set; }
        public int ISampleCount { get; set; }

        public ApplicationControllerMock()
        {
            _viewController = new ViewControllerMock() { ApplicationController = this };
        }


        public void ResetSampleCounts()
        {
            SampleCount = 0;
            ISampleCount = 0;
        }

        #region IApplicationController Members

        private ViewControllerMock _viewController;
        public IViewController ViewController
        {
            get { return _viewController; }
            set { _viewController = (ViewControllerMock)value; }
        }

        public string BackupDir
        {
            get;
            set; 
        }

        public BackUpMethod BackUpMethod
        {
            get;
            set;
        }

        public List<CruiserVM> Cruisers
        {
            get;
            set;
        }

        public CruiseDAL.DAL _cDal
        {
            get;
            set;
        }

        public List<CuttingUnitVM> CuttingUnits
        {
            get;
            set;
        }

        public List<CountTreeVM> Counts
        {
            get;
            set;
        }

        public CuttingUnitVM CurrentUnit
        {
            get;
            set;
        }

        public System.ComponentModel.BindingList<TallyAction> TallyHistory
        {
            get;
            set;
        }

        public List<TreeVM> CurrentUnitTreeList
        {
            get;
            set;
        }

        public System.ComponentModel.BindingList<TreeVM> CurrentUnitNonPlotTreeList
        {
            get;
            set;
        }

        public StratumVM DefaultStratum
        {
            get;
            set;
        }

        public bool EnableCruiserSelectionPopup
        {
            get;
            set;
        }

        public PlotVM AddPlot(StratumVM stratum)
        {
            throw new NotImplementedException();
        }

        public void AddTallyAction(TallyAction action)
        {
            System.Diagnostics.Trace.WriteLine("Added Tally action");
        }

        public TreeVM CreateNewTreeEntry(CountTreeVM count)
        {
            return new TreeVM();  
        }

        public TreeVM CreateNewTreeEntry(CountTreeVM count, PlotVM plot, bool isMeasure)
        {
            throw new NotImplementedException();
        }

        public TreeVM CreateNewTreeEntry(CruiseDAL.DataObjects.CuttingUnitDO unit, StratumVM stratum, SampleGroupVM sg, CruiseDAL.DataObjects.TreeDefaultValueDO tdv, PlotVM plot, bool isMeasure)
        {
            throw new NotImplementedException();
        }

        public CruiseDAL.DataObjects.SampleGroupDO CreateNewSampleGroup(CruiseDAL.DataObjects.StratumDO stratum)
        {
            throw new NotImplementedException();
        }

        public CruiseDAL.DataObjects.TreeDefaultValueDO CreateNewTreeDefaultValue(string pProd)
        {
            throw new NotImplementedException();
        }

        public void DeletePlot(PlotVM plot)
        {
            throw new NotImplementedException();
        }

        public void DeleteTree(TreeVM tree)
        {
            throw new NotImplementedException();
        }

        public void DeleteTree(TreeVM tree, PlotVM plot)
        {
            throw new NotImplementedException();
        }

        public object GetTreeSGList(TreeVM tree)
        {
            throw new NotImplementedException();
        }

        public ICollection<CruiseDAL.DataObjects.TreeDefaultValueDO> GetTreeTDVList(TreeVM tree)
        {
            throw new NotImplementedException();
        }

        public IList<StratumVM> GetUnitPlotStrata()
        {
            throw new NotImplementedException();
        }

        public IList<StratumVM> GetUnitTreeBasedStrata()
        {
            throw new NotImplementedException();
        }

        public IList<StratumVM> GetUnitStrata()
        {
            throw new NotImplementedException();
        }

        public CountTreeVM GetCountRecord(CruiseDAL.DataObjects.TreeDO tree)
        {
            throw new NotImplementedException();
        }

        public void AddCruiser(string initials)
        {
            throw new NotImplementedException();
        }

        public void RemoveCruiser(CruiserVM cruiser)
        {
            throw new NotImplementedException();
        }

        public CruiserVM[] GetCruiserList()
        {
            throw new NotImplementedException();
        }

        public int GetLogNumerIndexStart(TreeVM tree)
        {
            throw new NotImplementedException();
        }

        public void HandleNonCriticalException(Exception ex, string optMessage)
        {
            throw new NotImplementedException();
        }

        public void LoadCuttingUnit(CuttingUnitVM unit)
        {
            throw new NotImplementedException();
        }

        public void LoadCuttingUnitData()
        {
            throw new NotImplementedException();
        }

        public void AsyncLoadCuttingUnitData()
        {
            throw new NotImplementedException();
        }

        public void LoadDatabase(string path)
        {
            throw new NotImplementedException();
        }

        public FMSC.Sampling.SampleSelecter MakeSampleSelecter(SampleGroupVM sg)
        {
            SampleSelecter selecter = null;
            if (string.IsNullOrEmpty(sg.SampleSelectorState))
            {
                switch (sg.Stratum.Method)
                {
                    case "100":
                        {
                            selecter = null;
                            break;
                        }
                    case "STR":
                        {
                            if (sg.SampleSelectorType == "SystematicSelecter" && Constants.ALLOW_STR_SYSTEMATIC)
                            {
                                selecter = MakeSystematicSampleSelector(sg);
                            }
                            else
                            {
                                selecter = MakeBlockSampleSelector(sg);
                            }
                            break;
                        }
                    case "3P":
                    case "F3P":
                    case "P3P":
                        {
                            selecter = MakeThreePSampleSelector(sg);
                            break;
                        }
                    case "FIX":
                    case "PNT":
                        {
                            selecter = null;
                            break;
                        }
                    case "FCM":
                    case "PCM":
                        {
                            selecter = MakeSystematicSampleSelector(sg);
                            break;
                        }
                }
            }
            else
            {
                selecter = LoadSamplerState(sg);

                //ensure sampler frequency matches sample group freqency 
                if (selecter != null && selecter is FMSC.Sampling.IFrequencyBasedSelecter
                    && ((FMSC.Sampling.IFrequencyBasedSelecter)selecter).Frequency != sg.SamplingFrequency)
                {
                    if (sg.CanEditSampleGroup()
                        && this.ViewController.AskYesNo("Sample Frequency on sample group " + sg.Code + "has been modified. Would you like to reset the sampler?",
                        null, System.Windows.Forms.MessageBoxIcon.Exclamation))
                    {
                        sg.SampleSelectorState = string.Empty;
                        selecter = MakeSampleSelecter(sg);
                    }
                }
            }

            return selecter;
        }

        public SampleSelecter LoadSamplerState(SampleGroupDO sg)
        {
            XmlSerializer serializer = null;

            switch (sg.SampleSelectorType)
            {
                case "Block":
                case "BlockSelecter":
                    {
                        serializer = new XmlSerializer(typeof(BlockSelecter));
                        break;
                    }
                case "SRS":
                case "SRSSelecter":
                    {
                        return new SRSSelecter((int)sg.SamplingFrequency, (int)sg.InsuranceFrequency);
                    }
                case "Systematic":
                case "SystematicSelecter":
                    {
                        serializer = new XmlSerializer(typeof(SystematicSelecter));
                        break;
                    }
                case "ThreeP":
                case "ThreePSelecter":
                    {
                        return new ThreePSelecter((int)sg.KZ, 10000, (int)sg.InsuranceFrequency);
                    }
            }
            if (serializer != null)
            {
                using (StringReader reader = new StringReader(sg.SampleSelectorState))
                {
                    SampleSelecter sampler = (SampleSelecter)serializer.Deserialize(reader);
                    return sampler;
                }
            }

            return null;
        }

        private SampleSelecter MakeThreePSampleSelector(SampleGroupDO sg)
        {
            SampleSelecter selecter = null;
            int iFrequency = (int)sg.InsuranceFrequency;
            int KZ = (int)sg.KZ;
            int maxKPI = 100000;
            selecter = new FMSC.Sampling.ThreePSelecter(KZ, maxKPI, iFrequency);
            return selecter;
        }

        private SampleSelecter MakeSystematicSampleSelector(SampleGroupDO sg)
        {
            SampleSelecter selecter = null;
            int iFrequency = (int)sg.InsuranceFrequency;
            int frequency = (int)sg.SamplingFrequency;
            if (frequency == 0) { selecter = null; }
            else
            {
                selecter = new FMSC.Sampling.SystematicSelecter(frequency, iFrequency, true);
            }
            return selecter;
        }

        private SampleSelecter MakeBlockSampleSelector(SampleGroupDO sg)
        {
            SampleSelecter selecter = null;
            int iFrequency = (int)sg.InsuranceFrequency;
            int frequency = (int)sg.SamplingFrequency;
            if (frequency == 0) { selecter = null; }
            else
            {
                selecter = new FMSC.Sampling.BlockSelecter(frequency, iFrequency);
            }
            return selecter;
        }

        public void OnTally()
        {
            
        }

        public void OnLeavingCurrentUnit(System.ComponentModel.CancelEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void PerformBackup(string path)
        {
            throw new NotImplementedException();
        }

        public void PerformBackup(bool incluedTimeStamp)
        {
            throw new NotImplementedException();
        }

        //public void PopulateTallies(StratumVM stratumInfo, DataEntryMode stratumMode, CuttingUnitVM unit, System.Windows.Forms.Panel container, FSCruiserV2.Forms.ITallyView view)
        //{
        //    throw new NotImplementedException();
        //}

        public void Run()
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public void SaveCounts()
        {
            throw new NotImplementedException();
        }

        public bool TrySaveTree(TreeVM tree)
        {
            System.Diagnostics.Trace.WriteLine("Tree save");
            return true; 
        }

        public void SaveTrees(ICollection<TreeVM> list)
        {
            throw new NotImplementedException();
        }

        public void SetTreeTDV(TreeVM tree, CruiseDAL.DataObjects.TreeDefaultValueDO tdv)
        {
            throw new NotImplementedException();
        }

        public bool EnsureTreeNumberAvalible(long start)
        {
            throw new NotImplementedException();
        }

        public bool EnsureTreeNumberAvalible(long start, PlotVM plot)
        {
            throw new NotImplementedException();
        }

        public void Untally(TallyAction action)
        {
            throw new NotImplementedException();
        }

        public TreeVM UserAddTree(TreeVM templateTree, StratumVM knownStratum, PlotVM knownPlot)
        {
            throw new NotImplementedException();
        }

        public bool ValidateTrees()
        {
            throw new NotImplementedException();
        }

        public bool ValidateTrees(ICollection<TreeVM> list)
        {
            throw new NotImplementedException();
        }

        private Random rand = new Random();
        public int? GetKPI(int min, int max)
        {
            return rand.Next(min, max);
        }

        public int ShowNumericValueInput(int? min, int? max, int? initialValue)
        {
            throw new NotImplementedException();
        }

        public int? ShowNumericValueInput(int? min, int? max, int? initialValue, bool acceptNullInput)
        {
            throw new NotImplementedException();
        }

        public bool ShowLimitingDistanceDialog(StratumVM stratum, PlotVM plot, ref float dbh)
        {
            throw new NotImplementedException();
        }

        public void ShowLogs(TreeVM tree)
        {
            throw new NotImplementedException();
        }

        public int GetNextPlotNumber(CruiseDAL.DataObjects.CuttingUnitDO unit, CruiseDAL.DataObjects.StratumDO stratum)
        {
            throw new NotImplementedException();
        }

        public DataEntryMode GetStrataDataEntryMode(StratumDO stratum)
        {
            switch (stratum.Method)
            {
                case "100":
                    {
                        return DataEntryMode.Tree | DataEntryMode.HundredPct;
                    }
                case "STR":
                    {
                        return DataEntryMode.Tree | DataEntryMode.TallyTree;
                    }
                case "3P":
                    {
                        return DataEntryMode.Tree | DataEntryMode.ThreeP | DataEntryMode.TallyTree;
                    }
                case "PNT":
                case "FIX":
                    {
                        return DataEntryMode.Plot | DataEntryMode.OneStagePlot;
                    }
                case "FCM":
                case "PCM":
                    {
                        return DataEntryMode.Plot;
                    }

                case "F3P":
                case "P3P":
                case "3PPNT":
                    {
                        return DataEntryMode.Plot | DataEntryMode.ThreeP;
                    }
                case "S3P":
                default:
                    {
                        return DataEntryMode.HundredPct;//fall back on 100pct
                    }
            }

        }

        public DataEntryMode GetUnitDataEntryMode(CruiseDAL.DataObjects.CuttingUnitDO unit)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            return;
        }

        #endregion

        #region IApplicationController Members


        public bool OpenFile()
        {
            throw new NotImplementedException();
        }

        public bool ShowLimitingDistanceDialog(StratumVM stratum, PlotVM plot, TreeVM optTree)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IApplicationController Members


        public void LogTreeCountEdit(CountTreeDO countTree, long oldValue, long newValue)
        {
            throw new NotImplementedException();
        }

        public void LogSumKPIEdit(CountTreeDO countTree, long oldValue, long newValue)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
