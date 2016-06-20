using System;
using System.Collections.Generic;
using System.Linq;
using CruiseDAL;
using CruiseDAL.DataObjects;
using FMSC.ORM.Core;
using FMSC.ORM.EntityModel.Attributes;
using FSCruiser.Core.ViewInterfaces;

namespace FSCruiser.Core.Models
{
    [EntitySource(SourceName = CruiseDAL.Schema.TREEFIELDSETUP._NAME)]
    public class StratumFieldCollection
    {
        [Field(Alias = "FieldStr", SQLExpression = "group_concat(Field)")]
        public string FieldsStr { get; set; }

        string[] _fieldNames;

        public IEnumerable<string> FieldNames
        {
            get
            {
                if (_fieldNames == null
                    && !string.IsNullOrEmpty(FieldsStr))
                {
                    _fieldNames = FieldsStr.Split(',');
                }
                return _fieldNames;
            }
        }
    }

    public class TreeVM : TreeDO
    {
        int cachedLogCount = -1;

        public TreeVM(DAL dal)
            : base((DatastoreRedux)dal)
        {
        }

        public TreeVM()
            : base()
        {
        }

        [IgnoreField]
        public new CuttingUnitVM CuttingUnit
        {
            get
            {
                return (CuttingUnitVM)base.CuttingUnit;
            }
            set
            {
                base.CuttingUnit = value;
            }
        }

        [IgnoreField]
        public new SampleGroupModel SampleGroup
        {
            get
            {
                return (SampleGroupModel)base.SampleGroup;
            }
            set
            {
                base.SampleGroup = value;
            }
        }

        [IgnoreField]
        public new StratumModel Stratum
        {
            get
            {
                return (StratumModel)base.Stratum;
            }
            set
            {
                base.Stratum = value;
            }
        }

        public override float TreeCount
        {
            get
            {
                return base.TreeCount;
            }
            set
            {
                if (!base.PropertyChangedEventsDisabled // if not being inflated
                    && value < 0) { return; }
                base.TreeCount = value;
            }
        }

        [IgnoreField]
        public int LogCountActual
        {
            get
            {
                if (cachedLogCount == -1 || this.LogCountDirty)
                {
                    cachedLogCount = (int)this.DAL.GetRowCount("Log", "WHERE Tree_CN = ?", this.Tree_CN);
                    LogCountDirty = false;
                }
                return cachedLogCount;
            }
        }

        [IgnoreField]
        public double LogCountDesired
        {
            get;
            set;
        }

        [IgnoreField]
        public bool LogCountDirty { get; set; }

        [IgnoreField]
        public string LogLevelDiscription
        {
            get
            {
                return String.Format("Tree:{0}  Sp:{1}  DBH:{2} Ht:{3}  Log Length:{4}  Logs:{5}",
                TreeNumber,
                Species,
                DBH,
                TotalHeight,
                (TreeDefaultValue != null) ? TreeDefaultValue.MerchHeightLogLength.ToString() : String.Empty,
                LogCountDesired);
            }
        }

        //public override float HiddenPrimary
        //{
        //    get
        //    {
        //        if (this.TreeDefaultValue != null && base.HiddenPrimary == 0.0F)
        //        {
        //            return this.TreeDefaultValue.HiddenPrimary;
        //        }
        //        return base.HiddenPrimary;
        //    }
        //    set
        //    {
        //        if (this.PropertyChangedEventsDisabled)
        //        {
        //            base.HiddenPrimary = value;
        //        }
        //        else if (this.TreeDefaultValue != null)
        //        {
        //            if (this.TreeDefaultValue.HiddenPrimary == value)
        //            {
        //                base.HiddenPrimary = 0.0F;
        //            }
        //            else
        //            {
        //                base.HiddenPrimary = value;
        //                //base.NotifyPropertyChanged(CruiseDAL.Schema.TREE.HIDDENPRIMARY);
        //            }
        //        }
        //    }
        //}

        #region overridden methods

        public override CuttingUnitDO GetCuttingUnit()
        {
            if (DAL == null) { return null; }
            return DAL.From<CuttingUnitVM>().Where("CuttingUnit_CN = ?")
                .Read(this.CuttingUnit_CN).FirstOrDefault();
        }

        //public override PlotDO GetPlot()
        //{
        //    if (DAL == null) { return null; }
        //    return DAL.ReadSingleRow<PlotVM>(CruiseDAL.Schema.PLOT._NAME, this.Plot_CN);
        //}

        public override StratumDO GetStratum()
        {
            if (DAL == null) { return null; }
            return DAL.ReadSingleRow<StratumModel>(this.Stratum_CN);
        }

        public override SampleGroupDO GetSampleGroup()
        {
            if (DAL == null) { return null; }
            return DAL.ReadSingleRow<SampleGroupModel>(this.SampleGroup_CN);
        }

        protected override void NotifyPropertyChanged(string name)
        {
            base.NotifyPropertyChanged(name);
            if (base.PropertyChangedEventsDisabled) { return; }

            if (name == CruiseDAL.Schema.TREE.TREEDEFAULTVALUE_CN)
            {
                base.NotifyPropertyChanged(CruiseDAL.Schema.TREE.HIDDENPRIMARY);
            }
        }

        #endregion overridden methods

        #region validation

        static Dictionary<long, StratumFieldCollection> _stratumFieldsLookup = new Dictionary<long, StratumFieldCollection>();

        protected StratumFieldCollection GetStratumFieldCollection(long? stratum_CN)
        {
            if (stratum_CN == null) { return null; }
            if (!_stratumFieldsLookup.ContainsKey(stratum_CN.Value))
            {
                var stFields = DAL.From<StratumFieldCollection>()
                    .Where("Stratum_CN = ?")
                    .Query(stratum_CN.Value).FirstOrDefault();
                _stratumFieldsLookup.Add(stratum_CN.Value, stFields);
                return stFields;
            }
            else
            {
                return _stratumFieldsLookup[stratum_CN.Value];
            }
        }

