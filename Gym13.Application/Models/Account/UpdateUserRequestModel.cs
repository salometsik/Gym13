using Gym13.Common.Enums;

namespace Gym13.Application.Models.Account
{
    public class UpdateUserRequestModel
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
