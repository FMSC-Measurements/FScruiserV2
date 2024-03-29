﻿using FMSC.Sampling;
using FScruiser.Core.Services;
using FScruiser.Services;
using FSCruiser.Core;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

#if NetCF

using Microsoft.WindowsCE.Forms;

#endif

namespace FSCruiser.WinForms.DataEntry
{
    /// <summary>
    /// Base class for win-forms Data Entry Form
    /// </summary>
    public partial class FormDataEntry : FMSC.Controls.CustomForm, IDataEntryView
    {
        private IDataEntryPage _previousLayout;
        private List<IDataEntryPage> _layouts = new List<IDataEntryPage>();
        private LayoutTreeBased _tallyLayout;
        private int _treePageIndex;
        private int _tallyPageIndex;
        private ControlTreeDataGrid _treeView;

#if NetCF
        System.Threading.Timer preventSleepTimer;
        public InputPanel SIP { get; set; }

#endif

        protected IDataEntryDataService DataService { get; set; }

        protected ISampleSelectorRepository SampleSelectorRepository { get; set; }

        public IApplicationController Controller { get; protected set; }

        protected TabControl PageContainer
        {
            get { return _pageContainer; }//initialized in Initialize Component
        }

        #region Initialize Controls

        protected void InitializeCommon(IApplicationController controller,
            ApplicationSettings appSettings,
            IDataEntryDataService dataService,
            ISampleSelectorRepository sampleSelectorRepository)
        {
            KeyPreview = true;

            Controller = controller;
            DataService = dataService;
            AppSettings = appSettings;
            SampleSelectorRepository = sampleSelectorRepository;

            LogicController = new FormDataEntryLogic(Controller,
                DialogService.Instance,
                SoundService.Instance,
                DataService,
                AppSettings,
                this,
                sampleSelectorRepository);

            InitializePageContainer();
        }

        protected void InitializePageContainer()
        {

            PageContainer.SuspendLayout();

            if ((DataService.TreeStrata == null || !DataService.TreeStrata.Any())
                && (DataService.PlotStrata == null || !DataService.PlotStrata.Any()))
            {
                throw new UserFacingException("No Valid Strata Found", null);
            }

            //do we have any tree based strata in the unit
            if (DataService.TreeStrata != null && DataService.TreeStrata.Any())
            {
                InitializeTreesTab();

                // if any strata are not H_PCT
                if (DataService.TreeStrata.Any(
                    x => x.Method != CruiseDAL.Schema.CruiseMethods.H_PCT))
                {
                    InitializeTallyTab();
                }
            }

            InitializePlotTabs();

            PageContainer.SelectedIndexChanged += new EventHandler(OnFocusedLayoutChanged);

            PageContainer.ResumeLayout(false);

            this.ResumeLayout(false);
        }

        protected void InitializeTreesTab()
        {
#if NetCF
            _treeView = new ControlTreeDataGrid(DataService
                , AppSettings
                , this.LogicController)
            {
                Dock = DockStyle.Fill,
                UserCanAddTrees = true,
                SIP = SIP
            };
#else
            _treeView = new ControlTreeDataGrid(DataService
                , ApplicationSettings.Instance
                , this.LogicController);
#endif
            _treeView.UserCanAddTrees = true;

            _treePageIndex = AddLayout(_treeView);
        }

        private int AddLayout(IDataEntryPage page)
        {
            var pageControl = (Control)page;
            var tabPage = new TabPage();

            tabPage.Text = page.Text;
            pageControl.TextChanged += new EventHandler(
                (object sender, EventArgs ea) => tabPage.Text = page.Text);

            ((Control)page).Dock = DockStyle.Fill;
            tabPage.Controls.Add(pageControl);
            PageContainer.TabPages.Add(tabPage);
            _layouts.Add(page);

            return PageContainer.TabPages.IndexOf(tabPage);
        }

        protected void InitializeTallyTab()
        {
            _tallyLayout = new LayoutTreeBased(DataService,
                SampleSelectorRepository,
                AppSettings,
                LogicController);

            _tallyPageIndex = AddLayout(_tallyLayout);
        }

