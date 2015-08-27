using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSCruiserV2.Forms;

namespace FSCruiserV2.Logic
{
    public class FormLimitingDistanceLogic
    {
        private double _limitingDistance;

        private ILimitingDistanceView View { get; set; }

        public bool IsToFace { get; set; }
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
                    this.View.LimitingDistance = string.Empty;
                }
                else
                {
                    this.View.LimitingDistance = string.Format("{0:F}' to {1} of tree",
                        _limitingDistance,
                        (IsToFace) ? "face" : "center");
                }
            }
        }

        public float DBH
        {
            get
            {
                try
                {
                    return Convert.ToSingle(this.View.DBH);
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
                    this.View.DBH = String.Empty;
                }
                else
                {
                    this.View.DBH = value.ToString();
                }
            }
        }

        public float BAForFPSize
        {

            get
            {
                try
                {
                    return Convert.ToSingle(this.View.BAForFPSize);
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
                    this.View.BAForFPSize = String.Empty;
                }
                else
                {
                    this.View.BAForFPSize = value.ToString();
                }

            }

        }

        public int SlopePCT
        {
            get
            {
                try
                {
                    return Convert.ToInt32(this.View.SlopePCT);
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                if (value <= 0)
                {
                    this.View.SlopePCT = string.Empty;
                }
                else
                {
                    this.View.SlopePCT = value.ToString();
                }
            }

        }

        public double SlopeDistance { get; set; }//TODO

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

        public void HandleLoad()
        {
            this.SlopePCT = 0;
            this.DBH = 0;
            this.SlopeDistance = 0d;
            this.LimmitingDistance = 0d;
        }

        public void HandleCalculate()
        {

        }

        public bool CheckInputValid()
        {
            //check input values
            bool inputValid = true;
            inputValid = inputValid && this.SlopePCT > 0;
            inputValid = inputValid && this.SlopeDistance > 0d;
            inputValid = inputValid && this.DBH > 0;
            inputValid = inputValid && this.BAForFPSize > 0f;

            return inputValid;
        }
    }
}
