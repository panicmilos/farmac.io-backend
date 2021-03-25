using Farmacio_API.Contracts.Requests.Addresses;
using FluentValidation;

namespace Farmacio_API.Validations.Addresses
{
    public class UpdateAddressRequestValidator : AbstractValidator<UpdateAddressRequest>
    {
        public UpdateAddressRequestValidator()
        {
            RuleFor(request => request.Id).NotNull().WithMessage("Address Id can't be null.");
            RuleFor(request => request.City).NotNull().NotEmpty().WithMessage("City must be provided.");
            RuleFor(request => request.State).NotNull().NotEmpty().WithMessage("State must be provided.");
            RuleFor(request => request.StreetName).NotNull().NotEmpty().WithMessage("Street name must be provided.");
            RuleFor(request => request.StreetNumber).NotNull().NotEmpty().WithMessage("Street number must be provided.");
        }
    }
}