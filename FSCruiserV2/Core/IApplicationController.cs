using System;
using CruiseDAL.DataObjects;
using System.Collections.Generic;
using FSCruiser.Core.Models;
namespace FSCruiser.Core
{
    public interface IApplicationController : IDisposable
    {
        #region Properties =========================================================
        IViewController ViewController { get; }
        String BackupDir { get; set; }
        BackUpMethod BackUpMethod { get; set; }
        System.Collections.Generic.List<CruiserVM> Cruisers { get; }
        CruiseDAL.DAL _cDal { get; }

        System.Collections.Generic.List<CuttingUnitVM> CuttingUnits { get; }
        CuttingUnitVM CurrentUnit { get; set; }
        System.ComponentModel.BindingList<TallyAction> TallyHistory { get; }
        System.Collections.Generic.List<TreeVM> CurrentUnitTreeList { get; }
        System.ComponentModel.BindingList<TreeVM> CurrentUnitNonPlotTreeList { get; }
        StratumVM DefaultStratum { get; }

        
        //bool EnableCruiserSelectionPopup { get; set; }
        #endregion 

        #region ====================================================================
        //PlotVM AddPlot(StratumVM stratum);
        //void AddTallyAction(TallyAction action);
        //TreeVM CreateNewTreeEntry(CountTreeVM count);
        //TreeVM CreateNewTreeEntry(CountTreeVM count, PlotVM plot, bool isMeasure);
        //TreeVM CreateNewTreeEntry(CuttingUnitDO unit, StratumVM stratum, SampleGroupVM sg, CruiseDAL.DataObjects.TreeDefaultValueDO tdv, PlotVM plot, bool isMeasure);
        //StratumInfo CreateStratumInfo(CruiseDAL.DataObjects.CuttingUnitDO unit, CruiseDAL.DataObjects.StratumDO stratum);
        SampleGroupDO CreateNewSampleGroup(StratumDO stratum);
        TreeDefaultValueDO CreateNewTreeDefaultValue(String pProd);
        
        void DeletePlot(PlotVM plot);
        void DeleteTree(TreeVM tree);
        void DeleteTree(TreeVM tree, PlotVM plot);

        object GetTreeSGList(TreeVM tree);
        System.Collections.Generic.ICollection<TreeDefaultValueDO> GetTreeTDVList(TreeVM tree);
        System.Collections.Generic.IList<StratumVM> GetUnitPlotStrata();
        System.Collections.Generic.IList<StratumVM> GetUnitTreeBasedStrata();
        System.Collections.Generic.IList<StratumVM> GetUnitStrata();
        CountTreeVM GetCountRecord(TreeDO tree);

        void AddCruiser(string initials);
        void RemoveCruiser(CruiserVM cruiser);
        CruiserVM[] GetCruiserList();

        bool OpenFile();

        int GetLogNumerIndexStart(TreeVM tree);
        void HandleNonCriticalException(Exception ex, string optMessage);
        void LogTreeCountEdit(CountTreeDO countTree, long oldValue, long newValue);
        void LogSumKPIEdit(CountTreeDO countTree, long oldValue, long newValue);
        void LoadCuttingUnit(CuttingUnitVM unit);
        //void LoadCuttingUnitData();
        //void AsyncLoadCuttingUnitData();
        //void LoadDatabase(string path);
        //FMSC.Sampling.SampleSelecter MakeSampleSelecter(SampleGroupVM sg);
        void OnTally();
        void OnLeavingCurrentUnit(System.ComponentModel.CancelEventArgs e);
        void PerformBackup(string path);
        void PerformBackup(bool incluedTimeStamp);
        //void PopulateTallies(StratumVM stratumInfo, DataEntryMode stratumMode, CuttingUnitVM unit, System.Windows.Forms.Panel container, FSCruiserV2.Forms.ITallyView view);
        void Run();
        bool Save();
        //void SaveCounts();
        //bool TrySaveTree(TreeVM tree);
        //void SaveTrees(System.Collections.Generic.ICollection<TreeVM> list);
        //void SetTreeTDV(TreeVM tree, TreeDefaultValueDO tdv);
        bool EnsureTreeNumberAvalible(long start);
        bool EnsureTreeNumberAvalible(long start, PlotVM plot);
        //void Untally(TallyAction action);
        TreeVM UserAddTree(TreeVM templateTree, StratumVM knownStratum, PlotVM knownPlot);
        //bool ValidateTrees();
        //bool ValidateTrees(System.Collections.Generic.ICollection<TreeVM> list);
        //bool ValidateTrees(System.Collections.Generic.ICollection<TreeVM> list, System.Collections.Generic.ICollection<String> fields);

        #endregion

        #region  UI methods ========================================================

        //int? GetKPI(int min, int max);
        //void ShowDataEntry(CruiseDAL.DataObjects.CuttingUnitDO unit);
        //void ShowMain();
        int ShowNumericValueInput(int? min, int? max, int? initialValue);
        int? ShowNumericValueInput(int? min, int? max, int? initialValue, bool acceptNullInput);
        //System.Windows.Forms.DialogResult ShowPlotInfo(PlotInfo plotInfo, bool allowEdit);
        //void ShowTallySettings(CruiseDAL.DataObjects.CountTreeDO count);
        bool ShowLimitingDistanceDialog(StratumVM stratum, PlotVM plot, TreeVM optTree);
        void ShowLogs(TreeVM tree);
        //void SignalInsuranceTree();
        //void SignalMeasureTree();
        #endregion

        //moved to IViewController
        #region DataObject description helper methods ==============================
        //string GetCuttingUnitDiscription(CruiseDAL.DataObjects.CuttingUnitDO unit);
        //string GetPlotInfo(CruiseDAL.DataObjects.PlotDO plot);
        //string GetPlotInfoShort(CruiseDAL.DataObjects.PlotDO plot);
        //string GetSampleGroupInfo(CruiseDAL.DataObjects.SampleGroupDO sampleGroup);
        //string GetStratumInfo(CruiseDAL.DataObjects.StratumDO stratum);
        //string GetStratumInfo(CruiseDAL.DataObjects.StratumDO stratum, CruiseDAL.DataObjects.CuttingUnitDO unit);
        //string GetStratumInfoShort(CruiseDAL.DataObjects.StratumDO stratum);
        //string GetLogLevelTreeDescription(TreeVM tree);


        //int GetNextPlotNumber(CruiseDAL.DataObjects.CuttingUnitDO unit, CruiseDAL.DataObjects.StratumDO stratum);
        //DataEntryMode GetStrataDataEntryMode(CruiseDAL.DataObjects.StratumDO stratum);
        DataEntryMode GetUnitDataEntryMode(CruiseDAL.DataObjects.CuttingUnitDO unit);
        #endregion

        
    }
}
