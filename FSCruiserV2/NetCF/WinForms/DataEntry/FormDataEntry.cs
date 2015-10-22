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
        //private FormDataEntryLogic _logicController;
        //protected IApplicationController Controller { get; set; }


 


        //private LayoutTreeBased _tallyLayout;
        //private List<IDataEntryPage> _layouts = new List<IDataEntryPage>();
        //private IList<StratumVM> _plotStrataInfo;// = new List<StratumInfo>();
        //private TabControl _pageContainer;
        //private TabPage _tallyPage;
        //private TabPage _treePage;
        //private ControlTreeDataGrid _treeView;

        private Microsoft.WindowsCE.Forms.InputPanel _sip;
        public InputPanel SIP
        {
            get
            {
                return this._sip;
            }
        }

        //public FormDataEntryLogic LogicController 
        //{
        //    get
        //    {
        //        return this._logicController;
        //    }
        //}

        //public IDataEntryPage FocusedLayout
        //{
        //    get
        //    {
        //        if (_pageContainer == null)
        //        {
        //            if (_layouts.Count > 0)
        //            {
        //                return _layouts[0];
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //        else
        //        {
        //            return _layouts[_pageContainer.SelectedIndex];
        //        }
        //    }
        //}

        //public List<IDataEntryPage> Layouts
        //{
        //    get
        //    {
        //        return _layouts;
        //    }
        //}

        //public IList<StratumVM> PlotStrata
        //{
        //    get
        //    {
        //        return _plotStrataInfo;
        //    }
        //}

        public FormDataEntry():base()
        {
            //this.KeyPreview = true;// set in CustomForm class
            //this.HotKeyLookup = new Dictionary<char, CountTreeDO>();
            //this.StratumHotKeyLookup = new Dictionary<char, int>();

            InitializeComponent();

            if(ViewController.PlatformType == PlatformType.WM)
            {
                this._sip = new Microsoft.WindowsCE.Forms.InputPanel();
                this.components.Add(_sip);
            }
            else if (ViewController.PlatformType == PlatformType.WinCE)
            {

                this.WindowState = FormWindowState.Maximized;
            }

            //this._pageContainer.SelectedIndex = 1;
        }

        //private TabControl MakePageContainer()
        //{
        //    TabControl pc = new TabControl();
        //    pc.SelectedIndexChanged += new EventHandler(OnFocusedLayoutChanged);
        //    pc.SuspendLayout();
        //    pc.Dock = DockStyle.Fill;

        //    return pc;
        //}


        public FormDataEntry(IApplicationController controller, CuttingUnitDO unit) :this()
        {
            this.Controller = controller;
            this.LogicController = new FormDataEntryLogic(this.Controller, this);
            //this.Unit = unit;
            DataEntryMode unitMode = Controller.GetUnitDataEntryMode(unit);
            this.SuspendLayout();


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

                _treeView = new ControlTreeDataGrid(this.Controller,this.LogicController, this.SIP);
                _treeView.Dock = DockStyle.Fill;
                _treeView.UserCanAddTrees = true;

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
                foreach(StratumVM st in _plotStrataInfo)
                {
                    //load plots in stratum
                    st.Plots = controller._cDal.Read<PlotVM>("Plot", "WHERE Stratum_CN = ? AND CuttingUnit_CN = ? ORDER BY PlotNumber", st.Stratum_CN, Controller.CurrentUnit.CuttingUnit_CN);

                    if (_pageContainer != null)
                    {
                        TabPage page = new TabPage();
                        page.Text = String.Format("{0}-{1}[{2}]", st.Code, st.Method, st.Hotkey);
                        _pageContainer.TabPages.Add(page);

                        LayoutPlot view = new LayoutPlot(this.LogicController, page, st, this.SIP);
                        view.UserCanAddTrees = true;
                        _layouts.Add(view);

                        int pageIndex = _pageContainer.TabPages.IndexOf(page);
                        this.LogicController.AddStratumHotKey(st.Hotkey, pageIndex);

                    }
                    else
                    {
                        LayoutPlot view = new LayoutPlot(this.LogicController, this, st, this.SIP);
                        view.UserCanAddTrees = true;
                        _layouts.Add(view);
                        this.Controls.Add(view);
                    }
                }
            }

            if (this._pageContainer != null)
            {
                this.Controls.Add(this._pageContainer);
                this._pageContainer.ResumeLayout(false);
            }
            //DataGridAdjuster.InitializeGrid(this.editableDataGrid1);
            //DataGridTableStyle tableStyle = DataGridAdjuster.InitializeTreeColumns(this.Controller._cDal, this.editableDataGrid1,unit, null, LogsClicked);
            //this.editableDataGrid1.SIP = this.SIP;

            //_speciesColumn = tableStyle.GridColumnStyles["Species"] as EditableComboBoxColumn;
            //_sgColumn = tableStyle.GridColumnStyles["SampleGroup"] as EditableComboBoxColumn;
            //_stratumColumn = tableStyle.GridColumnStyles["Stratum"] as EditableComboBoxColumn;

            //if (_speciesColumn != null)
            //{
            //    _speciesColumn.SelectedValueChanged += new EventHandler(SelectedSpeciesChanged);
            //}

            //if (_sgColumn != null)
            //{
            //    _sgColumn.SelectedValueChanged += new EventHandler(SelectedSampleGroupChanged);
            //}

            //if (_stratumColumn != null)
            //{
            //    _stratumColumn.DataSource = unit.Strata;
            //    _stratumColumn.SelectedValueChanged += new EventHandler(SelectedStratumChanged);

            //}



            // Set the form title (Text) with current cutting unit and description.
            this.Text = this.LogicController.GetViewTitle();

            this.ResumeLayout(false);

        }

        protected override void OnFocusedLayoutChanged(object sender, EventArgs e)
        {
            base.OnFocusedLayoutChanged(sender, e);
            ITreeView view = this.FocusedLayout as ITreeView;
            this._addTreeMI.Enabled = view != null && view.UserCanAddTrees;
        }

        #region Form Life Cycle  ========================================================================


        //protected override void OnLoad(EventArgs e)
        //{
        //    base.OnLoad(e);

        //    _logicController.HandleViewLoading();
        //    this.OnFocusedLayoutChanged(null, null);
        //}

        //private void InternalHandleCuttingUnitDataLoaded()
        //{
        //    foreach (IDataEntryPage c in this._layouts)
        //    {
        //        ITreeView tv = c as ITreeView;
        //        if (tv != null)
        //        {
        //            tv.HandleLoad();
        //        }
        //    }

        //    if (this._tallyPage != null)
        //    {
        //        this._tallyLayout.HandleLoad();
        //    }

        //    // Turn off the waitcursor that was turned on in FormMain.button1_Click()
        //    Cursor.Current = Cursors.Default;
        //}

        //public void HandleCuttingUnitDataLoaded()
        //{
        //    if (this.InvokeRequired)
        //    {
        //        this.Invoke(new HandleCruiseDataLoadedEventHandler(this.InternalHandleCuttingUnitDataLoaded));
        //    }
        //    else
        //    {
        //        this.InternalHandleCuttingUnitDataLoaded();
        //    }
        //}

        //public void HandleEnableLogGradingChanged()
        //{
        //    foreach (IDataEntryPage c in this._layouts)
        //    {
        //        ITreeView tv = c as ITreeView;
        //        if (tv != null)
        //        {
        //            tv.HandleEnableLogGradingChanged();
        //        }
        //    }
        //}

        //public void HandleCruisersChanged()
        //{
        //    foreach (IDataEntryPage c in this._layouts)
        //    {
        //        ITreeView tv = c as ITreeView;
        //        if (tv != null)
        //        {
        //            tv.HandleCruisersChanged();
        //        }
        //    }
        //}

        //protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        //{
        //    base.OnClosing(e);
        //    if (!Controller.ValidateTrees())
        //    {
        //        MessageBox.Show("Error(s) found on tree records");
        //    }
        //}

        //protected override void OnClosed(EventArgs e)
        //{
        //    base.OnClosed(e);
        //    this.Controller.Save();
        //}

        //protected override void OnKeyDown(KeyEventArgs e)
        //{
        //    base.OnKeyDown(e);
        //    //if (e.Handled) { return; }

        //    //char key = (char)e.KeyValue;

        //    //if (this._pageContainer != null)
        //    //{
        //    //    if (e.KeyCode == Keys.Escape &&
        //    //    this._pageContainer.SelectedIndex == this._pageContainer.TabPages.IndexOf(_treePage))
        //    //    {
        //    //        this.GoToTallyPage();
        //    //        e.Handled = true;
        //    //        return;
        //    //    }
        //    //    else
        //    //    {
        //    //        e.Handled = _logicController.HandleHotKey(key);
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    ITallyView view = this.FocusedLayout as ITallyView;
        //    //    if (view == null) { return; }
        //    //    view.HandleKeyDown(key);
        //    //}           
        //}

        //protected override void OnKeyUp(KeyEventArgs e)
        //{
        //    base.OnKeyUp(e);
        //    if (e.Handled) { return; }

        //    char key = (char)e.KeyValue;

        //    if (this._pageContainer != null)
        //    {
        //        if (e.KeyCode == Keys.Escape &&
        //        this._pageContainer.SelectedIndex == this._pageContainer.TabPages.IndexOf(_treePage))
        //        {
        //            this.GoToTallyPage();
        //            e.Handled = true;
        //            return;
        //        }
        //        else
        //        {
        //            e.Handled = _logicController.HandleHotKey(key);
        //        }
        //    }
        //    else
        //    {
        //        ITallyView view = this.FocusedLayout as ITallyView;
        //        if (view == null) { return; }
        //        view.HandleKeyUp(key);
        //    }           
        //}
        #endregion 


        //public void HandleCuttingUnitDataLoaded()
        //{
        //    this.Unit = this.Controller.CurrentUnit;
        //    DataEntryMode unitMode = Controller.GetUnitDataEntryMode(unit);
        //    if (unitMode == DataEntryMode.Unknown)
        //    {
        //        unitMode = DataEntryMode.HundredPct;
        //    }
        //    this.SuspendLayout();

        //    _layouts = new List<Control>();

        //    //if the unit contains Tree based methods or multiple plot strata then we need a tab control
        //    if ((unitMode & DataEntryMode.Tree) == DataEntryMode.Tree ||
        //        ((unitMode & DataEntryMode.Plot) == DataEntryMode.Plot && unit.Strata.Count > 1))
        //    {
        //        this._pageContainer = new TabControl();
        //        this._pageContainer.SuspendLayout();
        //        this._pageContainer.Dock = DockStyle.Fill;
        //    }

        //    //do we have any tree based strata in the unit
        //    if ((unitMode & DataEntryMode.Tree) == DataEntryMode.Tree)
        //    {
        //        this._treePage = new TabPage();
        //        //this._treePage.SuspendLayout();
        //        this._treePage.Text = "Trees";

        //        ControlTreeDataGrid dg = new ControlTreeDataGrid(this.Controller, unit, this.SIP);
        //        dg.Dock = DockStyle.Fill;
        //        dg.TreeBindingSource = this._BS_nonPlotTrees;


        //        _treePage.Controls.Add(dg);
        //        this._pageContainer.TabPages.Add(_treePage);
        //        this._layouts.Add(dg);


        //        if ((unitMode & DataEntryMode.HundredPct) != DataEntryMode.HundredPct)
        //        {
        //            _tallyLayout = new LayoutTreeBased(this.Controller);
        //            _tallyLayout.Dock = DockStyle.Fill;

        //            this._tallyPage = new TabPage();
        //            this._tallyPage.Text = "Tally";
        //            this._pageContainer.TabPages.Add(this._tallyPage);
        //            this._tallyPage.Controls.Add(_tallyLayout);
        //            this._layouts.Add(_tallyLayout);
        //        }
        //    }

        //    if ((unitMode & DataEntryMode.Plot) == DataEntryMode.Plot)
        //    {
        //        for (int i = 0; i < unit.Strata.Count; i++)
        //        {
        //            DataEntryMode stratumMode = controller.GetStrataDataEntryMode(unit.Strata[i]);
        //            if ((stratumMode & DataEntryMode.Tree) == DataEntryMode.Tree)
        //            {
        //                continue;
        //            }

        //            StratumDO stratum = unit.Strata[i];
        //            StratumInfo stInfo = Controller.CreateStratumInfo(unit, stratum);
        //            if (stInfo == null)
        //            {
        //                continue;
        //            }
        //            _strataInfo.Add(stInfo);

        //            //TabPage page = new TabPage();
        //            //page.Text = String.Format("{0}-{1}[{2}]", stratum.Code, stratum.Method, stratum.Hotkey);
        //            //_layouts[i] = new LayoutPlot(Controller, page, stInfo, this.SIP);


        //            if (_pageContainer != null)
        //            {
        //                TabPage page = new TabPage();
        //                page.Text = String.Format("{0}-{1}[{2}]", stratum.Code, stratum.Method, stratum.Hotkey);
        //                _pageContainer.TabPages.Add(page);

        //                _layouts.Add(new LayoutPlot(this.Controller, page, stInfo, this.SIP));

        //                if (!string.IsNullOrEmpty(stratum.Hotkey))
        //                {
        //                    char stratumHotKey = char.ToUpper(stratum.Hotkey[0]);
        //                    int pageIndex = _pageContainer.TabPages.IndexOf(page);
        //                    StratumHotKeyLookup.Add(stratumHotKey, pageIndex);
        //                }
        //            }
        //            else
        //            {
        //                LayoutPlot view = new LayoutPlot(Controller, this, stInfo, this.SIP);
        //                _layouts.Add(view);
        //                this.Controls.Add(view);
        //            }
        //        }
        //    }

        //    if (this._pageContainer != null)
        //    {
        //        this.Controls.Add(this._pageContainer);
        //        this._pageContainer.ResumeLayout();
        //    }
        //    //DataGridAdjuster.InitializeGrid(this.editableDataGrid1);
        //    //DataGridTableStyle tableStyle = DataGridAdjuster.InitializeTreeColumns(this.Controller._cDal, this.editableDataGrid1,unit, null, LogsClicked);
        //    //this.editableDataGrid1.SIP = this.SIP;

        //    //_speciesColumn = tableStyle.GridColumnStyles["Species"] as EditableComboBoxColumn;
        //    //_sgColumn = tableStyle.GridColumnStyles["SampleGroup"] as EditableComboBoxColumn;
        //    //_stratumColumn = tableStyle.GridColumnStyles["Stratum"] as EditableComboBoxColumn;

        //    //if (_speciesColumn != null)
        //    //{
        //    //    _speciesColumn.SelectedValueChanged += new EventHandler(SelectedSpeciesChanged);
        //    //}

        //    //if (_sgColumn != null)
        //    //{
        //    //    _sgColumn.SelectedValueChanged += new EventHandler(SelectedSampleGroupChanged);
        //    //}

        //    //if (_stratumColumn != null)
        //    //{
        //    //    _stratumColumn.DataSource = unit.Strata;
        //    //    _stratumColumn.SelectedValueChanged += new EventHandler(SelectedStratumChanged);

        //    //}



        //    // Set the form title (Text) with current cutting unit and description.
        //    this.Text = "Unit: " + this.Unit.Code + ", " + this.Unit.Description;

        //    this.ResumeLayout(false);
        //}

        //public void GoToPageIndex(int i)
        //{
        //    if (i < 0 || i < _pageContainer.TabPages.Count - 1)
        //    {
        //        i = 0;
        //    }
        //    _pageContainer.SelectedIndex = i;
        //    _pageContainer.Focus();
        //}

        //public void GotoTreePage()
        //{
        //    if (this._pageContainer == null) { return; }
        //    int pageIndex = this._pageContainer.TabPages.IndexOf(_treePage);
        //    this.GoToPageIndex(pageIndex);
        //}


        //public void GoToTallyPage()
        //{
        //    if (this._pageContainer == null) { return; }
        //    int pageIndex = this._pageContainer.TabPages.IndexOf(_tallyPage);
        //    this.GoToPageIndex(pageIndex);
        //}

        //public void TreeViewMoveLast()
        //{
        //    if (_treeView != null)
        //    {
        //        _treeView.MoveLast();
        //    }
        //}

        //protected void LogsClicked(ButtonCellClickEventArgs e)
        //{
        //    TreeVM tree = this._BS_trees[e.RowNumber] as TreeVM;
        //    if (tree == null) { return; }

        //    this.Controller.ViewController.GetLogsView(tree.Stratum).ShowDialog(tree);
        //}


        //protected FormLogs GetLogsView(StratumDO stratum)
        //{
        //    if (_logViews.ContainsKey(stratum))
        //    {
        //        return _logViews[stratum];
        //    }
        //    FormLogs logView = new FormLogs(this.Controller, stratum.Stratum_CN.Value);
        //    _logViews.Add(stratum, logView);

        //    return logView;
        //}

        

        //void SelectedStratumChanged(object sender, EventArgs e)
        //{
        //    if (!_changingTree)
        //    {
        //        TreeDO tree = _BS_trees.Current as TreeDO;
        //        StratumDO stratum = null;
        //        if (_stratumColumn != null)
        //        {
        //            stratum = _stratumColumn.EditComboBox.SelectedItem as StratumDO;
        //        }
        //        if (tree == null || stratum == null) { return; }

        //        if (MessageBox.Show("You are changing the stratum of a tree, are you sure you want to do this?", "!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2)
        //            == DialogResult.Yes)
        //        {
        //            this.Controller._cDal.LogMessage(String.Format("Tree Stratum Changed (Cu:{0} St:{1} -> {2} Sg:{3} Tdv_CN:{4} T#: {5}",
        //                tree.CuttingUnit.Code,
        //                tree.Stratum.Code,
        //                stratum.Code,
        //                (tree.SampleGroup != null) ? tree.SampleGroup.Code : "?",
        //                (tree.TreeDefaultValue != null) ? tree.TreeDefaultValue.TreeDefaultValue_CN.ToString() : "?",
        //                tree.TreeNumber), "high");

        //            tree.Species = null;
        //            tree.SampleGroup = null;
        //            tree.TreeDefaultValue = null;
        //            tree.Stratum = stratum;
        //        }

        //    }
        //}

        //void SelectedSampleGroupChanged(object sender, EventArgs e)
        //{
        //    if (!_changingTree)
        //    {
        //        TreeDO tree = _BS_trees.Current as TreeDO;
        //        SampleGroupDO sg = null;
        //        if (_sgColumn != null)
        //        {
        //            sg = _sgColumn.EditComboBox.SelectedItem as SampleGroupDO;
        //        }
        //        if (tree == null || sg == null) { return; }

        //        if (MessageBox.Show("You are changing the Sample Group of a tree, are you sure you want to do this?", "!", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2)
        //            == DialogResult.Yes)
        //        {

        //            this.Controller._cDal.LogMessage(String.Format("Tree Sample Group Changed (Cu:{0} St:{1} Sg:{2} -> {3} Tdv_CN:{4} T#: {5}",
        //                tree.CuttingUnit.Code,
        //                tree.Stratum.Code,
        //                (tree.SampleGroup != null) ? tree.SampleGroup.Code : "?",
        //                sg.Code,
        //                (tree.TreeDefaultValue != null) ? tree.TreeDefaultValue.TreeDefaultValue_CN.ToString() : "?",
        //                tree.TreeNumber), "high");

        //            tree.SampleGroup = sg;
        //        }
        //    }
        //}

        //private void SelectedSpeciesChanged(object sender, EventArgs e)
        //{
        //    if (!_changingTree)
        //    {
        //        TreeDO tree = _BS_trees.Current as TreeDO;
        //        TreeDefaultValueDO tdv = null;
        //        if (_speciesColumn != null)
        //        {
        //            tdv = _speciesColumn.EditComboBox.SelectedItem as TreeDefaultValueDO;
        //        }
        //        if (tree == null || tdv == null) { return; }
        //        tree.Species = tdv.Species;
        //        tree.TreeDefaultValue = tdv;
        //        tree.LiveDead = tdv.LiveDead;
        //        tree.Grade = tdv.TreeGrade;
        //        tree.FormClass = tdv.FormClass;
        //        tree.RecoverablePrimary = tdv.Recoverable;
        //        tree.HiddenPrimary = tdv.HiddenPrimary;
        //    }
        //}


        //private void _BS_trees_AddingNew(object sender, System.ComponentModel.AddingNewEventArgs e)
        //{
        //    TreeVM prevTree = null;
        //    StratumDO assumedSt = null;
        //    if (_BS_nonPlotTrees.Count > 0)
        //    {
        //        prevTree = (TreeVM)_BS_nonPlotTrees[_BS_nonPlotTrees.Count - 1];
        //        assumedSt = prevTree.Stratum;
        //    }

        //    e.NewObject = Controller.UserAddTree(prevTree, assumedSt, null);

        //}

        //private void _BS_trees_CurrentChanged(object sender, EventArgs e)
        //{

        //    TreeDO tree = _BS_trees.Current as TreeDO;
        //    if (tree == null) { return; }
            
        //    _changingTree = true;
        //    if (_speciesColumn != null)
        //    {

        //        _speciesColumn.DataSource = Controller.GetTreeTDVList(tree);

        //    }

        //    if (_sgColumn != null)
        //    {
        //        _sgColumn.DataSource = Controller.GetTreeSGList(tree);
        //    }

        //    _changingTree = false; 
        //}


        private void _deleteRowButton_Click(object sender, EventArgs e)
        {
            this.LogicController.HandleDeleteRowButtonClick();

            //ITreeView layout = this.FocusedLayout as ITreeView;
            //if (layout != null)
            //{
            //    layout.DeleteRow();
            //}
        }


        //protected override void OnClosing(CancelEventArgs e)
        //{
            
        //    base.OnClosing(e);
        //    this._logicController.HandleViewClosing(e);
        //    //for(int i = 0; i < _layouts.Count; i++)
        //    //{
        //    //    ITreeView view = _layouts[i] as ITreeView;
        //    //    if (view != null)
        //    //    {
        //    //        view.EndEdit();
        //    //    }
        //    //}

        //    //if (!Controller.ValidateTrees())
        //    //{
        //    //    if (MessageBox.Show("Error(s) found on tree records Would you like to continue", "Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
        //    //    {
        //    //        e.Cancel = true;
        //    //    }
        //    //}

        //    //foreach (StratumInfo st in _strataInfo)
        //    //{
        //    //    foreach (PlotInfo p in st.Plots)
        //    //    {
        //    //        p.Save();
        //    //    }
        //    //}

        //    //if (!Controller.Save())
        //    //{
        //    //    MessageBox.Show("Something went wrong saving the data for this unit, check trees for errors and try again");
        //    //    e.Cancel = true;
        //    //}

        //}

        //protected override void OnClosed(EventArgs e)
        //{
        //    base.OnClosed(e);
            

            
        //}


        private void showHideErrorMessages_Click(object sender, EventArgs e)
        {
            this.LogicController.HandleShowHideErrorCol();
            //ITreeView view = this.FocusedLayout as ITreeView;
            //if (view == null) { return; }
            //view.ShowHideErrorCol();

        }

        private void LimitingDistance_Click(object sender, EventArgs e)
        {
            this.LogicController.HandleDisplayLimitingDistance();
            //LayoutPlot view = this.FocusedLayout as LayoutPlot;
            //if (view == null) { return; }
            //view.ShowLimitingDistanceDialog();
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
            //this.Controller.ViewController.EnableLogGrading = !this.Controller.ViewController.EnableLogGrading;
        }

        private void _addTreeMI_Click(object sender, EventArgs e)
        {
            this.LogicController.HandleAddTreeClick();
            //ITreeView view = this.FocusedLayout as ITreeView;
            //if (view == null) { return; }
            //view.UserAddTree();
        }

        


    }
}