using Gym13.Application.Interfaces;
using Gym13.Application.Models.Account;
using Gym13.Common;
using Gym13.Common.Enums;
using Gym13.Domain.Data;
using Gym13.Domain.Models;
using Microsoft.AspNetCore.Identity;
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
                existingUser = _userManager.Users.FirstOrDefault(u => u.PhoneNumber == request.PhoneNumber
                    && u.PhoneNumberConfirmed);
            if (existingUser != null && (existingUser.PhoneNumberConfirmed || existingUser.EmailConfirmed))
            {
                if (existingUser.Status != ApplicationUserStatus.NotConfirmed)
                    return Fail<RegistrationResponseModel>(message: Gym13Resources.UserExists);

                GenerateValidationCode(existingUser);
                var result = await _userManager.UpdateAsync(existingUser);
                if (!result.Succeeded)
                    return Fail<RegistrationResponseModel>();

                await SendValidationCode(existingUser, "რეგისტრაციის დასრულება");
                return Success(new RegistrationResponseModel { UserId = existingUser.Id });
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

        #endregion
    }
}
