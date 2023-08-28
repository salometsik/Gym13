using Gym13.Common.Enums;
using Gym13.Domain.Models;

namespace Gym13.Application.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmail(string email, string subject, string body, NotificationType type, ApplicationUser user);
    }
}
