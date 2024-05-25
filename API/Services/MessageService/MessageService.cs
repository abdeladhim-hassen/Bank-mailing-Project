using API.Data;
using API.Dtos;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services.MessageService
{
    public class MessageService(DataContext context) : IMessageService
    {
        private readonly DataContext _context = context;

        public async Task<ServiceResponse<List<UserMessageDto>>> GetMessagesAsync()
        {
            var response = new ServiceResponse<List<UserMessageDto>>();
            try
            {
                response.Data = await _context.Messages.Select(u => new UserMessageDto
                {
                   FirstName = u.User.FirstName,
                   LastName = u.User.LastName,
                   AvatarUrl = u.User.AvatarUrl,
                   Content = u.Content,
                })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                response.Message = $"Error: {ex.Message}";
            }
            return response;
        }

        public async Task<ServiceResponse<Message>> AddMessageAsync(Message message)
        {
            var response = new ServiceResponse<Message>();
            try
            {
                _context.Messages.Add(message);
                await _context.SaveChangesAsync();
                response.Data = message;
            }
            catch (Exception ex)
            {
                response.Message = $"Error: {ex.Message}";
            }
            return response;
        }
    }
}
