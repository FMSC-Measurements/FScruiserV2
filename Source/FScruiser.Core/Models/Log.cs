using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using CruiseDAL.DataObjects;
using FMSC.ORM.EntityModel.Attributes;
using CruiseDAL.Schema;
using FMSC.ORM.EntityModel;
using System.ComponentModel;
using CruiseDAL.EntityAttributes;

namespace FSCruiser.Core.Models
{
    [Table("Log")]
    public class Log : DataObject_Base, IDataErrorInfo
    {
        [PrimaryKeyField(Name = "Log_CN")]
        public Int64? Log_CN { get; set; }

        private Guid _log_guid = Guid.NewGuid();

        [Field(Name = "Log_GUID")]
        public virtual Guid Log_GUID
        {
            get
            {
                return _log_guid;
            }
            set
            {
                _log_guid = value;
            }
        }

        [Field("Tree_CN")]
        public virtual long? Tree_CN { get; set; }

        private String _grade;

        int _logNumber;

        [Field("LogNumber")]
        public int LogNumber
        {
            get { return _logNumber; }
            set
            {
                _logNumber = value;
                NotifyPropertyChanged("LogNumber");
            }
        }

        [Field(Name = "Grade")]
        public virtual String Grade
        {
            get
            {
                return _grade;
            }
            set
            {
                if (_grade == value) { return; }
                _grade = value;
                this.NotifyPropertyChanged(LOG.GRADE);
            }
        }

        private float _seendefect = 0.0f;

        [Field(Name = "SeenDefect")]
        public virtual float SeenDefect
        {
            get
            {
                return _seendefect;
            }
            set
            {
                if (Math.Abs(_seendefect - value) < float.Epsilon) { return; }
                _seendefect = value;
                this.NotifyPropertyChanged(LOG.SEENDEFECT);
            }
        }

        private float _percentrecoverable = 0.0f;

        [Field(Name = "PercentRecoverable")]
        public virtual float PercentRecoverable
        {
            get
            {
                return _percentrecoverable;
            }
            set
            {
                if (Math.Abs(_percentrecoverable - value) < float.Epsilon) { return; }
                _percentrecoverable = value;
                this.NotifyPropertyChanged(LOG.PERCENTRECOVERABLE);
            }
        }

        private Int64 _length;

        [Field(Name = "Length")]
        public virtual Int64 Length
        {
            get
            {
                return _length;
            }
            set
            {
                if (_length == value) { return; }
                _length = value;
                this.NotifyPropertyChanged(LOG.LENGTH);
            }
        }

        private String _exportgrade;

        [Field(Name = "ExportGrade")]
        public virtual String ExportGrade
        {
            get
            {
                return _exportgrade;
            }
            set
            {
                if (_exportgrade == value) { return; }
                _exportgrade = value;
                this.NotifyPropertyChanged(LOG.EXPORTGRADE);
            }
        }

        private float _smallenddiameter = 0.0f;

        [Field(Name = "SmallEndDiameter")]
        public virtual float SmallEndDiameter
        {
            get
            {
                return _smallenddiameter;
            }
            set
            {
                if (Math.Abs(_smallenddiameter - value) < float.Epsilon) { return; }
                _smallenddiameter = value;
                this.NotifyPropertyChanged(LOG.SMALLENDDIAMETER);
            }
        }

        private float _largeenddiameter = 0.0f;

        [Field(Name = "LargeEndDiameter")]
        public virtual float LargeEndDiameter
        {
            get
            {
                return _largeenddiameter;
            }
            set
            {
                if (Math.Abs(_largeenddiameter - value) < float.Epsilon) { return; }
                _largeenddiameter = value;
                this.NotifyPropertyChanged(LOG.LARGEENDDIAMETER);
            }
        }

        private float _grossboardfoot = 0.0f;

        [Field(Name = "GrossBoardFoot")]
        public virtual float GrossBoardFoot
        {
            get
            {
                return _grossboardfoot;
            }
            set
            {
                if (Math.Abs(_grossboardfoot - value) < float.Epsilon) { return; }
                _grossboardfoot = value;
                this.NotifyPropertyChanged(LOG.GROSSBOARDFOOT);
            }
        }

