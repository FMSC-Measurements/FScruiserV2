﻿using System;
using System.Xml.Serialization;

namespace FSCruiser.Core.Models
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