﻿using System;
using System.ComponentModel;
using System.Windows.Forms;
using FMSC.Sampling;
using FSCruiser.Core;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;
using FScruiser.Core.Services;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class Form3PPNTPlotInfo : Form
    {
        IPlotDataService DataService { get; set; }

        public Form3PPNTPlotInfo(IPlotDataService dataService)
        {
            DataService = dataService;
            InitializeComponent();

            if (ViewController.PlatformType == FMSC.Controls.PlatformType.WinCE)
            {
                this.WindowState = FormWindowState.Maximized;
                this._OKBtn.Visible = true;
            }
        }

        private bool _blockTBClick = false;
        Plot3PPNT _plot;
        PlotStratum _stratum;

        public DialogResult ShowDialog(Plot plot, PlotStratum stratum, bool allowEdit)
        {
            _plot = plot as Plot3PPNT;
            _BS_plot.DataSource = _plot;
            _stratum = stratum;

            this.DialogResult = DialogResult.OK;

            this._kz3ppnt_lbl.Text = stratum.KZ3PPNT.ToString();

            return this.ShowDialog();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (e.Cancel == true) { return; }
            if (this.DialogResult == DialogResult.Cancel) { return; }

            if (_plot.PlotNumber <= 0L)
            {
                MessageBox.Show("Plot Number Must Be Greater Than 0");
                e.Cancel = true;
            }
            else if (!_stratum.IsPlotNumberAvailable(_plot.PlotNumber))
            {
                MessageBox.Show("Plot Number Already Exists");
                e.Cancel = true;
            }
            if ((_plot.TreeCount == 0 && _plot.AverageHeight > 0)
                || (_plot.AverageHeight == 0 && _plot.TreeCount > 0))
            {
                MessageBox.Show("Invalid Tree Count or Average Height");
                e.Cancel = true;
                return;
            }

            _plot.StoreUserEnteredValues();
            if (_plot.TreeCount == 0 && _plot.AverageHeight == 0)
            {
                if (MessageBox.Show("Empty Plot?", null, MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1)
                    == DialogResult.Yes)
                {
                    _plot.IsNull = true;
                    _plot.Save();
                    return;
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }
            else
            {
                _plot.KPI = _plot.CalculateKPI();

                lock (_plot.DAL.TransactionSyncLock)
                {
                    _plot.DAL.BeginTransaction();
                    try
                    {
                        _plot.Save();

                        var kpi = (int)_plot.KPI;
                        var result = _stratum.SampleSelecter.Sample(kpi);

                        if (result == SampleResult.C)
                        {
                            SignalCountPlot();
                        }
                        else
                        {
                            _plot.CreateTrees(DataService);
                            SignalMeasurePlot();
                        }

                        _plot.DAL.CommitTransaction();
                    }
                    catch
                    {
                        _plot.DAL.RollbackTransaction();
                        throw;
                    }
                }
            }
        }

        private void SignalMeasurePlot()
        {
            Win32.MessageBeep(Win32.MB_ICONASTERISK);
            MessageBox.Show("Measure Plot");
        }

        private void SignalCountPlot()
        {
            Win32.MessageBeep(Win32.MB_ICONHAND);
            MessageBox.Show("Count Plot");
        }

        private void _cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void _gpsButton_Click(object sender, EventArgs e)
        {
        }

        private void _aveHtBTN_Click(object sender, EventArgs e)
        {
            if (_blockTBClick == true) { return; }
            _blockTBClick = true;

            int? initialValue = (_plot.AverageHeight <= 0) ? (int?)null : (int?)_plot.AverageHeight;

            using (var numPad = new FormNumPad())
            {
                numPad.ShowDialog(0, 999, initialValue, true);
                _plot.AverageHeight = (uint)numPad.UserEnteredValue.GetValueOrDefault();
            }

            //this.AverageHgt = (int)(Controller.ShowNumericValueInput(0, 999, (this.AverageHgt <= 0) ? (int?)null : (int?)this.AverageHgt, true) ?? -1);
            _plot.KPI = _plot.CalculateKPI();
            _blockTBClick = false;
        }

        private void _treeCntBTN_Click(object sender, EventArgs e)
        {
            if (_blockTBClick == true) { return; }
            _blockTBClick = true;

            int? initialValue = (_plot.TreeCount <= 0) ? (int?)null : (int?)_plot.TreeCount;

            using (var numpad = new FormNumPad())
            {
                numpad.ShowDialog(0, 999, initialValue, true);
                _plot.TreeCount = (uint)numpad.UserEnteredValue.GetValueOrDefault();
            }

            //this.TreeCount = (int)(Controller.ShowNumericValueInput(0, 999, (this.TreeCount <= 0) ? (int?)null : (int?)this.TreeCount, true) ?? -1);
            _plot.KPI = _plot.CalculateKPI();
            _blockTBClick = false;
        }
    }
}