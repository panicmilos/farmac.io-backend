using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Services.Exceptions
{
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