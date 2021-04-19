using System.Net;
using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Services.Exceptions
{
    public class PharmacyMedicineAlreadyExistsException : BadLogicException
    {
        public PharmacyMedicineAlreadyExistsException()
        {
        }

        public PharmacyMedicineAlreadyExistsException(string message) :
            base(message)
        {
        }
    }
}