using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Repositories.Implementation
{
    public class ThrowingService
    {
        public void Throw()
        {
            throw new BadLogicException("Bacio sam exception iz nekog random service.");
        }
    }
}