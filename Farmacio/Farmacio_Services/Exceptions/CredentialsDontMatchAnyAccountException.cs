using GlobalExceptionHandler.Exceptions;
using System;

namespace Farmacio_Services.Exceptions
{
    [Serializable]
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