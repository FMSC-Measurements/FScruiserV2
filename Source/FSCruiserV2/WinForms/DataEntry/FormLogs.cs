﻿using System;
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
        DataGridViewTextBoxColumn _logNumColumn;

        public FormLogs()
        {
            InitializeComponent();
            base.StartPosition = FormStartPosition.CenterParent;
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
                        if (!DataService.IsLogNumAvalible(newLogNumber))
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
            var log = _dataGrid.CurrentRow.DataBoundItem as Log;
            if (log == null) { return; }

            DataService.DeleteLog(log);
            _BS_Logs.ResetBindings(false);
        }

        private void _BTN_add_Click(object sender, EventArgs e)
        {
            _dataGrid.EndEdit();//because when we reset the binding we don't want to lose our current edits
            DataService.AddLogRec();
            _BS_Logs.ResetBindings(false);//Raises ListChanged on the binding source with ListChangedType.Reset
            _BS_Logs.MoveLast();
            _dataGrid.MoveFirstEmptyCell();
        }

        private void _dataGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
    }
}