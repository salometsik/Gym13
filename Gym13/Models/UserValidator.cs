using Gym13.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Gym13.Models
{
    public class UserValidator : IUserValidator<ApplicationUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
