using System.Net;

namespace GlobalExceptionHandler.Exceptions
{
    public class BadLogicException : HandlableException
    {
        public override HttpStatusCode Code { get; set; } = HttpStatusCode.BadRequest;

        public BadLogicException()
        {
        }

        public BadLogicException(string message) :
            base(message)
        {
        }
    }
}