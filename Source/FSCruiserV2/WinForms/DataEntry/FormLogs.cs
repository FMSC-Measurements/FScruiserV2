using System;
using System.ComponentModel;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FSCruiser.Core;
using FSCruiser.Core.Models;
using FScruiser.Core.Services;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class FormLogs : Form
    {
        public IApplicationController Controller { get; protected set; }

        TreeDO _currentTree;
        BindingList<LogDO> _logs;
        DataGridViewTextBoxColumn _logNumColumn;

        public FormLogs(IApplicationController controller, Stratum stratum)
        {
            Controller = controller;
            InitializeComponent();

            _dataGrid.AutoGenerateColumns = false;
            _dataGrid.SuspendLayout();
            _dataGrid.Columns.AddRange(
                stratum.MakeLogColumns().ToArray());
            _dataGrid.ResumeLayout();

            _logNumColumn = _dataGrid.Columns[CruiseDAL.Schema.LOG.LOGNUMBER] as DataGridViewTextBoxColumn;
        }

        public DialogResult ShowDialog(Tree tree)
        {
            if (tree == null) { throw new ArgumentNullException(nameof(tree)); }

            _logs = new BindingList<LogDO>(tree.LoadLogs());

            _currentTree = tree;
            _treeDesLbl.Text = tree.LogLevelDiscription;

            _dataGrid.DataSource = _logs;
            _dataGrid.Focus();

            var result = ShowDialog();
            tree.LogCountDirty = true;
            return result;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            for (int i = 0; i < _dataGrid.RowCount; i++)
            {
                _dataGrid.CurrentCell = _dataGrid[_dataGrid.CurrentCellAddress.X, i];
                if (_dataGrid.MoveFirstEmptyCell())
                {
                    break;
                }
            }
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
            _dataGrid.EndEdit();

            try
            {
                //this._currentTree.Save();//tree is saved before entering log screen.
                foreach (LogDO log in _logs)
                {
                    //log.Tree = this._currentTree;
                    Controller.DataStore.Save(log
                        , FMSC.ORM.Core.SQL.OnConflictOption.Fail
                        , false);
                }
            }
            catch (Exception)
            {
                e.Cancel = !DialogService.AskYesNo("Opps, logs weren't saved. Would you like to abort?", String.Empty);
            }
        }

        private LogDO AddLogRec()
        {
            LogDO newLog = new LogDO(Controller.DataStore);
            newLog.Tree_CN = _currentTree.Tree_CN;
            newLog.LogNumber = (GetHighestLogNum() + 1).ToString();

            _logs.Add(newLog);
            _dataGrid.GoToLastRow();
            _dataGrid.GoToFirstColumn();
            return newLog;
        }

        void _dataGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == _logNumColumn.Index)
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
                        if (!IsLogNumAvalible(newLogNumber))
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
            if (_dataGrid.CurrentRow == null) { return; }
            LogDO log = _dataGrid.CurrentRow.DataBoundItem as LogDO;
            if (log == null) { return; }

            log.Delete();
            _logs.Remove(log);
        }

        private void _BTN_add_Click(object sender, EventArgs e)
        {
            AddLogRec();
        }
    }
}