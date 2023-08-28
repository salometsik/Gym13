using Gym13.Application.Models.Account;

namespace Gym13.Application.Interfaces
{
    public interface IAccountService
    {
        Task<RegistrationResponseModel> CreateAccount(RegistrationRequestModel request);
    }
}
