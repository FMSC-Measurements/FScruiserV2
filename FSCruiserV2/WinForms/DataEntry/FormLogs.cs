using System;
using System.ComponentModel;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FSCruiser.Core;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class FormLogs : Form
    {
        public IApplicationController Controller { get; protected set; }

        TreeDO _currentTree;
        BindingList<LogDO> _logs;
        DataGridViewTextBoxColumn _logNumColumn;

        public FormLogs(IApplicationController controller, StratumVM stratum)
        {
            this.Controller = controller;
            InitializeComponent();

            this._dataGrid.AutoGenerateColumns = false;
            this._dataGrid.SuspendLayout();
            this._dataGrid.Columns.AddRange(
                stratum.MakeLogColumns().ToArray());
            this._dataGrid.ResumeLayout();

            _logNumColumn = _dataGrid.Columns[CruiseDAL.Schema.LOG.LOGNUMBER] as DataGridViewTextBoxColumn;
        }

        public DialogResult ShowDialog(TreeVM tree)
        {
            if (tree == null) { throw new ArgumentNullException("tree"); }

            this._logs = new BindingList<LogDO>(tree.LoadLogs());

            this._currentTree = tree;
            this._treeDesLbl.Text = tree.LogLevelDiscription;

            this._dataGrid.DataSource = this._logs;
            this._dataGrid.Focus();
            tree.LogCountDirty = true;
            return this.ShowDialog();
        }

        int GetHighestLogNum()
        {
            int highest = 0;
            foreach (var log in _logs)
            {
                int logNum = 0;
                if (int.TryParse(log.LogNumber, out logNum))
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
                if (int.TryParse(log.LogNumber, out logNum))
                {
                    if (newLogNum == logNum)
                    {
                        return false;
                    }
                }
            }
            return true;
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
            }
            catch (Exception)
            {
                e.Cancel = !this.Controller.ViewController.AskYesNo("Opps, logs weren't saved. Would you like to abort?", "", MessageBoxIcon.None);
            }
        }

        private LogDO AddLogRec()
        {
            LogDO newLog = new LogDO(this.Controller._cDal);
            newLog.Tree_CN = _currentTree.Tree_CN;
            newLog.LogNumber = (GetHighestLogNum() + 1).ToString();

            this._logs.Add(newLog);
            this._dataGrid.GoToLastRow();
            this._dataGrid.GoToFirstColumn();
            return newLog;
        }

        void _dataGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == this._logNumColumn.Index)
            {
                try
                {
                    var cell = _dataGrid[e.ColumnIndex, e.RowIndex];
                    var cellValue = cell.ParseFormattedValue(
                        e.FormattedValue
                        , cell.Style
                        , null, null) as string;

                    int newLogNumber;
                    if (int.TryParse(cellValue, out newLogNumber))
                    {
                        if (!this.IsLogNumAvalible(newLogNumber))
                        {
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
                catch
                {
                    e.Cancel = true;
                }
            }
        }

        private void _BTN_delete_Click(object sender, EventArgs e)
        {
            if (this._dataGrid.CurrentRow == null) { return; }
            LogDO log = this._dataGrid.CurrentRow.DataBoundItem as LogDO;
            if (log == null) { return; }

            log.Delete();
            this._logs.Remove(log);
        }

        private void _BTN_add_Click(object sender, EventArgs e)
        {
            AddLogRec();
        }
    }
}