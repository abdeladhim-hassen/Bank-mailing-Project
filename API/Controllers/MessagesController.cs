using API.Dtos;
using API.Models;
using API.Services.MessageService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController(IMessageService messageService) : ControllerBase
    {
        private readonly IMessageService _messageService = messageService;

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<UserMessageDto>>> GetMessages()
        {
            var response = await _messageService.GetMessagesAsync();
            if (response.Data == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
