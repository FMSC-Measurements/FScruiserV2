using System;
using System.Collections.Generic;
using System.Text;
using CruiseDAL;

namespace FSCruiserV2.Logic
{
    /// <summary>
    /// What is the purpose of this class? It is a place holder of sorts. I figure that the CountTree table needs to be broken up into seperate data/settings tables
    /// </summary>
    [Table(TableName="CountTree")]
    public class TallySettingsDO : DataObject
    {
        public TallySettingsDO() 
            : base() 
        { }

        

        public TallySettingsDO(DatastoreBase db)
            : base(db)
        { }

        [Field(FieldName = "SampleGroup_CN")]
        public long? SampleGroup_CN { get; set; }


        [Field(FieldName= "TreeDefaultValue_CN")]
        public long? TreeDefaultValue_CN { get; set; }

        [Field(FieldName= "Tally_CN")]
        public long? Tally_CN { get; set; }




        public override RowValidator Validator
        {
            get { throw new NotImplementedException(); }
        }

        protected override bool DoValidate()
        {
            throw new NotImplementedException();
        }

        public override void SetValues(DataObject obj)
        {
            throw new NotImplementedException();
        }
    }
}
