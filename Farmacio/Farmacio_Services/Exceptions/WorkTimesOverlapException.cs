using System.Net;
using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Services.Exceptions
{
    public class WorkTimesOverlapException : BadLogicException
    {
        public WorkTimesOverlapException()
        {
        }

        public WorkTimesOverlapException(string message) :
            base(message)
        {
        }
    }
}