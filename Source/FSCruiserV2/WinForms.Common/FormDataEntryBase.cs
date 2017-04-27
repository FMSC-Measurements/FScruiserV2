using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using FMSC.Sampling;
using FSCruiser.Core;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;
using FSCruiser.WinForms.DataEntry;
using FScruiser.Core.Services;

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
        protected List<IDataEntryPage> _layouts = new List<IDataEntryPage>();

        protected LayoutTreeBased _tallyLayout;
        protected TabPage _tallyPage;
        protected TabPage _treePage;
        protected ControlTreeDataGrid _treeView;

#if NetCF
        System.Threading.Timer preventSleepTimer;
        public InputPanel SIP { get; set; }

#endif

        protected IDataEntryDataService DataService { get; set; }

        protected virtual TabControl PageContainer
        {
            get { return _pageContainer; }
        }

        public IApplicationController Controller { get; protected set; }

        #region Initialize Controls

        protected void InitializeCommon(IApplicationController controller
            , ApplicationSettings appSettings
            , IDataEntryDataService dataService)
        {
            KeyPreview = true;

            Controller = controller;
            DataService = dataService;
            AppSettings = appSettings;

            LogicController = new FormDataEntryLogic(Controller
                , DialogService.Instance
                , SoundService.Instance
                , DataService
                , AppSettings
                , this);

            InitializePageContainer();
        }

        protected void InitializePageContainer()
        {
            Debug.Assert(DataService.CuttingUnit != null);

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
            _treePage = new TabPage();
            //this._treePage.SuspendLayout();
            _treePage.Text = "Trees";

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
            _treeView.Dock = DockStyle.Fill;

            _treeView.UserCanAddTrees = true;

            _treePage.Controls.Add(_treeView);
            this.PageContainer.TabPages.Add(_treePage);
            this._layouts.Add(_treeView);
        }

        protected void InitializeTallyTab()
        {
            _tallyLayout = new LayoutTreeBased(DataService
                , AppSettings
                , LogicController);

            _tallyLayout.Dock = DockStyle.Fill;

            this._tallyPage = new TabPage();
            this._tallyPage.Text = "Tally";
            this.PageContainer.TabPages.Add(this._tallyPage);
            this._tallyPage.Controls.Add(_tallyLayout);
            this._layouts.Add(_tallyLayout);
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
                    st.SampleSelecter = new ThreePSelecter((int)st.KZ3PPNT, 1000000, 0);
                }

                st.PopulatePlots(DataService.CuttingUnit.CuttingUnit_CN.GetValueOrDefault());

                TabPage page = new TabPage();
                page.Text = String.Format("{0}-{1}[{2}]", st.Code, st.Method, st.Hotkey);
                PageContainer.TabPages.Add(page);

                LayoutPlot view = new LayoutPlot(DataService
                    , AppSettings
                    , SoundService.Instance
                    , Controller.ViewController
                    , st);
                view.Parent = page;
#if NetCF
                view.Sip = SIP;
#endif

                view.UserCanAddTrees = true;
                _layouts.Add(view);

                int pageIndex = PageContainer.TabPages.IndexOf(page);
                this.LogicController.RegisterStratumHotKey(st.Hotkey, pageIndex);
            }
        }

        #endregion Initialize Controls

        #region Overrides

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            foreach (IDataEntryPage c in this._layouts)
            {
                ITreeView tv = c as ITreeView;
                if (tv != null)
                {
                    tv.HandleLoad();
                }
            }

            if (this._tallyPage != null)
            {
                this._tallyLayout.HandleLoad();
            }

            // Turn off the wait cursor that was turned on in when displaying main form
            Cursor.Current = Cursors.Default;
            this.OnFocusedLayoutChanged(null, null);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            this.LogicController.HandleViewClosing(e);
        }

        KeysConverter keyConverter = new KeysConverter();

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
            }
        }

        private void OnAppSettingChanging()
        {
            if (_appSettings != null)
            {
                _appSettings.HotKeysChanged -= _appSettings_HotKeysChanged;
            }
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

        protected void OnFocusedLayoutChangedInternal(object sender, EventArgs e)
        {
            SoundService.SignalPageChanged();
            if (_previousLayout != null)
            {
                //note: a view can be a tree view and a tally view,
                //so we'll handle both conditions

                var oldTreeView = _previousLayout as ITreeView;
                if (oldTreeView != null)
                {
                    oldTreeView.EndEdit();
                    if (oldTreeView.Trees != null)
                    {
                        try
                        {
                            var worker = new SaveTreesWorker(DataService.DataStore, oldTreeView.Trees);
                            worker.SaveAll();
                            //this.Controller.SaveTrees(((ITreeView)_previousLayout).Trees);
                        }
                        catch (Exception ex)
                        {
                            this.Controller.HandleNonCriticalException(ex, "Unable to complete last tree save");
                        }
                    }
                }

                var tallyView = _previousLayout as ITallyView;
                if (tallyView != null)
                {
                    tallyView.TrySaveCounts();
                }
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

        FormDataEntryLogic _presenter;

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
            int pageIndex = this.PageContainer.TabPages.IndexOf(_treePage);
            this.GoToPageIndex(pageIndex);
        }

        public void GoToTallyPage()
        {
            if (this.PageContainer == null) { return; }
            int pageIndex = PageContainer.TabPages.IndexOf(_tallyPage);
            this.GoToPageIndex(pageIndex);
        }

        public void GoToPageIndex(int i)
        {
            if (PageContainer == null) { return; }
            if (i < 0 || i > PageContainer.TabPages.Count - 1)
            { return; }

            PageContainer.SelectedIndex = i;
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