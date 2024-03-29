﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using CruiseDAL;
using CruiseDAL.DataObjects;
using FMSC.ORM.EntityModel.Attributes;
using FScruiser.Core.Services;
using System.Xml.Serialization;
using FMSC.ORM.Core;

namespace FSCruiser.Core.Models
{
    public class Plot : PlotDO
    {

        public Plot()
            : base()
        { }

        public Plot(DAL dal)
            : base(dal)
        { }

        public Plot(PlotDO obj)
            : base(obj)
        { }

        [IgnoreField]
        public new Stratum Stratum
        {
            get
            {
                return (Stratum)base.Stratum;
            }
            set
            {
                base.Stratum = value;
            }
        }

        [IgnoreField]
        public new CuttingUnit CuttingUnit
        {
            get
            {
                return (CuttingUnit)base.CuttingUnit;
            }
            set
            {
                base.CuttingUnit = value;
            }
        }

        [XmlArray]
        public IList<Tree> Trees { get; set; }

        //[IgnoreField]
        //public long HighestTreeNum
        //{
        //    get
        //    {
        //        if (this._trees == null || this.Trees.Count == 0)
        //        {
        //            return 0L;
        //        }
        //        return this.Trees[this.Trees.Count - 1].TreeNumber;
        //    }
        //}

        [IgnoreField]
        public bool IsNull
        {
            get
            {
                return base.IsEmpty == "True";
            }
            set
            {
                base.IsEmpty = (value) ? "True" : "False";
            }
        }

        public override StratumDO GetStratum()
        {
            if (DAL == null) { return null; }
            return DAL.ReadSingleRow<Stratum>(this.Stratum_CN);
        }

        public override CuttingUnitDO GetCuttingUnit()
        {
            if (DAL == null) { return null; }
            return DAL.ReadSingleRow<CuttingUnit>(this.CuttingUnit_CN);
        }

        public override void Delete()
        {
            //Debug.Assert(this.CuttingUnit != null);

            lock (DAL.TransactionSyncLock)
            {
                this.DAL.BeginTransaction();
                try
                {
                    DAL.Execute("DELETE FROM LogStock;");
                    DAL.Execute("DELETE FROM TreeCalculatedValues;");
                    DAL.Execute("DELETE FROM Log WHERE Tree_CN in (SELECT Tree_CN FROM Tree WHERE Plot_CN = @p1);", Plot_CN);
                    DAL.Execute("DELETE FROM Tree WHERE Plot_CN = @p1;", Plot_CN);
                    DAL.Execute("DELETE FROM Plot WHERE Plot_CN = @p1;", Plot_CN);
                    this.DAL.CommitTransaction();                    
                }
                catch
                {
                    this.DAL.RollbackTransaction();
                    throw;
                }
                OnDeleted();
            }
        }

        public virtual void PopulateTrees()
        {
            if (Trees == null)
            {
                List<Tree> tList = base.DAL.From<Tree>()
                    .Where("Stratum_CN = @p1 AND CuttingUnit_CN = @p2 AND Plot_CN = @p3")
                    .OrderBy("TreeNumber")
                    .Read(base.Stratum.Stratum_CN
                    , base.CuttingUnit.CuttingUnit_CN
                    , base.Plot_CN).ToList();

                foreach (var t in tList)
                {
                    t.ValidateVisableFields();
                }

                Trees = new BindingList<Tree>(tList); ;
            }
        }

        public void ResequenceTreeNumbers()
        {
            if (Trees != null)
            {
                int curTreeNumber = 1;
                foreach (var tree in Trees)
                {
                    tree.TreeNumber = curTreeNumber;
                    curTreeNumber++;
                }
            }
        }

        public bool IsTreeNumberAvalible(long treeNumber)
        {
            foreach (Tree tree in Trees)
            {
                if (tree.TreeNumber == treeNumber)
                {
                    return false;
                }
            }

            return true;
        }

        public void AddTree(Tree tree)
        {
            lock (((System.Collections.ICollection)Trees).SyncRoot)
            {
                this.Trees.Add(tree);
            }
        }

        public void DeleteTree(Tree tree)
        {
            tree.Delete();
            this.Trees.Remove(tree);
        }

        public bool ValidatePlot(out string message)
        {
            message = string.Empty;

            if (!ValidateTrees())
            {
                message = "Error(s) found on tree records in current plot";
                return false;
            }
            return true;
        }

        public bool ValidateTrees()
        {
            if (Trees == null) { return true; }
            var worker = new TreeValidationWorker(Trees);
            return worker.ValidateTrees();
        }

        public override string ToString()
        {
            return base.PlotNumber.ToString() + ((IsNull == true) ? "-Null" : string.Empty);
        }
    }
}