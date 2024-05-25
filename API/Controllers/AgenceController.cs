using API.Dtos;
using API.Services.AgenceService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AgenceController(IAgenceService agenceService) : ControllerBase
    {
        private readonly IAgenceService _agenceService = agenceService;
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<AgenceDetails>>>> GetAllAgences()
        {
            ServiceResponse<List<AgenceDetails>> response = await _agenceService.GetAllAgencesAsync();
            if (response.Data != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
