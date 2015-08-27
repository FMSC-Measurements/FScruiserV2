using CruiseDAL.DataObjects;
using CruiseDAL; 

namespace FSCruiserV2.Logic
{
    public class TreeVM : TreeDO
    {
        public TreeVM(DAL dal)
            : base(dal)
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

        //protected override void NotifyPropertyChanged(string name)
        //{
        //    base.NotifyPropertyChanged(name);
        //    if (name == CruiseDAL.Schema.TREE.TREEDEFAULTVALUE_CN)
        //    {
        //        base.NotifyPropertyChanged(CruiseDAL.Schema.TREE.HIDDENPRIMARY);
        //    }
        //}

       
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
        //        if (this.inWriteMode)
        //        {
        //            base.HiddenPrimary = value;
        //        }
        //        if (this.TreeDefaultValue != null)
        //        {
        //            if (this.TreeDefaultValue.HiddenPrimary == value)
        //            {
        //                base.HiddenPrimary = 0.0F;
        //            }
        //            else
        //            {
        //                base.HiddenPrimary = value;
        //                base.NotifyPropertyChanged(CruiseDAL.Schema.TREE.HIDDENPRIMARY);
        //            }
        //        }
                
        //    }
        //}
    }
}
