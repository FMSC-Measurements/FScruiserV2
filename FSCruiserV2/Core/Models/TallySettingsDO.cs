using System;
using System.Collections.Generic;
using System.Text;
using CruiseDAL;
using FMSC.ORM.EntityModel.Attributes;
using FMSC.ORM.EntityModel;
using FMSC.ORM.Core;


namespace FSCruiser.Core.Models
{
    /// <summary>
    /// What is the purpose of this class? It is a place holder of sorts. I figure that the CountTree table needs to be broken up into seperate data/settings tables
    /// </summary>
    /// TODO this class probably doen't need to inharet from DataObject
    [EntitySource(SourceName = "CountTree")]
    public class TallySettingsDO : DataObject_Base
    {
        public TallySettingsDO() 
            : base() 
        { }

        

        public TallySettingsDO(DatastoreRedux db)
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
