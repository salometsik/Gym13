namespace Gym13.Application.Models.Account
{
    public class UpdatePasswordModel
    {
        public string? UserId { get; set; }
        public string? EmailOrPhone { get; set; }
        public string? CurrentPassword { get; set; }
        public string Password { get; set; }
    }
}
