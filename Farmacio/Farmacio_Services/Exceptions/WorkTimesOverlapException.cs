using GlobalExceptionHandler.Exceptions;
using System;

namespace Farmacio_Services.Exceptions
{
    [Serializable]
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