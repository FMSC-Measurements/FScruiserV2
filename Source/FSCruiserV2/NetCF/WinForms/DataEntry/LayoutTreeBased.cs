using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
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

            this._untallyButton.Click += new System.EventHandler(this.OnUntallyButtonClicked);
        }

        public LayoutTreeBased(IApplicationController controller
            , IDataEntryDataService dataService
            , FormDataEntryLogic dataEntryController)
            : this()
        {
            base.Initialize(controller
                , dataService
                , dataEntryController
                , _leftContentPanel);
        }

        public override Control MakeTallyRow(Control container, SubPop subPop)
        {
            Button tallyButton = new Button();
            tallyButton.SuspendLayout();
            tallyButton.Text = subPop.TDV.Species;
            tallyButton.Click += new EventHandler(base.OnSpeciesButtonClick);
            tallyButton.Tag = subPop;
            tallyButton.Parent = container;
            tallyButton.Dock = DockStyle.Top;
            tallyButton.ResumeLayout(false);
            return tallyButton;
        }

        public override void MakeSGList(IEnumerable<SampleGroup> sampleGroups, Panel container)
        {
            var list = sampleGroups.ToList();
            if (list.Count == 1)
            {
                SampleGroup sg = list[0];

                if (sg.TreeDefaultValues.IsPopulated == false)
                {
                    sg.TreeDefaultValues.Populate();
                }
                foreach (TreeDefaultValueDO tdv in sg.TreeDefaultValues)
                {
                    SubPop subPop = new SubPop(sg, tdv);
                    MakeTallyRow(container, subPop);
                }
            }
            else
            {
                foreach (SampleGroup sg in list)
                {
                    Button sgButton = new Button();
                    Panel spContainer = new Panel();

                    if (sg.TreeDefaultValues.IsPopulated == false)
                    {
                        sg.TreeDefaultValues.Populate();
                    }
                    foreach (TreeDefaultValueDO tdv in sg.TreeDefaultValues)
                    {
                        SubPop subPop = new SubPop(sg, tdv);
                        MakeTallyRow(spContainer, subPop);
                    }

                    spContainer.Parent = container;
                    spContainer.Dock = DockStyle.Top;
                    spContainer.Visible = false;

                    sgButton.Parent = container;
                    sgButton.Dock = DockStyle.Top;
                    sgButton.Click += new EventHandler(base.OnSgButtonClick);
                }
            }
        }
    }
}