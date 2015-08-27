using System.Windows.Forms;
using FSCruiserV2.Logic;

namespace FSCruiserV2.Forms
{
    interface IPlotInfoDialog
    {
        DialogResult ShowDialog(PlotVM plotInfo, bool allowEdit);
    }
}
