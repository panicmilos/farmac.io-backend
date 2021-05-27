using Farmacio_API.Contracts.Requests.Appointments;
using FluentValidation;

namespace Farmacio_API.Validations.Appointments
{
    public class CreateAppointmentRequestValidator : AbstractValidator<CreateAppointmentRequest>
    {
        public CreateAppointmentRequestValidator()
        {
            RuleFor(request => request.Duration).NotNull().NotEmpty().WithMessage("Duration must be provided.");
            RuleFor(request => request.DateTime).NotNull().NotEmpty().WithMessage("Date-time must be provided.");
            RuleFor(request => request.PharmacyId).NotNull().NotEmpty().WithMessage("Pharmacy id name must be provided.");
            RuleFor(request => request.MedicalStaffId).NotNull().NotEmpty().WithMessage("Medical staff's id must be provided.");
        }
    }
}