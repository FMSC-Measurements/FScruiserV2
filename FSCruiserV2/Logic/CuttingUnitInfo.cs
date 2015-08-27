using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using System.ComponentModel;

namespace FSCruiserV2.Logic
{
    public class CuttingUnitVM : CuttingUnitDO
    {
        public List<CountTreeDO> Counts { get; set; }
        public BindingList<TreeDO> TreeList { get; set; }
        //public new BindingList<TallyAction> TallyHistoryBuffer { get; set; }
        public int UnitTreeNumIndex = 0;


        public CuttingUnitVM() 
            :base()
        { }

        public override string ToString()
        {
            return string.Format("{0}: {1}", base.Code, String.Format("{0} Area: {1}", base.Description, base.Area));
        }
    }
}
