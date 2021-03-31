using System.Net;
using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Services.Exceptions
{
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