using System;
using System.ComponentModel;

namespace FSCruiser.Core.DataEntry
{
    public class LimitingDistanceCalculator : INotifyPropertyChanged
    {
        public static readonly String MEASURE_TO_FACE = "Face";
        public static readonly String MEASURE_TO_CENTER = "Center";
        public static readonly String[] MEASURE_TO_OPTIONS = new String[] { MEASURE_TO_FACE, MEASURE_TO_CENTER };

        double _bafOrFPS;
        double _dbh;
        int _slopePCT;
        double _slopeDistance;

        public double BAForFPSize
        {
            get { return _bafOrFPS; }
            set
            {
                if (value < 0.0) { return; }
                _bafOrFPS = value;
                NotifyPropertyChanged("BAForFPSize");
                Recalculate();
            }
        }

        public double DBH
        {
            get { return _dbh; }
            set
            {
                if (value < 0.0) { return; }
                _dbh = value;
                NotifyPropertyChanged("DBH");
                Recalculate();
            }
        }

        public int SlopePCT
        {
            get { return _slopePCT; }
            set
            {
                if (value < 0) { return; }
                _slopePCT = value;
                NotifyPropertyChanged("SlopePCT");
                Recalculate();
            }
        }

        public double SlopeDistance
        {
            get { return _slopeDistance; }
            set
            {
                if (_slopeDistance < 0.0) { return; }
                _slopeDistance = value;
                NotifyPropertyChanged("SlopeDistance");
                Recalculate();
            }
        }

        double _limitingDistance;

        public double LimitingDistance
        {
            get { return _limitingDistance; }
            set
            {
                _limitingDistance = value;
                NotifyPropertyChanged("LimitingDistance");
            }
        }

        String _measureTo = MEASURE_TO_FACE;

        public string MeasureTo
        {
            get { return _measureTo; }
            set
            {
                if (value == _measureTo) { return; }
                _measureTo = value;
                NotifyPropertyChanged("MeasureTo");
                Recalculate();
            }
        }

        String _treeStatus;

        public String TreeStatus
        {
            get
            { return _treeStatus; }
            set
            {
                if (_treeStatus == value) { return; }
                _treeStatus = value;
                NotifyPropertyChanged("TreeStatus");
            }
        }

        public bool IsVariableRadius { get; protected set; }

        public LimitingDistanceCalculator(bool isVariableRadius)
        {
            IsVariableRadius = isVariableRadius;
            Reset();
        }

        public void Recalculate()
        {
            LimitingDistance = CalculateLimitingDistance(BAForFPSize,
                DBH, SlopePCT, IsVariableRadius, MeasureTo);

            if (SlopeDistance <= 0
                || LimitingDistance <= 0.0)
            {
                TreeStatus = string.Empty;
            }
            else
            {
                // FSH 2409.12 35.22a
                var isTreeIn = DeterminTreeInOrOut(SlopeDistance, LimitingDistance);
                if (isTreeIn)
                {
                    TreeStatus = "IN";
                }
                else
                {
                    TreeStatus = "OUT";
                }
            }
        }

        public static bool DeterminTreeInOrOut(double slopeDistance, double limitingDistance)
        {
            return Math.Round(slopeDistance, 2) <= Math.Round(limitingDistance, 2);
        }

        public string GenerateReport()
        {
            Recalculate();

            if (String.IsNullOrEmpty(TreeStatus))
            { return string.Empty; }

            return String.Format("Tree was {0} (DBH:{1}, slope:{2}%, slope distance:{3:F2}', limiting distance:{4:F2}' to {5} of tree, {6}:{7}) \r\n",
                    TreeStatus,
                    DBH,
                    SlopePCT,
                    SlopeDistance,
                    LimitingDistance,
                    MeasureTo.ToString(),
                    (IsVariableRadius) ? "BAF" : "FPS",
                    BAForFPSize);
        }

        public void Reset()
        {
            BAForFPSize = 0.0;
            SlopePCT = 0;
            SlopeDistance = 0.0;
            DBH = 0.0;
            MeasureTo = MEASURE_TO_FACE;
            TreeStatus = String.Empty;
        }

        public static double CalculateLimitingDistance(double BAForFPSize, double dbh,
            int slopPct, bool isVariableRadius, String measureTo)
        {
            if (dbh <= 0.0
                || BAForFPSize <= 0) { return 0.0; }

            double toFaceCorrection = (measureTo == MEASURE_TO_FACE) ?
                (dbh / 12.0) * 0.5
                : 0.0;

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

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                PropertyChanged(this, e);
            }
        }

        #endregion INotifyPropertyChanged Members
    }
}