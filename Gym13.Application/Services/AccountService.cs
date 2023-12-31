﻿using Gym13.Application.Interfaces;
using Gym13.Application.Models;
using Gym13.Application.Models.Account;
using Gym13.Application.Validators;
using Gym13.Common.Enums;
using Gym13.Common.Resources;
using Gym13.Domain.Data;
using Gym13.Domain.Models;
using IdentityModel.Client;
using IdentityServer4;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.Models;
using Gym13.Extensions;

namespace Gym13.Application.Services
{
    public class AccountService : BaseService, IAccountService
    {
        readonly Gym13DbContext _db;
        readonly UserManager<ApplicationUser> _userManager;
        readonly IEmailSender _emailSender;
        readonly ISmsSender _smsSender;
        readonly IHttpClientWrapper _httpClientWrapper;

        public AccountService(Gym13DbContext db, UserManager<ApplicationUser> userManager, IEmailSender emailSender, ISmsSender smsSender, IHttpClientWrapper httpClientWrapper)
        {
            _db = db;
            _userManager = userManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _httpClientWrapper = httpClientWrapper;
        }

        public async Task<RegistrationResponseModel> CreateAccount(RegistrationRequestModel request)
        {
            ApplicationUser? existingUser = null;
            bool notConfirmed = false;
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                if (!user.EmailConfirmed)
                    notConfirmed = true;
                else
                    existingUser = user;
            }
            else
            {
                existingUser = _userManager.Users.FirstOrDefault(u => u.PhoneNumber == request.PhoneNumber);
                if (existingUser != null && !existingUser.PhoneNumberConfirmed)
                    return Fail<RegistrationResponseModel>(message: Gym13Resources.UserExists);
            }
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

