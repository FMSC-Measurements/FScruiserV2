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

        void ShowAbout();

        void ShowBackupUtil();

        bool ShowOpenCruiseFileDialog(out string fileName);

        void ShowDataEntry(CuttingUnit unit);


        int? AskKPI(int min, int max);

        void ShowWait();

        void HideWait();
    }
}