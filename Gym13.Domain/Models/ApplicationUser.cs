using Gym13.Common.Enums;
using Microsoft.AspNetCore.Identity;

namespace Gym13.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() => RegistrationDate = DateTime.UtcNow.AddHours(4);
        public DateTime RegistrationDate { get; set; }
        public DateTime? BirthDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PersonalNumber { get; set; }
        public ApplicationUserStatus Status { get; set; }
        public string? PhonePrefix { get; set; }
        public Gender? Gender { get; set; }
        public string? ValidationCode { get; set; }
        public DateTime? ValidationCodeDateCreated { get; set; }
    }
}
