using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using FSCruiser.Core.Models;
using System.Linq;

namespace FSCruiser.Core
{
    [Serializable]
    public class ApplicationSettings
    {
        string _backupDir;
        List<CruiserVM> _cruisers;
        public List<RecentProject> RecentProjects { get; set; }

        public ApplicationSettings()
        {
            RecentProjects = new List<RecentProject>();
        }

        [XmlAttribute]
        public bool BackUpToCurrentDir { get; set; }

        [XmlAttribute]
        public string BackupDir 
        {
            get { return _backupDir; }
            set
            {
                if (!String.IsNullOrEmpty(value) && System.IO.Directory.Exists(value))
                {
                    this._backupDir = value;
                }
                else
                {
                    this._backupDir = null;
                }
            }
        }

        [XmlAttribute]
        public BackUpMethod BackUpMethod { get; set; }

        [XmlAttribute]
        public bool EnableCruiserPopup { get; set; }

        [XmlElement]
        public List<CruiserVM> Cruisers 
        {
            get
            {
                if (_cruisers == null)
                {
                    _cruisers = new List<CruiserVM>();
                }
                return _cruisers;
            }
            set { _cruisers = value; } 
        }

        [XmlElement]
        public float DataGridFontSize { get; set; }


        #region Cruisers
        //public CruiserVM[] GetCruiserList()
        //{
        //    if (this.Cruisers == null)
        //    {
        //        return new CruiserVM[0];
        //    }
        //    else
        //    {
        //        return this.Cruisers.ToArray();
        //    }

        //}

        public void AddCruiser(string initials)
        {
            if (this.Cruisers == null)
            {
                this.Cruisers = new List<CruiserVM>();
            }
            this.Cruisers.Add(new CruiserVM(initials));
        }

        public void RemoveCruiser(CruiserVM cruiser)
        {
            this.Cruisers.Remove(cruiser);
        }
        #endregion


        #region Recent Files

        public void AddRecentProject(RecentProject project)
        {
            if (RecentProjects == null)
                RecentProjects = new List<RecentProject>();

            if (RecentProjects.Contains(project))
                RecentProjects.Remove(project);

            RecentProjects.Insert(0, project);
        }

        public void ClearRecentProjects()
        {
            if (RecentProjects != null)
            {
                RecentProjects.Clear();
            }
        }
        #endregion
    }
}
