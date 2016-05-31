using System;
using System.Xml.Serialization;

namespace FSCruiser.Core.Models
{
    [Serializable]
    public class RecentProject
    {
        [XmlAttribute]
        public string ProjectName { get; set; }

        [XmlAttribute]
        public string FilePath { get; set; }

        public RecentProject() { }

        public RecentProject(string name, string path)
        {
            this.ProjectName = name;
            this.FilePath = path;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is RecentProject))
                return base.Equals(obj);

            return FilePath.Equals(((RecentProject)obj).FilePath, StringComparison.InvariantCultureIgnoreCase);
        }

        public override string ToString()
        {
            return ProjectName;
        }
    }
}