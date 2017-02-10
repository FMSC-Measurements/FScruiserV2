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

        System.Windows.Forms.Keys AddPlotKey { get; set; }
        System.Windows.Forms.Keys AddTreeKey { get; set; }
        System.Windows.Forms.Keys JumpTreeTallyKey { get; set; }
        System.Windows.Forms.Keys ResequencePlotTreesKey { get; set; }
        System.Windows.Forms.Keys UntallyKey { get; set; }
    }
}