                    //await SendCode(existingUser, "რეგისტრაციის დასრულება");
                    return Success(new RegistrationResponseModel { UserId = existingUser.Id });
                }
            }
            else
            {
                if (notConfirmed)
                    return Fail<RegistrationResponseModel>(message: Gym13Resources.UserExists);
                var newUser = new ApplicationUser
                {
                    UserName = request.Email,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Gender = request.Gender,
                    BirthDate = request.BirthDate,
                    IdentificationNumber = request.IdentificationNumber,
                    Status = ApplicationUserStatus.NotConfirmed
                };

                newUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(newUser, request.Password);
                GenerateValidationCode(newUser);

                var result = await _userManager.CreateAsync(newUser);
                if (!result.Succeeded)
                    return Fail<RegistrationResponseModel>(message: Gym13Resources.BadRequest);

                var response = await _httpClientWrapper.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = "https://localhost:7258/connect/token",
                    ClientId = "Gym13Client",
                    ClientSecret = "Gym13Secret",
                    UserName = request.Email,
                    Password = request.Password,
                    Scope = "Gym13ToApi"
                });

                //await SendCode(newUser, "რეგისტრაციის დასრულება");
                return Success(new RegistrationResponseModel { UserId = newUser.Id });
            }
        }

        public async Task<BaseResponseModel> ConfirmValidationCode(string? userId, string emailOrPhone, string code)
        {
            ApplicationUser? user = null;
            if (!string.IsNullOrEmpty(userId))
            {
                user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return Fail<BaseResponseModel>(message: Gym13Resources.UserNotExists);
                var isPhone = ValidateEmailOrPhone.IsValidPhone(emailOrPhone);
                if (isPhone)
                {
                    if (ConfirmCode(user, code))
                    {
                        user.PhoneNumber = emailOrPhone;
                        user.PhoneNumberConfirmed = true;
                    }
                    else
                        return Fail<BaseResponseModel>(message: Gym13Resources.IncorrectCode);
                }
                else
                {
                    if (ConfirmCode(user, code))
                    {
                        user.Email = emailOrPhone;
                        user.EmailConfirmed = true;
                    }
                    else
                        return Fail<BaseResponseModel>(message: Gym13Resources.IncorrectCode);
                }
                await _userManager.UpdateAsync(user);
                return Success<BaseResponseModel>();
            }
            user = await _userManager.FindByEmailAsync(emailOrPhone);
            if (user == null)
                return Fail<BaseResponseModel>(message: Gym13Resources.UserNotExists);

            if (ConfirmCode(user, code))
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                return Success<BaseResponseModel>();
            }
            else
                return Fail<BaseResponseModel>(message: Gym13Resources.IncorrectCode);

        }

        public async Task<BaseResponseModel> SendCodeFromProfile(string to, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Fail<BaseResponseModel>(message: Gym13Resources.UserNotExists);
            var userByEmail = await _userManager.FindByEmailAsync(to);
            if (userByEmail != null)
            {
                if (!user.Id.Equals(userByEmail.Id))
                    return Fail<BaseResponseModel>(message: Gym13Resources.EmailExists);
                else if (user.EmailConfirmed)
                    return Success(new BaseResponseModel(), message: Gym13Resources.EmailAlreadyConfirmed);
            }
            var userByPhone = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == to);
            if (userByPhone != null)
            {
                if (!userByPhone.Id.Equals(user.Id))
                    return Fail<BaseResponseModel>(message: Gym13Resources.PhoneNumberExists);
                else if (user.PhoneNumberConfirmed)
                    return Success(new BaseResponseModel(), message: Gym13Resources.PhoneAlreadyConfirmed);
                else
                {
                    GenerateValidationCode(user);
                    await SendCode(user, null, false);
                    return Success<BaseResponseModel>();
                }
            }
            bool sendOnEmail = false;
            var isPhone = ValidateEmailOrPhone.IsValidPhone(to);
            if (!isPhone)
                sendOnEmail = ValidateEmailOrPhone.IsValidEmail(to);
            if (isPhone || sendOnEmail)
            {
                string? subject = sendOnEmail ? "ელ.ფოსტის დადასტურების კოდი" : null;
                GenerateValidationCode(user);
                await SendCode(user, subject, sendOnEmail);
                return Success<BaseResponseModel>();
            }
            return Fail<BaseResponseModel>(message: Gym13Resources.IncorrectData);
        }

        public async Task<BaseResponseModel> SendValidationCode(string to)
        {
            var sendOnEmail = false;
            var user = await _userManager.FindByEmailAsync(to);
            if (user == null)
            {
                user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == to);
                if (user == null)
                    return Fail<BaseResponseModel>(message: Gym13Resources.UserNotExists);
            }
            else
                sendOnEmail = true;
            GenerateValidationCode(user);
            var subject = sendOnEmail ? "დადასტურების კოდი" : null;
            await SendCode(user, subject, sendOnEmail);
            return Success<BaseResponseModel>();
        }

        public async Task<BaseResponseModel> UpdatePassword(UpdatePasswordModel model)
        {
            if (!string.IsNullOrEmpty(model.UserId) && !string.IsNullOrEmpty(model.CurrentPassword))
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                    return Fail<BaseResponseModel>(message: Gym13Resources.UserNotExists);
                if (!await _userManager.CheckPasswordAsync(user, model.CurrentPassword))
                    return Fail<BaseResponseModel>(message: Gym13Resources.UserOrPasswordIncorrect);
                user.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user, model.Password);
                await _userManager.UpdateAsync(user);
                return Success<BaseResponseModel>();
            }
            var usr = await _userManager.FindByEmailAsync(model.EmailOrPhone);
            if (usr == null)
            {
                usr = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == model.EmailOrPhone);
                if (usr == null)
                    return Fail<BaseResponseModel>(message: Gym13Resources.UserNotExists);
            }
            usr.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(usr, model.Password);
            await _userManager.UpdateAsync(usr);
            return Success<BaseResponseModel>();

        }

        public async Task<UserProfileModel> GetUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Fail<UserProfileModel>(message: Gym13Resources.UserNotExists);
            var response = new UserProfileModel
            {
                UserId = user.Id,
                PhoneNumber = user.PhoneNumber,
                IdentificationNumber = user.IdentificationNumber,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                BirthDate = user.BirthDate
            };
            return Success(response);
        }

        public async Task<BaseResponseModel> UpdateUser(UpdateUserModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return Fail<BaseResponseModel>(message: Gym13Resources.UserNotExists);
            await InsertUserEntityHistory(user, model);
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.IdentificationNumber = model.IdentificationNumber;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Gender = model.Gender;
            user.BirthDate = model.BirthDate;
            await _userManager.UpdateAsync(user);
            return Success<BaseResponseModel>();
        }

        #region Private methods
        static string GenerateValidationCode(ApplicationUser user)
        {
            var code = new Random(Guid.NewGuid().GetHashCode()).Next(1000, 9999).ToString();
            user.ValidationCode = code;
            user.ValidationCodeDateCreated = DateTime.UtcNow.AddHours(4);
            return code;
        }
        async Task SendCode(ApplicationUser user, string? subject, bool sendOnEmail = true)
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
        async Task InsertUserEntityHistory(ApplicationUser user, UpdateUserModel request)
        {
            var histories = new List<EntityHistory>();
            if (user.Email != request.Email)
                histories.Add(new EntityHistory
                {
                    ActionType = EntityActionType.Updated,
                    Table = "Users",
                    Column = nameof(user.Email),
                    OldValue = user.Email,
                    NewValue = request.Email
                });
            if (user.PhoneNumber != request.PhoneNumber)
                histories.Add(new EntityHistory
                {
                    ActionType = EntityActionType.Updated,
                    Table = "Users",
                    Column = nameof(user.PhoneNumber),
                    OldValue = user.PhoneNumber,
                    NewValue = request.PhoneNumber
                });
            if (user.IdentificationNumber != request.IdentificationNumber)
                histories.Add(new EntityHistory
                {
                    ActionType = EntityActionType.Updated,
                    Table = "Users",
                    Column = nameof(user.IdentificationNumber),
                    OldValue = user.IdentificationNumber,
                    NewValue = request.IdentificationNumber
                });
            if (user.FirstName != request.FirstName)
                histories.Add(new EntityHistory
                {
                    ActionType = EntityActionType.Updated,
                    Table = "Users",
                    Column = nameof(user.FirstName),
                    OldValue = user.FirstName,
                    NewValue = request.FirstName
                });
            if (user.LastName != request.LastName)
                histories.Add(new EntityHistory
                {
                    ActionType = EntityActionType.Updated,
                    Table = "Users",
                    Column = nameof(user.LastName),
                    OldValue = user.LastName,
                    NewValue = request.LastName
                });
            if (user.Gender != request.Gender)
                histories.Add(new EntityHistory
                {
                    ActionType = EntityActionType.Updated,
                    Table = "Users",
                    Column = nameof(user.Gender),
                    OldValue = user.Gender.HasValue ? user.Gender.ToString() : null,
                    NewValue = request.Gender.HasValue ? request.Gender.ToString() : null
                });
            if (user.BirthDate != request.BirthDate)
                histories.Add(new EntityHistory
                {
                    ActionType = EntityActionType.Updated,
                    Table = "Users",
                    Column = nameof(user.BirthDate),
                    OldValue = user.BirthDate.HasValue ? user.BirthDate.ToString() : null,
                    NewValue = request.BirthDate.HasValue ? request.BirthDate.ToString() : null
                });
            if (histories.Count > 0)
            {
                _db.EntityHistories.AddRange(histories);
                await _db.SaveChangesAsync();
            }
        }
        #endregion
    }
}
