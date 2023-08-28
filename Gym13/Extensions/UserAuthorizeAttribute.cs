using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gym13.Extensions
{
    public class UserAuthorizeAttribute : TypeFilterAttribute
    {
        public UserAuthorizeAttribute() : base(typeof(AuthorizeFilter))
        {
        }
    }

    public class AuthorizeFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasClaim = context.HttpContext.User.Claims.Any(c => c.Type == ClaimTypes.NameIdentifier);
            if (!hasClaim)
                context.Result = new UnauthorizedResult();
        }
    }
}
