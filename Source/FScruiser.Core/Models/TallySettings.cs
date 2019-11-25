using FMSC.ORM.Core;
using FMSC.ORM.EntityModel;
using FMSC.ORM.EntityModel.Attributes;

namespace FSCruiser.Core.Models
{
    /// <summary>
    /// What is the purpose of this class? It is a place holder of sorts. I figure that the CountTree table needs to be broken up into seperate data/settings tables
    /// </summary>
    /// TODO this class probably doen't need to inharet from DataObject
    [Table("CountTree")]
    public class TallySettings : DataObject_Base
    {
        public TallySettings()
            : base()
        { }

        public TallySettings(Datastore db)
            : base(db)
        { }

        [Field(Name = "SampleGroup_CN")]
        public long? SampleGroup_CN { get; set; }

        [Field(Name = "TreeDefaultValue_CN")]
        public long? TreeDefaultValue_CN { get; set; }

        [Field(Name = "Tally_CN")]
        public long? Tally_CN { get; set; }
    }
}