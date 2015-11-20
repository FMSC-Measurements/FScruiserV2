using CruiseDAL.DataObjects;
using CruiseDAL;
using FMSC.ORM.Core;
using System;

namespace FSCruiser.Core.Models
{
    public class TreeVM : TreeDO
    {
        public TreeVM(DAL dal)
            : base((DatastoreRedux)dal)
        {
        }

        public TreeVM()
            : base()
        {
        }

        public TreeVM(TreeDO tree)
            : base(tree)
        {
        }

        public new SampleGroupVM SampleGroup
        {
            get
            {
                return (SampleGroupVM)base.SampleGroup;
            }
            set
            {
                base.SampleGroup = value;
            }
        }

        public new StratumVM Stratum
        {
            get
            {
                return (StratumVM)base.Stratum;
            }
            set
            {
                base.Stratum = value;
            }
        }

        //public override float HiddenPrimary
        //{
        //    get
        //    {
        //        if (base.HiddenPrimary == 0.0F && this.TreeDefaultValue != null)
        //        {
        //            return this.TreeDefaultValue.HiddenPrimary;
        //        }
        //        else
        //        {
        //            return base.HiddenPrimary;
        //        }
        //    }
        //    set
        //    {
        //        if (!base.PropertyChangedEventsDisabled &&  this.TreeDefaultValue != null && value == base.HiddenPrimary)
        //        {
        //            base.HiddenPrimary = 0;
        //        }
        //        else
        //        {
        //            base.HiddenPrimary = value;
        //        }
        //    }
        //}

        public override float HiddenPrimary
        {
            get
            {
                if (this.TreeDefaultValue != null && base.HiddenPrimary == 0.0F)
                {
                    return this.TreeDefaultValue.HiddenPrimary;
                }
                return base.HiddenPrimary;
            }
            set
            {
                if (this.PropertyChangedEventsDisabled)
                {
                    base.HiddenPrimary = value;
                }
                if (this.TreeDefaultValue != null)
                {
                    if (this.TreeDefaultValue.HiddenPrimary == value)
                    {
                        base.HiddenPrimary = 0.0F;
                    }
                    else
                    {
                        base.HiddenPrimary = value;
                        //base.NotifyPropertyChanged(CruiseDAL.Schema.TREE.HIDDENPRIMARY);
                    }
                }

            }
        }

        public override StratumDO GetStratum()
        {

            if (DAL == null) { return null; }
            return DAL.ReadSingleRow<StratumVM>(CruiseDAL.Schema.STRATUM._NAME, this.Stratum_CN);
        }

        public override SampleGroupDO GetSampleGroup()
        {
            if (DAL == null) { return null; }
            return DAL.ReadSingleRow<SampleGroupVM>(CruiseDAL.Schema.SAMPLEGROUP._NAME, this.SampleGroup_CN);
        }

        

        public override PlotDO GetPlot()
        {
            if (DAL == null) { return null; }
            return DAL.ReadSingleRow<PlotVM>(CruiseDAL.Schema.PLOT._NAME, this.Plot_CN);
        }

        private int cachedLogCount = -1;
        public int LogCount
        {
            get
            {
                if (cachedLogCount == -1 || this.LogCountDirty)
                {
                    cachedLogCount = (int)this.DAL.GetRowCount("Log", "WHERE Tree_CN = ?", this.Tree_CN);
                }
                return cachedLogCount;
            }
        }

        public bool LogCountDirty { get; set; }

        protected override void NotifyPropertyChanged(string name)
        {
            base.NotifyPropertyChanged(name);
            if (name == CruiseDAL.Schema.TREE.TREEDEFAULTVALUE_CN)
            {
                base.NotifyPropertyChanged(CruiseDAL.Schema.TREE.HIDDENPRIMARY);
            }
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

    }
}
