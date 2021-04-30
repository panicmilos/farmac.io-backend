using Farmacio_API.Contracts.Requests.Complaints;
using FluentValidation;

namespace Farmacio_API.Validations.Complaints
{
    public class CreateComplaintAnswerRequestValidator : AbstractValidator<CreateComplaintAnswerRequest>
    {
        public CreateComplaintAnswerRequestValidator()
        {
            RuleFor(request => request.WriterId).NotNull().NotEmpty().WithMessage("Writer id must be provided.");

            RuleFor(request => request.ComplaintId).NotNull().NotEmpty().WithMessage("Complaint id must be provided.");

            RuleFor(request => request.Text).NotNull().NotEmpty().WithMessage("Complaint text must be provided.");
        }
    }
}