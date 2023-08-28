namespace Gym13.Application.Models.Account
{
    public class ChangePasswordRequestModel
    {
        public string CurrentPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
