using Farmacio_API.Contracts.Requests.Medicines;
using Farmacio_API.Contracts.Requests.PharmacyMedicines;
using Farmacio_API.Contracts.Requests.PharmacyPriceLists;
using FluentValidation;

namespace Farmacio_API.Validations.PharmacyPriceLists
{
    public class CreatePharmacyPriceListRequestValidator : AbstractValidator<CreatePharmacyPriceListsRequest>
    {
        public CreatePharmacyPriceListRequestValidator()
        {
            RuleFor(request => request.PharmacyId).NotNull().WithMessage("Pharmacy id must be provided.");
            RuleFor(request => request.ExaminationPrice).GreaterThan(0)
                .WithMessage("Examination price must be grater than 0.");
            RuleFor(request => request.ConsultationPrice).GreaterThan(0)
                .WithMessage("Consultation price must be grater than 0.");
            RuleFor(request => request.MedicinePriceList).NotNull().NotEmpty()
                .WithMessage("Medicine price list must be provided.");
        }
    }
}