using Farmacio_API.Contracts.Requests.Medicines;
using Farmacio_API.Contracts.Requests.PharmacyMedicines;
using Farmacio_API.Contracts.Requests.PharmacyPriceLists;
using Farmacio_API.Contracts.Requests.PharmacyReports;
using FluentValidation;

namespace Farmacio_API.Validations.PharmacyReports
{
    public class TimePeriodRequestValidator : AbstractValidator<TimePeriodRequest>
    {
        public TimePeriodRequestValidator()
        {
            RuleFor(request => request.From).NotNull().WithMessage("Date from must be provided.");
            RuleFor(request => request.To).NotNull().WithMessage("Date to must be provided.");
        }
    }
}