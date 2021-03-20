using System.Net;

namespace GlobalExceptionHandler.Exceptions
{
    public class MissingEntityException : HandlableException
    {
        public override HttpStatusCode Code { get; set; } = HttpStatusCode.NotFound;

        public MissingEntityException()
        {
        }

        public MissingEntityException(string message) :
            base(message)
        {
        }
    }
}