﻿using System;
using System.ComponentModel;
using System.Windows.Forms;
using FMSC.Sampling;
using FSCruiser.Core;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class FormTallySettings : Form
    {
        public FormTallySettings()
        {
            InitializeComponent();
        }

        private CountTree _count;

        IApplicationController Controller { get; set; }

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

        public FormTallySettings(IApplicationController controller)
        {
            this.InitializeComponent();
            this.Controller = controller;
        }

        public DialogResult ShowDialog(CountTree count)
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
            SampleSelecter sampler = count.SampleGroup.Sampler;
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
            }
            this._samplingMethod_TB.Text = samplingMethod;

            _count = count;
            DialogResult result = this.ShowDialog();
            _count = null;
            return result;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

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
                    _tallyCount_TB.Text = this._count.TreeCount.ToString();
                    e.Cancel = true;
                    return;
                }
                try
                {
                    newSumKPI = Convert.ToInt64(_SumKPI_TB.Text);
                }
                catch
                {
                    _SumKPI_TB.Text = this._count.SumKPI.ToString();
                    e.Cancel = true;
                    return;
                }

                if (this._count.TreeCount != newTreeCount || this._count.SumKPI != newSumKPI)
                {
                    bool skpi = EnableSumKPI;
                    if (MessageBox.Show(String.Format("Please confirm the following.\nTree Count: {0}{1}",
                            newTreeCount,
                            skpi ? String.Format("Sum KPI: {0}", newSumKPI) : String.Empty),
                            String.Format("Confirm {0}", skpi ? "Values" : "Tree Count"),
                        MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        e.Cancel = true;
                        return;
                    }
                }

                if (this._count.TreeCount != newTreeCount)
                {
                    this.Controller.LogTreeCountEdit(this._count, this._count.TreeCount, newTreeCount);
                    this._count.TreeCount = newTreeCount;
                }

                if (this._count.SumKPI != newSumKPI)
                {
                    this.Controller.LogSumKPIEdit(this._count, this._count.SumKPI, newSumKPI);
                    this._count.SumKPI = newSumKPI;
                }
            }
        }
    }
}