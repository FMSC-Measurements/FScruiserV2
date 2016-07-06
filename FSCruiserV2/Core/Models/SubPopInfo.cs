using CruiseDAL.DataObjects;

namespace FSCruiser.Core.Models
{
    public class SubPop
    {
        public SubPop() { }

        public SubPop(SampleGroupModel sg, TreeDefaultValueDO tdv)
        {
            this.SG = sg;
            this.TDV = tdv;
        }

        public SampleGroupModel SG { get; set; }

        public TreeDefaultValueDO TDV { get; set; }
    }
}