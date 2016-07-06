using System;
using System.ComponentModel;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FMSC.Controls;
using FSCruiser.Core;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class FormLogs : FMSC.Controls.CustomForm
    {
        //        public FormLogs(IApplicationController controller)
        //        {
        //            this.Controller = controller;
        //            InitializeComponent();
        //#if WindowsCE
        //            this.WindowState = FormWindowState.Maximized;
        //#endif

        //        }

        private EditableTextBoxColumn _logNumColumn;

        private Microsoft.WindowsCE.Forms.InputPanel _sip;

        public FormLogs()
        {
            this.InitializeComponent();
        }

        public FormLogs(IApplicationController controller, StratumModel stratum)
            : base()
        {
            this.Controller = controller;
            InitializeComponent();
            this.KeyPreview = true;

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
            DataGridTableStyle tableStyle = DataGridAdjuster.InitializeLogColumns(stratum, _dataGrid);

            _logNumColumn = tableStyle.GridColumnStyles[CruiseDAL.Schema.LOG.LOGNUMBER] as EditableTextBoxColumn;
        }

        //void _sip_EnabledChanged(object sender, EventArgs e)
        //{
        //    this._sipPlaceHolder.Height = (this._sip.Enabled) ? this._sip.Bounds.Height : 0;
        //}

        public IApplicationController Controller { get; protected set; }

        private TreeDO _currentTree;
        private BindingList<LogDO> _logs;

        public DialogResult ShowDialog(TreeVM tree)
        {
            this._currentTree = tree;

            this._logs = new BindingList<LogDO>(tree.LoadLogs());

            this._treeDesLbl.Text = tree.LogLevelDiscription;

            this._BS_Logs.DataSource = this._logs;
            this._dataGrid.DataSource = this._BS_Logs;
            this._dataGrid.Focus();
            tree.LogCountDirty = true;
            return this.ShowDialog();
        }

        #region overridden methods

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
                //this._currentTree.Save();//tree is saved before entering log screen.
                foreach (LogDO log in this._logs)
                {
                    //log.Tree = this._currentTree;
                    this.Controller._cDal.Save(log
                        , FMSC.ORM.Core.SQL.OnConflictOption.Fail
                        , false);
                }
                //this.Controller._cDal.Save(this._logs);
            }
            catch (Exception)
            {
                e.Cancel = !this.Controller.ViewController.AskYesNo("Opps, logs weren't saved. Would you like to abort?", "", MessageBoxIcon.None);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }

        #endregion overridden methods

        #region event handlers

        private void _addBtn_Click(object sender, EventArgs e)
        {
            AddLogRec();
        }

        private void _doneBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void _deleteBtn_Click(object sender, EventArgs e)
        {
            if (this._dataGrid.CurrentRowIndex < 0
                || this._dataGrid.CurrentRowIndex >= this._logs.Count)
            { return; }

            LogDO log = this._logs[this._dataGrid.CurrentRowIndex] as LogDO;
            if (log == null) { return; }

            log.Delete();
            this._logs.Remove(log);
        }

        private void _BS_Logs_AddingNew(object sender, AddingNewEventArgs e)
        {
            e.NewObject = AddLogRec();
        }

        void _dataGrid_CellValidating(object sender, EditableDataGridCellValidatingEventArgs e)
        {
            if (e.Column == this._logNumColumn)
            {
                var cellValue = e.Value as string;

                int newLogNumber;
                if (TryParseInt(cellValue, out newLogNumber))
                {
                    if (!this.IsLogNumAvalible(newLogNumber))
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

        #endregion event handlers

        LogDO AddLogRec()
        {
            LogDO newLog = new LogDO(this.Controller._cDal);
            newLog.Tree_CN = _currentTree.Tree_CN;
            newLog.LogNumber = (GetHighestLogNum() + 1).ToString();

            this._logs.Add(newLog);
            this._dataGrid.CurrentRowIndex = this._dataGrid.RowCount - 1;
            this._dataGrid.MoveFirstEmptyCell();
            return newLog;
        }

        int GetHighestLogNum()
        {
            int highest = 0;
            foreach (var log in _logs)
            {
                int logNum = 0;
                if (TryParseInt(log.LogNumber, out logNum))
                {
                    highest = Math.Max(highest, logNum);
                }
            }
            return highest;
        }

        bool IsLogNumAvalible(int newLogNum)
        {
            foreach (var log in _logs)
            {
                int logNum = 0;
                if (TryParseInt(log.LogNumber, out logNum))
                {
                    if (newLogNum == logNum)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        bool TryParseInt(string s, out int result)
        {
            try
            {
                result = int.Parse(s);
                return true;
            }
            catch
            {
                result = default(int);
                return false;
            }
        }

        //private void LogColumn_Validating(object sender, CancelEventArgs e)
        //{
        //    TextBox tb = (TextBox)sender;
        //    try
        //    {
        //        int newLogNumber = Convert.ToInt32(tb.Text);
        //        if (!this.SetLogNumberSequance(newLogNumber))
        //        {
        //            tb.Undo();
        //        }
        //    }
        //    catch
        //    {
        //        tb.Undo();
        //    }

        //}
    }
}