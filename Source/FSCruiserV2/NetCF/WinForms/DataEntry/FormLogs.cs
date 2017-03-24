using System;
using System.ComponentModel;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FMSC.Controls;
using FSCruiser.Core;
using FSCruiser.Core.Models;
using FScruiser.Core.Services;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class FormLogs : FMSC.Controls.CustomForm
    {
        #region DataService
        ILogDataService _dataService;
        public ILogDataService DataService 
        {
            get { return _dataService; }
            set
            {
                OnDataServiceChanging();
                _dataService = value;
                OnDataServiceChanged();
            }
        }

        void OnDataServiceChanging()
        {
            
        }

        void OnDataServiceChanged()
        {
            if (DataService != null)
            {
                DataGridTableStyle tableStyle = DataService.Stratum.InitializeLogColumns(_dataGrid);
                _logNumColumn = tableStyle.GridColumnStyles[CruiseDAL.Schema.LOG.LOGNUMBER] as EditableTextBoxColumn;
            }
        }
        #endregion

        private EditableTextBoxColumn _logNumColumn;

        private Microsoft.WindowsCE.Forms.InputPanel _sip;

        public FormLogs() : base()
        {
            InitializeComponent();

            KeyPreview = true;

            if (ViewController.PlatformType == PlatformType.WinCE)
            {
                this.WindowState = FormWindowState.Maximized;
                this._ceControlPanel.Visible = true;
                this.Menu = null;
                this.mainMenu1.Dispose();
                this.mainMenu1 = null;
            }
            else if (ViewController.PlatformType == PlatformType.WM)
            {
                this._sip = new Microsoft.WindowsCE.Forms.InputPanel();
                this.components.Add(_sip);
                this._dataGrid.SIP = this._sip;
            }

            this._dataGrid.CellValidating += new EditableDataGridCellValidatingEventHandler(_dataGrid_CellValidating);

            DataGridAdjuster.InitializeGrid(this._dataGrid);
        }

        public FormLogs(ILogDataService dataService)
            : this()
        {
            DataService = dataService;
        }

        //void _sip_EnabledChanged(object sender, EventArgs e)
        //{
        //    this._sipPlaceHolder.Height = (this._sip.Enabled) ? this._sip.Bounds.Height : 0;
        //}

        #region overridden methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _BS_Logs.DataSource = DataService.Logs;
            _treeDesLbl.Text = DataService.Tree.LogLevelDiscription;

            _dataGrid.Focus();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            for (int i = 0; i < _dataGrid.RowCount; i++)
            {
                _dataGrid.CurrentRowIndex = i;
                if (_dataGrid.MoveFirstEmptyCell())
                {
                    break;
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Handled == true) { return; }
            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                this.Close();
            }
            if (e.KeyCode == Keys.Down)
            {
                //this.AddLogRec();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            this._dataGrid.EndEdit();

            try
            {
                DataService.Save();
            }
            catch (Exception)
            {
                e.Cancel = !DialogService.AskYesNo("Opps, logs weren't saved. Would you like to abort?"
                    , String.Empty);
            }
        }

        #endregion overridden methods

        #region event handlers

        void _addBtn_Click(object sender, EventArgs e)
        {
            DataService.AddLogRec();
            _BS_Logs.ResetBindings(false);
        }

        void _doneBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void _deleteBtn_Click(object sender, EventArgs e)
        {
            //if (this._dataGrid.CurrentRowIndex < 0
            //    || this._dataGrid.CurrentRowIndex >= DataService.Logs.Count)
            //{ return; }

            Log log = _BS_Logs.Current as Log;
            if (log == null) { return; }

            DataService.DeleteLog(log);

            _BS_Logs.ResetBindings(false);
        }

        void _dataGrid_CellValidating(object sender, EditableDataGridCellValidatingEventArgs e)
        {
            if (e.Column == this._logNumColumn)
            {
                var cellValue = e.Value as string;

                int newLogNumber;
                if (TryParseInt(cellValue, out newLogNumber))
                {
                    if (!DataService.IsLogNumAvalible(newLogNumber))
                    {
                        e.Cancel = true;
                    }
                }
                else
                {
                    //if value not a number, cancel
                    e.Cancel = true;
                }
            }
        }

        bool TryParseInt(string value, out int result)
        {
            try
            {
                result = int.Parse(value);
                return true;
            }
            catch
            {
                result = default(int);
                return false;
            }
        }

        #endregion event handlers

    }
}