using System.Net;
using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Services.Exceptions
{
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