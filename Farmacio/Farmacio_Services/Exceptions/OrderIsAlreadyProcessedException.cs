using GlobalExceptionHandler.Exceptions;
using System;

namespace Farmacio_Services.Exceptions
{
    [Serializable]
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