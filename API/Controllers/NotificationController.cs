using API.Dtos;
using API.Models;
using API.Services.NotificationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController(INotificationService notificationService) : ControllerBase
    {
        private readonly INotificationService _notificationService = notificationService;

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<int>>> CreateNotification([FromBody] AddNotification request)
        {
            var notification = new Notification
            {
                Name = request.Name,
                SendDate = request.SendDate,
                EmailBody = request.EmailBody,
                SMSMessage = request.SMSMessage,
                WhatsMessage = request.WhatsMessage,

            };
            var response = await _notificationService.AddNotificationToAllClientsAsync(notification);

            if (response.Data != 0)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult<ServiceResponse<NotificationDto>>> UpdateNotification([FromBody] UpdateNotificationDto dto)
        {
            var response = await _notificationService.UpdateNotificationAsync(dto);

            if (response.Data != null)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<NotificationDto>>> GetAllNotifications()
        {
            var response = await _notificationService.GetAllNotificationsAsync();

            if (response.Data != null)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}
