using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Services.Exceptions
{
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