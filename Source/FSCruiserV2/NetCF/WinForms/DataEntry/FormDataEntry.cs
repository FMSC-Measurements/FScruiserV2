using System;
using System.Windows.Forms;
using FMSC.Controls;
using FSCruiser.Core;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;
using FSCruiser.WinForms.Common;
using FSCruiser.WinForms;
using FScruiser.Core.Services;
using FScruiser.Services;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class FormDataEntry : FMSC.Controls.CustomForm, IDataEntryView
    {
        public FormDataEntry() : base()
        {
            InitializeComponent();

            if (ViewController.PlatformType == PlatformType.WM)
            {
                this.SIP = new Microsoft.WindowsCE.Forms.InputPanel();
                this.components.Add(SIP);
            }
            else if (ViewController.PlatformType == PlatformType.WinCE)
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        public FormDataEntry(IApplicationController controller,
            ApplicationSettings appSettings,
            IDataEntryDataService dataService,
            ISampleSelectorRepository sampleSelectorRepository) : this()
        {
            InitializeCommon(controller, appSettings, dataService, sampleSelectorRepository);
        }

        void RefreshPreventSleepTimer()
        {
            if (AppSettings != null 
                && AppSettings.KeepDeviceAwake 
                && preventSleepTimer == null)
            {
                preventSleepTimer = new System.Threading.Timer(CallSystemIdleTimerReset, null, 0, 30 * 1000);
            }
            else
            {
                if (preventSleepTimer != null)
                {
                    preventSleepTimer.Dispose();
                    preventSleepTimer = null;
                }
            }
        }

        void CallSystemIdleTimerReset(object obj)
        {
            Win32.SystemIdleTimerReset();
        }

        protected void OnFocusedLayoutChanged(object sender, EventArgs e)
        {

            OnFocusedLayoutChangedCommon(sender, e);
            var view = FocusedLayout as ITreeView;
            _addTreeMI.Enabled = view != null && view.UserCanAddTrees;

        }

        private void _appSettings_HotKeysChanged()
        {
            //do nothing
        }


        private void menuItem1_Popup(object sender, EventArgs e)
        {
            this._limitingDistanceMI.Enabled = this.FocusedLayout is LayoutPlot;
            this._showHideErrorColMI.Enabled = this.FocusedLayout is ITreeView;
            this._deleteRowButton.Enabled = this.FocusedLayout is ITreeView;
            this._showHideLogColMI.Enabled = this.FocusedLayout is ITreeView;
        }
    }
}