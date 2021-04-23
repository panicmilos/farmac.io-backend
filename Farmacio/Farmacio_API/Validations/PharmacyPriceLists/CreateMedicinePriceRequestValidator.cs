using Farmacio_API.Contracts.Requests.Medicines;
using Farmacio_API.Contracts.Requests.PharmacyMedicines;
using Farmacio_API.Contracts.Requests.PharmacyPriceLists;
using FluentValidation;

namespace Farmacio_API.Validations.PharmacyPriceLists
{
    public class CreateMedicinePriceRequestValidator : AbstractValidator<CreateMedicinePriceRequest>
    {
        public CreateMedicinePriceRequestValidator()
        {
            RuleFor(request => request.MedicineId).NotNull().WithMessage("Medicine id must be provided.");
            RuleFor(request => request.Price).GreaterThan(0)
                .WithMessage("Price must be grater than 0.");
        }
    }
}