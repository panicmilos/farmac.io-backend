using System.Net;

namespace GlobalExceptionHandler.Exceptions
{
    public class ForbiddenException : HandlableException
    {
        public override HttpStatusCode Code { get; set; } = HttpStatusCode.Forbidden;

        public ForbiddenException()
        {
        }

        public ForbiddenException(string message) :
            base(message)
        {
        }
    }
}