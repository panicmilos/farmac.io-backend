using System.Net;

namespace GlobalExceptionHandler.Exceptions
{
    public class AuthorizationException : HandlableException
    {
        public override HttpStatusCode Code { get; set; } = HttpStatusCode.Unauthorized;

        public AuthorizationException()
        {
        }

        public AuthorizationException(string message) :
            base(message)
        {
        }
    }
}