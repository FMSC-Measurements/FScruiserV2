using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FMSC.Sampling;
using FSCruiser.Core;
using FSCruiser.Core.Models;
using CruiseDAL.DataObjects;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class Form3PPNTPlotInfo : Form
    {


        public Form3PPNTPlotInfo(IViewController viewController)
        {
            InitializeComponent();
        }


        private bool _blockTBClick = false;
        Plot3PPNT _plot;
        PlotStratum _stratum;



        public DialogResult ShowDialog(PlotVM plot, PlotStratum stratum, bool allowEdit)
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

                        ThreePItem item = (ThreePItem)_plot.Stratum.SampleSelecter.NextItem();

                        if (_plot.KPI >= item.KPI)
                        {
                            _plot.CreateTrees();
                            SignalMeasurePlot();
                        }
                        else
                        {
                            this.SignalCountPlot();
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
       

        void SignalMeasurePlot()
        {
            System.Media.SystemSounds.Asterisk.Play();
            MessageBox.Show("Measure Plot");
        }

        private void SignalCountPlot()
        {
            System.Media.SystemSounds.Hand.Play();
            MessageBox.Show("Count Plot");
        }

        private void TB_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void _BS_plot_CurrentItemChanged(object sender, EventArgs e)
        {
            _plot.KPI = _plot.CalculateKPI();
        }        
    }
}
