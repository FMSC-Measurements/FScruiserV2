using System;
using System.Collections.Generic;
using System.Text;
using CruiseDAL;
using FMSC.ORM.Core.EntityAttributes;
using FMSC.ORM.Core.EntityModel;
using FMSC.ORM.Core;


namespace FSCruiser.Core.Models
{
    /// <summary>
    /// What is the purpose of this class? It is a place holder of sorts. I figure that the CountTree table needs to be broken up into seperate data/settings tables
    /// </summary>
    /// TODO this class probably doen't need to inharet from DataObject
    [SQLEntity(SourceName = "CountTree")]
    public class TallySettingsDO : DataObject
    {
        public TallySettingsDO() 
            : base() 
        { }

        

        public TallySettingsDO(DatastoreRedux db)
            : base(db)
        { }

        [Field(FieldName = "SampleGroup_CN")]
        public long? SampleGroup_CN { get; set; }


        [Field(FieldName= "TreeDefaultValue_CN")]
        public long? TreeDefaultValue_CN { get; set; }

        [Field(FieldName= "Tally_CN")]
        public long? Tally_CN { get; set; }


        public override void SetValues(DataObject obj)
        {
            throw new NotSupportedException();
            //throw new NotImplementedException();
        }
    }
}
