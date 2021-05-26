using GlobalExceptionHandler.Exceptions;
using System;

namespace Farmacio_Services.Exceptions
{
    [Serializable]
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