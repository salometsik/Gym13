namespace Gym13.Application.Models.Account
{
    public class ConfirmUserRequestModel : ConfirmEmailOrPhoneRequestModel
    {
        public string UserId { get; set; }
    }
}
