using FMSC.Sampling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSCruiser.Core.Models
{
    public class ClickerSelecter : SampleSelecter
    {
        public override SampleItem NextItem()
        {
            return new boolItem() { IsSelected = true };
        }

        public override bool Ready(bool throwException)
        {
            return true;
        }
    }
}