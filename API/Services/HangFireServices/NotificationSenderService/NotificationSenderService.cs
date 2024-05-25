using API.Data;
using API.Services.CommunicationService;
using Microsoft.EntityFrameworkCore;

namespace API.Services.HangFireServices.NotificationSenderService
{
    public class NotificationSenderService(DataContext context,
                                    ILogger<NotificationSenderService> logger,
                                    ICommunicationService communicationService) : INotificationSenderService
    {

        private readonly DataContext _context = context;
        private readonly ICommunicationService _communicationService = communicationService;
        private readonly ILogger<NotificationSenderService> _logger = logger;
        public async Task SendingNotificationsAsync()
        {
            try
            {
                // Retrieve all notifications from the database
                var currentDate = DateTime.Now;
                var notifications = await _context.Notifications
                    .Where(x => !x.IsDelivered &&
                           x.SendDate.Year == currentDate.Year &&
                           x.SendDate.Month == currentDate.Month &&
                           x.SendDate.Day == currentDate.Day &&
                           x.SendDate.Hour == currentDate.Hour &&
                           x.SendDate.Minute == currentDate.Minute
                           )
                    .ToListAsync();     

                foreach (var notification in notifications)
                {                   
                        bool success = true;

                        foreach (var clientNotification in notification.ClientNotifications)
                        {
                            try
                            {
                                switch (clientNotification.Client.CanalPrefere.ToUpper())
                                {
                                    case "EMAIL":
                                        await _communicationService.SendEmailAsync(
                                            recipient: clientNotification.Client.AdresseEmail,
                                            subject: notification.Name,
                                            body: BuildMessage(notification.EmailBody, clientNotification.Client.Prenom, clientNotification.Client.Nom));
                                        break;

                                    case "SMS":
                                        var smsResult = await _communicationService.SendSMSAsync(
                                            recipient: clientNotification.Client.NumeroTelephone,
                                            message: BuildMessage(notification.EmailBody, clientNotification.Client.Prenom, clientNotification.Client.Nom));

                                        if (!smsResult.Contains("successfully"))
                                        {
                                            success = false;
                                            _logger.LogWarning($"Failed to send SMS for Notification ID: {notification.Id} to Client ID: {clientNotification.Client.Id}, Error: {smsResult}");
                                        }
                                        break;

                                    case "WHATSAPP":
                                        await _communicationService.SendWhatsAppMessageAsync(
                                            recipient: clientNotification.Client.NumeroTelephone,
                                            message: BuildMessage(notification.EmailBody, clientNotification.Client.Prenom, clientNotification.Client.Nom));
                                        break;

                                    default:
                                        _logger.LogWarning($"Unknown channel '{clientNotification.Client.CanalPrefere}' for Notification ID: {notification.Id}");
                                        success = false;
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                success = false;
                                _logger.LogWarning($"Failed to send {clientNotification.Client.CanalPrefere} message for Notification ID: {notification.Id} to Client ID: {clientNotification.Client.Id}, Error: {ex.Message}");
                            }
                        }

                        if (success)
                        {
                            notification.IsDelivered = true;
                        }
                    
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
            }
        }
        private static string BuildMessage(string template, string FirstName, string LastName)
        {
            string messageContent = template;

            // Replace hashtags with client information if they exist
            messageContent = messageContent.Replace("#[Nom Client]", FirstName);
            messageContent = messageContent.Replace("#[Prenom Client]", LastName);

            return messageContent;
        }
    }
}
