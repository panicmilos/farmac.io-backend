using Farmacio_API.Contracts.Requests.Reservations;
using FluentValidation;

namespace Farmacio_API.Validations.Reservations
{
    public class CreateReservationRequestValidator : AbstractValidator<CreateReservationRequest>
    {
        public CreateReservationRequestValidator()
        {
            RuleFor(request => request.PatientId).NotNull().WithMessage("PatientId must be provided.");

            RuleFor(request => request.PharmacyId).NotNull().WithMessage("PharmacyId must be provided.");

            RuleFor(request => request.PickupDeadline).NotNull().WithMessage("Pickup deadline must be provided.");

            RuleFor(request => request.Medicines).NotNull().NotEmpty().WithMessage("At least one medicine have to be reserved.");

            RuleForEach(request => request.Medicines).NotNull().SetValidator(new CreateReservedMedicineRequestValidator()).WithMessage("Valid reserved medicine must be provided.");
        }
    }
}