using GlobalExceptionHandler.Exceptions;
using System;

namespace Farmacio_Services.Exceptions
{
    [Serializable]
    public class InvalidWorkTimeException : BadLogicException
    {
        public InvalidWorkTimeException()
        {
        }

        public InvalidWorkTimeException(string message) :
            base(message)
        {
        }
    }
}