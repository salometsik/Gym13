using Gym13.Application.Interfaces;
using Gym13.Application.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym13.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AccountController : ControllerBase
    {
        readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<RegistrationResponseModel> Register(RegistrationRequestModel request) => await _accountService.CreateAccount(request);

        //[HttpGet]
        //public async Task<ApplicationUser?> GetUser(string userId) => await _accountService.GetUser(userId);
    }
}
