using System.Windows.Forms;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;
using FScruiser.Core.Services;

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

        void ShowCurrentPlotInfo();

        bool ShowPlotInfo(IDataEntryDataService dataService, Plot plot, PlotStratum stratum, bool isNewPlot);

        void RefreshTreeView(Plot currentPlot);

        void BindTreeData(BindingSource treeBS);

        void BindPlotData(BindingSource plotBS);

        void HandleCurrentTreeChanged(Tree tree);

        void ViewEndEdit();
    }
}