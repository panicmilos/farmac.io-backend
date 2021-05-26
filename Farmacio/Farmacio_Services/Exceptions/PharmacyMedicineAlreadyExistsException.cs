using GlobalExceptionHandler.Exceptions;
using System;

namespace Farmacio_Services.Exceptions
{
    [Serializable]
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