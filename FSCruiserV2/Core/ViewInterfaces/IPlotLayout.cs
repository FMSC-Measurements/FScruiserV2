using System.Windows.Forms;
using FSCruiserV2.Logic;

namespace FSCruiserV2.Forms
{
    public interface IPlotLayout : ITallyView, ITreeView
    {
        #region properties
        LayoutPlotLogic ViewLogicController { get; set; }

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
