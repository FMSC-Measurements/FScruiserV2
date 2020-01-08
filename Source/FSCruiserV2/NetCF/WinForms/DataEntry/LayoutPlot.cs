using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FMSC.Controls;
using FScruiser.Core.Services;
using FSCruiser.Core;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;
using Microsoft.WindowsCE.Forms;
using FScruiser.Services;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class LayoutPlot : UserControl, ITallyView, ITreeView, IPlotLayout
    {
        public const int LOG_COLUMN_WIDTH = 40;

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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                DataService = null;
                AppSettings = null;
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
            this._plotNavPanel = new System.Windows.Forms.Panel();
            this._plotSelectCB = new System.Windows.Forms.ComboBox();
            this._nextPlotButton = new FMSC.Controls.ButtonPanel();

            this._gotoLastPlotButton = new FMSC.Controls.ButtonPanel();
            this._addPlotButton = new FMSC.Controls.ButtonPanel();
            this._deletePlotButton = new FMSC.Controls.ButtonPanel();
            this._plotInfoButton = new FMSC.Controls.ButtonPanel();
            this._prevPlotButton = new FMSC.Controls.ButtonPanel();
            this._gotoFirstPlotButton = new FMSC.Controls.ButtonPanel();
            this._dataGrid = new FMSC.Controls.EditableDataGrid();
            this._expandGridButton = new FMSC.Controls.ButtonPanel();
            this._tallyListPanel = new System.Windows.Forms.Panel();
            this._BS_TDV = new System.Windows.Forms.BindingSource(this.components);
            this._plotNavPanel.SuspendLayout();
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
            //
            // _plotSelectCB
            //
            this._plotSelectCB.Dock = System.Windows.Forms.DockStyle.Fill;
            this._plotSelectCB.Location = new System.Drawing.Point(55, 0);
            this._plotSelectCB.Name = "_plotSelectCB";
            this._plotSelectCB.Size = new System.Drawing.Size(61, 23);
            this._plotSelectCB.TabIndex = 2;
            //
            // _nextPlotButton
            //
            this._nextPlotButton.Dock = System.Windows.Forms.DockStyle.Right;
            this._nextPlotButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular);

            this._nextPlotButton.Location = new System.Drawing.Point(116, 0);
            this._nextPlotButton.Name = "_nextPlotButton";
            this._nextPlotButton.Size = new System.Drawing.Size(27, 22);
            this._nextPlotButton.TabIndex = 6;
            this._nextPlotButton.Click += new System.EventHandler(this._nextPlotButton_Click);

            //
            // _gotoLastPlotButton
            //
            this._gotoLastPlotButton.Dock = System.Windows.Forms.DockStyle.Right;
            this._gotoLastPlotButton.Font = new System.Drawing.Font("Courier New", 14F, System.Drawing.FontStyle.Bold);

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

            this._gotoFirstPlotButton.Location = new System.Drawing.Point(0, 0);
            this._gotoFirstPlotButton.Name = "_gotoFirstPlotButton";
            this._gotoFirstPlotButton.Size = new System.Drawing.Size(28, 22);
            this._gotoFirstPlotButton.TabIndex = 0;
            this._gotoFirstPlotButton.Click += new System.EventHandler(this._gotoFirstPlotButton_Click);
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
            ((System.ComponentModel.ISupportInitialize)(this._dataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._BS_TDV)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion Component Designer generated code

        private System.Windows.Forms.Panel _plotNavPanel;
        private FMSC.Controls.ButtonPanel _gotoFirstPlotButton;
        private FMSC.Controls.ButtonPanel _prevPlotButton;
        private FMSC.Controls.ButtonPanel _deletePlotButton;
        private FMSC.Controls.ButtonPanel _plotInfoButton;
        private FMSC.Controls.ButtonPanel _nextPlotButton;
        private FMSC.Controls.ButtonPanel _addPlotButton;
        private FMSC.Controls.ButtonPanel _gotoLastPlotButton;
        private FMSC.Controls.EditableDataGrid _dataGrid;
        private FMSC.Controls.ButtonPanel _expandGridButton;
        public System.Windows.Forms.ComboBox _plotSelectCB;
        public System.Windows.Forms.Panel _tallyListPanel;

        private System.Windows.Forms.BindingSource _BS_TDV;

        #endregion Designer code

        //private bool _disableCheckPlot = false;
        const int SUB_POP_BUTTON_WIDTH = 48;

        ImageList _imageList;
        EditableComboBoxColumn _speciesColumn;
        EditableComboBoxColumn _sgColumn;
        EditableTextBoxColumn _treeNumberColumn;
        EditableComboBoxColumn _initialsColoumn;
        DataGridButtonColumn _logsColumn;
        EditableTextBoxColumn _kpiColumn;
        DataGridTextBoxColumn _errorsColumn;
        DataGridTableStyle _tableStyle;
        //private String[] _visableFields;

        bool _isGridExpanded = false;

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

        //#region DataService

        //IDataEntryDataService _dataService;

        //IDataEntryDataService DataService
        //{
        //    get { return _dataService; }
        //    set
        //    {
        //        OnDataServiceChanging();
        //        _dataService = value;
        //        OnDataServiceChanged();
        //    }
        //}

        //private void OnDataServiceChanged()
        //{
        //    if (_dataService != null)
        //    {
        //        _dataService.EnableLogGradingChanged += HandleEnableLogGradingChanged;
        //    }
        //}

        //private void OnDataServiceChanging()
        //{
        //    if (_dataService != null)
        //    {
        //        _dataService.EnableLogGradingChanged -= HandleEnableLogGradingChanged;
        //    }
        //}

        //void HandleEnableLogGradingChanged(object sender, EventArgs e)
        //{
        //    LogColumnVisable = DataService.EnableLogGrading;
        //}

        //#endregion DataService

        //#region AppSettings

        //ApplicationSettings _appSettings;

        //public ApplicationSettings AppSettings
        //{
        //    get { return _appSettings; }
        //    set
        //    {
        //        OnAppSettingsChanging();
        //        _appSettings = value;
        //        OnAppSettingsChanged();
        //    }
        //}

        //private void OnAppSettingsChanged()
        //{
        //    if (_appSettings != null)
        //    {
        //        _appSettings.CruisersChanged += Settings_CruisersChanged;
        //    }
        //}

        //private void OnAppSettingsChanging()
        //{
        //    if (_appSettings != null)
        //    {
        //        _appSettings.CruisersChanged -= Settings_CruisersChanged;
        //    }
        //}

        //#endregion AppSettings

        //public LayoutPlotLogic ViewLogicController { get; set; }

        //public PlotStratum Stratum { get { return ViewLogicController.Stratum; } }

        public InputPanel Sip
        {
            get
            {
                return _dataGrid.SIP;
            }
            set
            {
                _dataGrid.SIP = value;
            }
        }

        public LayoutPlot()
        {
            InitializeComponent();
            InitializePlotNavIcons();

            //Setup Plot Nav Bar
            this._addPlotButton.Text = "+";
            this._deletePlotButton.Text = "-";
            this._plotInfoButton.Text = "i";
            this._expandGridButton.ImageIndex = 0;
            this._expandGridButton.ButtonImageLayout = ImageLayout.Tile;
            this._nextPlotButton.ButtonImageLayout = ImageLayout.Zoom;
            this._gotoLastPlotButton.ButtonImageLayout = ImageLayout.Zoom;
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

            DataGridAdjuster.InitializeGrid(this._dataGrid);

            this._dataGrid.CellValidating += new EditableDataGridCellValidatingEventHandler(_dataGrid_CellValidating);
            this._dataGrid.CellValueChanged += new EditableDataGridCellValueChangedEventHandler(this._dataGrid_CellValueChanged);
            //this._dataGrid.DataSource = typeof(FSCruiserV2.Logic.TreeVM);//_BS_Trees;
            this._dataGrid.Click += new EventHandler(_dataGrid_Click);
            this._dataGrid.ReadOnly = true;
            this._dataGrid.AllowUserToAddRows = false;

            this.Dock = DockStyle.Fill;
        }

        public LayoutPlot(IDataEntryDataService dataService
            , ISampleSelectorRepository sampleSelectorRepository
            , ApplicationSettings appSettings
            , ISoundService soundService
            , IViewController viewController
            , PlotStratum stratum) : this()
        {
            DataService = dataService;
            AppSettings = appSettings;

            this.ViewLogicController = new LayoutPlotLogic(stratum,
                this,
                dataService,
                soundService,
                DialogService.Instance,
                AppSettings,
                viewController,
                sampleSelectorRepository);

            //Setup DataGrid
            _tableStyle = stratum.InitializeTreeColumns(_dataGrid);
            
            _speciesColumn = _tableStyle.GridColumnStyles["TreeDefaultValue"] as EditableComboBoxColumn;
            _sgColumn = _tableStyle.GridColumnStyles["SampleGroup"] as EditableComboBoxColumn;
            _treeNumberColumn = _tableStyle.GridColumnStyles["TreeNumber"] as EditableTextBoxColumn;
            _initialsColoumn = _tableStyle.GridColumnStyles["Initials"] as EditableComboBoxColumn;
            _logsColumn = _tableStyle.GridColumnStyles["LogCountActual"] as DataGridButtonColumn;
            _kpiColumn = _tableStyle.GridColumnStyles["KPI"] as EditableTextBoxColumn;
            _errorsColumn = _tableStyle.GridColumnStyles["Error"] as DataGridTextBoxColumn;

            if (_logsColumn != null)
            {
                _logsColumn.Click += this.LogsClicked;
                LogColumnVisable = DataService.EnableLogGrading;
            }
            Settings_CruisersChanged(null, null);//initialize initials column

            InitializeTallyPanel();

            this.ViewLogicController.UpdateCurrentPlot();
        }

        void InitializeTallyPanel()
        {
            var stratum = this.ViewLogicController.Stratum;
            _tallyListPanel.SuspendLayout();

            if (stratum is FixCNTStratum)
            {
                var openFixCNTTallyButton = new Button()
                {
                    Text = "Open Tally Screen"
                    ,
                    Dock = DockStyle.Fill
                };
                openFixCNTTallyButton.Click += new EventHandler(openFixCNTTallyButton_Click);
                this._tallyListPanel.Controls.Add(openFixCNTTallyButton);
            }
            else if (stratum.IsSingleStage)
            {
                if (stratum.Method == "3PPNT")
                {
                    //no need to initialize any counts or samplegroup info for 3PPNT
                    IsGridExpanded = false;
                }
                else if (stratum.Counts.Count() > 0)
                {
                    foreach (CountTree count in stratum.Counts)
                    {
                        MakeTallyRow(_tallyListPanel, count);
                    }
                }
                else
                {
                    MakeSGList(stratum.SampleGroups, _tallyListPanel);
                }
            }
            else
            {
                foreach (CountTree count in stratum.Counts)
                {
                    MakeTallyRow(_tallyListPanel, count);
                }
            }
            //_tallyListPanel.AutoScroll = true;
            this._tallyListPanel.ResumeLayout();
        }

        void MakeSGList(IEnumerable<SampleGroup> sampleGroups, Panel container)
        {
            var list = sampleGroups.ToList();
            if (list.Count == 1)
            {
                SampleGroup sg = list[0];

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
                foreach (SampleGroup sg in list)
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

        Control MakeTallyRow(Control container, SubPop subPop)
        {
            var tallyButton = new SpeciesRow();
            tallyButton.SubPopulation = subPop;

            tallyButton.Click += new EventHandler(SpeciesButton_Click);
            tallyButton.Parent = container;
            tallyButton.Dock = DockStyle.Left;
            tallyButton.Width = SUB_POP_BUTTON_WIDTH;

            return tallyButton;
        }

        Control MakeTallyRow(Control container, CountTree count)
        {
            var row = new PlotTallyButton(count);
            row.SuspendLayout();

            row.TallyButtonClicked += new EventHandler(this.tallyRow_TallyButtonClicked);
            row.SettingsButtonClicked += new EventHandler(this.tallyRow_InfoButtonClicked);

            //row.Width = 90;
            row.Parent = container;

            row.Dock = DockStyle.Left;
            row.ResumeLayout(false);
            return row;
        }

        void InitializePlotNavIcons()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayoutPlot));
            this._imageList = new System.Windows.Forms.ImageList();

            this._imageList.Images.Clear();
            this._imageList.Images.Add(((System.Drawing.Image)(resources.GetObject("resource"))));
            this._imageList.Images.Add(((System.Drawing.Image)(resources.GetObject("resource1"))));
            this._imageList.Images.Add(((System.Drawing.Image)(resources.GetObject("resource2"))));
            this._imageList.Images.Add(((System.Drawing.Image)(resources.GetObject("resource3"))));
            this._imageList.Images.Add(((System.Drawing.Image)(resources.GetObject("resource4"))));
            this._imageList.Images.Add(((System.Drawing.Image)(resources.GetObject("resource5"))));

            this._gotoLastPlotButton.ImageIndex = 4;
            this._gotoLastPlotButton.ImageList = this._imageList;

            this._prevPlotButton.ImageIndex = 1;
            this._prevPlotButton.ImageList = this._imageList;

            this._nextPlotButton.ImageIndex = 2;
            this._nextPlotButton.ImageList = this._imageList;

            this._gotoFirstPlotButton.ImageIndex = 5;
            this._gotoFirstPlotButton.ImageList = this._imageList;

            this._expandGridButton.ImageIndex = -1;
            this._expandGridButton.ImageList = this._imageList;
        }

        void UpdateSpeciesColumn(Tree tree)
        {
            if (_speciesColumn != null)
            {
                _speciesColumn.DataSource = tree.ReadValidTDVs();
            }
        }

        void UpdateSampleGroupColumn(Tree tree)
        {
            if (_sgColumn != null)
            {
                _sgColumn.DataSource = tree.ReadValidSampleGroups();
            }
        }

        #region event handlers

        void _expandGridButton_Click(object sender, EventArgs e)
        {
            IsGridExpanded = !IsGridExpanded;
        }

        protected void LogsClicked(ButtonCellClickEventArgs e)
        {
            Tree tree;

            try
            {
                tree = this.ViewLogicController.CurrentTree;
            }
            catch { return; }
            if (tree == null) { return; }

            ShowLogs(tree);
        }

        void _dataGrid_CellValidating(object sender, EditableDataGridCellValidatingEventArgs e)
        {
            Tree tree = null;
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
                var newSG = e.Value as SampleGroupDO;
                tree.HandleSampleGroupChanging(newSG);

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
                    long newTreeNum = (long)e.Value;
                    if (tree.TreeNumber != newTreeNum
                    && !this.ViewLogicController.CurrentPlot.IsTreeNumberAvalible(newTreeNum))
                    {
                        DialogService.ShowMessage("Tree Number already exists");
                        e.Cancel = true;
                    }
                }
                catch
                {
                    e.Cancel = true;
                }
            }
        }

        void _dataGrid_CellValueChanged(object sender, EditableDataGridCellEventArgs e)
        {
            Tree tree = this.ViewLogicController.CurrentTree;
            if (tree == null || e.Column == null) { return; }
            if (e.Column == _sgColumn)
            {
                tree.HandleSampleGroupChanged();
                this.UpdateSpeciesColumn(tree);
            }
            else if (e.Column == _speciesColumn)
            {
                var newTDV = _speciesColumn.EditComboBox.SelectedItem as TreeDefaultValueDO;
                tree.HandleSpeciesChanged(newTDV);
            }
        }

        void _dataGrid_Click(object sender, EventArgs e)
        {
            if (IsGridExpanded == false)
            {
                IsGridExpanded = true;
            }
        }

        //ocures during a FIX or PNT cruise, when user clicks a Sample Group.
        //Expands a list of species in the sample group
        void sgButton_Click(object sender, EventArgs e)
        {
            Button sgButton = (Button)sender;
            Panel spContainer = (Panel)sgButton.Tag;
            spContainer.Visible = !spContainer.Visible;
        }

        void SpeciesButton_Click(object sender, EventArgs e)
        {
            if (!this.ViewLogicController.EnsureCurrentPlotWorkable()) { return; }

            var button = sender as SpeciesRow;
            if (button == null) { return; }

            var subPop = button.SubPopulation;
            ViewLogicController.AddTree(subPop);
        }

        #region plot nav events

        void _gotoFirstPlotButton_Click(object sender, EventArgs e)
        {
            this.ViewLogicController.SelectFirstPlot();
        }

        void _prevPlotButton_Click(object sender, EventArgs e)
        {
            this.ViewLogicController.SelectPreviousPlot();
        }

        void _nextPlotButton_Click(object sender, EventArgs e)
        {
            this.ViewLogicController.SelectNextPlot();
        }

        void _gotoLastPlotButton_Click(object sender, EventArgs e)
        {
            this.ViewLogicController.SelectLastPlot();
        }

        void _addPlotButton_Click(object sender, EventArgs e)
        {
            this.ViewLogicController.HandleAddPlot();
            this.IsGridExpanded = false;
        }

        private void _deletePlotButton_Click(object sender, EventArgs e)
        {
            this.ViewLogicController.HandleDeletePlot();
        }

        void _plotInfoButton_Click(object sender, EventArgs e)
        {
            ShowCurrentPlotInfo();
        }

        #endregion plot nav events

        #endregion event handlers

        #region IPlotLayout

        public void BindPlotData(BindingSource plotBS)
        {
            this._plotSelectCB.DataSource = plotBS;
        }

        public void BindTreeData(BindingSource treeBS)
        {
            this._dataGrid.DataSource = treeBS;
        }

        public void HandleCurrentTreeChanged(Tree tree)
        {
            UpdateSampleGroupColumn(tree);
            UpdateSpeciesColumn(tree);
        }

        public void RefreshTreeView(Plot currentPlot)
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

        public void ViewEndEdit()
        {
            this._dataGrid.EndEdit();
        }

        #endregion IPlotLayout

        #region ITreeView

        public bool ErrorColumnVisable
        {
            get
            {
                return _errorsColumn != null
                    && _errorsColumn.Width > 0;
            }
            set
            {
                if (_errorsColumn != null)
                {
                    _errorsColumn.Width = (value) ? Screen.PrimaryScreen.WorkingArea.Width : -1;
                }
            }
        }

        public bool LogColumnVisable
        {
            get
            {
                return _logsColumn != null
                    && _logsColumn.Width > 0;
            }
            set
            {
                if (_logsColumn != null)
                {
                    _logsColumn.Width = (value) ? Constants.LOG_COLUMN_WIDTH : -1;
                }
            }
        }

        public void MoveHomeField()
        {
            this._dataGrid.MoveFirstEmptyCell();
        }

        #endregion ITreeView

        #region ITallyView
        public bool HotKeyEnabled
        {
            get
            {
                return !this._isGridExpanded;
            }
        }
        #endregion ITallyView
    }
}