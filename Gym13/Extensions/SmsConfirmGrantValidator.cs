using Gym13.Persistence.DTOs;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gym13.Extensions
{
    public class SmsConfirmGrantValidator : IExtensionGrantValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public SmsConfirmGrantValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public string GrantType => "SmsConfirm";
        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var code = context.Request.Raw.Get("Code");
            var phoneNumber = context.Request.Raw.Get("PhoneNumber");
            if (string.IsNullOrEmpty(phoneNumber) && string.IsNullOrEmpty(code))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest);
                return;
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(s=>s.PhoneNumber==phoneNumber);
            if (user == null)
            {
                user = !string.IsNullOrEmpty(phoneNumber) ? await _userManager.FindByEmailAsync(phoneNumber) : null;
            }
            if (user == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest);
                return;
            }
            //if (user.ValidationCode == code)
            //{
            //    context.Result = new GrantValidationResult(user.Id, AuthenticationMethods.OneTimePassword);
            //    user.ValidationCode = "";
            //   await _userManager.UpdateAsync(user);
            //    return;
            //}
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
        }
    }
}
