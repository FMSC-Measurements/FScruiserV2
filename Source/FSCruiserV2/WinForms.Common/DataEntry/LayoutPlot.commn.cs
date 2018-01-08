using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FScruiser.Core.Services;
using FSCruiser.Core;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class LayoutPlot
    {
        #region ViewLogicController

        LayoutPlotLogic _viewLogicController;

        public LayoutPlotLogic ViewLogicController
        {
            get
            {
                return _viewLogicController;
            }
            set
            {
                _viewLogicController = value;
                OnViewLogicControllerChanged();
            }
        }

        private void OnViewLogicControllerChanged()
        {
            var viewModel = ViewLogicController;
            if (viewModel != null)
            {
                UpdatePageText(viewModel);
            }
        }

        #endregion ViewLogicController

        public PlotStratum Stratum
        {
            get { return ViewLogicController.Stratum; }
        }

        public ICollection<Tree> Trees
        {
            get
            {
                var curPlot = this.ViewLogicController.CurrentPlot;
                if (curPlot != null)
                {
                    return curPlot.Trees;
                }
                return null;
            }
        }

        void UpdatePageText(LayoutPlotLogic viewModel)
        {
            if (viewModel == null) { return; }
            var st = viewModel.Stratum;
            var pageText = String.Format("{3}{0}-{1}[{2}]",
                st.Code,
                st.Method,
                st.Hotkey,
                (HasBadSaveState) ? "!" : "");
            Text = pageText;
        }

        #region DataService

        IPlotDataService _dataService;

        IPlotDataService DataService
        {
            get { return _dataService; }
            set
            {
                OnDataServiceChanging();
                _dataService = value;
                OnDataServiceChanged();
            }
        }

        private void OnDataServiceChanged()
        {
            if (_dataService != null)
            {
            }
        }

        private void OnDataServiceChanging()
        {
            if (_dataService != null)
            {
            }
        }

        void HandleEnableLogGradingChanged(object sender, EventArgs e)
        {
            if (this._logsColumn != null)
            {
                LogColumnVisable = DataService.EnableLogGrading;
            }
        }

        #endregion DataService

        #region AppSettings

        ApplicationSettings _appSettings;

        public ApplicationSettings AppSettings
        {
            get { return _appSettings; }
            set
            {
                OnAppSettingsChanging();
                _appSettings = value;
                OnAppSettingsChanged();
            }
        }

        private void OnAppSettingsChanged()
        {
            if (_appSettings != null)
            {
                _appSettings.CruisersChanged += Settings_CruisersChanged;
            }
        }

        private void OnAppSettingsChanging()
        {
            if (_appSettings != null)
            {
                _appSettings.CruisersChanged -= Settings_CruisersChanged;
            }
        }

        void Settings_CruisersChanged(object sender, EventArgs e)
        {
            if (this._initialsColoumn != null)
            {
                _initialsColoumn.DataSource = AppSettings.Cruisers
                    .OrEmpty().Select(x => x.Initials)
                    .Union(Trees.OrEmpty().Select(x => x.Initials).Distinct())
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToArray();
            }
        }

        #endregion AppSettings

        #region event handlers

        void openFixCNTTallyButton_Click(object sender, EventArgs e)
        {
            var stratum = Stratum as FixCNTStratum;
            if (stratum == null) { return; }
            var currentPlot = ViewLogicController.CurrentPlot as FixCNTPlot;
            if (currentPlot == null) { ShowNoPlotSelectedMessage(); return; }
            using (var view = new FSCruiser.WinForms.Common.FixCNTForm(stratum, DataService))
            {
                view.ShowDialog(currentPlot);
            }
        }

        void sgRow_SpeciesClicked(object sender, SubPop sp)
        {
            if (!this.ViewLogicController.EnsureCurrentPlotWorkable()) { return; }

            this.ViewLogicController.AddTree(sp);
        }

        private void tallyRow_TallyButtonClicked(object sender, EventArgs e)
        {
            if (!this.ViewLogicController.EnsureCurrentPlotWorkable()) { return; }

            var row = (ITallyButton)sender;
            var count = row.Count;
            OnTally(count);
        }

        private void tallyRow_InfoButtonClicked(object sender, EventArgs e)
        {
            ITallyButton row = (ITallyButton)sender;
            var count = row.Count;
            ShowTallySettings(count);
            //row.DiscriptionLabel.Text = count.Tally.Description;
        }

        #endregion event handlers

        #region IDataEntryPage

        private bool _viewLoading = true;
        public bool ViewLoading { get { return _viewLoading; } }

        public bool PreviewKeypress(string keyStr)
        {
            if (_viewLoading) { return false; }
            if (string.IsNullOrEmpty(keyStr)) { return false; }

            var settings = AppSettings;

            if (keyStr == settings.AddPlotKeyStr)
            {
                this._addPlotButton_Click(null, null);
                return true;
            }
            else if (keyStr == settings.AddTreeKeyStr)
            {
                return ViewLogicController.UserAddTree() != null;
            }
            else if (keyStr == settings.ResequencePlotTreesKeyStr)
            {
                return ViewLogicController.ResequenceTreeNumbers();
            }
#if NetCF
            else if (keyStr.StartsWith("ESC", StringComparison.InvariantCultureIgnoreCase))
            {
                IsGridExpanded = !IsGridExpanded;
                return true;
            }
#endif
            else { return false; }
        }

        public void NotifyEnter()
        {
#if NetCF
            MoveLastTree();
            if (IsGridExpanded)
            {
                _dataGrid.Edit();
            }
#endif
        }

        public void HandleLoad()
        {
            _viewLoading = false;
            this.ViewLogicController.HandleViewLoad();
        }

        #endregion IDataEntryPage

        #region ITallyView Members

        public Dictionary<char, CountTree> HotKeyLookup
        {
            get
            {
                return this.ViewLogicController.Stratum.HotKeyLookup;
            }
        }

        void ShowTallySettings(CountTree count)
        {
            this.ViewLogicController.SavePlotTrees();
            try
            {
                count.Save();
                var countDataService = new CountTreeDataService(DataService.DataStore, count);
                using (FormTallySettings view = new FormTallySettings(countDataService))
                {
#if !NetCF
                    view.ShowDialog(this);
#else
                    view.ShowDialog();
#endif
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return;
            }
        }

        public void OnTally(CountTree count)
        {
            this.ViewLogicController.OnTally(count);
        }

        public Exception TrySaveCounts()
        {
            return ViewLogicController.TrySaveCounts();
        }

        #endregion ITallyView Members

        #region ITreeView Members

        bool _hasBadSaveState;

        public bool HasBadSaveState
        {
            get { return _hasBadSaveState; }
            set
            {
                _hasBadSaveState = value;
                OnHasBadSaveStateChanged();
            }
        }

        private void OnHasBadSaveStateChanged()
        {
            UpdatePageText(ViewLogicController);
        }

        public bool UserCanAddTrees
        {
            get { return this.ViewLogicController.UserCanAddTrees; }
            set { this.ViewLogicController.UserCanAddTrees = value; }
        }

        public void DeleteSelectedTree()
        {
            this.ViewLogicController.HandleDeleteCurrentTree();
        }

        public void EndEdit()
        {
            this.ViewLogicController.EndEdit();
        }

        public void MoveLastTree()
        {
            this.ViewLogicController.SelectLastTree();
        }

        public Tree UserAddTree()
        {
            return this.ViewLogicController.UserAddTree();
        }

        public void ShowLogs(Tree tree)
        {
            if (tree.TrySave())
            {
                var dataService = new ILogDataService(tree, DataService.Region, DataService.DataStore);
                using (var view = new FormLogs(dataService))
                {
#if !NetCF
                    view.StartPosition = FormStartPosition.CenterParent;
                    view.ShowDialog(this);
#else
                    view.ShowDialog();
#endif
                }
            }
            else
            {
                DialogService.ShowMessage("Unable to save tree. Ensure Tree Number, Sample Group and Stratum are valid"
                    , null);
            }
        }

        #endregion ITreeView Members

        #region IPlotLayout Members

        public bool AskContinueOnCurrnetPlotTreeError()
        {
            return DialogService.AskYesNo("Error(s) found on tree records in current plot, Would you like to continue?", "Continue?", true);
        }

        public void ShowNoPlotSelectedMessage()
        {
            DialogService.ShowMessage("No Plot Selected");
        }

        public void ShowNullPlotMessage()
        {
            DialogService.ShowMessage("Can't perform action on null plot");
        }

        public void ShowLimitingDistanceDialog()
        {
            var plot = ViewLogicController.CurrentPlot;
            if (plot == null)
            {
                ShowNoPlotSelectedMessage();
                return;
            }

            using (var view = new FormLimitingDistance() { Plot = plot })
            {
#if !NetCF
                var result = view.ShowDialog(this);
#else
                var result = view.ShowDialog();
#endif

                if (result == DialogResult.OK)
                {
                    plot.Remarks += view.Report;
                }
            }
        }

        public void ShowCurrentPlotInfo()
        {
            var currentPlot = ViewLogicController.CurrentPlot;
            if (currentPlot == null)
            {
                ShowNoPlotSelectedMessage();
                return;
            }

            if (ShowPlotInfo(DataService, currentPlot, Stratum, false))
            {
                currentPlot.Save();
                ViewLogicController.UpdateCurrentPlot();
            }
        }

        public bool ShowPlotInfo(IPlotDataService dataService, Plot plot, PlotStratum stratum, bool isNewPlot)
        {
            System.Diagnostics.Debug.Assert(plot != null);
            System.Diagnostics.Debug.Assert(stratum != null);

            if (stratum.Is3PPNT && isNewPlot)
            {
                using (var view = new Form3PPNTPlotInfo(dataService))
                {
#if !NetCF
                    view.Owner = this.TopLevelControl as Form;
                    view.StartPosition = FormStartPosition.CenterParent;
#endif
                    return view.ShowDialog(plot, stratum, isNewPlot) == DialogResult.OK;
                }
            }
            else
            {
                using (var view = new FormPlotInfo())
                {
#if !NetCF
                    view.Owner = this.TopLevelControl as Form;
                    view.StartPosition = FormStartPosition.CenterParent;
#endif
                    return view.ShowDialog(plot, stratum, isNewPlot) == DialogResult.OK;
                }
            }
        }

        #endregion IPlotLayout Members
    }
}