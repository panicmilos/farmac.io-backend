using FluentValidation;

namespace Farmacio_API.Validations.Extensions
{
    public static class AccountValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> Username<T>(this IRuleBuilder<T, string> username)
        {
            return username.NotNull()
                .NotEmpty()
                .Matches(@"^[a-zA-Z0-9]+$");
        }

        public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> password)
        {
            return password.NotNull()
                .NotEmpty()
                .Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$");
        }
    }
}