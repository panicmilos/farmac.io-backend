using Farmacio_API.Contracts.Requests.Medicines;
using Farmacio_API.Contracts.Requests.PharmacyMedicines;
using Farmacio_API.Contracts.Requests.PharmacyPriceLists;
using FluentValidation;

namespace Farmacio_API.Validations.PharmacyPriceLists
{
    public class UpdatePharmacyPriceListRequestValidator : AbstractValidator<UpdatePharmacyPriceListRequest>
    {
        public UpdatePharmacyPriceListRequestValidator()
        {
            RuleFor(request => request.Id).NotNull().WithMessage("Id must be provided.");
            RuleFor(request => request.PharmacyId).NotNull().WithMessage("Pharmacy id must be provided.");
            RuleFor(request => request.ExaminationPrice).GreaterThan(0)
                .WithMessage("Examination price must be grater than 0.");
            RuleFor(request => request.ConsultationPrice).GreaterThan(0)
                .WithMessage("Consultation price must be grater than 0.");
            RuleFor(request => request.MedicinePriceList).NotNull().NotEmpty()
                .WithMessage("Medicine price list must be provided.");
            RuleForEach(request => request.MedicinePriceList).NotNull()
                .SetValidator(new CreateMedicinePriceRequestValidator()).WithMessage("Invalid medicine price.");
        }
    }
}