using GlobalExceptionHandler.Exceptions;
using System;

namespace Farmacio_Services.Exceptions
{
    [Serializable]
    public class InvalidAppointmentDateTimeException : BadLogicException
    {
        public InvalidAppointmentDateTimeException()
        {
        }

        public InvalidAppointmentDateTimeException(string message) :
            base(message)
        {
        }
    }
}