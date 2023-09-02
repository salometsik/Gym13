using FluentValidation;
using Gym13.Application.Models.Account;
using Gym13.Common.Resources;

namespace Gym13.Application.Validators
{
    public sealed class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequestModel>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.Password).NotEmpty().NotNull().MinimumLength(6)
                .WithMessage(Gym13Resources.PasswordMustContainSixCharacters);
            RuleFor(x => x.ConfirmPassword).NotEmpty().NotNull().MinimumLength(6).Equal(x => x.Password)
                .WithMessage(Gym13Resources.PasswordsMustMatch);
            RuleFor(x => x.EmailOrPhone).NotEmpty().NotNull();
        }
    }
}
