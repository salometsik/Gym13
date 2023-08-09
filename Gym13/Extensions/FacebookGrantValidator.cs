using Gym13.Domain.Models;
using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace Gym13.Extensions
{
    public class FacebookGrantValidator : IExtensionGrantValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly IUserService _userService;
        readonly ILogger<FacebookGrantValidator> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityResourceOwnerPasswordValidator{TUser}"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="signInManager">The sign in manager.</param>
        public FacebookGrantValidator(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            //IUserService userService,
            ILogger<FacebookGrantValidator> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            //_userService = userService;
            _logger = logger;
        }

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            //var userToken = context.Request.Raw.Get("token");
            var externalId = context.Request.Raw.Get("externalId");
            if (string.IsNullOrEmpty(externalId) || externalId == "undefined")
            {
                context.Result = new GrantValidationResult(IdentityServer4.Models.TokenRequestErrors.InvalidRequest, "undefined");
                return;
            }
            var user = await _userManager.FindByLoginAsync("Facebook", externalId);
            if (user != null)
            {
                var result = await _signInManager.ExternalLoginSignInAsync("Facebook", externalId, isPersistent: false);

                if (result.Succeeded)
                {
                    context.Result = new GrantValidationResult(user.Id.ToString(), OidcConstants.AuthenticationMethods.MultipleChannelAuthentication);
                    return;
                }
            }
            else
            {
                var email = context.Request.Raw.Get("email"); //todo: info.Principal.FindFirstValue(ClaimTypes.Email);
                var displayName = context.Request.Raw.Get("displayName");

                user = await _userManager.FindByEmailAsync(email);

                IdentityResult result = null;
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true,
                        RegistrationDate = DateTime.UtcNow.AddHours(4),
                    };

                    result = await _userManager.CreateAsync(user);
                }

                if (result == null || result.Succeeded)
                {
                    //var @event = new UserCreatedIntegrationEvent(user);
                    //await _integrationEventService.AddAndSaveEventAsync(@event);
                    //await _integrationEventService.PublishThroughEventBusAsync(@event);

                    if (user != null && string.IsNullOrEmpty(user.SecurityStamp))
                        user.SecurityStamp = Guid.NewGuid().ToString();
                    result = await _userManager.AddLoginAsync(user, new UserLoginInfo("Facebook", externalId, displayName));
                    _logger.LogCritical("FacebookGrantValidator" + JsonConvert.SerializeObject(result));
                    _logger.LogCritical("FacebookGrantValidator user " + JsonConvert.SerializeObject(user));
                    _logger.LogCritical("FacebookGrantValidator UserLoginInfo: " + "Facebook " + " : " + externalId + " : " + displayName);
                    if (result.Succeeded)
                    {
                        if (string.IsNullOrEmpty(user.SecurityStamp))
                        {
                            await _userManager.UpdateSecurityStampAsync(user);
                        }
                        await _signInManager.SignInAsync(user, isPersistent: false);

                        context.Result = new GrantValidationResult(user.Id.ToString(), OidcConstants.AuthenticationMethods.MultipleChannelAuthentication);
                        return;
                    }
                    else
                    {
                        var err = string.Join(", ", result.Errors.Select(x => x.Code + ": " + x.Description).ToArray());
                        context.Result = new GrantValidationResult(IdentityServer4.Models.TokenRequestErrors.InvalidRequest, err);
                    }
                }
                else
                {
                    var err = string.Join(", ", result.Errors.Select(x => x.Code + ": " + x.Description).ToArray());
                    context.Result = new GrantValidationResult(IdentityServer4.Models.TokenRequestErrors.InvalidRequest, err);
                }

            }

        }

        public string GrantType => "facebook";
    }
}
