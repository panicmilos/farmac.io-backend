using GlobalExceptionHandler.Exceptions;
using System;

namespace Farmacio_Services.Exceptions
{
    [Serializable]
    public class NotEmployedInPharmacyException : BadLogicException
    {
        public NotEmployedInPharmacyException()
        {
        }

        public NotEmployedInPharmacyException(string message) :
            base(message)
        {
        }
    }
}