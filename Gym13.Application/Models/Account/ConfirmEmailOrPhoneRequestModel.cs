namespace Gym13.Application.Models.Account
{
    public class ConfirmEmailOrPhoneRequestModel
    {
        public string EmailOrPhone { get; set; }
        public string Code { get; set; }
    }
}
