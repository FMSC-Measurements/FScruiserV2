using System;

using System.Collections.Generic;
using System.Text;

namespace FSCruiser.Core
{
    public class TallyHistoryPersistanceException : Exception
    {
        public TallyHistoryPersistanceException(string message, Exception innerEx)
            : base(message, innerEx)
        { }
    }
}
