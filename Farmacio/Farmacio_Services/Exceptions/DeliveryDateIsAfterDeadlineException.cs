using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Services.Exceptions
{
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