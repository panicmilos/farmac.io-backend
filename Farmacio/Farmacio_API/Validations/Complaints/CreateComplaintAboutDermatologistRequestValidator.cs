using Farmacio_API.Contracts.Requests.Complaints;
using FluentValidation;

namespace Farmacio_API.Validations.Complaints
{
    public class CreateComplaintAboutDermatologistRequestValidator : AbstractValidator<CreateComplaintAboutDermatologistRequest>
    {
        public CreateComplaintAboutDermatologistRequestValidator()
        {
            RuleFor(request => request.WriterId).NotNull().NotEmpty().WithMessage("Writer id must be provided.");

            RuleFor(request => request.DermatologistId).NotNull().NotEmpty().WithMessage("Dermatologist id must be provided.");

            RuleFor(request => request.Text).NotNull().NotEmpty().WithMessage("Complaint text must be provided.");
        }
    }
}