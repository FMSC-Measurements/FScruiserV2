using System.Windows.Forms;
using FSCruiser.Core.Models;

namespace FSCruiser.Core.ViewInterfaces
{
    interface IPlotInfoDialog
    {
        //uint PlotNumber { get; set; }
        //bool IsNull { get; set; }
        //float Slope { get; set; }
        //float Aspect { get; set; }
        //string Remarks { get; set; }

        //PlotVM Plot { get; }

        DialogResult ShowDialog(PlotVM plot, PlotStratum stratum, bool isNewPlot);
    }
}
