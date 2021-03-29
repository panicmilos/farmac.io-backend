using System.Net;
using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Services.Exceptions
{
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