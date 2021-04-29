using Farmacio_API.Contracts.Requests.Complaints;
using FluentValidation;

namespace Farmacio_API.Validations.Complaints
{
    public class CreateComplaintAboutPharmacistRequestValidator : AbstractValidator<CreateComplaintAboutPharmacistRequest>
    {
        public CreateComplaintAboutPharmacistRequestValidator()
        {
            RuleFor(request => request.WriterId).NotNull().NotEmpty().WithMessage("Writer id must be provided.");

            RuleFor(request => request.PharmacistId).NotNull().NotEmpty().WithMessage("Pharmacist id must be provided.");

            RuleFor(request => request.Text).NotNull().NotEmpty().WithMessage("Complaint text must be provided.");
        }
    }
}