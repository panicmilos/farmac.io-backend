﻿using Farmacio_API.Contracts.Requests.Accounts;
using FluentValidation;

namespace Farmacio_API.Validations.Accounts
{
    public class UpdatePharmacistUserRequestValidator : UpdateUserRequestValidator<UpdatePharmacistUserRequest>
    {
        public UpdatePharmacistUserRequestValidator() : base()
        {
            RuleFor(request => request.PharmacyId).NotNull().WithMessage("PharmacyId must be provided.");
        }
    }
}