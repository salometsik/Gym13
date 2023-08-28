using FluentValidation;
using Gym13.Application.Models.Account;

namespace Gym13.Application.Validators
{
    public sealed class RegistrationRequestValidator : AbstractValidator<RegistrationRequestModel>
    {
        public RegistrationRequestValidator()
        {
            RuleFor(x => x.FirstName).NotNull().NotEmpty();
            RuleFor(x => x.LastName).NotNull().NotEmpty();
            RuleFor(x => x.Gender).NotNull().NotEmpty();
            RuleFor(x => x.Email).NotNull().NotEmpty();
            RuleFor(x => x.PhoneNumber).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty().MinimumLength(6);
            RuleFor(x => x.ConfirmPassword).NotNull().NotEmpty().MinimumLength(6).Equal(x => x.Password);
        }
    }
}
