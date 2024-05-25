using API.Dtos;
using API.Models;

namespace API.Services.NotificationService
{
    public interface INotificationService
    {
        Task<ServiceResponse<int>> AddNotificationToAllClientsAsync(Notification notification);
        Task<ServiceResponse<NotificationDto>> UpdateNotificationAsync(UpdateNotificationDto dto);
        Task<ServiceResponse<List<NotificationDto>>> GetAllNotificationsAsync();
    }
}
