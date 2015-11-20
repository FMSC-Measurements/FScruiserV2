using System;

using System.Collections.Generic;
using System.Text;

namespace FSCruiser.Core
{
    public class UserFacingException : Exception
    {
        public UserFacingException(String message, Exception innerException)
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
