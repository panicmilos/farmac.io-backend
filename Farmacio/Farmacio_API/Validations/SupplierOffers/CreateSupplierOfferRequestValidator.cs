using Farmacio_API.Contracts.Requests;
using FluentValidation;
using System;

namespace Farmacio_API.Validations.SupplierOffers
{
    public class CreateSupplierOfferRequestValidator : AbstractValidator<CreateSupplierOfferRequest>
    {
        public CreateSupplierOfferRequestValidator()
        {
            RuleFor(request => request.SupplierId).NotNull().WithMessage("SupplierId must be provided.");

            RuleFor(request => request.PharmacyOrderId).NotNull().WithMessage("PharmacyOrderId must be provided.");

            RuleFor(request => request.TotalPrice).GreaterThan(0).WithMessage("Total price must be greater than 0.");

            RuleFor(request => request.DeliveryDeadline).NotNull().Must(date => date > DateTime.Now.AddHours(1)).WithMessage("Delivery date must be at least one hour from now.");
        }
    }
}