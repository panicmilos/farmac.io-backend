using GlobalExceptionHandler.Exceptions;
using System;

namespace Farmacio_Services.Exceptions
{
    [Serializable]
    public class EarlyWrittenReportException : BadLogicException
    {
        public EarlyWrittenReportException()
        {
        }

        public EarlyWrittenReportException(string message) :
            base(message)
        {
        }
    }
}
