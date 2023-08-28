using Gym13.Common.Enums;
using Gym13.Domain.Models;

namespace Gym13.Application.Interfaces
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message, NotificationType notificationType, ApplicationUser user);
    }
}
