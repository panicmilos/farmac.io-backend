using GlobalExceptionHandler.Exceptions;
using System;

namespace Farmacio_Services.Exceptions
{
    [Serializable]
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