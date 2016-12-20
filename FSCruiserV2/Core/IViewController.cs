using System;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FSCruiser.Core.Models;
using FSCruiser.WinForms;
using FSCruiser.WinForms.DataEntry;

namespace FSCruiser.Core
{
    public delegate void HandleCruiseDataLoadedEventHandler();

    public interface IViewController : IDisposable
    {
        event System.ComponentModel.CancelEventHandler ApplicationClosing;

        IApplicationController ApplicationController { get; set; }

        bool EnableLogGrading { get; set; }

        //bool EnableCruiserSelectionPopup { get; set; }

        FormMain MainView { get; }

        FormNumPad NumPadDialog { get; }

        Form3PNumPad ThreePNumPad { get; }

        void BeginShowSplash();

        //FSCruiserV2.Forms.FormPlot GetPlotView(CruiseDAL.DataObjects.CuttingUnitDO unit);
        //FSCruiserV2.Forms.FormDataEntry GetTreeBasedView(CruiseDAL.DataObjects.CuttingUnitDO unit);
        //void HandleCuttingUnitDataLoaded();
        void HandleCruisersChanged();

        void HandleFileStateChanged();

        void SignalInvalidAction();

        //FSCruiserV2.Forms.Form3PPNTPlotInfo PlotInfo3PPNTView { get; }
        //FSCruiserV2.Forms.FormPlotInfo PlotInfoView { get; }
        void ShowMain();

        //FSCruiserV2.Forms.FormTallySettings TallySettingsView { get; }
        //FSCruiserV2.Forms.FormLogs LogsView { get; }
        //FSCruiserV2.Forms.FormLogs GetLogsView(StratumDO stratum);
        //FSCruiserV2.Forms.FormLimitingDistance LimitingDistanceView { get; }
        //FormManageCruisers ManageCruisersView { get; }
        //FormCruiserSelection CruiserSelectionView { get; }
        void ShowAbout();

        TreeDefaultValueDO ShowAddPopulation();

        TreeDefaultValueDO ShowAddPopulation(SampleGroupDO sg);

        void ShowBackupUtil();

        void ShowCruiserSelection(Tree tree);

        DialogResult ShowEditSampleGroup(SampleGroupDO sg, bool allowEdit);

        DialogResult ShowEditTreeDefault(TreeDefaultValueDO tdv);

        DialogResult ShowLimitingDistanceDialog(float baf, bool isVariableRadius, out string logMessage);

        void ShowLogsView(Stratum stratum, Tree tree);

        void ShowManageCruisers();

        DialogResult ShowOpenCruiseFileDialog(out string fileName);

        void ShowDataEntry(CuttingUnit unit);

        DialogResult ShowPlotInfo(Plot plotInfo, PlotStratum stratum, bool allowEdit);

        void ShowTallySettings(CountTree count);

        void ShowMessage(String message, String caption, MessageBoxIcon icon);

        bool AskYesNo(String message, String caption, MessageBoxIcon icon);

        bool AskYesNo(String message, String caption, MessageBoxIcon icon, bool defaultNo);

        bool AskCancel(String message, String caption, MessageBoxIcon icon, bool defaultCancel);

        int? AskKPI(int min, int max);

        void SignalMeasureTree(bool showMessage);

        void SignalInsuranceTree();

        void ShowWait();

        void HideWait();
    }
}