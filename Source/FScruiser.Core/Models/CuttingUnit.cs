using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CruiseDAL.DataObjects;
using FMSC.ORM.EntityModel.Attributes;
using FScruiser.Core.Services;

namespace FSCruiser.Core.Models
{
    public class CuttingUnit : CuttingUnitDO //, ITreeFieldProvider
    {
        //Sale _sale;

        //[IgnoreField]
        //public Sale Sale
        //{
        //    get
        //    {
        //        if (_sale == null)
        //        {
        //            this._sale = DAL.From<Sale>().Limit(1, 0).Query().FirstOrDefault();
        //        }
        //        return _sale;
        //    }
        //}

        public CuttingUnit()
            : base()
        {
        }

        #region ITreeFieldProvider

        //object _treeFieldsReadLock = new object();
        //IEnumerable<TreeFieldSetupDO> _treeFields;

        //public IEnumerable<TreeFieldSetupDO> TreeFields
        //{
        //    get
        //    {
        //        lock (_treeFieldsReadLock)
        //        {
        //            if (_treeFields == null && DAL != null)
        //            { _treeFields = ReadTreeFields().ToList(); }
        //            return _treeFields;
        //        }
        //    }
        //    set { _treeFields = value; }
        //}

        //public IEnumerable<TreeFieldSetupDO> ReadTreeFields()
        //{
        //    var fields = DAL.From<TreeFieldSetupDO>()
        //        .Join("CuttingUnitStratum", "USING (Stratum_CN)")
        //        .Join("Stratum", "USING (Stratum_CN)")
        //        .Where(String.Format("CuttingUnit_CN = ? AND Stratum.Method NOT IN ({0})"
        //        , string.Join(",", CruiseDAL.Schema.CruiseMethods.PLOT_METHODS.Select(s => "'" + s + "'").ToArray())))
        //        .GroupBy("Field")
        //        .OrderBy("FieldOrder")
        //        .Query(CuttingUnit_CN).ToList();

        //    if (fields.Count == 0)
        //    {
        //        fields.AddRange(Constants.DEFAULT_TREE_FIELDS);
        //    }

        //    //if unit has multiple tree strata
        //    //but stratum column is missing
        //    if (this.TreeStrata.Count > 1
        //        && fields.FindIndex(x => x.Field == "Stratum") == -1)
        //    {
        //        //find the location of the tree number field
        //        int indexOfTreeNum = fields.FindIndex(x => x.Field == CruiseDAL.Schema.TREE.TREENUMBER);
        //        //if user doesn't have a tree number field, fall back to the last field index
        //        if (indexOfTreeNum == -1) { indexOfTreeNum = fields.Count - 1; }//last item index
        //        //add the stratum field to the filed list
        //        TreeFieldSetupDO tfs = new TreeFieldSetupDO() { Field = "Stratum", Heading = "St", Format = "[Code]" };
        //        fields.Insert(indexOfTreeNum + 1, tfs);
        //    }

        //    return fields;
        //}

        #endregion ITreeFieldProvider

        public override string ToString()
        {
            return string.Format("{0}: {1} Area: {2}"
                , base.Code
                , base.Description
                , base.Area);
        }
    }
}