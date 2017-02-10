using System;

namespace FSCruiser.Core
{
    public class UserFacingException : Exception
    {
        public UserFacingException(String message, Exception innerException)
            : base(message, innerException)
        { }
    }

    public class CruiseConfigurationException : UserFacingException
    {
        public string Table { get; set; }

        public string RecordID { get; set; }

        public CruiseConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }

    public class TallyHistoryPersistanceException : Exception
    {
        public TallyHistoryPersistanceException(string message, Exception innerEx)
            : base(message, innerEx)
        { }
    }
}