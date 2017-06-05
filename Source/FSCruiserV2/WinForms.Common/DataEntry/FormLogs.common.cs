using FScruiser.Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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

        #endregion DataService

        public FormLogs(ILogDataService dataService) : this()
        {
            DataService = dataService;
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