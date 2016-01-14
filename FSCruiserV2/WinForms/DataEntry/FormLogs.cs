using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FSCruiser.Core;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class FormLogs : Form
    {
        public IApplicationController Controller { get; protected set; }
        private TreeDO _currentTree;
        private BindingList<LogDO> _logs;
        private int _logNumIndex;
        DataGridViewTextBoxColumn _logNumColumn;


        public FormLogs(IApplicationController controller, long stratum_cn)
        {
            this.Controller = controller;
            InitializeComponent();

            this._dataGrid.AutoGenerateColumns = false;
            this._dataGrid.SuspendLayout();
            this._dataGrid.Columns.AddRange(
                DataGridAdjuster.MakeLogColumns(controller._cDal, stratum_cn).ToArray());
            this._dataGrid.ResumeLayout();

            _logNumColumn = _dataGrid.Columns[CruiseDAL.Schema.LOG.LOGNUMBER] as DataGridViewTextBoxColumn;
        }


        public DialogResult ShowDialog(TreeVM tree)
        {
            this._currentTree = tree;
            this._treeDesLbl.Text = tree.GetLogLevelDescription();

            this._logs = new BindingList<LogDO>(tree.QueryLogs());
            this._logNumIndex = tree.ReadHighestLogNumber();
            //if (_logs.Count == 0)
            //{
            //    _logNumIndex = 0;
            //}
            //else
            //{
            //    try
            //    {
            //        _logNumIndex = Convert.ToInt32(this._logs[_logs.Count - 1].LogNumber);
            //    }
            //    catch
            //    {
            //        _logNumIndex = 0;
            //    }
            //}
            //this._BS_Logs.DataSource = this._logs;
            this._dataGrid.DataSource = this._logs;
            this._dataGrid.Focus();
            tree.LogCountDirty = true;
            return this.ShowDialog();
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
            newLog.LogNumber = (++_logNumIndex).ToString();

            this._logs.Add(newLog);
            this._dataGrid.GoToLastRow();
            this._dataGrid.GoToFirstColumn();
            return newLog;
        }

        private bool SetLogNumberSequance(int start)
        {
            foreach (LogDO log in this._logs)
            {
                if (log.LogNumber == start.ToString())
                {
                    return false;
                }
            }
            if (start > this._logNumIndex)
            {
                this._logNumIndex = start;

            }
            return true;
        }

        //private void _BS_Logs_AddingNew(object sender, AddingNewEventArgs e)
        //{
        //    e.NewObject = AddLogRec();
        //}

        void _dataGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == this._logNumColumn.Index)
            {
                try
                {
                    var cell = _dataGrid[e.ColumnIndex, e.RowIndex];
                    var cellValue = cell.ParseFormattedValue(e.FormattedValue, cell.Style, null, null);
                    if (cellValue == null || !(cellValue is Int32))
                    {
                        e.Cancel = true;
                        return;
                    }

                    int newLogNumber = (int)cellValue ;
                    if (!this.SetLogNumberSequance(newLogNumber))
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
            LogDO log = this._BS_Logs.Current as LogDO;
            if (log == null) { return; }

            try
            {
                if (Convert.ToInt32(log.LogNumber) == _logNumIndex - 1)
                {
                    _logNumIndex--;
                }
            }
            catch (System.FormatException) { }//do nothing}

            log.Delete();
            this._logs.Remove(log);
        }

        private void _BTN_add_Click(object sender, EventArgs e)
        {
            AddLogRec();
        }
    }
}
