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

#if NetCF

using Microsoft.WindowsCE.Forms;

#endif

namespace FSCruiser.WinForms.DataEntry
{
    /// <summary>
    /// Base class for winform Data Entry Form
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

        public InputPanel SIP { get; set; }

#endif

        protected virtual TabControl PageContainer
        {
            get { return _pageContainer; }
        }

        public IApplicationController Controller { get; protected set; }

        protected void InitializeCommon(IApplicationController controller
            , CuttingUnit unit)
        {
            KeyPreview = true;

            Controller = controller;
            LogicController = new FormDataEntryLogic(unit, Controller, this);

            // Set the form title (Text) with current cutting unit and description.
            this.Text = this.LogicController.GetViewTitle();

            InitializePageContainer();
        }

        #region Inialize Controlls

        //protected virtual TabControl MakePageContainer()
        //{
        //    TabControl pc = new TabControl();
        //    pc.SelectedIndexChanged += new EventHandler(OnFocusedLayoutChanged);
        //    pc.SuspendLayout();
        //    pc.Dock = DockStyle.Fill;

        //    return pc;
        //}

        protected void InitializePageContainer()
        {
            Debug.Assert(Unit != null);

            PageContainer.SuspendLayout();

            //do we have any tree based strata in the unit
            if (Unit.TreeStrata != null && Unit.TreeStrata.Count > 0)
            {
                InitializeTreesTab();

                // if any strata are not H_PCT
                if (Unit.TreeStrata.Any(
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
            _treeView = new ControlTreeDataGrid(this.Controller
                , this.LogicController
                , this.SIP)
            {
                Dock = DockStyle.Fill,
                UserCanAddTrees = true
            };
#else
            _treeView = new ControlTreeDataGrid(this.Controller, this.LogicController);
#endif
            _treeView.Dock = DockStyle.Fill;

            _treeView.UserCanAddTrees = true;

            _treePage.Controls.Add(_treeView);
            this.PageContainer.TabPages.Add(_treePage);
            this._layouts.Add(_treeView);
        }

        protected void InitializeTallyTab()
        {
            _tallyLayout = new LayoutTreeBased(this.Controller, this.LogicController);
            _tallyLayout.Dock = DockStyle.Fill;

            this._tallyPage = new TabPage();
            this._tallyPage.Text = "Tally";
            this.PageContainer.TabPages.Add(this._tallyPage);
            this._tallyPage.Controls.Add(_tallyLayout);
            this._layouts.Add(_tallyLayout);
        }

        protected void InitializePlotTabs()
        {
            foreach (PlotStratum st in Unit.PlotStrata)
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

                st.PopulatePlots(Unit.CuttingUnit_CN.GetValueOrDefault());

                TabPage page = new TabPage();
                page.Text = String.Format("{0}-{1}[{2}]", st.Code, st.Method, st.Hotkey);
                PageContainer.TabPages.Add(page);

#if NetCF
                LayoutPlot view = new LayoutPlot(this.LogicController, page, st, this.SIP);
#else
                LayoutPlot view = new LayoutPlot(this.LogicController, page, st);
#endif
                view.UserCanAddTrees = true;
                _layouts.Add(view);

                int pageIndex = PageContainer.TabPages.IndexOf(page);
                this.LogicController.AddStratumHotKey(st.Hotkey, pageIndex);
            }
        }

        #endregion Inialize Controlls

        #region Overrides

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.LogicController != null)
            {
                this.LogicController.HandleViewLoading();
            }
            this.OnFocusedLayoutChanged(null, null);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            this.LogicController.HandleViewClosing(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Handled) { return; }

            // HACK when the escape key is pressed on some controls
            // the device will make a invalid key press sound if OnKeyDown is not handled
            // we handle the key press in OnKeyUp
            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
            }
        }

        protected void OnKeyUpInternal(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.Handled) { return; }

            e.Handled = this.LogicController.HandleKeyPress(e);
        }

        #endregion Overrides

        #region Event Handlers

        private void _addTreeBTN_Clicked(object sender, EventArgs e)
        {
            this.LogicController.HandleAddTreeClick();
        }

        private void _deleteTreeBTN_Click(object sender, EventArgs e)
        {
            this.LogicController.HandleDeleteRowButtonClick();
        }

        protected void OnFocusedLayoutChangedInternal(object sender, EventArgs e)
        {
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
                            var worker = new SaveTreesWorker(LogicController.Database, oldTreeView.Trees);
                            worker.SaveAll();
                            //this.Controller.SaveTrees(((ITreeView)_previousLayout).Trees);
                        }
                        catch (Exception ex)
                        {
                            this.Controller.HandleNonCriticalException(ex, "Unable to compleate last tree save");
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

        public FormDataEntryLogic LogicController { get; protected set; }

        public CuttingUnit Unit
        {
            get
            {
                return LogicController.Unit;
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

        public bool AskEnterMeasureTreeData()
        {
            return this.AskYesNo("Would you like to enter tree data now?", "Sample", MessageBoxIcon.Question, false);
        }

        public void HandleCuttingUnitDataLoaded()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(this.HandleCuttingUnitDataLoaded));
            }
            else
            {
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

                // Turn off the waitcursor that was turned on in FormMain.button1_Click()
                Cursor.Current = Cursors.Default;
            }
        }

        public void HandleEnableLogGradingChanged()
        {
            foreach (IDataEntryPage c in this._layouts)
            {
                ITreeView tv = c as ITreeView;
                if (tv != null)
                {
                    tv.HandleEnableLogGradingChanged();
                }
            }
        }

        public void HandleCruisersChanged()
        {
            foreach (IDataEntryPage c in this._layouts)
            {
                ITreeView tv = c as ITreeView;
                if (tv != null)
                {
                    tv.HandleCruisersChanged();
                }
            }
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

        public void TreeViewMoveLast()
        {
            if (_treeView != null)
            {
                _treeView.MoveLastTree();
            }
        }

        #endregion IDataEntryView
    }
}