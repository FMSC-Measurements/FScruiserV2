using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FSCruiser.Core;
using FSCruiser.Core.ViewInterfaces;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class FormPlotInfo : Form, IPlotInfoDialog
    {
        public IApplicationController Controller { get; set; }

        private PlotDO _initialState; 
        private PlotVM _currentPlotInfo;
        public PlotVM CurrentPlotInfo { get { return _currentPlotInfo; } }

        public FormPlotInfo(IApplicationController controller)
        {
            this.KeyPreview = true;
            this.Controller = controller;
            InitializeComponent();

            if (ViewController.PlatformType == FMSC.Controls.PlatformType.WinCE)
            {
                this.WindowState = FormWindowState.Maximized;
                this._ceControlPanel.Visible = true;
                this.Menu = null;
                this.mainMenu1.Dispose();
                this.mainMenu1 = null;
            }

        }

        public DialogResult ShowDialog(PlotVM plotInfo, bool allowEdit)
        {
            if (plotInfo == null) { return DialogResult.None; }
            this.DialogResult = DialogResult.OK;
            _currentPlotInfo = plotInfo;
            //PlotDO plot = plotInfo.Plot;
            this._plotNumTB.Enabled = allowEdit;
            //this._isNullCB.Checked = plotInfo.IsNull;//Hack, binding isn't working 
            //this._slope.Enabled = allowEdit;
            //this._aspect.Enabled = allowEdit;

            this._BS_Plot.DataSource = plotInfo;
            if(this._BS_Plot.IsBindingSuspended)
            {
                this._BS_Plot.ResumeBinding();
            }
            if (allowEdit == true)//only if we allow edits do we need to save the initial state
            {
                _initialState = new PlotDO(plotInfo);
            }
            else
            {
                _initialState = null;
            }
            this._plotStatsTB.Text = ApplicationController.GetPlotInfo(plotInfo);
            return this.ShowDialog();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            this._BS_Plot.EndEdit();
            if (this.DialogResult == DialogResult.Cancel) 
            {
                if (_initialState != null)
                {
                    _currentPlotInfo.SetValues(_initialState);
                }
                return; 
            }
            if (this.CurrentPlotInfo.IsNull && this.CurrentPlotInfo.Trees.Count > 0)
            {
                MessageBox.Show("Null plot can not contain trees");
                e.Cancel = true;
            }

        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.Handled) { return; }
            if (!this._remarksTB.Focused && e.KeyData == Keys.Enter)
            {
                e.Handled = true;
                this.DialogResult = DialogResult.OK;//setting dialogResult causes form to close
                //this.Close();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            
            //if (this.DialogResult == DialogResult.OK)
            //{
            //    CurrentPlotInfo.Plot.Save();
            //}
            this._BS_Plot.SuspendBinding();
            base.OnClosed(e);
        }

        private void _cancelButton_Click(object sender, EventArgs e)
        {
            //if (_initialState != null)
            //{
            //    _currentPlotInfo.SetValues(_initialState);
            //}
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void _gpsButton_Click(object sender, EventArgs e)
        {
            
        }



        //private void _isNullCB_CheckStateChanged(object sender, EventArgs e)
        //{
        //    CurrentPlotInfo.IsEmpty = (this._isNullCB.Checked) ? "True" : "False";
        //}
    }

    
}