using Gym13.Application.Interfaces;
using Gym13.Common.Enums;
using Gym13.Domain.Data;
using Gym13.Domain.Models;
using System.Net;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;
using Gym13.Application.Models;
using Microsoft.Extensions.Logging;

namespace Gym13.Application.Services
{
    public class MessageSender : IEmailSender, ISmsSender
    {
        readonly Gym13DbContext _db;
        readonly SendGridClient _emailClient;
        private readonly string _fromName;
        private readonly string _fromEmail;
        readonly SmsSenderOptions smsSenderOptions;

        public MessageSender(Gym13DbContext db, IConfiguration configuration)
        {
            _db = db;
            var apiKey = configuration.GetValue<string>("EmailSender:ApiKey");
            _fromName = configuration.GetValue<string>("EmailSender:SenderName");
            _fromEmail = configuration.GetValue<string>("EmailSender:SenderEmail");
            _emailClient = new SendGridClient(apiKey);
        }

        public async Task SendEmail(string email, string subject, string body, NotificationType type, ApplicationUser user)
        {
            var notification = new Notification
            {
                Subject = subject,
                Body = body,
                DeliveryType = NotificationDeliveryType.SMS,
                NotificationType = type,
                Email = email,
                SendDate = DateTime.UtcNow.AddHours(4),
                SendStatus = NotificationSendStatus.Pending,
                User = user
            };

            _db.Notifications.Add(notification);
            _db.SaveChanges();
            try
            {
                var sendEmail = await SendMail(email, subject, body);
                if (sendEmail == HttpStatusCode.Accepted)
                {
                    notification.SendStatus = NotificationSendStatus.Sent;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                notification.SendStatus = NotificationSendStatus.Failed;
                notification.SendStatusDescription = ex.Message;
                _db.SaveChanges();
                throw;
            }
        }

        public async Task SendSmsAsync(string number, string message, NotificationType notificationType, ApplicationUser user)
        {
            var notification = new Notification
            {
                Body = message,
                DeliveryType = NotificationDeliveryType.SMS,
                Phone = number,
                NotificationType = notificationType,
                SendDate = DateTime.UtcNow.AddHours(4),
                SendStatus = NotificationSendStatus.Pending,
                User = user
            };

            _db.Notifications.Add(notification);
            _db.SaveChanges();

            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{smsSenderOptions.BaseUrl}?username={smsSenderOptions.UserName}&password={smsSenderOptions.Password}&" +
                        $"client_id={smsSenderOptions.ClientId}&service_id={smsSenderOptions.ServiceId}&to=995{number}&text={message}&utf=1";
                    var response = await client.GetAsync(url);
                    var content = await response.Content.ReadAsStringAsync();

                    notification.SendStatus = NotificationSendStatus.Sent;
                    notification.SendStatusDescription = content;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                notification.SendStatus = NotificationSendStatus.Failed;
                notification.SendStatusDescription = ex.Message;
                _db.SaveChanges();

                throw;
            }
        }

        async Task<HttpStatusCode> SendMail(string email, string subject, string body)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var msg = new SendGridMessage();
                msg.SetFrom(new EmailAddress(_fromEmail, _fromName));
                msg.SetSubject(subject);
                msg.AddContent(MimeType.Html, body);
                msg.AddTo(new EmailAddress(email));
                var response = await _emailClient.SendEmailAsync(msg);
                return response.StatusCode;
            }
            return HttpStatusCode.BadRequest;
        }
    }
}
