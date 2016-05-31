using System;
using System.ComponentModel;

namespace FSCruiser.Core.DataEntry
{
    public class LimitingDistanceCalculator : INotifyPropertyChanged
    {
        public event EventHandler LimitingDistanceChanged;

        public enum MeasurmentLocation { Face, Center }

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
                UpdateLimitingDistance();
            }
        }

        public double DBH
        {
            get { return _dbh; }
            set
            {
                if (value < 0.0) { return; }
                _dbh = value;
                UpdateLimitingDistance();
            }
        }

        public int SlopePCT
        {
            get { return _slopePCT; }
            set
            {
                if (value < 0) { return; }
                _slopePCT = value;
                UpdateLimitingDistance();
            }
        }

        public double SlopeDistance
        {
            get { return _slopeDistance; }
            set
            {
                if (_slopeDistance < 0.0) { return; }
                _slopeDistance = value;
            }
        }

        double _limitingDistance;

        public double LimitingDistance
        {
            get { return _limitingDistance; }
            set
            {
                _limitingDistance = value;
                OnLimitingDistanceChanged();
            }
        }

        MeasurmentLocation _measureTo;

        public MeasurmentLocation MeasureTo
        {
            get { return _measureTo; }
            set
            {
                _measureTo = value;
                UpdateLimitingDistance();
            }
        }

        public string MeasureToStr
        {
            get { return MeasureTo.ToString(); }
            set
            {
                try
                {
                    MeasureTo = (MeasurmentLocation)Enum.Parse(typeof(MeasurmentLocation), value, true);
                }
                catch { }
            }
        }

        public bool IsVariableRadius { get; set; }

        protected void UpdateLimitingDistance()
        {
            LimitingDistance = CalculateLimitingDistance(BAForFPSize,
                DBH, SlopePCT, IsVariableRadius, MeasureTo);
        }

        protected void OnLimitingDistanceChanged()
        {
            if (LimitingDistanceChanged != null)
            {
                LimitingDistanceChanged(this, null);
            }
        }

        public string GenerateReport()
        {
            UpdateLimitingDistance();

            if (LimitingDistance <= 0.0 || SlopeDistance <= 0.0)
            { return string.Empty; }

            var isTreeIn = SlopeDistance <= LimitingDistance;
#warning bad Comparison

            return String.Format("Tree was {0} (DBH:{1}, slope:{2}%, slope distance:{3}', limiting distance:{4:F}' to {5} of tree, {6}:{7}) \r\n",
                    (isTreeIn) ? "IN" : "OUT",
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
            _bafOrFPS = 0.0;
            _slopePCT = 0;
            _slopeDistance = 0.0;
            _dbh = 0.0;
            _measureTo = LimitingDistanceCalculator.MeasurmentLocation.Face;
        }

        public static double CalculateLimitingDistance(double BAForFPSize, double dbh,
            int slopPct, bool isVariableRadius, MeasurmentLocation measureTo)
        {
            if (dbh <= 0.0
                || BAForFPSize <= 0) { return 0.0; }

            double toFaceCorrection = (measureTo == MeasurmentLocation.Face) ? (dbh / 12.0) * 0.5 : 0.0;

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