        public bool ValidateVisableFields()
        {
            var stFields = GetStratumFieldCollection(Stratum_CN);
            if (stFields != null)
            {
                return Validate(stFields.FieldNames);
            }
            else
            {
                return true;
            }
        }

        #endregion validation

        public bool HandleSampleGroupChanging(SampleGroupDO newSG, IView view)
        {
            if (newSG == null) { return false; }
            if (SampleGroup != null
                && SampleGroup.SampleGroup_CN == newSG.SampleGroup_CN) { return true; }

            if (SampleGroup != null)
            {
                if (!view.AskYesNo("You are changing the Sample Group of a tree, are you sure you want to do this?"
                    , "!"
                    , System.Windows.Forms.MessageBoxIcon.Asterisk
                    , true))
                {
                    return false;
                }
                else
                {
                    DAL.LogMessage(String.Format("Tree Sample Group Changed (Cu:{0} St:{1} Sg:{2} -> {3} Tdv_CN:{4} T#: {5}",
                        CuttingUnit.Code,
                        Stratum.Code,
                        (SampleGroup != null) ? SampleGroup.Code : "?",
                        newSG.Code,
                        (TreeDefaultValue != null) ? TreeDefaultValue.TreeDefaultValue_CN.ToString() : "?",
                        TreeNumber), "high");
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        public bool HandleSampleGroupChanged()
        {
            if (TreeDefaultValue != null)
            {
                if (!SampleGroup.HasTreeDefault(TreeDefaultValue))
                {
                    SetTreeTDV(null);
                }
            }
            return TrySave();
        }

        public bool HandleStratumChanging(StratumDO newStratum, IView view)
        {
            if (newStratum == null) { return false; }
            if (Stratum != null
                && Stratum.Stratum_CN == newStratum.Stratum_CN)
            { return false; }

            if (Stratum != null)
            {
                if (!view.AskYesNo("You are changing the stratum of a tree" +
                    ", are you sure you want to do this?"
                    , "!", System.Windows.Forms.MessageBoxIcon.Asterisk))
                {
                    return false;//do not change stratum
                }
                else
                {
                    //log stratum changed
                    DAL.LogMessage(String.Format("Tree Stratum Changed (Cu:{0} St:{1} -> {2} Sg:{3} Tdv_CN:{4} T#: {5} P#:{6}"
                        , CuttingUnit.Code
                        , Stratum.Code
                        , newStratum.Code
                        , (SampleGroup != null) ? SampleGroup.Code : "?"
                        , (TreeDefaultValue != null) ? TreeDefaultValue.TreeDefaultValue_CN.ToString() : "?"
                        , TreeNumber
                        , (Plot != null) ? Plot.PlotNumber.ToString() : "-"), "I");
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        public bool HandleStratumChanged()
        {
            Species = null;
            SampleGroup = null;
            SetTreeTDV(null);
            return TrySave();
        }

        public void SetTreeTDV(TreeDefaultValueDO tdv)
        {
            //if (tdv == _newPopPlaceHolder && this.SampleGroup != null)
            //{
            //    tdv = ViewController.ShowAddPopulation(this.SampleGroup);
            //}

            this.TreeDefaultValue = tdv;
            if (tdv != null)
            {
                this.Species = tdv.Species;

                this.LiveDead = tdv.LiveDead;
                this.Grade = tdv.TreeGrade;
                this.FormClass = tdv.FormClass;
                this.RecoverablePrimary = tdv.Recoverable;
                //tree.HiddenPrimary = tdv.HiddenPrimary;//#367
            }
            else
            {
                this.Species = string.Empty;
                this.LiveDead = string.Empty;
                this.Grade = string.Empty;
                this.FormClass = 0;
                this.RecoverablePrimary = 0;
                //this.HiddenPrimary = 0;
            }
        }

        public bool TrySave()
        {
            try
            {
                this.Save();
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Fail(e.GetType().Name, e.Message);
                //this.HandleNonCriticalException(e, "Unable to save tree. Ensure Tree Number, Sample Group and Stratum are valid");
                return false;
            }
        }

        public double GetDefaultLogCount()
        {
            if (TreeDefaultValue == null) { return 0.0; }

            var retionLogInfo = this.CuttingUnit.Sale.GetRegionLogInfo();
            var mrchHtLL = TreeDefaultValue.MerchHeightLogLength;

            if (retionLogInfo != null)
            {
                var logRule = retionLogInfo.GetLogRule(this.Species);
                if (logRule != null)
                {
                    return logRule.GetDefaultLogCount(this.TotalHeight, this.DBH, mrchHtLL);
                }
            }
            return 0;
        }

        public IEnumerable<LogDO> QueryLogs()
        {
            return this.DAL.From<LogDO>()
                .Where("Tree_CN = ?")
                .OrderBy("CAST (LogNumber AS NUMERIC)")
                .Query(Tree_CN);
        }

        public IList<LogDO> LoadLogs()
        {
            var logs = QueryLogs().ToList();
            var defaultLogCnt = GetDefaultLogCount();
            this.LogCountDesired = defaultLogCnt;

            if (logs.Count == 0)
            {
                defaultLogCnt = Math.Ceiling(defaultLogCnt);
                for (int i = 0; i < defaultLogCnt; i++)
                {
                    logs.Add(
                        new LogDO(this.DAL)
                        {
                            LogNumber = (i + 1).ToString(),
                            Tree = this
                        });
                }
            }

            return logs;
        }
    }
}