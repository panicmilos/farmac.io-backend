using Farmacio_API.Contracts.Requests.Followings;
using FluentValidation;

namespace Farmacio_API.Validations.Followings
{
    public class CreatePatientFollowRequestValidator : AbstractValidator<CreatePatientFollowRequest>
    {
        public CreatePatientFollowRequestValidator()
        {
            RuleFor(request => request.PatientId).NotNull().WithMessage("PatientId must be provided.");

            RuleFor(request => request.PharmacyId).NotNull().WithMessage("PharmacyId must be provided.");
        }
    }
}