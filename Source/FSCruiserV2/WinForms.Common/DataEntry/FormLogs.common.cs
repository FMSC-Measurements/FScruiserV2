using FScruiser.Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

#if NetCF
using FMSC.Controls;
#endif

namespace FSCruiser.WinForms.DataEntry
{
    public partial class FormLogs
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
#if !NetCF
                _dataGrid.SuspendLayout();
                _dataGrid.Columns.AddRange(
                    DataService.Stratum.MakeLogColumns().ToArray());
                _dataGrid.ResumeLayout();

                _logNumColumn = _dataGrid.Columns[CruiseDAL.Schema.LOG.LOGNUMBER] as DataGridViewTextBoxColumn;
#else
                DataGridTableStyle tableStyle = DataService.Stratum.InitializeLogColumns(_dataGrid);
                _logNumColumn = tableStyle.GridColumnStyles[CruiseDAL.Schema.LOG.LOGNUMBER] as EditableTextBoxColumn;
#endif

                _BS_Logs.DataSource = DataService.Logs;
                _treeDesLbl.Text = DataService.LogLevelDiscription;
            }
        }

        #endregion DataService

        public FormLogs(ILogDataService dataService) : this()
        {
            DataService = dataService;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _dataGrid.Focus();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            this._dataGrid.EndEdit();

            if (!DataService.ValidateLogGrades())
            {
                if (!DialogService.AskYesNo("One or more logs have failed audits", "Continue? - Warning"))
                {
                    e.Cancel = true;
                    return;
                }
            }

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
    }
}