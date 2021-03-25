using System;
using System.Linq;
using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_API.Validations.Addresses;
using FluentValidation;

namespace Farmacio_API.Validations.Accounts
{
    public class UpdatePharmacistUserRequestValidator : AbstractValidator<UpdatePharmacistUserRequest>
    {
        public UpdatePharmacistUserRequestValidator() : base()
        {
            RuleFor(request => request.Id).NotNull().WithMessage("Pharmacist's Id can't be null.");
            RuleFor(request => request.FirstName).NotNull().NotEmpty().WithMessage("First name must be provided.");
            RuleFor(request => request.LastName).NotNull().NotEmpty().WithMessage("Last name name must be provided.");
            RuleFor(request => request.DateOfBirth).NotNull().Must(dateOfBirth => dateOfBirth.AddYears(13) < DateTime.Now).WithMessage("User must be at least 13 years old.");
            RuleFor(request => request.PID).NotNull().Length(13).Must(pid => pid.All(charInPid => '0' <= charInPid && charInPid <= '9')).WithMessage("Pid must be 13 digits.");
            RuleFor(request => request.PhoneNumber).NotNull().NotEmpty().WithMessage("Phone number must be provided.");
            RuleFor(request => request.Address).SetValidator(new UpdateAddressRequestValidator()).WithMessage("Valid address must be provided.");
        }
    }
}