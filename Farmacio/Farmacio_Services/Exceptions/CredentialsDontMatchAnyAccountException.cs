using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Services.Exceptions
{
    public class CredentialsDontMatchAnyAccountException : AuthorizationException
    {
        public CredentialsDontMatchAnyAccountException()
        {
        }

        public CredentialsDontMatchAnyAccountException(string message) :
            base(message)
        {
        }
    }
}