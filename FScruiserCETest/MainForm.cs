using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FSCruiserV2.Logic;

namespace FSCruiserV2.Test
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void _editTDV_BTN_Click(object sender, EventArgs e)
        {
            TreeDefaultValueDO tdv = new TreeDefaultValueDO();

            FSCruiserV2.Logic.IApplicationController appController = new Mocks.ApplicationControllerMock();


            using (FSCruiserV2.Forms.FormEditTreeDefault view = new FSCruiserV2.Forms.FormEditTreeDefault(appController))
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
            PlotVM plot = new PlotVM();

            FSCruiserV2.Logic.IApplicationController appController = new Mocks.ApplicationControllerMock();
            using (FSCruiserV2.Forms.FormPlotInfo view = new FSCruiserV2.Forms.FormPlotInfo(appController))
            {
                view.ShowDialog(plot, false);
            }
        }

        
    }
}