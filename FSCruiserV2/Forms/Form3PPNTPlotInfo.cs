﻿using System;
using System.ComponentModel;
using System.Windows.Forms;
using FSCruiserV2.Logic;
using CruiseDAL.DataObjects;
using FMSC.Sampling;

namespace FSCruiserV2.Forms
{
    public partial class Form3PPNTPlotInfo : Form, IPlotInfoDialog
    {
        public IApplicationController Controller { get; set; }

        //public readonly decimal DEFAULT_VOL_FACTOR = new Decimal(333, 0, 0, false, 3);
        

        public Form3PPNTPlotInfo(IApplicationController controller)
        {
            InitializeComponent();

            if (ViewController.PlatformType == FMSC.Controls.PlatformType.WinCE)
            {
                this.WindowState = FormWindowState.Maximized;
                this._OKBtn.Visible = true;
            }

            this.Controller = controller;
            this.VolFactor = Constants.DEFAULT_VOL_FACTOR; 
            
        }



        private bool _blockTBClick = false;


        private PlotVM _currentPlotInfo;
        
        private decimal _volFactor;
        public decimal VolFactor 
        {
            get { return _volFactor; }
            set
            {
                this._volFactor = value;
                this._volFactorTB.Text = value.ToString();
            }
        }

        public float KPI
        {
            get
            {
                return _currentPlotInfo.KPI;
            }

            protected set
            {
                
                _currentPlotInfo.KPI = (value >= 0) ? value : 0;
                this._kpiLBL.Text = (value >= 0) ? value.ToString() : string.Empty;
            }
        }

        private int _treeCount;
        public int TreeCount 
        { 
            get
            {

                return _treeCount;
            }
            set
            {
                _treeCntBTN.Text = (value >= 0) ? value.ToString() : string.Empty;
                _treeCount = value;
            }
        }

        private int _aveHgt;
        public int AverageHgt 
        { 
            get 
            {
                return _aveHgt;
            }
            set
            {
                _aveHgt = value;
                _aveHtBTN.Text = (value >= 0) ? value.ToString() : string.Empty;
            }
        }


        public DialogResult ShowDialog(PlotVM plotInfo, bool allowEdit)
        {
            
            this._currentPlotInfo = plotInfo;
            this._BS_plot.DataSource = plotInfo;

            this.TreeCount = -1;
            this.AverageHgt = -1;
            this.KPI = -1;

            this.DialogResult = DialogResult.OK;
 
            return this.ShowDialog();
        }

        private void _OKBtn_Click(object sender, EventArgs e)
        {

        }

        private void _cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void _gpsButton_Click(object sender, EventArgs e)
        {

        }


        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (e.Cancel == true) { return; }
            if (this.DialogResult == DialogResult.Cancel) { return; }

            if (this.TreeCount < 0 && this.AverageHgt < 0)
            {
                MessageBox.Show("You havn't entered anything");
                e.Cancel = true;
                return;
            }

            if ((this.TreeCount <= 0 && this.AverageHgt > 0) || (this.AverageHgt == 0 && this.TreeCount > 0))
            {
                MessageBox.Show("Invalid Tree Count or Average Height");
                e.Cancel = true;
                return; 
            }

            if (this.TreeCount <= 0)
            {
                this._currentPlotInfo.IsNull = true;
                this.SignalCountPlot();//if it's an empty plot then it will be a count plot
                return;
            }

            ThreePItem item = (ThreePItem)this._currentPlotInfo.Stratum.SampleSelecter.NextItem();
            if (this.KPI >= item.KPI)
            {
                CreateTrees();
                SignalMeasurePlot();
            }
            else
            {
                this.SignalCountPlot();
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


        private void CreateTrees()
        {
            TreeVM[] newTrees = new TreeVM[this.TreeCount];
            try
            {
                this.Controller._cDal.BeginTransaction();
                for (long i = 0; i < this.TreeCount; i++)
                {
                    TreeVM t = Controller.CreateNewTreeEntry(null, _currentPlotInfo.Stratum, null, null, this._currentPlotInfo, false);
                    t.TreeCount = 1;
                    t.CountOrMeasure = "M";
                    t.TreeNumber = i + 1;
                    newTrees[i] = t;
                    t.Save();
                }
                this.Controller._cDal.EndTransaction();
            }
            catch (Exception e)
            {
                this.Controller._cDal.CancelTransaction();
                throw e;
            }

            foreach (TreeVM t in newTrees)
            {
                this._currentPlotInfo.Trees.Add(t);
            }
        }

        private void CalculateKPI()
        {
            if (this.TreeCount <= 0 || this.AverageHgt <= 0)
            {
                this.KPI = 0;
            }
            else
            {
                this.KPI =(float)Math.Round((Decimal)(this.TreeCount * _currentPlotInfo.Stratum.BasalAreaFactor * this.AverageHgt) * this.VolFactor);
            }
        }

        private void _aveHtBTN_Click(object sender, EventArgs e)
        {
            if (_blockTBClick == true) { return; }
            _blockTBClick = true;
            //int? num = null;
            //try
            //{
            //    num = Convert.ToInt32(_aveHtTB.Text);
            //}
            //catch
            //{ }

            this.AverageHgt = (int)(Controller.ShowNumericValueInput(0, 999, (this.AverageHgt <= 0) ? (int?)null : (int?)this.AverageHgt, true) ?? -1);
            CalculateKPI();
            _blockTBClick = false;
        }

        private void _treeCntBTN_Click(object sender, EventArgs e)
        {
            if (_blockTBClick == true) { return; }
            _blockTBClick = true;


            this.TreeCount = (int)(Controller.ShowNumericValueInput(0, 999, (this.TreeCount <= 0) ? (int?)null : (int?)this.TreeCount, true) ?? -1);
            CalculateKPI();
            _blockTBClick = false;
        }

    }
}