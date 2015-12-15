using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

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

        protected IList<StratumVM> _plotStrataInfo;


        public IApplicationController Controller { get; protected set; }
        public FormDataEntryLogic LogicController { get; protected set; }

        public IList<StratumVM> PlotStrata
        {
            get
            {
                return _plotStrataInfo;
            }
        }


        protected FormDataEntryBase():base() 
        {
        }

        //public FormDataEntryBase(IApplicationController controller, CruiseDAL.DataObjects.CuttingUnitDO unit): this()
        //{
        //    this.Controller = controller;
        //    this._logicController = new FormDataEntryLogic(this.Controller, this);
        //    //this.Unit = unit;
        //    //DataEntryMode unitMode = Controller.GetUnitDataEntryMode(unit);
        //    //this.SuspendLayout();
        //}

        protected virtual TabControl MakePageContainer()
        {
            TabControl pc = new TabControl();
            pc.SelectedIndexChanged += new EventHandler(OnFocusedLayoutChanged);
            pc.SuspendLayout();
            pc.Dock = DockStyle.Fill;

            return pc;
        }


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
            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
            }
            //if (this._pageContainer != null)
            //{
            //    if (e.KeyCode == Keys.Escape &&
            //    this._pageContainer.SelectedIndex == this._pageContainer.TabPages.IndexOf(_treePage))
            //    {
            //        this.GoToTallyPage();
            //        e.Handled = true;
            //        return;
            //    }
            //    else
            //    {
            //        e.Handled = _logicController.HandleHotKey(key);
            //    }
            //}
            //else
            //{
            //    ITallyView view = this.FocusedLayout as ITallyView;
            //    if (view == null) { return; }
            //    view.HandleKeyDown(key);
            //}           
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

            //if (this._pageContainer != null)
            //{
            //    if (e.KeyCode == Keys.Escape &&
            //    this._pageContainer.SelectedIndex == this._pageContainer.TabPages.IndexOf(_treePage))
            //    {
            //        this.GoToTallyPage();
            //        e.Handled = true;
            //        return;
            //    }
            //    else
            //    {
            //        e.Handled = this.LogicController.HandleHotKey(key);
            //    }
            //}
            //else
            //{
            //    ITallyView view = this.FocusedLayout as ITallyView;
            //    if (view == null) { return; }
            //    view.HandleHotKeyFirst(key);
            //}
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


        protected void InternalHandleCuttingUnitDataLoaded()
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

        public void HandleCuttingUnitDataLoaded()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new HandleCruiseDataLoadedEventHandler(this.InternalHandleCuttingUnitDataLoaded));
            }
            else
            {
                this.InternalHandleCuttingUnitDataLoaded();
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
                if (_previousLayout is ITreeView)
                {
                    ((ITreeView)_previousLayout).EndEdit();
                    try
                    {
                        var worker = new SaveTreesWorker(LogicController.Database, ((ITreeView)_previousLayout).Trees);
                        worker.SaveAll();
                        //this.Controller.SaveTrees(((ITreeView)_previousLayout).Trees);
                    }
                    catch (Exception ex)
                    {
                        this.Controller.HandleNonCriticalException(ex, "Unable to compleate last tree save");
                    }
                }
                if (_previousLayout is ITallyView)
                {
                    try
                    {
                        this.LogicController.SaveCounts();
                    }
                    catch (Exception ex)
                    {
                        this.Controller.HandleNonCriticalException(ex, "counts didn't save");
                    }
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
