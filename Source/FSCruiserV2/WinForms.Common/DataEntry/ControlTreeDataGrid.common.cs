using FScruiser.Core.Services;
using FSCruiser.Core;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class ControlTreeDataGrid
    {
        bool _viewLoading = true;

        #region Properties

        public FormDataEntryLogic DataEntryController { get; set; }

        public bool ViewLoading { get { return _viewLoading; } }

        public ICollection<Tree> Trees
        {
            get
            {
                return DataService.NonPlotTrees;
            }
        }

        #region DataService

        IDataEntryDataService _dataService;

        IDataEntryDataService DataService
        {
            get { return _dataService; }
            set
            {
                OnDataServiceChanging();
                _dataService = value;
                OnDataServiceChanged();
            }
        }

        void OnDataServiceChanged()
        {
            var dataService = DataService;
            if (dataService != null)
            {
                dataService.EnableLogGradingChanged += HandleEnableLogGradingChanged;

                _BS_trees.DataSource = dataService.NonPlotTrees;
            }
        }

        void OnDataServiceChanging()
        {
            var dataService = DataService;
            if (dataService != null)
            {
                dataService.EnableLogGradingChanged -= HandleEnableLogGradingChanged;
            }
        }

        void HandleEnableLogGradingChanged(object sender, EventArgs e)
        {
            var logGradingEnabled = DataService.EnableLogGrading;
            LogColumnVisable = logGradingEnabled;

#if !NetCF
            _logToolStripMenuItem.Text = logGradingEnabled ?
                "Disable Log Grading" : "Enable Log Grading";
#endif
        }

        #endregion DataService

        #region AppSettings

        private ApplicationSettings _appSettings;

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

        private void OnAppSettingsChanging()
        {
            if (_appSettings != null)
            {
                _appSettings.CruisersChanged -= Settings_CruisersChanged;
            }
        }

        private void OnAppSettingsChanged()
        {
            if (_appSettings != null)
            {
                _appSettings.CruisersChanged += Settings_CruisersChanged;
            }
        }

        void Settings_CruisersChanged(object sender, EventArgs e)
        {
            if (_initialsColoumn != null)
            {
                _initialsColoumn.DataSource = AppSettings.Cruisers.ToArray();
            }
        }

        #endregion AppSettings

        #endregion Properties

        #region IDataEntryPage

        public void HandleLoad()
        {
            _viewLoading = false;
        }

        public void NotifyEnter()
        {
#if NetCF
            MoveLastTree();
            MoveHomeField();
            Edit();
#endif
        }

        public bool PreviewKeypress(string keyStr)
        {
            if (string.IsNullOrEmpty(keyStr)) { return false; }

            var settings = AppSettings;
            if (settings == null) { return false; }

            if (keyStr == settings.JumpTreeTallyKeyStr)
            {
                DataEntryController.View.GoToTallyPage();
                return true;
            }
            else if (keyStr == settings.AddTreeKeyStr)
            {
                return UserAddTree() != null;
            }
            else
            {
                return false;
            }
        }

        #endregion IDataEntryPage

        #region ITreeView Members

        bool _hasBadSaveState;

        public bool HasBadSaveState
        {
            get { return _hasBadSaveState; }
            set
            {
                _hasBadSaveState = value;
                UpdatePageText();
            }
        }

        void UpdatePageText()
        {
            this.Text = ((HasBadSaveState) ? "!" : "") + "Trees";
        }

        bool _userCanAddTrees;

        public bool UserCanAddTrees
        {
            get
            {
                return _userCanAddTrees;
                //return this.AllowUserToAddRows;
            }
            set
            {
                _userCanAddTrees = value;
                //this.AllowUserToAddRows = value;
            }
        }

        public void DeleteSelectedTree()
        {
            var curTree = _BS_trees.Current as Tree;
            if (curTree == null)
            {
                MessageBox.Show("No Tree Selected");
            }
            else
            {
                if (DialogResult.Yes == MessageBox.Show("Delete Tree #" + curTree.TreeNumber.ToString() + "?",
                    "Delete Tree?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2))
                {
                    DataService.DeleteTree(curTree);
                }
            }
        }

        public new void EndEdit()
        {
            base.EndEdit();
        }

        public void MoveHomeField()
        {
            MoveFirstEmptyCell();
        }

        public void MoveLastTree()
        {
            this._BS_trees.MoveLast();
#if NetCF
            this.MoveFirstEmptyCell();
#endif
        }

        public Tree UserAddTree()
        {
            if (_viewLoading
                || !UserCanAddTrees) { return null; }

            EndEdit();

            var newTree = DataService.UserAddTree();
            if (newTree != null)
            {
                MoveLastTree();
                MoveFirstEmptyCell();
            }
            return newTree;
        }

        public void ShowLogs(Tree tree)
        {
            if (tree.TrySave())
            {
                var dataService = DataService.MakeLogDataService(tree);
                using (var view = new FormLogs(dataService))
                {
                    view.ShowDialog();
                }
            }
            else
            {
                DialogService.ShowMessage("Unable to save tree. Ensure Tree Number, Sample Group and Stratum are valid"
                    , null);
            }
        }

        #endregion ITreeView Members
    }
}