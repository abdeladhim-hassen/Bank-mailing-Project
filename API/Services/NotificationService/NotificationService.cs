using API.Data;
using API.Dtos;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services.NotificationService
{
    public class NotificationService(DataContext context) : INotificationService
    {
        private readonly DataContext _context = context;
        public async Task<ServiceResponse<int>> AddNotificationToAllClientsAsync(Notification notification)
        {
            var response = new ServiceResponse<int>();

            try
            {
                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();

                var clients = await _context.Clients.ToListAsync();

                foreach (var client in clients)
                {
                    var clientNotification = new ClientNotification
                    {
                        ClientId = client.Id,
                        NotificationId = notification.Id
                    };

                    _context.ClientNotifications.Add(clientNotification);
                }

                await _context.SaveChangesAsync();

                response.Data = notification.Id;
                response.Message = "Notification added and associated with all clients successfully.";
            }
            catch (Exception ex)
            {
                response.Message = $"An error occurred: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<NotificationDto>> UpdateNotificationAsync(UpdateNotificationDto dto)
        {
            var response = new ServiceResponse<NotificationDto>();

            try
            {
                var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.Id == dto.Id);
                if (notification == null)
                {
                    response.Data = null;
                    response.Message = "Notification not found.";
                    return response;
                }

                notification.SendDate = dto.SendDate;

                if (!string.IsNullOrEmpty(dto.Name))
                {
                    notification.Name = dto.Name;
                }
                if (!string.IsNullOrEmpty(dto.SMSMessage))
                {
                    notification.SMSMessage = dto.SMSMessage;
                }

                notification.SendDate = dto.SendDate;

                if(dto.SendDate.CompareTo(DateTime.Now) >= 0)
                {
                    notification.IsDelivered = false;
                }

                if (!string.IsNullOrEmpty(dto.EmailBody))
                {
                    notification.EmailBody = dto.EmailBody;
                }

                if (!string.IsNullOrEmpty(dto.WhatsMessage))
                {
                    notification.WhatsMessage = dto.WhatsMessage;
                }
                
                _context.Notifications.Update(notification);
                await _context.SaveChangesAsync();

                var updatedNotification = new NotificationDto
                {
                    Id = notification.Id,
                    Name = notification.Name,
                    DateCreated = notification.DateCreated,
                    SendDate = notification.SendDate,
                    EmailBody = notification.EmailBody,
                    SMSMessage = notification.SMSMessage,
                    WhatsMessage = notification.WhatsMessage
                };
                response.Data = updatedNotification;
                response.Message = "Notification updated successfully.";
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.Message = $"An error occurred: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<List<NotificationDto>>> GetAllNotificationsAsync()
        {
            var response = new ServiceResponse<List<NotificationDto>>();

            try
            {
                var notifications = await _context.Notifications.ToListAsync();

                var notificationDtos = notifications.Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Name = n.Name,
                    DateCreated = n.DateCreated,
                    SendDate = n.SendDate,
                    EmailBody = n.EmailBody,
                    SMSMessage = n.SMSMessage,
                    WhatsMessage = n.WhatsMessage
                }).ToList();

                response.Data = notificationDtos;
                response.Message = "Notifications retrieved successfully.";
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.Message = $"An error occurred: {ex.Message}";
            }

            return response;
        }
    }
}
