using GlobalExceptionHandler.Exceptions;
using System;

namespace Farmacio_Services.Exceptions
{
    [Serializable]
    public class SupplierMedicineAlreadyExistsException : BadLogicException
    {
        public SupplierMedicineAlreadyExistsException()
        {
        }

        public SupplierMedicineAlreadyExistsException(string message) :
            base(message)
        {
        }
    }
}