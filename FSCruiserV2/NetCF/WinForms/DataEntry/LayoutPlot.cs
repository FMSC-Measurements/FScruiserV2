using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FMSC.Sampling;
using System.ComponentModel;
using FMSC.Controls;
using Microsoft.WindowsCE.Forms;
using FSCruiser.Core.ViewInterfaces;
using FSCruiser.Core;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class LayoutPlot : UserControl, ITallyView, ITreeView, IPlotLayout
    {
        #region Designer code
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayoutPlot));
            this._plotNavPanel = new System.Windows.Forms.Panel();
            //this._BS_Plots = new System.Windows.Forms.BindingSource(this.components);
            this._plotSelectCB = new System.Windows.Forms.ComboBox();
            this._nextPlotButton = new FMSC.Controls.Mobile.ButtonPanel();
            this._imageList = new System.Windows.Forms.ImageList();
            this._gotoLastPlotButton = new FMSC.Controls.Mobile.ButtonPanel();
            this._addPlotButton = new FMSC.Controls.Mobile.ButtonPanel();
            this._deletePlotButton = new FMSC.Controls.Mobile.ButtonPanel();
            this._plotInfoButton = new FMSC.Controls.Mobile.ButtonPanel();
            this._prevPlotButton = new FMSC.Controls.Mobile.ButtonPanel();
            this._gotoFirstPlotButton = new FMSC.Controls.Mobile.ButtonPanel();
            //this._BS_Trees = new System.Windows.Forms.BindingSource(this.components);
            this._dataGrid = new FMSC.Controls.EditableDataGrid();
            this._expandGridButton = new FMSC.Controls.Mobile.ButtonPanel();
            this._tallyListPanel = new System.Windows.Forms.Panel();
            this._BS_TDV = new System.Windows.Forms.BindingSource(this.components);
            this._plotNavPanel.SuspendLayout();
            //((System.ComponentModel.ISupportInitialize)(this._BS_Plots)).BeginInit();
            //((System.ComponentModel.ISupportInitialize)(this._BS_Trees)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._BS_TDV)).BeginInit();
            this.SuspendLayout();
            // 
            // _plotNavPanel
            // 
            this._plotNavPanel.Controls.Add(this._plotSelectCB);
            this._plotNavPanel.Controls.Add(this._nextPlotButton);
            this._plotNavPanel.Controls.Add(this._gotoLastPlotButton);
            this._plotNavPanel.Controls.Add(this._addPlotButton);
            this._plotNavPanel.Controls.Add(this._deletePlotButton);
            this._plotNavPanel.Controls.Add(this._plotInfoButton);
            this._plotNavPanel.Controls.Add(this._prevPlotButton);
            this._plotNavPanel.Controls.Add(this._gotoFirstPlotButton);
            this._plotNavPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._plotNavPanel.Location = new System.Drawing.Point(0, 0);
            this._plotNavPanel.Name = "_plotNavPanel";
            this._plotNavPanel.Size = new System.Drawing.Size(240, 22);
            //// 
            //// _BS_Plots
            //// 
            //this._BS_Plots.DataSource = typeof(FSCruiserV2.Logic.PlotVM);
            //this._BS_Plots.CurrentChanged += new System.EventHandler(this._BS_Plots_CurrentChanged);
            // 
            // _plotSelectCB
            // 
            //this._plotSelectCB.DataSource = this._BS_Plots;
            this._plotSelectCB.Dock = System.Windows.Forms.DockStyle.Fill;
            this._plotSelectCB.Location = new System.Drawing.Point(55, 0);
            this._plotSelectCB.Name = "_plotSelectCB";
            this._plotSelectCB.Size = new System.Drawing.Size(61, 22);
            this._plotSelectCB.TabIndex = 2;
            // 
            // _nextPlotButton
            // 
            this._nextPlotButton.Dock = System.Windows.Forms.DockStyle.Right;
            this._nextPlotButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular);
            this._nextPlotButton.ImageIndex = 2;
            this._nextPlotButton.ImageList = this._imageList;
            this._nextPlotButton.Location = new System.Drawing.Point(116, 0);
            this._nextPlotButton.Name = "_nextPlotButton";
            this._nextPlotButton.Size = new System.Drawing.Size(27, 22);
            this._nextPlotButton.TabIndex = 6;
            this._nextPlotButton.Click += new System.EventHandler(this._nextPlotButton_Click);
            this._imageList.Images.Clear();
            this._imageList.Images.Add(((System.Drawing.Image)(resources.GetObject("resource"))));
            this._imageList.Images.Add(((System.Drawing.Image)(resources.GetObject("resource1"))));
            this._imageList.Images.Add(((System.Drawing.Image)(resources.GetObject("resource2"))));
            this._imageList.Images.Add(((System.Drawing.Image)(resources.GetObject("resource3"))));
            this._imageList.Images.Add(((System.Drawing.Image)(resources.GetObject("resource4"))));
            this._imageList.Images.Add(((System.Drawing.Image)(resources.GetObject("resource5"))));
            // 
            // _gotoLastPlotButton
            // 
            this._gotoLastPlotButton.Dock = System.Windows.Forms.DockStyle.Right;
            this._gotoLastPlotButton.Font = new System.Drawing.Font("Courier New", 14F, System.Drawing.FontStyle.Bold);
            this._gotoLastPlotButton.ImageIndex = 4;
            this._gotoLastPlotButton.ImageList = this._imageList;
            this._gotoLastPlotButton.Location = new System.Drawing.Point(143, 0);
            this._gotoLastPlotButton.Name = "_gotoLastPlotButton";
            this._gotoLastPlotButton.Size = new System.Drawing.Size(28, 22);
            this._gotoLastPlotButton.TabIndex = 7;
            this._gotoLastPlotButton.Click += new System.EventHandler(this._gotoLastPlotButton_Click);
            // 
            // _addPlotButton
            // 
            this._addPlotButton.Dock = System.Windows.Forms.DockStyle.Right;
            this._addPlotButton.Font = new System.Drawing.Font("Courier New", 14F, System.Drawing.FontStyle.Bold);
            this._addPlotButton.ImageIndex = 0;
            this._addPlotButton.Location = new System.Drawing.Point(171, 0);
            this._addPlotButton.Name = "_addPlotButton";
            this._addPlotButton.Size = new System.Drawing.Size(32, 22);
            this._addPlotButton.TabIndex = 3;
            this._addPlotButton.Click += new System.EventHandler(this._addPlotButton_Click);
            // 
            // _deletePlotButton
            // 
            this._deletePlotButton.Dock = System.Windows.Forms.DockStyle.Right;
            this._deletePlotButton.Font = new System.Drawing.Font("Courier New", 14F, System.Drawing.FontStyle.Bold);
            this._deletePlotButton.ImageIndex = 0;
            this._deletePlotButton.Location = new System.Drawing.Point(203, 0);
            this._deletePlotButton.Name = "_deletePlotButton";
            this._deletePlotButton.Size = new System.Drawing.Size(20, 22);
            this._deletePlotButton.TabIndex = 4;
            this._deletePlotButton.Click += new System.EventHandler(this._deletePlotButton_Click);
            // 
            // _plotInfoButton
            // 
            this._plotInfoButton.Dock = System.Windows.Forms.DockStyle.Right;
            this._plotInfoButton.Font = new System.Drawing.Font("Courier New", 14F, System.Drawing.FontStyle.Bold);
            this._plotInfoButton.ImageIndex = 0;
            this._plotInfoButton.Location = new System.Drawing.Point(223, 0);
            this._plotInfoButton.Name = "_plotInfoButton";
            this._plotInfoButton.Size = new System.Drawing.Size(17, 22);
            this._plotInfoButton.TabIndex = 5;
            this._plotInfoButton.Click += new System.EventHandler(this._plotInfoButton_Click);
            // 
            // _prevPlotButton
            // 
            this._prevPlotButton.Dock = System.Windows.Forms.DockStyle.Left;
            this._prevPlotButton.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Bold);
            this._prevPlotButton.ImageIndex = 1;
            this._prevPlotButton.ImageList = this._imageList;
            this._prevPlotButton.Location = new System.Drawing.Point(28, 0);
            this._prevPlotButton.Name = "_prevPlotButton";
            this._prevPlotButton.Size = new System.Drawing.Size(27, 22);
            this._prevPlotButton.TabIndex = 1;
            this._prevPlotButton.Click += new System.EventHandler(this._prevPlotButton_Click);
            // 
            // _gotoFirstPlotButton
            // 
            this._gotoFirstPlotButton.Dock = System.Windows.Forms.DockStyle.Left;
            this._gotoFirstPlotButton.Font = new System.Drawing.Font("Courier New", 14F, System.Drawing.FontStyle.Bold);
            this._gotoFirstPlotButton.ImageIndex = 5;
            this._gotoFirstPlotButton.ImageList = this._imageList;
            this._gotoFirstPlotButton.Location = new System.Drawing.Point(0, 0);
            this._gotoFirstPlotButton.Name = "_gotoFirstPlotButton";
            this._gotoFirstPlotButton.Size = new System.Drawing.Size(28, 22);
            this._gotoFirstPlotButton.TabIndex = 0;
            this._gotoFirstPlotButton.Click += new System.EventHandler(this._gotoFirstPlotButton_Click);
            //// 
            //// _BS_Trees
            //// 
            //this._BS_Trees.DataSource = typeof(FSCruiserV2.Logic.TreeVM);
            //this._BS_Trees.CurrentChanged += new System.EventHandler(this._BS_Trees_CurrentChanged);
            // 
            // _dataGrid
            // 
            this._dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dataGrid.Location = new System.Drawing.Point(0, 22);
            this._dataGrid.Name = "_dataGrid";
            this._dataGrid.Size = new System.Drawing.Size(240, 148);
            this._dataGrid.TabIndex = 1;
            // 
            // _expandGridButton
            // 
            this._expandGridButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._expandGridButton.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular);
            this._expandGridButton.ImageIndex = -1;
            this._expandGridButton.ImageList = this._imageList;
            this._expandGridButton.Location = new System.Drawing.Point(0, 170);
            this._expandGridButton.Name = "_expandGridButton";
            this._expandGridButton.Size = new System.Drawing.Size(240, 15);
            this._expandGridButton.TabIndex = 2;
            this._expandGridButton.Click += new System.EventHandler(this._expandGridButton_Click);
            // 
            // _tallyListPanel
            // 
            this._tallyListPanel.AutoScroll = true;
            this._tallyListPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._tallyListPanel.Location = new System.Drawing.Point(0, 185);
            this._tallyListPanel.Name = "_tallyListPanel";
            this._tallyListPanel.Size = new System.Drawing.Size(240, 60);
            // 
            // _BS_TDV
            // 
            this._BS_TDV.DataSource = typeof(CruiseDAL.DataObjects.TreeDefaultValueDO);
            // 
            // LayoutPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._dataGrid);
            this.Controls.Add(this._expandGridButton);
            this.Controls.Add(this._tallyListPanel);
            this.Controls.Add(this._plotNavPanel);
            this.Name = "LayoutPlot";
            this.Size = new System.Drawing.Size(240, 245);
            this._plotNavPanel.ResumeLayout(false);
            //((System.ComponentModel.ISupportInitialize)(this._BS_Plots)).EndInit();
            //((System.ComponentModel.ISupportInitialize)(this._BS_Trees)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._BS_TDV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _plotNavPanel;
        private FMSC.Controls.Mobile.ButtonPanel _gotoFirstPlotButton;
        private FMSC.Controls.Mobile.ButtonPanel _prevPlotButton;
        private FMSC.Controls.Mobile.ButtonPanel _deletePlotButton;
        private FMSC.Controls.Mobile.ButtonPanel _plotInfoButton;
        private FMSC.Controls.Mobile.ButtonPanel _nextPlotButton;
        private FMSC.Controls.Mobile.ButtonPanel _addPlotButton;
        private FMSC.Controls.Mobile.ButtonPanel _gotoLastPlotButton;
        private FMSC.Controls.EditableDataGrid _dataGrid;
        private FMSC.Controls.Mobile.ButtonPanel _expandGridButton;
        public System.Windows.Forms.ComboBox _plotSelectCB;
        public System.Windows.Forms.Panel _tallyListPanel;
        //public System.Windows.Forms.BindingSource _BS_Trees;
        //public System.Windows.Forms.BindingSource _BS_Plots;
        private System.Windows.Forms.BindingSource _BS_TDV;
        #endregion

        private bool _viewLoading = true;
        //private bool _disableCheckPlot = false;
        private const int SUB_POP_BUTTON_WIDTH = 48;
        private DataEntryMode _mode;
        //private bool _changingTree = false;
        private EditableComboBoxColumn _speciesColumn;
        private EditableComboBoxColumn _sgColumn;
        private EditableTextBoxColumn _treeNumberColumn;
        private EditableComboBoxColumn _initialsColoumn;
        private DataGridButtonColumn _logsColumn;
        private EditableTextBoxColumn _kpiColumn;
        private DataGridTableStyle _tableStyle; 
        //private String[] _visableFields;


        //public static int DATAGRID_INFLATED_SIZE = 186;
        //public static int DATAGRID_DEFLATED_SIZE = 86;
        //public static readonly string INFLATE_GRID_BUTTON_TEXT = "";//"<5><5><5>";//"▼ ▼ ▼";
        //public static readonly string DEFLATE_GRID_BUTTON_TEXT = "";//"<6><6><6>";//"▲ ▲ ▲";
        //public static readonly TreeDefaultValueDO[] EMPTY_SPECIES_LIST = new TreeDefaultValueDO[] { };

        public IApplicationController AppController { get { return this.ViewLogicController.Controller; } }
        public FormDataEntryLogic DataEntryController { get { return this.ViewLogicController.DataEntryController; } }

        public LayoutPlotLogic ViewLogicController { get; set; }
        public bool ViewLoading { get { return this._viewLoading; } }

        //public StratumVM StratumInfo { get { return this.ViewLogicController.StratumInfo; } }
        public bool HotKeyEnabled 
        {
            get
            {
                return !this._isGridExpanded;
            }
        }
        public Dictionary<char, CountTreeVM> HotKeyLookup 
        {
            get
            {
                return this.ViewLogicController.Stratum.HotKeyLookup;
            }
             
        }

        public IList<TreeVM> Trees
        {
            get
            {
                return this.ViewLogicController.CurrentPlotTreeList;
            }
        }

        public bool UserCanAddTrees
        {
            get { return this.ViewLogicController.UserCanAddTrees; }
            set { this.ViewLogicController.UserCanAddTrees = value; }
        }

        private ImageList _imageList;
        private bool _isGridExpanded = false;
        
        public bool IsGridExpanded
        {
            get
            {
                return _isGridExpanded;
            }
            set
            {
                if (this.ViewLogicController.Is3PPNT)
                {
                    value = true;
                }
                if (value == _isGridExpanded) { return; }
                if (value == true)
                {
                    _tallyListPanel.Visible = false;
                    _dataGrid.ReadOnly = false;

                    _expandGridButton.ImageIndex = 3;
                }
                else
                {
                    _tallyListPanel.Visible = true;
                    _dataGrid.ReadOnly = true;

                    _expandGridButton.ImageIndex = 0;
                }
                _isGridExpanded = value;
            }
        }

        public LayoutPlot(FormDataEntryLogic dataEntryController, Control parent, PlotStratum stratum, InputPanel sip)
        {
            //this.HotKeyLookup = new Dictionary<char, CountTreeDO>();

            //this.DataEntryController = dataEntryController;
            //this.StratumInfo = stInfo;
            //this.Controller = controller;
            this.ViewLogicController = new LayoutPlotLogic(stratum, this, dataEntryController, dataEntryController.ViewController);
            InitializeComponent();


            //Setup Plot Nav Bar
            this._addPlotButton.Text = "+";
            this._deletePlotButton.Text = "-";
            this._plotInfoButton.Text = "i";
            this._expandGridButton.ImageIndex = 0;
            this._expandGridButton.ButtonImageLayout = ImageLayout.Tile;
            this._nextPlotButton.ButtonImageLayout = ImageLayout.Zoom;
            this._gotoLastPlotButton.ButtonImageLayout = ImageLayout.Zoom;
            //this._addPlotButton.ButtonImageLayout = ImageLayout.Tile;
            //this._deletePlotButton.ButtonImageLayout = ImageLayout.Zoom;
            //this._plotInfoButton.ButtonImageLayout = ImageLayout.Zoom;
            this._prevPlotButton.ButtonImageLayout = ImageLayout.Zoom;
            this._gotoFirstPlotButton.ButtonImageLayout = ImageLayout.Zoom;

            if (ViewController.PlatformType == PlatformType.WinCE)
            {
                this._expandGridButton.Font = new System.Drawing.Font("Arial", this._expandGridButton.Font.Size, this._expandGridButton.Font.Style);
                this._gotoFirstPlotButton.Font = new System.Drawing.Font("Arial", this._gotoFirstPlotButton.Font.Size, this._gotoFirstPlotButton.Font.Style);
                this._gotoLastPlotButton.Font = new System.Drawing.Font("Arial", this._gotoLastPlotButton.Font.Size, this._gotoLastPlotButton.Font.Style);
                this._nextPlotButton.Font = new System.Drawing.Font("Arial", this._nextPlotButton.Font.Size, this._nextPlotButton.Font.Style);
                this._prevPlotButton.Font = new System.Drawing.Font("Arial", this._prevPlotButton.Font.Size, this._prevPlotButton.Font.Style);
            }

            
            //Setup DataGrid
            DataGridAdjuster.InitializeGrid(this._dataGrid);
            _tableStyle = DataGridAdjuster.InitializeTreeColumns(this.AppController._cDal, this._dataGrid, null, stratum, this.AppController.ViewController.EnableLogGrading);
            this._dataGrid.SIP = sip;
            this._dataGrid.CellValidating += new EditableDataGridCellValidatingEventHandler(_dataGrid_CellValidating);
            this._dataGrid.CellValueChanged += new EditableDataGridCellValueChangedEventHandler(this._dataGrid_CellValueChanged);
            //this._dataGrid.DataSource = typeof(FSCruiserV2.Logic.TreeVM);//_BS_Trees;
            this._dataGrid.Click += new EventHandler(_dataGrid_Click);
            this._dataGrid.ReadOnly = true;
            this._dataGrid.AllowUserToAddRows = false;

            _speciesColumn = _tableStyle.GridColumnStyles["TreeDefaultValue"] as EditableComboBoxColumn;
            _sgColumn = _tableStyle.GridColumnStyles["SampleGroup"] as EditableComboBoxColumn;
            _treeNumberColumn = _tableStyle.GridColumnStyles["TreeNumber"] as EditableTextBoxColumn;
            _initialsColoumn = _tableStyle.GridColumnStyles["Initials"] as EditableComboBoxColumn;
            _logsColumn = _tableStyle.GridColumnStyles["LogCount"] as DataGridButtonColumn;
            _kpiColumn = _tableStyle.GridColumnStyles["KPI"] as EditableTextBoxColumn;

            if (_logsColumn != null)
            {
                _logsColumn.Click += this.LogsClicked;
            }

            HandleCruisersChanged();

            this.Dock = DockStyle.Fill;
            this.Parent = parent;
            this._tallyListPanel.SuspendLayout();
            this._mode = stratum.GetDataEntryMode();
            this.ViewLogicController.DataEntryController.PopulateTallies(stratum, this._mode, DataEntryController.Unit, this._tallyListPanel, this);
            if (stratum.Method == "3PPNT")
            {
                this.IsGridExpanded = true;
            }
            this.ViewLogicController.UpdateCurrentPlot();
            _tallyListPanel.ResumeLayout(false);

            
        }

        //protected void RefreshColumnReferences()
        //{
        //    _speciesColumn = _tableStyle.GridColumnStyles["TreeDefaultValue"] as EditableComboBoxColumn;
        //    _sgColumn = _tableStyle.GridColumnStyles["SampleGroup"] as EditableComboBoxColumn;
        //    _treeNumberColumn = _tableStyle.GridColumnStyles["TreeNumber"] as EditableTextBoxColumn;
        //    _initialsColoumn = _tableStyle.GridColumnStyles["Initials"] as EditableComboBoxColumn;
        //    _logsColumn = _tableStyle.GridColumnStyles["LogCount"] as DataGridButtonColumn;
        //    _kpiColumn = _tableStyle.GridColumnStyles["KPI"] as EditableTextBoxColumn;
        //}

        private void _dataGrid_CellValidating(object sender, EditableDataGridCellValidatingEventArgs e)
        {
            bool cancel = false;
            TreeVM tree = null;
            try
            {
                tree = this.ViewLogicController.CurrentTree;
            }
            catch 
            {
                e.Cancel = true;
                return;
            }

            if (e.Column == _sgColumn)
            {
                this.ViewLogicController.DataEntryController.HandleSampleGroupChanging(tree, e.Value as SampleGroupDO, out cancel);
                //this.HandleSampleGroupChanging(tree, e.Value as SampleGroupDO, out cancel);
            }
            else if (e.Column == _speciesColumn)
            {
                //no action required
                return;
            }
            else if (e.Column == _treeNumberColumn)
            {
                try
                {
                    long treeNumber = (long)e.Value;
                    this.ViewLogicController.HandleTreeNumberChanging(treeNumber, out cancel);
                }
                catch
                {
                    cancel = true;
                }
            }
            else if (e.Column == _kpiColumn)
            {
                this.ViewLogicController.DataEntryController.HandleKPIChanging(tree, (float)e.Value, true, out cancel);
                //this.HandleKPIChanging(tree, (float)e.Value, out cancel);
            }
            e.Cancel = cancel;
        }

        private void _dataGrid_CellValueChanged(object sender, EditableDataGridCellEventArgs e)
        {
            TreeVM tree = this.ViewLogicController.CurrentTree;
            if (tree == null || e.Column == null) { return; }
            if (e.Column == _sgColumn)
            {
                this.ViewLogicController.DataEntryController.HandleSampleGroupChanged(this, tree);
                //this.HandleSampleGroupChanged(tree);
            }
            else if (e.Column == _speciesColumn)
            {

                this.ViewLogicController.DataEntryController.HandleSpeciesChanged(tree, _speciesColumn.EditComboBox.SelectedItem as TreeDefaultValueDO);
                //this.HandleSpeciesChanged(tree, col.EditComboBox.SelectedItem as TreeDefaultValueDO);
            }
        }


        private void _dataGrid_Click(object sender, EventArgs e)
        {
            if (IsGridExpanded == false)
            {
                IsGridExpanded = true;
            }
        }


        public void HandleLoad()
        {
            _viewLoading = false;
            this.ViewLogicController.HandleViewLoad();
            //this.RefreshColumnReferences();
            //_BS_Plots.DataSource = this.StratumInfo.Plots;
            
        }

        protected void LogsClicked(ButtonCellClickEventArgs e)
        {
            TreeVM tree;

            try
            {
                tree = this.ViewLogicController.CurrentTree;
            }
            catch { return; }
            if (tree == null) { return; }

            //this.LogsView.ShowDialog(tree);
            //this.Controller.ViewController.ShowLogsView(this.StratumInfo, tree);
            this.DataEntryController.ShowLogs(tree);
        }

        public void HandleStratumLoaded(Control container)
        {
            //do nothing
            return;
        }

        public void HandleEnableLogGradingChanged()
        {
            if (_logsColumn == null) { return; }
            if ((_logsColumn.Width > 0) == this.AppController.ViewController.EnableLogGrading) { return; }
            if (this.AppController.ViewController.EnableLogGrading)
            {
                _logsColumn.Width = Constants.LOG_COLUMN_WIDTH;
            }
            else
            {
                _logsColumn.Width = -1;
            }
        }

        public void HandleCruisersChanged()
        {
            if (this._initialsColoumn != null)
            {
                this._initialsColoumn.DataSource = this.AppController.Settings.Cruisers.ToArray();
            }
        }


        public TreeVM UserAddTree()
        {            
            return this.ViewLogicController.UserAddTree();      
        }

        public void MakeSGList(List<SampleGroupVM> list, Panel container)
        {
            if (list.Count == 1)
            {
                SampleGroupVM sg = list[0];

                if (sg.TreeDefaultValues.IsPopulated == false)
                {
                    sg.TreeDefaultValues.Populate();
                }
                foreach (TreeDefaultValueDO tdv in sg.TreeDefaultValues)
                {
                    SubPop subPop = new SubPop(sg, tdv);
                    MakeTallyRow(container, subPop);
                }
            }
            else
            {
                foreach (SampleGroupVM sg in list)
                {
                    Button sgButton = new Button();
                    Panel spContainer = new Panel();
                    
                    if (sg.TreeDefaultValues.IsPopulated == false)
                    {
                        sg.TreeDefaultValues.Populate();
                    }
                    foreach (TreeDefaultValueDO tdv in sg.TreeDefaultValues)
                    {
                        SubPop subPop = new SubPop(sg, tdv);
                        MakeTallyRow(spContainer, subPop);
                    }
                    
                    spContainer.Parent = container;
                    spContainer.Dock = DockStyle.Left;
                    spContainer.Width = (SUB_POP_BUTTON_WIDTH * sg.TreeDefaultValues.Count) + 10;
                    spContainer.Visible = false;

                    sgButton.Text = sg.Code;
                    sgButton.Tag = spContainer;
                    sgButton.Parent = container;
                    sgButton.BackColor = System.Drawing.SystemColors.ControlDark;
                    sgButton.Dock = DockStyle.Left;
                    sgButton.Click += new EventHandler(sgButton_Click);
                }
            }
        }

        public bool HandleHotKeyFirst(char key)
        {
            //TODO handle non-tally hot keys such as species for single stage stuff

            return false;
        }

        public bool HandleEscKey()
        {
            if (_viewLoading) { return false; }
            IsGridExpanded = !IsGridExpanded;
            return true;
            //if (!this.IsGridExpanded)
            //{
            //    this.IsGridExpanded = true;
            //    return true;
            //}
            //return false;
            
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.Handled == true) { return; }
            char key = (char)e.KeyValue;
            if (_viewLoading) { return; }
            e.Handled = this.ViewLogicController.DataEntryController.ProcessHotKey(key, this);
        }

        private void _expandGridButton_Click(object sender, EventArgs e)
        {
            IsGridExpanded = !IsGridExpanded;
        }


        public void RefreshTreeView(PlotVM currentPlot)
        {
            if (currentPlot != null)
            {
                this._dataGrid.ReadOnly = !this._isGridExpanded;
                this._dataGrid.Enabled = true;
                //if (this.StratumInfo.Method != "3PPNT")
                //{
                //    this._dataGrid.AllowUserToAddRows = !this.CurrentPlotInfo.IsNull;
                //}
                //else
                //{
                //    this._dataGrid.AllowUserToAddRows = this.CurrentPlotInfo.Trees.Count > 0;
                //}





                this._dataGrid.Focus();
            }
            else //no plot is selected 
            {
                this._dataGrid.Enabled = false;     //disable data grid
            }
        }

        public void BindPlotData(BindingSource plotBS)
        {
            this._plotSelectCB.DataSource = plotBS;
        }

        public void BindTreeData(BindingSource treeBS)
        {
            this._dataGrid.DataSource = treeBS;
        }






        public Control MakeTallyRow(Control container, SubPop subPop)
        {
            Button tallyButton = new Button();

            tallyButton.Text = subPop.TDV.Species;
            tallyButton.Click += new EventHandler(SpeciesButton_Click);
            tallyButton.Tag = subPop;
            tallyButton.Parent = container;
            tallyButton.Dock = DockStyle.Left;
            tallyButton.Width = SUB_POP_BUTTON_WIDTH;

            return tallyButton;

        }

        public Control MakeTallyRow(Control container, CountTreeVM count)
        {
            char hotKey = count.Tally.Hotkey[0];
            Button row = new Button();
            row.Text = count.Tally.Description + "[" + hotKey + "]";
            row.Click += new EventHandler(TallyButton_Click);
            Button settingsButton = new Button();
            settingsButton.Text = "i";
            settingsButton.Click += new EventHandler(SettingsButton_Click);
            //row.DiscriptionLabel.Text = count.Tally.Description;
            //row.TallyButton.Click += new EventHandler(TallyButton_Click);
            //row.SettingsButton.Click += new EventHandler(SettingsButton_Click);
            //row._hotKeyLabel.Text = hotKey.ToString();

            //row.TallyButton.DataBindings.Add(new Binding("Text", count, "TreeCount"));

            settingsButton.Tag = count;
            settingsButton.Width = 15;
            settingsButton.Parent = container;
            settingsButton.Dock = DockStyle.Left;
            FMSC.Controls.DpiHelper.AdjustControl(settingsButton);

            row.Tag = count;
            //this.HotKeyLookup.Add(hotKey, count);
            row.Width = 80;
            row.Parent = container;
            row.Dock = DockStyle.Left;

            
            return row;
        }



        
        public bool AskContinueOnCurrnetPlotTreeError()
        {
            return this.AppController.ViewController.AskYesNo("Error(s) found on tree records in current plot, Would you like to continue?", "Continue?", MessageBoxIcon.Question, true);
        }

        public void ShowNoPlotSelectedMessage()
        {
            MessageBox.Show("No Plot Selected");
        }

        public void ShowNullPlotMessage()
        {
            MessageBox.Show("Can't perform action on null plot");
        }

        #region event handlers

        //ocures during a FIX or PNT cruise, when user clicks a Sample Group.
        //Expands a list of species in the sample group
        void sgButton_Click(object sender, EventArgs e)
        {
            Button sgButton = (Button)sender;
            Panel spContainer = (Panel)sgButton.Tag;
            spContainer.Visible = !spContainer.Visible;

            //Controller.OnTally();
        }

        void SettingsButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            CountTreeVM count = (CountTreeVM)button.Tag;
            this.ViewLogicController.SavePlotTrees();
            AppController.ViewController.ShowTallySettings(count);            

        }

        void SpeciesButton_Click(object sender, EventArgs e)
        {
            if (!this.ViewLogicController.EnsureCurrentPlotWorkable()) { return; }

            Button button = (Button)sender;
            SubPop subPop = (SubPop)button.Tag;

            this.ViewLogicController.AddTree(subPop.SG, subPop.TDV);
        }


        void TallyButton_Click(object sender, EventArgs e)
        {
            if (!this.ViewLogicController.EnsureCurrentPlotWorkable()) { return; }

            Button button = (Button)sender;
            //TallyRow row = (TallyRow)button.Parent;
            CountTreeVM count = (CountTreeVM)button.Tag;
            this.ViewLogicController.OnTally(count);
        }

        private void _gotoFirstPlotButton_Click(object sender, EventArgs e)
        {
            this.ViewLogicController.SelectFirstPlot();
        }

        private void _prevPlotButton_Click(object sender, EventArgs e)
        {
            this.ViewLogicController.SelectPreviousPlot();

        }

        private void _nextPlotButton_Click(object sender, EventArgs e)
        {
            this.ViewLogicController.SelectNextPlot();
        }

        private void _gotoLastPlotButton_Click(object sender, EventArgs e)
        {
            this.ViewLogicController.SelectLastPlot();
        }

        private void _addPlotButton_Click(object sender, EventArgs e)
        {
            this.ViewLogicController.HandleAddPlot();
            this.IsGridExpanded = false;
        }

        private void _deletePlotButton_Click(object sender, EventArgs e)
        {
            this.ViewLogicController.HandleDeletePlot();
        }

        private void _plotInfoButton_Click(object sender, EventArgs e)
        {
            this.ViewLogicController.ShowCurrentPlotInfo();
        }

        #endregion


        public void ViewEndEdit()
        {
            this._dataGrid.EndEdit();
        }

        public void EndEdit()
        {
            this.ViewLogicController.EndEdit();
        }

        public void HandleCurrentTreeChanged(TreeVM tree)
        {
            UpdateSampleGroupColumn(tree);
            UpdateSpeciesColumn(tree);            
        }

        public void UpdateSpeciesColumn(TreeVM tree)
        {
            if (_speciesColumn != null)
            {
                _speciesColumn.DataSource = tree.ReadValidTDVs();
            }
        }

        public void UpdateSampleGroupColumn(TreeVM tree)
        {
            if (_sgColumn != null)
            {
                _sgColumn.DataSource = tree.ReadValidSampleGroups();
            }
        }

        public void OnTally(CountTreeVM count)
        {
            this.ViewLogicController.OnTally(count);
        }

        public void DeleteRow()
        {

            this.ViewLogicController.HandleDeleteCurrentTree();
        }

        public void MoveLast()
        {
            this.ViewLogicController.SelectLastTree();
        }

        public void MoveHomeField()
        {
            this._dataGrid.MoveFirstEmptyCell();
        }

        public void ShowHideErrorCol()
        {
            DataGridAdjuster.ShowHideErrorCol(this._dataGrid);
        }

        public void ShowLimitingDistanceDialog()
        {
            if (this.ViewLogicController.CurrentPlot == null)
            {
                ShowNoPlotSelectedMessage();
                return;
            }

            TreeVM tree = null;
            //see if the user is in the DBH column 
            if (this._dataGrid.CurrentCollumn.MappingName == "DBH")
            {
                //is a tree selected and if so grab it and take its dbh
                TreeVM curTree = this.ViewLogicController.CurrentTree;
                if (curTree != null && curTree.DBH == 0)
                {
                    tree = curTree;
                }
            }

            this.DataEntryController.ShowLimitingDistanceDialog(this.ViewLogicController.Stratum, this.ViewLogicController.CurrentPlot, tree);
        }



    }
}