        protected void InitializePlotTabs()
        {
            foreach (PlotStratum st in DataService.PlotStrata)
            {
                if (st.Method == "3PPNT")
                {
                    if (st.KZ3PPNT <= 0)
                    {
                        MessageBox.Show("error 3PPNT missing KZ value, please return to Cruise System Manger and fix");
                        continue;
                    }
                    st.SampleSelecter = new ThreePSelecter((int)st.KZ3PPNT, 0);
                }

                LayoutPlot view = new LayoutPlot(DataService,
                    SampleSelectorRepository,
                    AppSettings,
                    SoundService.Instance,
                    Controller.ViewController,
                    st);
                view.UserCanAddTrees = true;

#if NetCF
                view.Sip = SIP;
#endif
                AddLayout(view);
                this.LogicController.RegisterStratumHotKey(st.Hotkey, view);
            }
        }

        #endregion Initialize Controls

        #region Overrides

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            foreach (var tv in this._layouts.OfType<ITreeView>())
            {
                tv.HandleLoad();
            }

            if (this._tallyLayout != null)
            {
                this._tallyLayout.HandleLoad();
            }

#if !NetCF
            _startupStopwatch.Stop();
            var elapsed = _startupStopwatch.ElapsedMilliseconds;
            Microsoft.AppCenter.Analytics.Analytics.TrackEvent("DataEntry_Startup_Time", 
                new Dictionary<string, string> { { "Milliseconds", elapsed.ToString() }, });

            var gcMemory = Math.Round((double)GC.GetTotalMemory(false) / (1 << 20), 2); // convert to MB
            using (var proc = Process.GetCurrentProcess())
            {
                var fullWorkingSet = Math.Round((double)proc.WorkingSet64 / (1 << 20), 2); // convert to MB
                var privateMem = Math.Round((double)proc.PrivateMemorySize64 / (1 << 20), 2);
                Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Memory_Checkpoint",
                    new Dictionary<string, string>
                    {
                        {"Event", "DataEntry_OnLoad" },
                        {"managedMem", gcMemory.ToString() },
                        {"fullWorkingSet", fullWorkingSet.ToString() },
                        {"privateMem", privateMem.ToString() },
                    });
            }
#endif

