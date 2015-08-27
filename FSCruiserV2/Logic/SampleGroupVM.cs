using System;
using System.Collections.Generic;
using System.Text;
using CruiseDAL.DataObjects;
using CruiseDAL.Schema;
using FMSC.Sampling;

namespace FSCruiserV2.Logic
{
    public class SampleGroupVM : SampleGroupDO
    {
        public SampleGroupVM()
            : base()
        { }

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
            return DAL.ReadSingleRow<StratumVM>(STRATUM._NAME, this.Stratum_CN);
        }

        public SampleSelecter Sampler
        {
            get;
            set;
        }


    }
}
