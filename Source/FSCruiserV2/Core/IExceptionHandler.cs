using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSCruiser.Core
{
    public interface IExceptionHandler
    {
        bool Handel(Exception e);
    }

    public static class IExceptionHandlerExtentions
    {
        public static void HandelEx(this IExceptionHandler handler, Exception e)
        {
            if (handler != null)
            {
                handler.Handel(e);
            }
            else
            {
                throw e;
            }
        }
    }
}