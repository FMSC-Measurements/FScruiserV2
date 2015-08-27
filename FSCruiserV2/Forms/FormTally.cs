using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FSCruiserV2.Logic;
using FSCruiserV2.Logic.DataObjectExtensionMethods;
using CruiseDAL.DataObjects;
using FMSC.Sampling;

namespace FSCruiserV2.Forms
{
    public partial class FormTally : Form
    {
        protected ApplicationController Controller { get; set; }
        public CuttingUnitDO Unit { get; set; }
        public List<CountTreeDO> Counts { get; set; }

        public FormTally(ApplicationController controller, CuttingUnitDO unit)
        {
            this.Controller = controller;
            this.Unit = unit;
            InitializeComponent();
            //PopulateTallies();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            PopulateTallies();
            this.treeDOBindingSource.DataSource = Controller.TreeList;

        }

        private void PopulateTallies()
        {
            this.SuspendLayout();
            //read all counts in unit
            this.Counts = Controller._cDal.Read<CountTreeDO>(CruiseDAL.Schema.COUNTTREE._NAME, "WHERE CuttingUnit_CN = ?", Unit.CuttingUnit_CN);
            //create a list of sample groups in unit
            List<CountTreeDO> sgGroups = Controller._cDal.Read<CountTreeDO>(CruiseDAL.Schema.COUNTTREE._NAME, "WHERE CuttingUnit_CN = ? Group BY SampleGroup_CN", Unit.CuttingUnit_CN);
            List<List<CountTreeDO>> countsBySG = new List<List<CountTreeDO>>(sgGroups.Count);
            //read counts by  sample group
            foreach (CountTreeDO ct in sgGroups)
            {
                List<CountTreeDO> list = Controller._cDal.Read<CountTreeDO>(CruiseDAL.Schema.COUNTTREE._NAME, "WHERE CuttingUnit_CN = ? AND  SampleGroup_CN = ?", Unit.CuttingUnit_CN, ct.SampleGroup_CN);
                countsBySG.Add(list);
            }

            //make list of tallies for counts grouped by sample group
            foreach (List<CountTreeDO> group in countsBySG)
            {
                Panel sgGroupPanel = new Panel();
                sgGroupPanel.Parent = this._leftContentPanel;
                sgGroupPanel.Dock = DockStyle.Top;


                foreach (CountTreeDO count in group)
                {
                    MakeSampleSelecter(count);
                    MakeTallyRow(sgGroupPanel, count);
                }
                Label sgGroupTitle = new Label();
                sgGroupTitle.Parent = sgGroupPanel;
                sgGroupTitle.Dock = DockStyle.Top;
                sgGroupTitle.Text = group[0].SampleGroup.Code;
                sgGroupTitle.BackColor = Color.Red;
            }
            this.ResumeLayout(true);

        }

        private SampleSelecter MakeSampleSelecter(CountTreeDO count)
        {
            SampleSelecter selecter;
            if (String.IsNullOrEmpty(count.SampleSelectorState))
            {
                int frequency = (int)count.SampleGroup.SamplingFrequency;
                frequency = 10;
                selecter = new FMSC.Sampling.BlockSelecter(frequency, 0);
                selecter.Count = (int)count.TreeCount;
            }
            else
            {

                selecter = ApplicationController.DeserializeCountSampleState(count);
            }
            if (selecter.Count != count.TreeCount)
            {
                throw new InvalidOperationException("selecter.Count != count.TreeCount");
            }

            count.Tag = selecter;
            return selecter;
        }

        private TallyRow MakeTallyRow(Control container, CountTreeDO count)
        {
            TallyRow row = new TallyRow();
            row.DiscriptionLabel.Text = count.Tally.Description;
            row.TallyButton.Click += new EventHandler(TallyButton_Click);
            row.SettingsButton.Click += new EventHandler(SettingsButton_Click);

            SampleSelecter sampler = count.Tag as SampleSelecter;
            row.TallyButton.Text = sampler.Count.ToString();

            row.Tag = count;

            row.Parent = container;
            row.Dock = DockStyle.Top;
            return row;
        }


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            //this.Controller.SaveCounts();
        }


        void SettingsButton_Click(object sender, EventArgs e)
        {
            Button settingsbutton = (Button)sender; Button button = (Button)sender;
            TallyRow row = (TallyRow)button.Parent;
            CountTreeDO count = (CountTreeDO)row.Tag;
            Controller.ShowTallySettings(count);
            row.DiscriptionLabel.Text = count.Tally.Description;

        }

        void TallyButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            TallyRow row = (TallyRow)button.Parent;
            CountTreeDO count = (CountTreeDO)row.Tag;
            SampleSelecter sampler = (SampleSelecter)count.Tag;

            bool isNextSample = sampler.Next();
            button.Text = sampler.Count.ToString();

            if (isNextSample)
            {
                button.Text += "*";
                //Controller.ShowInputTreeData(count);
            }
        }
    }
}