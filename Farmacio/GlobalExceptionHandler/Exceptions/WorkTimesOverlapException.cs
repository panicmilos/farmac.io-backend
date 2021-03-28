using System.Net;

namespace GlobalExceptionHandler.Exceptions
{
    public class WorkTimesOverlapException : HandlableException
    {
        public override HttpStatusCode Code { get; set; } = HttpStatusCode.BadRequest;

        public WorkTimesOverlapException()
        {
        }

        public WorkTimesOverlapException(string message) :
            base(message)
        {
        }
    }
}