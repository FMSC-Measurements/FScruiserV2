using System;

using System.Collections.Generic;
using System.Text;
using CruiseDAL.DataObjects;
using FMSC.ORM.EntityModel.Attributes;

namespace FSCruiser.Core.Models
{
    public class Plot3PPNT : PlotVM
    {
        uint _treeCount;
        [IgnoreField]
        public uint TreeCount 
        {
            get { return _treeCount; }
            set
            {
                _treeCount = value;
                NotifyPropertyChanged("TreeCount");
            }
        }

        uint _averageHeight;
        [IgnoreField]
        public uint AverageHeight 
        {
            get { return _averageHeight; }
            set
            {
                _averageHeight = value;
                NotifyPropertyChanged("AverageHeight");
            }
        }


        decimal _volFactor = Constants.DEFAULT_VOL_FACTOR;
        [IgnoreField]
        public decimal VolFactor 
        {
            get { return _volFactor; }
            set 
            { 
                _volFactor = value;
                NotifyPropertyChanged("VolFactor");
            }
        }

        public Plot3PPNT() : base() { }
        public Plot3PPNT(CruiseDAL.DAL dal) : base(dal) { }
        public Plot3PPNT(PlotDO plot) : base(plot) { }

        public void CreateTrees()
        {
            for (long i = 0; i < this.TreeCount; i++)
            {
                TreeVM t = this.CreateNewTreeEntry((SampleGroupVM)null, (TreeDefaultValueDO)null, false);
                t.TreeCount = 1;
                t.CountOrMeasure = "M";
                t.TreeNumber = i + 1;
                t.Save();

                this.AddTree(t);
            }
        }

        public float CalculateKPI()
        {
            if (this.TreeCount <= 0 || this.AverageHeight <= 0)
            {
                return 0;
            }
            else
            {
                return (float)Math.Round((Decimal)(this.TreeCount * Stratum.BasalAreaFactor * this.AverageHeight) * this.VolFactor);
            }
        }

        public void StoreUserEnteredValues()
        {
            this.Remarks += string.Format("|Tree Cnt = {0}, Avg Ht = {1}, KPI = {2}|", TreeCount, AverageHeight, KPI);
        }
    }
}
