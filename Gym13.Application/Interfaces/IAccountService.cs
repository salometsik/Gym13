using Gym13.Application.Models;
using Gym13.Application.Models.Account;

namespace Gym13.Application.Interfaces
{
    public interface IAccountService
    {
        Task<RegistrationResponseModel> CreateAccount(RegistrationRequestModel request);
        Task<BaseResponseModel> ConfirmValidationCode(string? userId, string emailOrPhone, string code);
        Task<BaseResponseModel> SendCodeFromProfile(string to, string userId);
        Task<BaseResponseModel> SendValidationCode(string to);
        Task<BaseResponseModel> UpdatePassword(UpdatePasswordModel model);
        Task<UserProfileModel> GetUser(string userId);
        Task<BaseResponseModel> UpdateUser(UpdateUserModel model);
    }
}
