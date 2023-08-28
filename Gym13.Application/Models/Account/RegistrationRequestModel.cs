using Gym13.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Gym13.Application.Models.Account
{
    public class RegistrationRequestModel
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string Password { get; set; }
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
