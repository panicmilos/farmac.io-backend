using GlobalExceptionHandler.Exceptions;
using System;

namespace Farmacio_Services.Exceptions
{
    [Serializable]
    public class AlreadyEmployedInPharmacyException : BadLogicException
    {
        public AlreadyEmployedInPharmacyException()
        {
        }

        public AlreadyEmployedInPharmacyException(string message) :
            base(message)
        {
        }
    }
}