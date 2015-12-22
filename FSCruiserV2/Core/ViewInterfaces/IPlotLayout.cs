using System.Windows.Forms;
using FSCruiser.Core.Models;
using FSCruiser.Core.DataEntry;


namespace FSCruiser.Core.ViewInterfaces
{
    public interface IPlotLayout : ITallyView, ITreeView
    {
        #region properties
        LayoutPlotLogic ViewLogicController { get; set; }

        //PlotVM CurrentPlot { get; set; }
        #endregion

        

        /// <returns>true if user choses to continue</returns>
        bool AskContinueOnCurrnetPlotTreeError();
        void ShowNoPlotSelectedMessage();
        void ShowNullPlotMessage();
        void ShowLimitingDistanceDialog();

        void RefreshTreeView(PlotVM currentPlot);
        void BindTreeData(BindingSource treeBS);
        void BindPlotData(BindingSource plotBS);

        void HandleCurrentTreeChanged(TreeVM tree);

        void ViewEndEdit();
    }
}
