﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FSCruiserV2.Logic;
using CruiseDAL.DataObjects;

namespace FSCruiserV2.Forms
{
    public partial class FormDataEntry : FormDataEntryBase, IDataEntryView 
    {

        public FormDataEntry()
        {
            InitializeComponent();
        }

        public FormDataEntry(IApplicationController controller, CuttingUnitDO unit)
        {
            base.Controller = controller;
            this.InitializeComponent();
            base.LogicController = new FormDataEntryLogic(this.Controller, this);

            this.InitLayout(unit);
        }

        protected override void OnFocusedLayoutChanged(object sender, EventArgs e)
        {
            base.OnFocusedLayoutChanged(sender, e);
            ITreeView view = base.FocusedLayout as ITreeView;
            this._addTreeBTN.Enabled = (view != null && view.UserCanAddTrees);
            this._deleteTreeBTN.Enabled = (view != null);
        }

        private void _addTreeBTN_Clicked(object sender, EventArgs e)
        {
            this.LogicController.HandleAddTreeClick();
        }

        protected void InitLayout(CuttingUnitDO unit)
        {
            DataEntryMode unitMode = Controller.GetUnitDataEntryMode(unit);
            this.SuspendLayout();

            _layouts = new List<IDataEntryPage>();

            //if the unit contains Tree based methods or multiple plot strata then we need a tab control
            if ((unitMode & DataEntryMode.Tree) == DataEntryMode.Tree ||
                ((unitMode & DataEntryMode.Plot) == DataEntryMode.Plot && unit.Strata.Count > 1))
            {
                this._pageContainer = MakePageContainer();
            }

            //do we have any tree based strata in the unit
            if ((unitMode & DataEntryMode.Tree) == DataEntryMode.Tree)
            {
                this._treePage = new TabPage();
                //this._treePage.SuspendLayout();
                this._treePage.Text = "Trees";

                _treeView = new ControlTreeDataGrid(this.Controller, this.LogicController);
                _treeView.Dock = DockStyle.Fill;
                _treeView.AllowUserToAddRows = true;

                _treePage.Controls.Add(_treeView);
                this._pageContainer.TabPages.Add(_treePage);
                this._layouts.Add(_treeView);

                //List<StratumInfo> strata = Controller.GetUnitTreeBasedStrata();
                if ((unitMode & DataEntryMode.TallyTree) == DataEntryMode.TallyTree)
                {
                    _tallyLayout = new LayoutTreeBased(this.Controller, this.LogicController);
                    _tallyLayout.Dock = DockStyle.Fill;

                    this._tallyPage = new TabPage();
                    this._tallyPage.Text = "Tally";
                    this._pageContainer.TabPages.Add(this._tallyPage);
                    this._tallyPage.Controls.Add(_tallyLayout);
                    this._layouts.Add(_tallyLayout);
                }
            }

            if ((unitMode & DataEntryMode.Plot) == DataEntryMode.Plot)
            {
                _plotStrataInfo = Controller.GetUnitPlotStrata();
                foreach (StratumVM st in _plotStrataInfo)
                {
                    st.Plots = this.Controller._cDal.Read<PlotVM>("Plot", "WHERE Stratum_CN = ? AND CuttingUnit_CN = ? ORDER BY PlotNumber", st.Stratum_CN, Controller.CurrentUnit.CuttingUnit_CN);

                    if (_pageContainer != null)
                    {
                        TabPage page = new TabPage();
                        page.Text = String.Format("{0}-{1}[{2}]", st.Code, st.Method, st.Hotkey);
                        _pageContainer.TabPages.Add(page);

                        LayoutPlot view = new LayoutPlot(this.LogicController, page, st);
                        view.UserCanAddTrees = true;
                        _layouts.Add(view);

                        int pageIndex = _pageContainer.TabPages.IndexOf(page);
                        this.LogicController.AddStratumHotKey(st.Hotkey, pageIndex);

                    }
                    else
                    {
                        LayoutPlot view = new LayoutPlot(this.LogicController, this._mainContenPanel, st);
                        view.UserCanAddTrees = true;
                        _layouts.Add(view);
                        //this.Controls.Add(view);
                    }
                }
            }

            if (this._pageContainer != null)
            {
                this._mainContenPanel.Controls.Add(this._pageContainer);
                this._pageContainer.ResumeLayout(false);
            }




            // Set the form title (Text) with current cutting unit and description.
            this.Text = this.LogicController.GetViewTitle();

            this.ResumeLayout(false);
        }

        private void _deleteTreeBTN_Click(object sender, EventArgs e)
        {
            this.LogicController.HandleDeleteRowButtonClick();
        }


        







    }
}
