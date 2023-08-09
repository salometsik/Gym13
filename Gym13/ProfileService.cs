using System.Security.Claims;
using Gym13.Domain.Models;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace Gym13
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            var user = await userManager.FindByIdAsync(subjectId);

            if (user == null)
                throw new NullReferenceException("User Not Found");

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.PreferredUserName, user.UserName)
            };

            if (user.Email != null)
            {
                claims.Add(new Claim(JwtClaimTypes.Email, user.Email));
                claims.Add(new Claim(JwtClaimTypes.EmailVerified, user.EmailConfirmed.ToString()));
            }

            if (user.PhoneNumber != null)
            {
                claims.Add(new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber));
                claims.Add(new Claim(JwtClaimTypes.PhoneNumberVerified, user.PhoneNumberConfirmed.ToString()));
            }

            claims.Add(new Claim(JwtClaimTypes.Name, user.UserName));
            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            var user = await userManager.FindByIdAsync(subjectId);

            context.IsActive = user != null;
        }
    }
}