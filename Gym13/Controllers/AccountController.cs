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

        [HttpGet]
        [UserAuthorize]
        public async Task<UserProfileModel> GetUser() => await _accountService.GetUser(UserId);

        [HttpPost]
        [AllowAnonymous]
        public async Task<RegistrationResponseModel> Register(RegistrationRequestModel request)
            => await _accountService.CreateAccount(request);

        [HttpPut]
        [UserAuthorize]
        public async Task<BaseResponseModel> UpdateUser(UpdateUserRequestModel request)
        {
            var model = new UpdateUserModel
            {
                UserId = UserId,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = request.Gender,
                BirthDate = request.BirthDate
            };
            return await _accountService.UpdateUser(model);
        }

        [HttpPut("confirm-user")]
        public async Task<BaseResponseModel> ConfirmUser(ConfirmEmailOrPhoneRequestModel request)
            => await _accountService.ConfirmValidationCode(null, request.EmailOrPhone, request.Code);

        [HttpPut("confirm-code")]
        [UserAuthorize]
        public async Task<BaseResponseModel> ConfirmCode(ConfirmEmailOrPhoneRequestModel request)
            => await _accountService.ConfirmValidationCode(UserId, request.EmailOrPhone, request.Code);

        [HttpPatch("send-code-from-profile")]
        [UserAuthorize]
        public async Task<BaseResponseModel> SendCodeFromProfile(string to) => await _accountService.SendCodeFromProfile(to, UserId);

        [HttpPatch("send-validation-code")]
        public async Task<BaseResponseModel> SendValidationCode(string to) => await _accountService.SendValidationCode(to);

        [HttpPatch("reset-password")]
        public async Task<BaseResponseModel> ResetPassword(ResetPasswordRequestModel request)
        {
            var model = new UpdatePasswordModel
            {
                EmailOrPhone = request.EmailOrPhone,
                Password = request.Password
            };
            return await _accountService.UpdatePassword(model);
        }

        [HttpPatch("change-password")]
        [UserAuthorize]
        public async Task<BaseResponseModel> ChangePassword(ChangePasswordRequestModel request)
        {
            var model = new UpdatePasswordModel
            {
                UserId = UserId,
                Password = request.Password,
                CurrentPassword = request.CurrentPassword
            };
            return await _accountService.UpdatePassword(model);
        }

    }
}
