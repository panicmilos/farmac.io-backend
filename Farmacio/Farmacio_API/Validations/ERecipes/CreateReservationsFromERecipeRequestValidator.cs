using Farmacio_API.Contracts.Requests.ERecipes;
using FluentValidation;

namespace Farmacio_API.Validations.ERecipes
{
    public class CreateReservationsFromERecipeRequestValidator : AbstractValidator<CreateReservationFromERecipeRequest>
    {
        public CreateReservationsFromERecipeRequestValidator()
        {
            RuleFor(request => request.ERecipeId).NotNull().NotEmpty().WithMessage("ERecipe id must be provided.");

            RuleFor(request => request.PharmacyId).NotNull().NotEmpty().WithMessage("Pharmacy id must be provided.");

            RuleFor(request => request.PickupDeadline).NotNull().WithMessage("Pickup deadline must be provided.");
        }
    }
}