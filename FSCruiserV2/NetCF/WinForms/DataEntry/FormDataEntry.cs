using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FMSC.Sampling;
using FMSC.Controls;
using Microsoft.WindowsCE.Forms;
using System.ComponentModel;
using FSCruiser.WinForms.Common;
using FSCruiser.Core.ViewInterfaces;
using FSCruiser.Core;
using FSCruiser.Core.Models;
using FSCruiser.Core.DataEntry;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class FormDataEntry : FormDataEntryBase, IDataEntryView
    {

        protected override TabControl PageContainer
        {
            get { return this._pageContainer; }
        }


        protected FormDataEntry():base()
        {
            //this.KeyPreview = true;// set in CustomForm class
            //this.HotKeyLookup = new Dictionary<char, CountTreeDO>();
            //this.StratumHotKeyLookup = new Dictionary<char, int>();

            InitializeComponent();

            //this.Controls.Add(this._pageContainer);

            if(ViewController.PlatformType == PlatformType.WM)
            {
                this.SIP = new Microsoft.WindowsCE.Forms.InputPanel();
                this.components.Add(SIP);
            }
            else if (ViewController.PlatformType == PlatformType.WinCE)
            {

                this.WindowState = FormWindowState.Maximized;
            }
        }

        public FormDataEntry(IApplicationController controller
            , CuttingUnitVM unit) :this()
        {
            base.Initialize(controller, unit);

        }

        #region override methods
        //protected override void InitializeTreesTab()
        //{
        //    this._treePage = new TabPage();
        //    //this._treePage.SuspendLayout();
        //    this._treePage.Text = "Trees";

        //    _treeView = new ControlTreeDataGrid(this.Controller
        //        , this.LogicController
        //        , this.SIP)
        //    {
        //        Dock = DockStyle.Fill,
        //        UserCanAddTrees = true
        //    };



        //    _treePage.Controls.Add(_treeView);
        //    this._pageContainer.TabPages.Add(_treePage);
        //    this._layouts.Add(_treeView);
        //}

        //protected override void InitializeTallyTab()
        //{
        //    _tallyLayout = new LayoutTreeBased(this.Controller, this.LogicController);
        //    _tallyLayout.Dock = DockStyle.Fill;

        //    this._tallyPage = new TabPage();
        //    this._tallyPage.Text = "Tally";
        //    this._pageContainer.TabPages.Add(this._tallyPage);
        //    this._tallyPage.Controls.Add(_tallyLayout);
        //    this._layouts.Add(_tallyLayout);
        //}

        //protected override void InitializePlotTabs()
        //{
        //    foreach (PlotStratum st in Unit.PlotStrata)
        //    {
        //        if (st.Method == "3PPNT")
        //        {
        //            if (st.KZ3PPNT <= 0)
        //            {
        //                MessageBox.Show("error 3PPNT missing KZ value, please return to Cruise System Manger and fix");
        //                continue;
        //            }
        //            st.SampleSelecter = new ThreePSelecter((int)st.KZ3PPNT, 1000000, 0);
        //        }
        //        st.LoadTreeFieldNames();

        //        st.PopulatePlots(Unit.CuttingUnit_CN.GetValueOrDefault());

        //        if (_pageContainer != null)
        //        {
        //            TabPage page = new TabPage();
        //            page.Text = String.Format("{0}-{1}[{2}]", st.Code, st.Method, st.Hotkey);
        //            _pageContainer.TabPages.Add(page);

        //            LayoutPlot view = new LayoutPlot(this.LogicController, page, st, this.SIP);
        //            view.UserCanAddTrees = true;
        //            _layouts.Add(view);

        //            int pageIndex = _pageContainer.TabPages.IndexOf(page);
        //            this.LogicController.AddStratumHotKey(st.Hotkey, pageIndex);
        //        }
        //        else
        //        {
        //            LayoutPlot view = new LayoutPlot(this.LogicController, this, st, this.SIP);
        //            view.UserCanAddTrees = true;
        //            _layouts.Add(view);
        //            this.Controls.Add(view);
        //        }
        //    }

        //}

        protected override void OnFocusedLayoutChanged(object sender, EventArgs e)
        {
            base.OnFocusedLayoutChanged(sender, e);
            ITreeView view = this.FocusedLayout as ITreeView;
            this._addTreeMI.Enabled = view != null && view.UserCanAddTrees;
        }

        #endregion

        private void _deleteRowButton_Click(object sender, EventArgs e)
        {
            this.LogicController.HandleDeleteRowButtonClick();

        }

        private void showHideErrorMessages_Click(object sender, EventArgs e)
        {
            this.LogicController.HandleShowHideErrorCol();
        }

        private void LimitingDistance_Click(object sender, EventArgs e)
        {
            this.LogicController.HandleDisplayLimitingDistance();
        }

        private void _editCruisersMI_Click(object sender, EventArgs e)
        {
            Controller.ViewController.ShowManageCruisers();
        }

        private void menuItem1_Popup(object sender, EventArgs e)
        {
            this._limitingDistanceMI.Enabled = this.FocusedLayout is LayoutPlot;
            this._showHideErrorColMI.Enabled = this.FocusedLayout is ITreeView;
            this._deleteRowButton.Enabled = this.FocusedLayout is ITreeView;
            this._showHideLogColMI.Enabled = this.FocusedLayout is ITreeView;
        }

        private void _showHideLogColMI_Click(object sender, EventArgs e)
        {
            this.LogicController.HandleShowHideLogCol();
        }

        private void _addTreeMI_Click(object sender, EventArgs e)
        {
            this.LogicController.HandleAddTreeClick();
        }

        


    }
}