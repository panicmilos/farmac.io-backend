using FluentValidation;
using System;

namespace Farmacio_API.Validations.Extensions
{
    public static class NumericalValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> IsDouble<T>(this IRuleBuilder<T, string> rule)
        {
            return rule.NotEmpty()
                       .NotNull()
                       .Must(IsDouble);
        }

        private static bool IsDouble(String number)
        {
            return Double.TryParse(number, out _);
        }
    }
}