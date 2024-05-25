using API.Dtos;
using API.Services.EventCarteConfigurationService;
using API.Services.EventCreditConfigurationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class EventController(IEventCarteConfigurationService eventCarteConfigurationService,
        IEventCreditConfigurationService eventCreditConfigurationService) : ControllerBase
    {
        private readonly IEventCarteConfigurationService _eventCarteConfigurationService = eventCarteConfigurationService;
        private readonly IEventCreditConfigurationService _eventCreditConfigurationService = eventCreditConfigurationService;

        [HttpGet("{categoryId}")]
        public async Task<ActionResult<ServiceResponse<List<EvenementDto>>>> GetAllEvent(int categoryId)
        {
            ServiceResponse<List<EvenementDto>> response;
            if (categoryId == 1)
            {
                response = await _eventCarteConfigurationService.GetAllEventCartAsync();
            }
            else if (categoryId == 2)
            {
                response = await _eventCreditConfigurationService.GetAllEventCreditAsync();
            }
            else
            {
                response = new ServiceResponse<List<EvenementDto>>{
                    Message = "Invalid categoryId"
                };
            }

            if (response.Data != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("UpdateEvent")]
        public async Task<ActionResult<ServiceResponse<EvenementDto>>> UpdateEventCarte(EvenementDto request)
        {
            ServiceResponse<EvenementDto> response;
            if (request.CategoryId == 1)
            {
                response = await _eventCarteConfigurationService.UpdateEventCarteAsync(request);
            }
            else if (request.CategoryId == 2)
            {
                response = await _eventCreditConfigurationService.UpdateEventCreditAsync(request);
            }
            else
            {
                response = new ServiceResponse<EvenementDto>
                {
                    Message = "Invalid categoryId"
                };
            }

            if (response.Data != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
