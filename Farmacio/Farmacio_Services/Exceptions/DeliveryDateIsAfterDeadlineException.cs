using GlobalExceptionHandler.Exceptions;
using System;

namespace Farmacio_Services.Exceptions
{
    [Serializable]
    public class DeliveryDateIsAfterDeadlineException : BadLogicException
    {
        public DeliveryDateIsAfterDeadlineException()
        {
        }

        public DeliveryDateIsAfterDeadlineException(string message) :
            base(message)
        {
        }
    }
}