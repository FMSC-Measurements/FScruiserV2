using FScruiser.Core.Services;
using FSCruiser.Core;
using FSCruiser.Core.DataEntry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class LayoutTreeBased
    {
        LayoutTreeBased()
            : base()
        {
            InitializeComponent();
            Text = "Tally";

            ((System.ComponentModel.ISupportInitialize)(this._BS_tallyHistory)).BeginInit();
            this._tallyHistoryLB.DataSource = this._BS_tallyHistory;
            ((System.ComponentModel.ISupportInitialize)(this._BS_tallyHistory)).EndInit();

            this._untallyBTN.Click += new System.EventHandler(this.OnUntallyButtonClicked);
        }

        public LayoutTreeBased(IDataEntryDataService dataService
            , ApplicationSettings appSettings
            , FormDataEntryLogic dataEntryController)
            : this()
        {
            base.Initialize(dataService, appSettings, dataEntryController, _leftContentPanel);
        }

        protected override void UpdateUntallyButton()
        {
            var untallyKey = AppSettings.UntallyKeyStr;
            if (!string.IsNullOrEmpty(untallyKey))
            {
                _untallyBTN.Text = "Untally" + "(" + untallyKey + ")";
            }
            else
            {
                _untallyBTN.Text = "Untally";
            }
        }
    }
}