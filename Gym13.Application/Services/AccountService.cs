using Gym13.Application.Interfaces;
using Gym13.Application.Models;
using Gym13.Application.Models.Account;
using Gym13.Common;
using Gym13.Common.Enums;
using Gym13.Domain.Data;
using Gym13.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Gym13.Application.Services
{
    public class AccountService : BaseService, IAccountService
    {
        readonly Gym13DbContext _db;
        readonly UserManager<ApplicationUser> _userManager;
        readonly IEmailSender _emailSender;
        readonly ISmsSender _smsSender;

        public AccountService(Gym13DbContext db, UserManager<ApplicationUser> userManager, IEmailSender emailSender, ISmsSender smsSender)
        {
            _db = db;
            _userManager = userManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
        }

        public async Task<RegistrationResponseModel> CreateAccount(RegistrationRequestModel request)
        {
            ApplicationUser? existingUser = null;
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null && user.EmailConfirmed)
                existingUser = user;
            else
                existingUser = _userManager.Users.FirstOrDefault(u => u.PhoneNumber == request.PhoneNumber);
            if (existingUser != null)
            {
                if (existingUser.EmailConfirmed && existingUser.Status != ApplicationUserStatus.NotConfirmed)
                    return Fail<RegistrationResponseModel>(message: Gym13Resources.UserExists);
                else
                {
                    existingUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(existingUser, request.Password);
                    GenerateValidationCode(existingUser);
                    var result = await _userManager.UpdateAsync(existingUser);
                    if (!result.Succeeded)
                        return Fail<RegistrationResponseModel>();

                    await SendValidationCode(existingUser, "რეგისტრაციის დასრულება");
                    return Success(new RegistrationResponseModel { UserId = existingUser.Id });
                }
            }
            else
            {
                var newUser = new ApplicationUser
                {
                    UserName = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Gender = request.Gender,
                    Status = ApplicationUserStatus.NotConfirmed
                };

                newUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(newUser, request.Password);
                GenerateValidationCode(newUser);

                var result = await _userManager.CreateAsync(newUser);
                if (!result.Succeeded)
                    return Fail<RegistrationResponseModel>(message: Gym13Resources.BadRequest);

                await SendValidationCode(existingUser, "რეგისტრაციის დასრულება");
                return Success(new RegistrationResponseModel { UserId = newUser.Id });
            }
        }

        public async Task<BaseResponseModel> ConfirmEmail(ConfirmUserRequestModel request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return Fail(new BaseResponseModel(), message: Gym13Resources.UserNotExists);
            if (!user.Email.Equals(request.EmailOrPhone))
            {
                var userByEmail = await _userManager.FindByEmailAsync(request.EmailOrPhone);
                if (userByEmail != null)
                    return Fail(new BaseResponseModel(), message: Gym13Resources.EmailExists);
                else
                    user.Email = request.EmailOrPhone;
            }
            var success = ConfirmCode(user, request.Code);
            if (success)
            {
                if (user.Status == ApplicationUserStatus.NotConfirmed)
                    user.Status = ApplicationUserStatus.Active;
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                return Success(new BaseResponseModel());
            }
            else
                return Fail(new BaseResponseModel(), message: Gym13Resources.IncorrectCode);
        }

        public async Task<BaseResponseModel> ConfirmPhoneNumber(ConfirmUserRequestModel request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return Fail(new BaseResponseModel(), message: Gym13Resources.UserNotExists);
            if (!user.PhoneNumber.Equals(request.EmailOrPhone))
            {
                var userByPhone = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.EmailOrPhone);
                if (userByPhone != null)
                    return Fail(new BaseResponseModel(), message: Gym13Resources.PhoneNumberExists);
                else
                    user.PhoneNumber = request.EmailOrPhone;
            }
            var success = ConfirmCode(user, request.Code);
            if (success)
            {
                user.PhoneNumberConfirmed = true;
                await _userManager.UpdateAsync(user);
                return Success(new BaseResponseModel());
            }
            else
                return Fail(new BaseResponseModel(), message: Gym13Resources.IncorrectCode);
        }

        #region Private methods
        static string GenerateValidationCode(ApplicationUser user)
        {
            var code = new Random(Guid.NewGuid().GetHashCode()).Next(1000, 9999).ToString();
            user.ValidationCode = code;
            user.ValidationCodeDateCreated = DateTime.UtcNow.AddHours(4);
            return code;
        }
        async Task SendValidationCode(ApplicationUser user, string? subject, bool sendOnEmail = true)
        {
            var text = $"დადასტურების კოდი: {user.ValidationCode}";
            if (sendOnEmail)
                await _emailSender.SendEmail(user.UserName, subject, text, NotificationType.Registration, user);
            else
                await _smsSender.SendSmsAsync(user.UserName, text, NotificationType.Registration, user);
        }
        static bool ConfirmCode(ApplicationUser user, string code)
        {
            if (string.IsNullOrEmpty(user.ValidationCode) || !user.ValidationCodeDateCreated.HasValue)
                return false;
            if (user.ValidationCodeDateCreated.HasValue && user.ValidationCodeDateCreated.Value.AddHours(12) < DateTime.UtcNow.AddHours(4))
                return false;
            else if (!user.ValidationCode.Equals(code))
                return false;
            else
            {
                user.ValidationCode = null;
                user.ValidationCodeDateCreated = null;
                return true;
            }
        }
        #endregion
    }
}
