using System;
using CruiseDAL.DataObjects;
using FSCruiser.Core.Models;

namespace FSCruiser.Core
{
    public delegate void HandleCruiseDataLoadedEventHandler();

    public interface IViewController : IDisposable
    {
        event System.ComponentModel.CancelEventHandler ApplicationClosing;

        IApplicationController ApplicationController { get; set; }

        bool EnableLogGrading { get; set; }

        void HandleFileStateChanged();

        void SignalInvalidAction();

        void ShowAbout();

        TreeDefaultValueDO ShowAddPopulation();

        TreeDefaultValueDO ShowAddPopulation(SampleGroupDO sg);

        void ShowBackupUtil();

        void ShowCruiserSelection(Tree tree);

        bool ShowEditSampleGroup(SampleGroupDO sg, bool allowEdit);

        bool ShowEditTreeDefault(TreeDefaultValueDO tdv);

        bool ShowLimitingDistanceDialog(float baf, bool isVariableRadius, out string logMessage);

        void ShowLogsView(Stratum stratum, Tree tree);

        void ShowManageCruisers();

        bool ShowOpenCruiseFileDialog(out string fileName);

        void ShowDataEntry(CuttingUnit unit);

        bool ShowPlotInfo(Plot plotInfo, PlotStratum stratum, bool allowEdit);

        void ShowTallySettings(CountTree count);

        int? AskKPI(int min, int max);

        void SignalMeasureTree(bool showMessage);

        void SignalInsuranceTree();

        void SignalTally();

        void SignalPageChanged();

        void ShowWait();

        void HideWait();
    }
}