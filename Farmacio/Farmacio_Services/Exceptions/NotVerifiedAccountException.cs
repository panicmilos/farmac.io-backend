using GlobalExceptionHandler.Exceptions;
using System;

namespace Farmacio_Services.Exceptions
{
    [Serializable]
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