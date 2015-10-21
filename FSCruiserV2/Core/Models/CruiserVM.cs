using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace FSCruiserV2.Logic
{
    [Serializable]
    public class CruiserVM
    {
        [XmlAttribute]
        public string Initials { get; set; }

        public CruiserVM() { }
        public CruiserVM(string initials)
        {
            this.Initials = initials;
        }


    }
}
