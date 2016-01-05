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
        protected TabControl _pageContainer;
        protected TabPage _tallyPage;
        protected TabPage _treePage;
        protected ControlTreeDataGrid _treeView;


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
                    x => x.Method != CruiseDAL.Schema.Constants.CruiseMethods.H_PCT))
                {
                    InitializeTallyTab();
                }
            }

            InitializePlotTabs();

            this._pageContainer.ResumeLayout(false);

            // Set the form title (Text) with current cutting unit and description.
            this.Text = this.LogicController.GetViewTitle();

            this.ResumeLayout(false);
        }

        #region virtual methods
        protected virtual void InitializeTreesTab()
        { }
        

        protected virtual void InitializeTallyTab()
        { }

        protected virtual void InitializePlotTabs()
        { }

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

            char key = (char)e.KeyValue;

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

            switch(e.KeyData)
            {
                case Keys.Escape:
                    {
                        e.Handled = this.LogicController.HandleEscKey();
                        break;
                    }
                default:
                    {
                        char key = (char)e.KeyValue;
                        e.Handled = this.LogicController.HandleHotKey(key);
                        break;
                    }
            }
        }


        #endregion

        public IDataEntryPage FocusedLayout
        {
            get
            {
                if (_pageContainer == null)
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
                    return _layouts[_pageContainer.SelectedIndex];
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
            if (this._pageContainer == null) { return; }
            int pageIndex = this._pageContainer.TabPages.IndexOf(_treePage);
            this.GoToPageIndex(pageIndex);
        }

        public void GoToTallyPage()
        {
            if (this._pageContainer == null) { return; }
            int pageIndex = this._pageContainer.TabPages.IndexOf(_tallyPage);
            this.GoToPageIndex(pageIndex);
        }

        public void GoToPageIndex(int i)
        {
            if (_pageContainer == null) { return; }
            if (i < 0 || i > _pageContainer.TabPages.Count - 1)
            {
                i = 0;
            }
            _pageContainer.SelectedIndex = i;
            _pageContainer.Focus();
        }

        public void TreeViewMoveLast()
        {
            if (_treeView != null)
            {
                _treeView.MoveLast();
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
