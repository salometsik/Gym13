using FluentValidation;
using Gym13.Application.Models.Account;
using Gym13.Common;

namespace Gym13.Application.Validators
{
    public sealed class ConfirmEmailOrPhoneRequestValidator : AbstractValidator<ConfirmEmailOrPhoneRequestModel>
    {
        public ConfirmEmailOrPhoneRequestValidator()
        {
            RuleFor(x => x.EmailOrPhone).NotEmpty().NotNull().WithMessage(Gym13Resources.UserOrPhoneNull);
            RuleFor(x => x.Code).NotEmpty().NotNull().WithMessage(Gym13Resources.ValidationCodeNull);
        }
    }
}
