namespace Gym13.Application.Models.Account
{
    public class ResetPasswordRequestModel
    {
        public string EmailOrPhone { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
