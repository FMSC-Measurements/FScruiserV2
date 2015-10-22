using System.Windows.Forms;
using FSCruiser.Core.Models;

namespace FSCruiser.Core.ViewInterfaces
{
    interface IPlotInfoDialog
    {
        DialogResult ShowDialog(PlotVM plotInfo, bool allowEdit);
    }
}
