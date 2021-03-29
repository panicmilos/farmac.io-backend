using Farmacio_API.Contracts.Requests.Reservations;
using FluentValidation;

namespace Farmacio_API.Validations.Reservations
{
    public class CreateReservedMedicineRequestValidator : AbstractValidator<CreateReservedMedicineRequest>
    {
        public CreateReservedMedicineRequestValidator()
        {
            RuleFor(request => request.Quantity).GreaterThan(0).WithMessage("At least one medicine have to be reserved.");

            RuleFor(request => request.MedicineId).NotNull().WithMessage("MedicineId must be provided.");
        }
    }
}