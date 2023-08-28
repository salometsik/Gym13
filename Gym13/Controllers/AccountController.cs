using Gym13.Application.Interfaces;
using Gym13.Application.Models;
using Gym13.Application.Models.Account;
using Gym13.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gym13.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AccountController : ControllerBase
    {
        readonly IAccountService _accountService;
        protected string UserId => User?.Claims?.FirstOrDefault(a => a.Type == "sub" || a.Type == ClaimTypes.NameIdentifier)?.Value;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<RegistrationResponseModel> Register(RegistrationRequestModel request)
            => await _accountService.CreateAccount(request);

        [HttpPut("confirm-user")]
        public async Task<BaseResponseModel> ConfirmUser(ConfirmUserRequestModel request)
            => await _accountService.ConfirmEmail(request);

        [HttpPut("confirm-email")]
        [UserAuthorize]
        public async Task<BaseResponseModel> ConfirmEmail(ConfirmEmailOrPhoneRequestModel request)
        {
            var requestModel = new ConfirmUserRequestModel
            {
                Code = request.Code,
                EmailOrPhone = request.EmailOrPhone,
                UserId = UserId
            };
            return await _accountService.ConfirmEmail(requestModel);
        }

        [HttpPut("confirm-phone")]
        [UserAuthorize]
        public async Task<BaseResponseModel> ConfirmPhoneNumber(ConfirmEmailOrPhoneRequestModel request)
        {
            var requestModel = new ConfirmUserRequestModel
            {
                Code = request.Code,
                EmailOrPhone = request.EmailOrPhone,
                UserId = UserId
            };
            return await _accountService.ConfirmPhoneNumber(requestModel);
        }

        //[HttpGet]
        //public async Task<ApplicationUser?> GetUser(string userId) => await _accountService.GetUser(userId);
    }
}
