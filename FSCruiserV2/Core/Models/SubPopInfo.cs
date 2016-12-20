using CruiseDAL.DataObjects;

namespace FSCruiser.Core.Models
{
    public class SubPop
    {
        public SubPop() { }

        public SubPop(SampleGroup sg, TreeDefaultValueDO tdv)
        {
            this.SG = sg;
            this.TDV = tdv;
        }

        public SampleGroup SG { get; set; }

        public TreeDefaultValueDO TDV { get; set; }
    }
}