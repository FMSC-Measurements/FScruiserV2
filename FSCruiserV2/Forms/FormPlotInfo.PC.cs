﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FSCruiserV2.Logic;

namespace FSCruiserV2.Forms
{
    public partial class FormPlotInfo : FormPlotInfoBase
    {
        public FormPlotInfo(IApplicationController controller)
        {
            InitializeComponent();
        }

        protected override void OnShowing(PlotVM plotInfo, bool allowEdit)
        {
            base.OnShowing(plotInfo, allowEdit);
            this._plotNumTB.Enabled = allowEdit;
            //this._plotStatsTB.Text = ApplicationController.GetPlotInfo(plotInfo);

            
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            
            base.OnClosing(e);
            if (this.CurrentPlotInfo.IsNull && this.CurrentPlotInfo.Trees.Count > 0)
            {
                MessageBox.Show("Null plot can not contain trees");
                e.Cancel = true;
            }
        }
    }
}
