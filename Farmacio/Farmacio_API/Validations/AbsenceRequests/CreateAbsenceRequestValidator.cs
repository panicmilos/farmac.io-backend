using Farmacio_API.Contracts.Requests.AbsenceRequests;
using Farmacio_API.Contracts.Requests.Reservations;
using FluentValidation;

namespace Farmacio_API.Validations.AbsenceRequests
{
    public class CreateAbsenceRequestValidator : AbstractValidator<CreateAbsenceRequestRequest>
    {
        public CreateAbsenceRequestValidator()
        {
            RuleFor(request => request.Type).NotNull().WithMessage("Type must be provided.");
            RuleFor(request => request.RequesterId).NotNull().WithMessage("Requester id must be provided.");
            RuleFor(request => request.FromDate).NotNull().WithMessage("From date must be provided.");
            RuleFor(request => request.ToDate).NotNull().WithMessage("To date must be provided.");
        }
    }
}