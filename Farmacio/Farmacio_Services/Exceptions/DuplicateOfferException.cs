using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Services.Exceptions
{
    public class DuplicateOfferException : BadLogicException
    {
        public DuplicateOfferException()
        {
        }

        public DuplicateOfferException(string message) :
            base(message)
        {
        }
    }
}