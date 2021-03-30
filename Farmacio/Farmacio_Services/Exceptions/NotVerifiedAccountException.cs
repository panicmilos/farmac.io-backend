using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Services.Exceptions
{
    public class NotVerifiedAccountException : AuthorizationException
    {
        public NotVerifiedAccountException()
        {
        }

        public NotVerifiedAccountException(string message) :
            base(message)
        {
        }
    }
}