using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Services.Exceptions
{
    public class OrderIsAlreadyProcessedException : BadLogicException
    {
        public OrderIsAlreadyProcessedException()
        {
        }

        public OrderIsAlreadyProcessedException(string message) :
            base(message)
        {
        }
    }
}