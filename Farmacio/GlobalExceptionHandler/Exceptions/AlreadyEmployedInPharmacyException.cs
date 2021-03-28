using System.Net;

namespace GlobalExceptionHandler.Exceptions
{
    public class AlreadyEmployedInPharmacyException : HandlableException
    {
        public override HttpStatusCode Code { get; set; } = HttpStatusCode.BadRequest;

        public AlreadyEmployedInPharmacyException()
        {
        }

        public AlreadyEmployedInPharmacyException(string message) :
            base(message)
        {
        }
    }
}