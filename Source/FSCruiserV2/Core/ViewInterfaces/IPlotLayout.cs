using System.Windows.Forms;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;

namespace FSCruiser.Core.ViewInterfaces
{
    public interface IPlotLayout : ITallyView, ITreeView
    {
        #region properties

        LayoutPlotLogic ViewLogicController { get; set; }

        //PlotVM CurrentPlot { get; set; }

        #endregion properties

        void ShowNoPlotSelectedMessage();

        void ShowNullPlotMessage();

        void ShowLimitingDistanceDialog();

        void RefreshTreeView(Plot currentPlot);

        void BindTreeData(BindingSource treeBS);

        void BindPlotData(BindingSource plotBS);

        void HandleCurrentTreeChanged(Tree tree);

        void ViewEndEdit();
    }
}