using System.Net;

namespace GlobalExceptionHandler.Exceptions
{
    public class InvalidWorkTimeException : HandlableException
    {
        public override HttpStatusCode Code { get; set; } = HttpStatusCode.BadRequest;

        public InvalidWorkTimeException()
        {
        }

        public InvalidWorkTimeException(string message) :
            base(message)
        {
        }
    }
}