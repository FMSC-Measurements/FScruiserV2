using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FSCruiser.Core;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms.DataEntry
{
    public partial class FormLimitingDistance : Form
    {
        private Microsoft.WindowsCE.Forms.InputPanel _sip;
        public FormLimitingDistance(IApplicationController controller)
        {
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
            else if (ViewController.PlatformType == FMSC.Controls.PlatformType.WM)
            {
                this.components = this.components ?? new System.ComponentModel.Container();
                this._sip = new Microsoft.WindowsCE.Forms.InputPanel();
                this.components.Add(_sip);
                this._sip.EnabledChanged += new EventHandler(_sip_EnabledChanged);
            }
        }




        
        private double _limitingDistance;
        private int _isTreeIn;

        public bool IsVariableRadius { get; set; }
        public string LogMessage { get; set; }
        public double LimmitingDistance 
        {
            get
            {
                return _limitingDistance;
            }
            set
            {
                this._limitingDistance = value;
                if (value <= 0)
                {
                    this._limitingDistanceLBL.Text = string.Empty;
                }
                else
                {
                    this._limitingDistanceLBL.Text = string.Format("{0:F}' to {1} of tree", _limitingDistance, (IsToFace) ? "face" : "center");
                }
            }
        }

        public IApplicationController Controller { get; set; }
        
        /// <summary>
        /// range 0 to inf, 0 indcates no data or invalid data
        /// </summary>
        public float DBH
        {
            get
            {
                try
                {
                    return Convert.ToSingle(this._dbhTB.Text);
                }
                catch
                {
                    return 0f;
                }
            }
            set
            {
                if (value <= 0f)
                {
                    this._dbhTB.Text = String.Empty;
                }
                else
                {
                    this._dbhTB.Text = value.ToString();
                }
            }
        }

        public float BAForFPSize
        {

            get
            {
                try
                {
                    return Convert.ToSingle(this._bafTB.Text);
                }
                catch
                {
                    return 0.0f;
                }
            }
            set
            {
                if (value <= 0.0f)
                {
                    this._bafTB.Text = String.Empty;
                }
                else
                {
                    this._bafTB.Text = value.ToString();
                }

            }

        }

        public int SlopePCT
        {
            get
            {
                try
                {
                    return Convert.ToInt32(this._slopeTB.Text);
                }
                catch
                {
                    return -1;
                }
            }
            set
            {
                if (value < 0)
                {
                    this._slopeTB.Text = string.Empty;
                }
                else
                {
                    this._slopeTB.Text = value.ToString();
                }
            }

        }

        public bool IsToFace
        {
            get
            {
                switch (this._measureToCB.Text)
                {
                    case "face":
                        {
                            return true;
                        }
                    case "center":
                        {
                            return false;
                        }
                    default:
                        {
                            return true;//default measure to face
                        }
                }
            }
            set
            {
                if (value == true)
                {
                    this._measureToCB.Text = "face";
                }
                else
                {
                    this._measureToCB.Text = "center";
                }
            }


        }

        public double SlopeDistance
        {
            get
            {
                try
                {
                    return Convert.ToDouble(this._distanceTB.Text);
                }
                catch
                {
                    return default(double);
                }
            }
            set
            {
                if (value <= 0d)
                {
                    this._distanceTB.Text = String.Empty;
                }
                else
                {
                    this._distanceTB.Text = value.ToString();
                }
            }
        }

        public int IsTreeIn 
        {
            get { return _isTreeIn; }
            set
            {
                _isTreeIn = value;
                if (_isTreeIn == 1)
                {
                    this._treeIsLBL.Text = "IN";
                }
                else if (_isTreeIn == 0)
                {
                    this._treeIsLBL.Text = "OUT";
                }
                else
                {
                    this._treeIsLBL.Text = string.Empty;
                }
            }
        }

        void _sip_EnabledChanged(object sender, EventArgs e)
        {
            this._sipPlaceholder.Height = (this._sip.Enabled) ? this._sip.Bounds.Height : 0;
        }

        private void OnInputChanged()
        {
            //check input values
            bool inputValid = true;
            inputValid = inputValid && this.SlopePCT >= 0;
            inputValid = inputValid && this.SlopeDistance > 0d;
            inputValid = inputValid && this.DBH > 0;
            inputValid = inputValid && this.BAForFPSize > 0f;

            this._calculateMI.Enabled = inputValid;
            this._calculateBTN.Enabled = inputValid;
            

        }

        private static double CalculateLimitingDistance(float BAForFPSize, float dbh,
            int slopPct, bool isVariableRadius, bool isToFace)
        {
            double toFaceCorrection = 0.0;
            if (isToFace)
            {
                toFaceCorrection = (dbh / 12.0) * 0.5;
            }
            double slope = slopPct / 100.0d;

            if (isVariableRadius)
            {
                // Reference: FSH 2409.12 35.22a
                double plotRadiusFactor = (8.696 / Math.Sqrt(BAForFPSize));
                double limitingDistance = dbh * plotRadiusFactor;
                double correctedPRF = (limitingDistance - toFaceCorrection) / dbh;

                
                double slopeCorrectionFactor = Math.Sqrt(1.0d + (slope * slope));
                double correctedLimitingDistance = dbh * correctedPRF * slopeCorrectionFactor;
                return correctedLimitingDistance; 
            }
            else
            {
                // Reference: FSH 2409.12 34.22
                double plotRad = Math.Sqrt((43560 / BAForFPSize) / Math.PI);
                double slopeCorrectionFactor = 1 / Math.Cos(Math.Atan(slope));
                double limitingDistance = (plotRad - toFaceCorrection) * slopeCorrectionFactor;
                return limitingDistance;
            }

        }


        public DialogResult ShowDialog(float bafOrFPS, bool isVariableRadius, TreeVM optTree, out string logMessage)
        {
            this.BAForFPSize = bafOrFPS;
            this.IsVariableRadius = isVariableRadius;

            //initailize form state
            this._calculateMI.Enabled = false;
            this.IsToFace = true;
            this._bafOrfpsLBL.Text = (this.IsVariableRadius) ? "BAF" : "FPS";
            this.SlopePCT = 0;
            this.DBH = 0;
            this.SlopeDistance = 0d;
            this.LimmitingDistance = 0d;
            this.IsTreeIn = -1;
            

            DialogResult result = this.ShowDialog();
            if (result == DialogResult.OK && this.IsTreeIn != -1)
            {
                logMessage = String.Format("Tree was {0} (DBH:{1}, slope:{2}%, slope distance:{3}', limiting distance:{4:F}' to {5} of tree, {6}:{7}) \r\n",
                    (this.IsTreeIn == 1) ? "IN" : "OUT",
                    this.DBH,
                    this.SlopePCT,
                    this.SlopeDistance,
                    this.LimmitingDistance,
                    (this.IsToFace) ? "Face" : "Center",
                    (this.IsVariableRadius) ? "BAF" : "FPS",
                    this.BAForFPSize);
                //if (optTree != null && this.IsTreeIn == 1)
                //{
                //    optTree.DBH = this.DBH;
                //}
            }
            else
            {
                logMessage = string.Empty;
            }
            
            return result; 
        }

        private void input_TextChanged(object sender, EventArgs e)
        {
            OnInputChanged();

        }

        private void _calculateBTN_Click(object sender, EventArgs e)
        {

            this.LimmitingDistance = CalculateLimitingDistance(this.BAForFPSize, this.DBH, this.SlopePCT, this.IsVariableRadius, this.IsToFace);

            // FSH 2409.12 35.22a
            this.IsTreeIn = (this.SlopeDistance <= this.LimmitingDistance) ? 1 : 0;
        }

        private void _cancelMI_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void _TB_GotFocus(object sender, EventArgs e)
        {
            var tb = sender as TextBox;
            if (tb == null) { return; }
            tb.BeginInvoke(new Action(tb.SelectAll));
        }




    }
}