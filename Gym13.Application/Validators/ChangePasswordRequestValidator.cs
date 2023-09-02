using FluentValidation;
using Gym13.Application.Models.Account;
using Gym13.Common.Resources;

namespace Gym13.Application.Validators
{
    public sealed class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequestModel>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.Password).NotNull().NotEmpty().MinimumLength(6)
                .WithMessage(Gym13Resources.PasswordMustContainSixCharacters);
            RuleFor(x => x.ConfirmPassword).NotNull().NotEmpty().MinimumLength(6).Equal(x => x.Password)
                .WithMessage(Gym13Resources.PasswordsMustMatch);
        }
    }
}