            // Turn off the wait cursor that was turned on in when displaying main form
            Cursor.Current = Cursors.Default;
            this.OnFocusedLayoutChanged(null, null);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            this.LogicController.HandleViewClosing(e);
        }

        private KeysConverter keyConverter = new KeysConverter();

        #region appSettings

        private ApplicationSettings _appSettings;

        public ApplicationSettings AppSettings
        {
            get { return _appSettings; }
            set
            {
                OnAppSettingChanging();
                _appSettings = value;
                OnAppSettingChanged();
            }
        }

        private void OnAppSettingChanged()
        {
            if (_appSettings != null)
            {
                _appSettings.HotKeysChanged += _appSettings_HotKeysChanged;
                _appSettings.PropertyChanged += _appSettings_PropertyChanged;
            }
#if NetCF
            RefreshPreventSleepTimer();
#endif
        }

        private void OnAppSettingChanging()
        {
            if (_appSettings != null)
            {
                _appSettings.HotKeysChanged -= _appSettings_HotKeysChanged;
            }
        }

        private void _appSettings_PropertyChanged(object sender, PropertyChangedEventArgs ea)
        {
#if NetCF
            if (ea.PropertyName == "KeepDeviceAwake")
            {
                RefreshPreventSleepTimer();
            }
#endif
        }

        #endregion appSettings

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Handled || e.KeyData == Keys.None) { return; }
            var keyStr = keyConverter.ConvertToString(e.KeyData);
            e.Handled = this.LogicController.HandleKeyPress(keyStr);

            //// HACK when the escape key is pressed on some controls
            //// the device will make a invalid key press sound if OnKeyDown is not handled
            //// we handle the key press in OnKeyUp
            //if (e.KeyCode == Keys.Escape)
            //{
            //    e.Handled = true;
            //}
        }

        #endregion Overrides

        #region Event Handlers

        protected void _addTreeMI_Click(object sender, EventArgs e)
        {
            ITreeView view = FocusedLayout as ITreeView;
            if (view == null) { return; }
            view.UserAddTree();
        }

        protected void _editCruisersMI_Click(object sender, EventArgs e)
        {
            using (FormManageCruisers view = new FormManageCruisers(AppSettings))
            {
#if NetCF
                view.ShowDialog();
#else
                view.ShowDialog(this);
#endif
            }
        }

        protected void _showHideLogColMI_Click(object sender, EventArgs e)
        {
            ITreeView view = FocusedLayout as ITreeView;
            if (view == null) { return; }
            view.ToggleLogColumn();
        }

        protected void LimitingDistance_Click(object sender, EventArgs e)
        {
            LayoutPlot view = FocusedLayout as LayoutPlot;
            if (view == null) { return; }
            view.ShowLimitingDistanceDialog();
        }

        protected void _deleteTreeBTN_Click(object sender, EventArgs e)
        {
            ITreeView layout = FocusedLayout as ITreeView;
            if (layout != null)
            {
                layout.DeleteSelectedTree();
            }
        }

        private void showHideErrorMessages_Click(object sender, EventArgs e)
        {
            ITreeView view = FocusedLayout as ITreeView;
            if (view == null) { return; }
            view.ToggleErrorColumn();
        }

        private void _settingsMI_Click(object sender, EventArgs e)
        {
            using (var view = new FormSettings())
            {
#if NetCF
                view.ShowDialog();
#else
                view.ShowDialog(this);
#endif
            }
        }

        protected void OnFocusedLayoutChangedCommon(object sender, EventArgs e)
        {
            SoundService.SignalPageChanged();

            var previousLayout = _previousLayout;
            if (previousLayout != null)
            {
                previousLayout.NotifyLeave();
            }

            var currentLayout = FocusedLayout;
            if (currentLayout != null)
            {
                currentLayout.NotifyEnter();
            }
            _previousLayout = currentLayout;
        }

        #endregion Event Handlers

        #region IDataEntryView

        private FormDataEntryLogic _presenter;

        public FormDataEntryLogic LogicController
        {
            get { return _presenter; }
            protected set
            {
                _presenter = value;
                OnLogicControllerChanged();
            }
        }

        private void OnLogicControllerChanged()
        {
            if (_presenter != null)
            {
                // Set the form title (Text) with current cutting unit and description.
                this.Text = this.LogicController.GetViewTitle();
            }
        }

        public IDataEntryPage FocusedLayout
        {
            get
            {
                return _layouts[PageContainer.SelectedIndex];
            }
        }

        public List<IDataEntryPage> Layouts
        {
            get { return _layouts; }
        }

        public void GotoTreePage()
        {
            if (this.PageContainer == null) { return; }
            //int pageIndex = this.PageContainer.TabPages.IndexOf(_treePage);
            this.GoToPageIndex(_treePageIndex);
        }

        public void GoToTallyPage()
        {
            if (this.PageContainer == null) { return; }
            //int pageIndex = PageContainer.TabPages.IndexOf(_tallyPage);
            this.GoToPageIndex(_tallyPageIndex);
        }

        public void GoToPageIndex(int i)
        {
            if (PageContainer == null) { return; }
            if (i < 0 || i > PageContainer.TabPages.Count - 1)
            { return; }

            PageContainer.SelectedIndex = i;
        }

        public void GoToPage(IDataEntryPage page)
        {
            var pageIndex = Layouts.IndexOf(page);
            if (pageIndex >= 0)
            {
                GoToPageIndex(pageIndex);
            }
        }

        #endregion IDataEntryView

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                AppSettings = null;
#if NetCF
                if (preventSleepTimer != null)
                {
                    preventSleepTimer.Dispose();
                    preventSleepTimer = null;
                }
#endif
            }
            base.Dispose(disposing);
        }
    }
}