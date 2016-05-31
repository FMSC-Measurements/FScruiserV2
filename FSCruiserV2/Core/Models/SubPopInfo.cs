using CruiseDAL.DataObjects;

namespace FSCruiser.Core.Models
{
    public class SubPop
    {
        public SubPop() { }

        public SubPop(SampleGroupVM sg, TreeDefaultValueDO tdv)
        {
            this.SG = sg;
            this.TDV = tdv;
        }

        public SampleGroupVM SG { get; set; }

        public TreeDefaultValueDO TDV { get; set; }
    }
}