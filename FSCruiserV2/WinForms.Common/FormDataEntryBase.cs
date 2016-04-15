using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Linq;
using FSCruiser.Core.ViewInterfaces;
using FSCruiser.Core;
using FSCruiser.Core.Models;
using FSCruiser.Core.DataEntry;
using FSCruiser.WinForms.DataEntry;
using FMSC.Sampling;

#if NetCF
using Microsoft.WindowsCE.Forms;
#endif

namespace FSCruiser.WinForms.Common
{
    /// <summary>
    /// Base class for winform Data Entry Form
    /// </summary>
    public class FormDataEntryBase : FMSC.Controls.CustomForm, IDataEntryView
    {
        private IDataEntryPage _previousLayout;
        protected List<IDataEntryPage> _layouts = new List<IDataEntryPage>();
        protected LayoutTreeBased _tallyLayout;
        //protected TabControl _pageContainer;
        protected TabPage _tallyPage;
        protected TabPage _treePage;
        protected ControlTreeDataGrid _treeView;

#if NetCF
        public InputPanel SIP { get; set; }
#endif

        protected virtual TabControl PageContainer
        {
            get { throw new NotSupportedException(); }
        }

        public IApplicationController Controller { get; protected set; }
        public FormDataEntryLogic LogicController { get; protected set; }

        public CuttingUnitVM Unit
        {
            get
            {
                return LogicController.Unit;
            }
        }


        protected FormDataEntryBase():base()
        {
            this.KeyPreview = true;
        }

        protected virtual TabControl MakePageContainer()
        {
            TabControl pc = new TabControl();
            pc.SelectedIndexChanged += new EventHandler(OnFocusedLayoutChanged);
            pc.SuspendLayout();
            pc.Dock = DockStyle.Fill;

            return pc;
        }

        protected void Initialize(IApplicationController controller
            , CuttingUnitVM unit)
        {
            this.Controller = controller;
            this.LogicController = new FormDataEntryLogic(unit, this.Controller, this);

            this.SuspendLayout();

            ////if the unit contains Tree based methods or multiple plot strata then we need a tab control
            //if ((unitMode & DataEntryMode.Tree) == DataEntryMode.Tree ||
            //    ((unitMode & DataEntryMode.Plot) == DataEntryMode.Plot && unit.Strata.Count > 1))
            //{
            //    this._pageContainer = MakePageContainer();
            //}



            //do we have any tree based strata in the unit
            if (unit.TreeStrata != null && unit.TreeStrata.Count > 0)
            {
                InitializeTreesTab();

                // if any strata are not H_PCT
                if (unit.TreeStrata.Any(
                    x => x.Method != CruiseDAL.Schema.CruiseMethods.H_PCT))
                {
                    InitializeTallyTab();
                }
            }

            InitializePlotTabs();

            this.PageContainer.ResumeLayout(false);

            // Set the form title (Text) with current cutting unit and description.
            this.Text = this.LogicController.GetViewTitle();

            this.ResumeLayout(false);
        }

        #region virtual methods
        protected void InitializeTreesTab()
        {
            this._treePage = new TabPage();
            //this._treePage.SuspendLayout();
            this._treePage.Text = "Trees";

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
                st.LoadTreeFieldNames();

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

        #endregion

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

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.Handled) { return; }


            e.Handled = this.LogicController.HandleKeyPress(e);

            //switch(e.KeyData)
            //{
            //    case Keys.Escape:
            //        {
            //            e.Handled = this.LogicController.HandleEscKey();
            //            break;
            //        }
                
            //    default:
            //        {
            //            char key = (char)e.KeyValue;
            //            e.Handled = this.LogicController.HandleHotKey(key);
            //            break;
            //        }
            //}
        }


        #endregion

        public IDataEntryPage FocusedLayout
        {
            get
            {
                if (PageContainer == null)
                {
                    if (_layouts.Count > 0)
                    {
                        return _layouts[0];
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return _layouts[PageContainer.SelectedIndex];
                }
            }
        }

        public List<IDataEntryPage> Layouts
        {
            get { return _layouts; }
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
            int pageIndex = this.PageContainer.TabPages.IndexOf(_tallyPage);
            this.GoToPageIndex(pageIndex);
        }

        public void GoToPageIndex(int i)
        {
            if (PageContainer == null) { return; }
            if (i < 0 || i > PageContainer.TabPages.Count - 1)
            {
                i = 0;
            }
            PageContainer.SelectedIndex = i;
            PageContainer.Focus();
        }

        public void TreeViewMoveLast()
        {
            if (_treeView != null)
            {
                _treeView.MoveLastTree();
            }
        }

        
        protected virtual void OnFocusedLayoutChanged(object sender, EventArgs e)
        {
            if (_previousLayout != null)
            {
                ITreeView treeView = _previousLayout as ITreeView;
                if (treeView != null)
                {
                    
                    treeView.EndEdit();
                    if (treeView.Trees != null)
                    {
                        try
                        {
                            var worker = new SaveTreesWorker(LogicController.Database, treeView.Trees);
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

            _previousLayout = this.FocusedLayout;
        }


        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDataEntryBase));
            this.SuspendLayout();
            // 
            // FormDataEntryBase
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDataEntryBase";
            this.ResumeLayout(false);

        }
    }
}
