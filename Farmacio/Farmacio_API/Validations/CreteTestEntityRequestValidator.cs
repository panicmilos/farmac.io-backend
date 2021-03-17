using Farmacio_API.Contracts.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farmacio_API.Validations
{
    public class CreteTestEntityRequestValidator : AbstractValidator<CreateTestEntityRequest>
    {
        public CreteTestEntityRequestValidator()
        {
            RuleFor(request => request.Text)
                .NotNull().WithMessage("Ne sme biti null")
                .NotEmpty().WithMessage("Ne sme biti prazna")
                .MaximumLength(100).WithMessage("TEXT ima vise od 50 karaktera ")
                .Must(text => text.StartsWith("Jovana")).WithMessage("NIJE JOVANA :(");
        }
    }
}