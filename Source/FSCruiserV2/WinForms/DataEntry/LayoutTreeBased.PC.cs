using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FSCruiser.Core;
using FSCruiser.Core.DataEntry;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;
using FScruiser.Core.Services;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class LayoutTreeBased : TreeBasedTallyView_Base, ITallyView
    {
        LayoutTreeBased()
            : base()
        {
            InitializeComponent();

            ((System.ComponentModel.ISupportInitialize)(this._BS_tallyHistory)).BeginInit();
            this._tallyHistoryLB.DataSource = this._BS_tallyHistory;
            ((System.ComponentModel.ISupportInitialize)(this._BS_tallyHistory)).EndInit();

            this._untallyBTN.Click += new System.EventHandler(this.OnUntallyButtonClicked);
        }

        public LayoutTreeBased(IApplicationController controller
            , IDataEntryDataService dataService
            , ApplicationSettings appSettings
            , FormDataEntryLogic dataEntryController)
            : this()
        {
            base.Initialize(controller, dataService, appSettings, dataEntryController, _leftContentPanel);
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