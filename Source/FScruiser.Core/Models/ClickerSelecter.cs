using FMSC.Sampling;
using System;

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