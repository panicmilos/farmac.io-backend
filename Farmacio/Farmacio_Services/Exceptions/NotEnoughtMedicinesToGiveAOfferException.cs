using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Services.Exceptions
{
    public class NotEnoughtMedicinesToGiveAOfferException : BadLogicException
    {
        public NotEnoughtMedicinesToGiveAOfferException()
        {
        }

        public NotEnoughtMedicinesToGiveAOfferException(string message) :
            base(message)
        {
        }
    }
}