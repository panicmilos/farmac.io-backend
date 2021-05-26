using GlobalExceptionHandler.Exceptions;
using System;

namespace Farmacio_Services.Exceptions
{
    [Serializable]
    public class WrongCurrentPasswordException : BadLogicException
    {
        public WrongCurrentPasswordException()
        {
        }

        public WrongCurrentPasswordException(string message) :
            base(message)
        {
        }
    }
}