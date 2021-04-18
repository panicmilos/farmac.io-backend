using Farmacio_API.Contracts.Requests.Addresses;
using Farmacio_API.Contracts.Requests.WorkTimes;
using Farmacio_Models.Domain;
using Farmacio_Services.Implementation.Validation;
using FluentValidation;

namespace Farmacio_API.Validations.WorkTimes
{
    public class WorkTimeRequestValidator : AbstractValidator<WorkTimeRequest>
    {
        public WorkTimeRequestValidator()
        {
            RuleFor(request => request.From).NotNull().NotEmpty().WithMessage("From date-time must be provided.");
            RuleFor(request => request.To).NotNull().NotEmpty().WithMessage("To date-time must be provided.");
        }
    }
}