using Farmacio_API.Contracts.Requests.Complaints;
using FluentValidation;

namespace Farmacio_API.Validations.Complaints
{
    public class CreateComplaintAboutDharmacyRequestValidator : AbstractValidator<CreateComplaintAboutPharmacyRequest>
    {
        public CreateComplaintAboutDharmacyRequestValidator()
        {
            RuleFor(request => request.WriterId).NotNull().NotEmpty().WithMessage("Writer id must be provided.");

            RuleFor(request => request.PharmacyId).NotNull().NotEmpty().WithMessage("Pharmacy id must be provided.");

            RuleFor(request => request.Text).NotNull().NotEmpty().WithMessage("Complaint text must be provided.");
        }
    }
}