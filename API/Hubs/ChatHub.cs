using API.Dtos;
using API.Models;
using API.Services.MessageService;
using API.Services.UserService;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace API.Hubs
{
    public class ChatHub(IMessageService messageService, IUserService userService) : Hub
    {
        private readonly IMessageService _messageService = messageService;
        private readonly IUserService _userService = userService;

        public async Task SendMessage(string message)
        {
            var principal = Context.User ?? throw new InvalidOperationException("User principal is null.");
            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int parsedUserId))
            {
                throw new InvalidOperationException("Invalid user id.");
            }

            var user = await _userService.GetUserById(parsedUserId) ?? throw new InvalidOperationException("User not found.");
            var msg = new Message
            {
                UserId = user.Id,
                Content = message
            };

            await _messageService.AddMessageAsync(msg);

            var userMessage = new UserMessageDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                AvatarUrl = user.AvatarUrl,
                Content = message
            };

            await Clients.All.SendAsync("ReceiveMessage", userMessage);
        }
    }
}
