using Gym13.Common.Enums;

namespace Gym13.Application.Models.Account
{
    public class RegistrationRequestModel : UpdateUserRequestModel
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
