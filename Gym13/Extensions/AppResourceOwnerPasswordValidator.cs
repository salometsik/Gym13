using Gym13.Persistence.DTOs;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using static IdentityModel.OidcConstants;

namespace Gym13.Extensions
{
    public class AppResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AppResourceOwnerPasswordValidator> _logger;
        public AppResourceOwnerPasswordValidator(ILogger<AppResourceOwnerPasswordValidator> logger, UserManager<ApplicationUser> userManager
            , SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _userManager.FindByNameAsync(context.UserName);

            if (user == null)
                user = await _userManager.FindByEmailAsync(context.UserName);

            if (user == null)
                user = _userManager.Users.FirstOrDefault(x => x.PhoneNumber == context.UserName);

            if (user == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest);
                return;
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, context.Password, false);

            if (result.Succeeded)
            {
                _logger.LogInformation("Credentials validated for username: {username}", context.UserName);

                context.Result = new GrantValidationResult(user.Id, AuthenticationMethods.Password);
                return;
            }

            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest);
        }
    }
}

