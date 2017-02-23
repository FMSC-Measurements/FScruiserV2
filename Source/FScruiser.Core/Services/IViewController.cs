using System;
using CruiseDAL.DataObjects;
using FSCruiser.Core.Models;
using FScruiser.Core.Services;

namespace FSCruiser.Core
{
    public delegate void HandleCruiseDataLoadedEventHandler();

    public interface IViewController : IDisposable
    {
        event System.ComponentModel.CancelEventHandler ApplicationClosing;

        IApplicationController ApplicationController { get; set; }

        //bool EnableLogGrading { get; set; }

        void HandleFileStateChanged();

        void ShowAbout();

        TreeDefaultValueDO ShowAddPopulation();

        TreeDefaultValueDO ShowAddPopulation(SampleGroupDO sg);

        void ShowBackupUtil();

        bool ShowEditSampleGroup(SampleGroupDO sg, bool allowEdit);

        bool ShowEditTreeDefault(TreeDefaultValueDO tdv);

        bool ShowLimitingDistanceDialog(float baf, bool isVariableRadius, out string logMessage);

        void ShowLogsView(Stratum stratum, Tree tree);

        void ShowManageCruisers();

        bool ShowOpenCruiseFileDialog(out string fileName);

        void ShowDataEntry(CuttingUnit unit);

        bool ShowPlotInfo(IDataEntryDataService dataService, Plot plotInfo, PlotStratum stratum, bool allowEdit);

        void ShowTallySettings(CountTree count);

        int? AskKPI(int min, int max);

        void ShowWait();

        void HideWait();
    }
}