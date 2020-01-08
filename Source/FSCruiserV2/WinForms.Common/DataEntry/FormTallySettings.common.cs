using FMSC.Sampling;
using FScruiser.Core.Services;
using FScruiser.Services;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class FormTallySettings : Form
    {
        #region properties

        private ICountTreeDataService DataService { get; set; }

        private ISampleSelectorRepository SampleSelectorRepository { get; set; }

        public bool EnableTallyCount
        {
            set
            {
                this._tallyCount_TB.Visible = value;
                this._tallyCount_LBL.Visible = value;
            }
        }

        public bool EnableSumKPI
        {
            get { return this._SumKPI_TB.Visible; }
            set
            {
                this._SumKPI_TB.Visible = value;
                this._sumKPI_LBL.Visible = value;
            }
        }

        public bool EnableBigBAF
        {
            set
            {
                this._bigBAFTB.Visible = value;
                this._bigBAF_LBL.Visible = value;
            }
        }

        public bool EnableKZ
        {
            set
            {
                this._kzTB.Visible = value;
                this._kz_LBL.Visible = value;
            }
        }

        public bool EnableFrequency
        {
            set
            {
                this._frequencyTB.Visible = value;
                this._frequency_LBL.Visible = value;
            }
        }

        public bool EnableIFrequency
        {
            set
            {
                this._iFreq_LBL.Visible = value;
                this._iFreqTB.Visible = value;
            }
        }

        public bool EnableTotalTreeCount
        {
            set
            {
                this._totalTreeCount_LBL.Visible = value;
                this._totalTreeCount_TB.Visible = value;
            }
        }

        #endregion properties

        protected FormTallySettings()
        {
            InitializeComponent();

#if NetCF
            if (ViewController.PlatformType == FMSC.Controls.PlatformType.WinCE)
            {
                this.WindowState = FormWindowState.Maximized;
            }
#else
            StartPosition = FormStartPosition.CenterParent;
#endif
        }

        public FormTallySettings(ICountTreeDataService dataService, 
            ISampleSelectorRepository sampleSelectorRepository) : this()
        {
            DataService = dataService;
            SampleSelectorRepository = sampleSelectorRepository;

            InitializeData();
        }

        private void InitializeData()
        {
            var count = DataService.Count;
            if (count != null)
            {
                var method = count.SampleGroup.Stratum.Method;
                bool isPlot = Array.IndexOf(CruiseDAL.Schema.CruiseMethods.PLOT_METHODS, method) >= 0;
                bool isThreep = count.SampleGroup.Stratum.Is3P;

                this.EnableTallyCount = !isPlot;
                this.EnableBigBAF = isPlot;
                this.EnableIFrequency = !isPlot;
                this.EnableTotalTreeCount = !isPlot;

                this.EnableFrequency = !isThreep;
                this.EnableKZ = isThreep;
                this.EnableSumKPI = (isThreep && !isPlot);

                //this.comboBox1.SelectedItem = (count.Tag is BlockSelecter) ? TallyMode.Block.ToString() : (count.Tag is SRSSelecter) ? TallyMode.SRS.ToString() : (count.Tag is SystematicSelecter) ? TallyMode.Systematic.ToString() : "???";
                this._tallyDescription_TB.Text = count.Tally.Description;
                this._SGDescription_TB.Text = count.SampleGroup.Description;
                this._frequencyTB.Text = count.SampleGroup.ToString("1 in [SamplingFrequency]", null);
                this._kzTB.Text = count.SampleGroup.KZ.ToString();
                this._SumKPI_TB.Text = count.SumKPI.ToString();
                this._bigBAFTB.Text = count.SampleGroup.BigBAF.ToString();
                this._iFreqTB.Text = count.SampleGroup.InsuranceFrequency.ToString();
                this._tallyCount_TB.Text = count.TreeCount.ToString();
                long countsFromTrees = count.GetCountsFromTrees();
                this._countsFromTrees_TB.Text = countsFromTrees.ToString();
                this._totalTreeCount_TB.Text = (countsFromTrees + count.TreeCount).ToString();
                this._measureTrees_TB.Text = count.GetMeasureTreeCount().ToString();

                String samplingMethod = "Manual";

                var sgCode = count.SampleGroup.Code;
                var stCode = count.SampleGroup.Stratum.Code;
                var sampler = SampleSelectorRepository.GetSamplerBySampleGroupCode(stCode, sgCode);

                if (sampler != null)
                {
                    if (sampler is SystematicSelecter)
                    {
                        samplingMethod = "Systematic with random start";
                    }
                    else if (sampler is BlockSelecter)
                    {
                        samplingMethod = "Blocked";
                    }
                    else if (sampler is ThreePSelecter)
                    {
                        samplingMethod = "Three P";
                    }
                    else
                    { samplingMethod = "undefined"; }
                }
                this._samplingMethod_TB.Text = samplingMethod;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            var count = DataService.Count;

            if (!e.Cancel && this.DialogResult != DialogResult.Cancel)
            {
                long newTreeCount;
                long newSumKPI;
                try
                {
                    newTreeCount = Convert.ToInt64(_tallyCount_TB.Text);
                }
                catch
                {
                    _tallyCount_TB.Text = count.TreeCount.ToString();
                    e.Cancel = true;
                    return;
                }
                try
                {
                    newSumKPI = Convert.ToInt64(_SumKPI_TB.Text);
                }
                catch
                {
                    _SumKPI_TB.Text = count.SumKPI.ToString();
                    e.Cancel = true;
                    return;
                }

                //if (count.TreeCount != newTreeCount || count.SumKPI != newSumKPI)
                //{
                //    bool skpi = EnableSumKPI;
                //    if (MessageBox.Show(String.Format("Please confirm the following.\nTree Count: {0}{1}",
                //            newTreeCount,
                //            skpi ? String.Format("Sum KPI: {0}", newSumKPI) : String.Empty),
                //            String.Format("Confirm {0}", skpi ? "Values" : "Tree Count"),
                //        MessageBoxButtons.YesNo) != DialogResult.Yes)
                //    {
                //        e.Cancel = true;
                //        return;
                //    }
                //}

                if (count.TreeCount != newTreeCount)
                {
                    DataService.LogTreeCountEdit(count, count.TreeCount, newTreeCount);
                    count.TreeCount = newTreeCount;
                }

                if (count.SumKPI != newSumKPI)
                {
                    this.DataService.LogSumKPIEdit(count, count.SumKPI, newSumKPI);
                    count.SumKPI = newSumKPI;
                }

                var newDescription = _tallyDescription_TB.Text;
                if (count.Tally.Description != newDescription)
                {
                    count.Tally.Description = newDescription;
                    count.Tally.Save();
                }
            }
        }
    }
}