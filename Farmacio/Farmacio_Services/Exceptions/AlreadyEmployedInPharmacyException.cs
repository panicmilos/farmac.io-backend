using System.Net;
using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Services.Exceptions
{
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