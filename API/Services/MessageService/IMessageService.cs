using API.Dtos;
using API.Models;

namespace API.Services.MessageService
{
    public interface IMessageService
    {
        Task<ServiceResponse<List<UserMessageDto>>> GetMessagesAsync();
        Task<ServiceResponse<Message>> AddMessageAsync(Message message);
    }
}
