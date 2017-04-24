using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSCruiser
{
    interface IHotKeySettings
    {
        string AddPlotKeyStr { get; set; }
        string AddTreeKeyStr { get; set; }
        string JumpTreeTallyKeyStr { get; set; }
        string ResequencePlotTreesKeyStr { get; set; }
        string UntallyKeyStr { get; set; }
    }
}
