using Gym13.Common.Enums;

namespace Gym13.Domain.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public NotificationType NotificationType { get; set; }
        public NotificationDeliveryType DeliveryType { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public NotificationSendStatus SendStatus { get; set; }
        public DateTime SendDate { get; set; }
        public string SendStatusDescription { get; set; }
    }
}