        private float _netboardfoot = 0.0f;

        [Field(Name = "NetBoardFoot")]
        public virtual float NetBoardFoot
        {
            get
            {
                return _netboardfoot;
            }
            set
            {
                if (Math.Abs(_netboardfoot - value) < float.Epsilon) { return; }
                _netboardfoot = value;
                this.NotifyPropertyChanged(LOG.NETBOARDFOOT);
            }
        }

        private float _grosscubicfoot = 0.0f;

        [Field(Name = "GrossCubicFoot")]
        public virtual float GrossCubicFoot
        {
            get
            {
                return _grosscubicfoot;
            }
            set
            {
                if (Math.Abs(_grosscubicfoot - value) < float.Epsilon) { return; }
                _grosscubicfoot = value;
                this.NotifyPropertyChanged(LOG.GROSSCUBICFOOT);
            }
        }

        private float _netcubicfoot = 0.0f;

        [Field(Name = "NetCubicFoot")]
        public virtual float NetCubicFoot
        {
            get
            {
                return _netcubicfoot;
            }
            set
            {
                if (Math.Abs(_netcubicfoot - value) < float.Epsilon) { return; }
                _netcubicfoot = value;
                this.NotifyPropertyChanged(LOG.NETCUBICFOOT);
            }
        }

        private float _boardfootremoved = 0.0f;

        [Field(Name = "BoardFootRemoved")]
        public virtual float BoardFootRemoved
        {
            get
            {
                return _boardfootremoved;
            }
            set
            {
                if (Math.Abs(_boardfootremoved - value) < float.Epsilon) { return; }
                _boardfootremoved = value;
                this.NotifyPropertyChanged(LOG.BOARDFOOTREMOVED);
            }
        }

        private float _cubicfootremoved = 0.0f;

        [Field(Name = "CubicFootRemoved")]
        public virtual float CubicFootRemoved
        {
            get
            {
                return _cubicfootremoved;
            }
            set
            {
                if (Math.Abs(_cubicfootremoved - value) < float.Epsilon) { return; }
                _cubicfootremoved = value;
                this.NotifyPropertyChanged(LOG.CUBICFOOTREMOVED);
            }
        }

        private float _dibclass = 0.0f;

        [Field(Name = "DIBClass")]
        public virtual float DIBClass
        {
            get
            {
                return _dibclass;
            }
            set
            {
                if (Math.Abs(_dibclass - value) < float.Epsilon) { return; }
                _dibclass = value;
                this.NotifyPropertyChanged(LOG.DIBCLASS);
            }
        }

        private float _barkthickness = 0.0f;

        [Field(Name = "BarkThickness")]
        public virtual float BarkThickness
        {
            get
            {
                return _barkthickness;
            }
            set
            {
                if (Math.Abs(_barkthickness - value) < float.Epsilon) { return; }
                _barkthickness = value;
                this.NotifyPropertyChanged(LOG.BARKTHICKNESS);
            }
        }

        [CreatedByField()]
        public string CreatedBy { get; set; }

        [Field(Name = "CreatedDate",
        PersistanceFlags = PersistanceFlags.Never)]
        public DateTime CreatedDate { get; set; }

        [ModifiedByField()]
        public string ModifiedBy { get; set; }

        #region IDataErrorInfo members

        string _error;
        IDictionary<string, string> _errors = new Dictionary<string, string>();

        public string Error
        {
            get
            {
                return _error ?? string.Empty;
            }
        }

        [IgnoreField]
        public string this[string columnName]
        {
            get
            {
                if (_errors.ContainsKey(columnName))
                { return _errors[columnName]; }
                return String.Empty;
            }
            set
            {
                if (_errors.ContainsKey(columnName))
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        _errors[columnName] = value;
                    }
                    else
                    {
                        _errors.Remove(columnName);
                    }
                    OnErrorChanged();
                }
                else if (!string.IsNullOrEmpty(value))
                {
                    _errors.Add(columnName, value);
                    OnErrorChanged();
                }
            }
        }

        private void OnErrorChanged()
        {
            _error = String.Join(" | ", _errors.Values.ToArray());

            NotifyPropertyChanged("Error");
        }

        #endregion IDataErrorInfo members
    }
}