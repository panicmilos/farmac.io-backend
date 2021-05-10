using Farmacio_API.Contracts.Requests.AbsenceRequests;
using Farmacio_API.Contracts.Requests.Reservations;
using FluentValidation;

namespace Farmacio_API.Validations.AbsenceRequests
{
    public class DeclineAbsenceRequestValidator : AbstractValidator<DeclineAbsenceRequestRequest>
    {
        public DeclineAbsenceRequestValidator()
        {
            RuleFor(request => request.Reason).NotNull().WithMessage("The reason for declination must be provided.");
        }
    }
}