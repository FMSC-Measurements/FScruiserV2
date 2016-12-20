using System;
using System.Xml.Serialization;

namespace FSCruiser.Core.Models
{
    [Serializable]
    public class Cruiser
    {
        [XmlAttribute]
        public string Initials { get; set; }

        public Cruiser() { }

        public Cruiser(string initials)
        {
            this.Initials = initials;
        }
    }
}