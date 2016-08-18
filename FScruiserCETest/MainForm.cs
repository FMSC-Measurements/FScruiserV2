using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FSCruiser.Core.Models;
using FSCruiser.WinForms;
using FSCruiser.WinForms.DataEntry;

namespace FSCruiserV2.Test
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public Exception Exception { get; set; }

        private void _editTDV_BTN_Click(object sender, EventArgs e)
        {
            TreeDefaultValueDO tdv = new TreeDefaultValueDO();

            var appController = new Mocks.ApplicationControllerMock();

            using (var view = new FormEditTreeDefault(appController))
            {
                view.ShowDialog(tdv);
            }
        }

        private void STRSampplingTest_BTN_Click(object sender, EventArgs e)
        {
            OnTallyTestForm view = new OnTallyTestForm();
            view.ShowDialog();
        }

        private void _plotInfo_BTN_Click(object sender, EventArgs e)
        {
            var stratum = new PlotStratum();
            var plot = new PlotVM() { Stratum = stratum };

            using (var view = new FormPlotInfo())
            {
                view.ShowDialog(plot, stratum, false);
            }
        }

        private void LimitingDistance_Click(object sender, EventArgs e)
        {
            var fpsOrBaf = 10;
            var isVariableRadious = true;
            using (var view = new FormLimitingDistance(fpsOrBaf, isVariableRadious))
            {
                view.ShowDialog();

                if (!String.IsNullOrEmpty(view.Report))
                {
                    MessageBox.Show(view.Report);
                }
            }
        }
    }
}