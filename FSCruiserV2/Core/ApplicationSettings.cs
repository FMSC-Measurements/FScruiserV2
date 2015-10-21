using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using FSCruiser.Core.Models;

namespace FSCruiser.Core
{
    [Serializable]
    public class ApplicationSettings
    {
        public ApplicationSettings()
        {
        }

        public ApplicationSettings(IApplicationController controller)
        {
            this.BackupDir = controller.BackupDir;
            this.BackUpMethod = controller.BackUpMethod;
            this.Cruisers = (controller.Cruisers != null && controller.Cruisers.Count > 0) ? controller.Cruisers : null;
        }

        [XmlAttribute]
        public string BackupDir { get; set; }

        [XmlAttribute]
        public BackUpMethod BackUpMethod { get; set; }

        [XmlElement]
        public List<CruiserVM> Cruisers { get; set; }

        [XmlElement]
        public float DataGridFontSize { get; set; }





        //public static ApplicationSettings GetInstance()
        //{

        //}

        //public static void Save()
        //{

        //}

    }
}
