using FluentValidation;
using Gym13.Application.Models.Account;
using Gym13.Common;
using System.Text.RegularExpressions;

namespace Gym13.Application.Validators
{
    public sealed class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequestModel>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.FirstName).NotNull().NotEmpty();
            RuleFor(x => x.LastName).NotNull().NotEmpty();
            RuleFor(x => x.Email).NotNull().NotEmpty()
                .Must(x => ValidateEmailOrPhone.IsValidEmail(x)).WithMessage(Gym13Resources.IncorrectData);
            RuleFor(x => x.PhoneNumber).NotNull().NotEmpty()
                .Must(x => Regex.IsMatch(x, @"^\d{9}$")).WithMessage(Gym13Resources.IncorrectData);
        }
    }
}
