using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FMSC.Sampling;
using FSCruiserV2.Logic;

namespace FSCruiserV2.Forms
{
    public partial class Form3PPNTPlotInfo : FormPlotInfoBase
    {
        private bool _blockTBClick; 

        public Form3PPNTPlotInfo(IApplicationController controller)
        {
            InitializeComponent();
            base.Controller = controller;

            this.VolFactor = Constants.DEFAULT_VOL_FACTOR;
        }

        #region properties 
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
                return base.CurrentPlotInfo.KPI;
            }

            protected set
            {

                base.CurrentPlotInfo.KPI = (value >= 0) ? value : 0;
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
        #endregion

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
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

            if (this.TreeCount == 0)
            {
                base.CurrentPlotInfo.IsNull = true;
            }

            ThreePItem item = (ThreePItem)base.CurrentPlotInfo.Stratum.SampleSelecter.NextItem();
            if (this.KPI >= item.KPI)
            {
                SignalMeasurePlot();
                for (int i = 0; i < this.TreeCount; i++)
                {
                    TreeVM tree = Controller.CreateNewTreeEntry(null, base.CurrentPlotInfo.Stratum, null, null, base.CurrentPlotInfo, false);
                    tree.TreeCount = 1;
                    tree.CountOrMeasure = "M";
                    tree.TreeNumber = i + 1; 
                    base.CurrentPlotInfo.Trees.Add(tree);
                }
            }


        }

        private void SignalMeasurePlot()
        {
            System.Media.SystemSounds.Asterisk.Play();
            MessageBox.Show("Measure Plot");
        }

        private void CalculateKPI()
        {
            if (this.TreeCount <= 0 || this.AverageHgt <= 0)
            {
                this.KPI = 0;
            }
            else
            {
                this.KPI = (float)Math.Round((Decimal)(this.TreeCount * base.CurrentPlotInfo.Stratum.BasalAreaFactor * this.AverageHgt) * this.VolFactor);
            }
        }

        protected void HandleAveHtButtonClick(object sender, EventArgs e)
        {
            if (_blockTBClick == true) { return; }
            _blockTBClick = true;

            this.AverageHgt = (int)(Controller.ShowNumericValueInput(0, 999, (this.AverageHgt <= 0) ? (int?)null : (int?)this.AverageHgt, true) ?? -1);
            CalculateKPI();
            _blockTBClick = false;
        }

        protected void HandleTreeCntButtonClick(object sender, EventArgs e)
        {
            if (_blockTBClick == true) { return; }
            _blockTBClick = true;


            this.TreeCount = (int)(Controller.ShowNumericValueInput(0, 999, (this.TreeCount <= 0) ? (int?)null : (int?)this.TreeCount, true) ?? -1);
            CalculateKPI();
            _blockTBClick = false;
        }
    }
}
